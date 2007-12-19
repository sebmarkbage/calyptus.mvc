using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Web;
using Calyptus.MVC.Binding;
using System.Collections;

namespace Calyptus.MVC.RoutingEngines
{
    internal class RoutingEngine : IRoutingEngine
    {
		private struct ControllerBinding
		{
			public Type ControllerType;
			public ControllerAttribute Binding;
		}

		private ControllerBinding[] _controllers;
		private Dictionary<Type, ChildControllerAttribute> _childControllers;

		public RoutingEngine() : this(System.Web.Compilation.BuildManager.GetReferencedAssemblies())
		{
		}

        public RoutingEngine(ICollection assemblies)
        {
			List<ControllerBinding> controllers = new List<ControllerBinding>();
			Dictionary<Type, ChildControllerAttribute> childControllers = new Dictionary<Type, ChildControllerAttribute>();

			foreach (Assembly a in assemblies)
				foreach (Type t in a.GetTypes())
				{
					object[] attributes = t.GetCustomAttributes(typeof(ControllerBaseAttribute), false);
					foreach (ControllerBaseAttribute attr in attributes)
					{
						attr.Initialize(t);
						if (attr is ControllerAttribute)
							controllers.Add(new ControllerBinding { ControllerType = t, Binding = (ControllerAttribute)attr });
						else if (attr is ChildControllerAttribute)
							childControllers.Add(t, (ChildControllerAttribute)attr);
					}
				}

			if (controllers.Count > 0)
			{
				controllers.Sort((c1, c2) => c1.Binding is DefaultControllerAttribute ? (c2.Binding is DefaultControllerAttribute ? 0 : 1) : (c2.Binding is DefaultControllerAttribute ? -1 : 0));
				_controllers = controllers.ToArray();
			}

			if (childControllers.Count > 0)
				_childControllers = childControllers;
		}

		public IHttpHandler ParseRoute(IHttpContext context, IPathStack path)
        {
			if (_controllers == null)
				return null;

			foreach (ControllerBinding c in _controllers)
			{
				RoutingCommand command;
				int index = path.Index;
				if (c.Binding.TryBinding(context, path, out command))
					return new ControllerHandler { ControllerType = c.ControllerType, Command = command, Stack = path, Engine = this };
				else
					path.ReverseToIndex(index);
			}
			return null;
        }

		public bool TryParseChildRoute(Type controllerType, IHttpContext context, IPathStack path, out RoutingCommand command)
		{
			ChildControllerAttribute a;
			if (_childControllers != null && _childControllers.TryGetValue(controllerType, out a))
			{
				if (a.TryBinding(context, path, out command))
					return true;
			}
			command = null;
			return false;
		}

		public string GetRelativePath(Expression<Action> action)
        {
            throw new NotImplementedException();
        }

        public string GetURL(Expression<Action> action)
        {
            throw new NotImplementedException();
        }

        public string GetAbsolutePath(Expression<Action> action)
        {
            throw new NotImplementedException();
        }
    }
}
