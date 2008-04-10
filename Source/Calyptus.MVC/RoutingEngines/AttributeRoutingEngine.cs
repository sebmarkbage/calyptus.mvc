using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Web;
using Calyptus.MVC;
using System.Collections;

namespace Calyptus.MVC
{
    internal class AttributeRoutingEngine : IRoutingEngine
    {
		private static Dictionary<Type, IControllerBinding[]> _typeControllers;

		static AttributeRoutingEngine()
		{
			_typeControllers = new Dictionary<Type, IControllerBinding[]>();
		}

		public static IControllerBinding[] GetControllerBindings(Type type)
		{
			IControllerBinding[] bindings;
			if (!_typeControllers.TryGetValue(type, out bindings))
			{
				lock(_typeControllers)
				{
					object[] attributes = type.GetCustomAttributes(typeof(IControllerBinding), false);
					if (attributes.Length == 0)
					{
						if (typeof(IController).IsAssignableFrom(type))
						{
							IControllerBinding b = new ControllerAttribute();
							b.Initialize(type);
							bindings = new IControllerBinding[] { b };
						}
						else
							return null;
					}
					else
					{
						bindings = new IControllerBinding[attributes.Length];
						for (int i = 0; i < attributes.Length; i++)
						{
							IControllerBinding b = (IControllerBinding)attributes[i];
							b.Initialize(type);
							bindings[i] = b;
						}
					}
					_typeControllers.Add(type, bindings);
				}
			}
			return bindings;
		}

		private IEntryControllerBinding[] _controllers;

		public AttributeRoutingEngine() : this(System.Web.Compilation.BuildManager.GetReferencedAssemblies())
		{
		}

        public AttributeRoutingEngine(ICollection assemblies)
        {
			List<IEntryControllerBinding> controllers = new List<IEntryControllerBinding>();

			foreach (Assembly a in assemblies)
				foreach (Type t in a.GetTypes())
				{
					IControllerBinding[] bindings = GetControllerBindings(t);
					if (bindings != null)
						foreach (IControllerBinding b in bindings)
						{
							IEntryControllerBinding eb = b as IEntryControllerBinding;
							if (eb != null) controllers.Add(eb);
						}
				}

			_controllers = controllers.ToArray();

			GC.Collect(); // Trigger garbage collection to make sure application is lean after start up
		}

		public IHttpHandler ParseRoute(IHttpContext context, IPathStack path)
        {
			if (_controllers == null)
				return null;

			foreach (IEntryControllerBinding c in _controllers)
			{
				IHttpHandler handler;
				int index = path.Index;
				context.Route.ReverseToIndex(-1);
				if (c.TryBinding(context, path, out handler))
				{
					return handler;
				}
				else
					path.ReverseToIndex(index);
			}
			return null;
        }

		public void SerializeAbsoutePath(IRouteAction action, IPathStack path)
		{
			SerializePath(action, path, true);
		}

		public void SerializeRelativePath(IRouteAction action, IPathStack path)
		{
			SerializePath(action, path, false);
		}

		private void SerializePath(IRouteAction action, IPathStack path, bool requireEntry)
		{
			IControllerBinding[] bindings = GetControllerBindings(action.ControllerType);
			if (bindings == null || bindings.Length == 0) throw new BindingException(String.Format("Type \"{0}\" is not bindable.", action.ControllerType.FullName));
			IPathStack bestStack = null;
			foreach (IControllerBinding b in bindings)
				if (!requireEntry || b is IEntryControllerBinding)
				{
					IPathStack trialStack = new PathStack(false);
					b.SerializeToPath(action, trialStack);
					if (bestStack == null || trialStack.Index > bestStack.Index || (trialStack.Index == bestStack.Index && trialStack.QueryString.Count > bestStack.QueryString.Count))
						bestStack = trialStack;
				}
			if (bestStack != null)
			{
				path.Push(bestStack);
			}
			else
				throw new BindingException(String.Format("Type \"{0}\" is not a bindable EntryController.", action.ControllerType.FullName));
		}
	}
}
