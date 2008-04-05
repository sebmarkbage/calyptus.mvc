using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace Calyptus.MVC
{
	public abstract class ControllerBaseAttribute : Attribute, IControllerBinding
	{
		protected IList<IMappingBinding> Mappings;

		private IDictionary<MethodInfo, IActionBinding[]> _methods;
		private IActionBinding[] _bindings;
		private IPropertyBinding[] _propBindings;

		private Type _controllerType;

		private IExtension[] _extensions;

		// OnBeforeActionDelegate
		// OnAfterActionDelegate
		// OnBeforeRenderDelegate
		// OnAfterRenderDelegate
		// DisposeDelegate

		protected ControllerBaseAttribute()
		{
			Mappings = new List<IMappingBinding>();
		}

		public virtual void Initialize(Type controllerType)
		{
			if (Mappings != null)
				return;

			_controllerType = controllerType;

			_methods = new Dictionary<MethodInfo, IActionBinding[]>();
			List<IActionBinding> bindings = new List<IActionBinding>();
			foreach (MethodInfo m in controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
			{
				object[] actionAttributes = m.GetCustomAttributes(typeof(IActionBinding), false);
				if (actionAttributes.Length > 0)
				{
					IActionBinding[] bs = new IActionBinding[actionAttributes.Length];
					for (int i = 0; i < actionAttributes.Length; i++)
					{
						IActionBinding a = bs[i] = (IActionBinding)actionAttributes[i];
						a.Initialize(m);
						bindings.Add(a);
					}
					_methods.Add(m, bs);
				}
			}


			List<IPropertyBinding> propBindings = new List<IPropertyBinding>();
			foreach (PropertyInfo p in controllerType.GetProperties())
			{
				object[] propAttributes = p.GetCustomAttributes(typeof(IPropertyBinding), false);
				foreach (IPropertyBinding b in propAttributes)
				{
					b.Initialize(p);
					propBindings.Add(b);
				}
			}
			if (propBindings.Count > 0)
				_propBindings = propBindings.ToArray();

			object[] exts = controllerType.GetCustomAttributes(typeof(IExtension), true);
			if (exts.Length > 0)
			{
				_extensions = new IExtension[exts.Length];
				for (int i = 0; i < exts.Length; i++)
				{
					IExtension ext = (IExtension)exts[i];
					ext.Initialize(controllerType);
					_extensions[i] = ext;
				}
			}
		}

		public virtual bool TryBinding(IHttpContext context, IPathStack path, out IHttpHandler handler)
		{
			int index = path.Index;
			object[] parameters;
			int overloadWeight;
			foreach (IActionBinding b in _bindings)
				if (b.TryBinding(context, path, out parameters, out overloadWeight))
				{
					handler = new ActionHandler
					{
						Arguments = parameters,
						Controller = Activator.CreateInstance(_controllerType),
						ControllerExtensions = _extensions,
						Context = context
					};
					return true;
				}
				else
					path.ReverseToIndex(index);

			handler = null;
			return false;
		}

		public virtual void SerializeToPath(IPathStack path, MethodInfo method, object[] arguments)
		{
			IActionBinding[] bindings;
			if (_methods.TryGetValue(method, out bindings))
				foreach(IActionBinding b in _bindings)
					b.SerializePath(path, arguments);
					return;

			throw new BindingException(String.Format("Method \"{0}\" is not bindable.", method.Name));
		}

		private class ActionHandler : IHttpAsyncHandler, IRequiresSessionState
		{
			public object Controller { get; set; }
			public IExtension[] ControllerExtensions { get; set; }
			public IExtension[] ActionExtensions { get; set; }
			public MethodInfo Method { get; set; }
			public object[] Arguments { get; set; }
			public IHttpContext Context { get; set; }

			public ActionHandler()
			{
			}

			public bool IsReusable
			{
				get { return false; }
			}

			public void ProcessRequest(HttpContext context)
			{
				ProcessRequest(Context);
			}

			public void ProcessRequest(IHttpContext context)
			{
				if (ControllerExtensions != null)
					foreach (IExtension ext in ControllerExtensions)
						ext.OnBeforeAction(context, Arguments);
				if (ActionExtensions != null)
					foreach (IExtension ext in ActionExtensions)
						ext.OnBeforeAction(context, Arguments);

				object returnValue;
				try
				{
					returnValue = Method.Invoke(Controller, Arguments);
				}
				catch (Exception e)
				{
					bool catched = false;
					if (ActionExtensions != null)
						foreach (IExtension ext in ActionExtensions)
							if (!ext.OnError(context, e))
								catched = true;
					if (ControllerExtensions != null)
						foreach (IExtension ext in ControllerExtensions)
							if (!ext.OnError(context, e))
								catched = true;
					if (!catched)
						throw e;
					return;
				}

				if (ActionExtensions != null)
					foreach (IExtension ext in ActionExtensions)
						ext.OnAfterAction(context, returnValue);
				if (ControllerExtensions != null)
					foreach (IExtension ext in ControllerExtensions)
						ext.OnAfterAction(context, returnValue);
			}

			public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback callback, object state)
			{
				throw new NotImplementedException();
			}

			public void EndProcessRequest(IAsyncResult result)
			{
				throw new NotImplementedException();
			}
		}
	}
}
