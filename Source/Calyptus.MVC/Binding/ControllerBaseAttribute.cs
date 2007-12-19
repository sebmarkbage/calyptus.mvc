using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Calyptus.MVC.RoutingEngines;

namespace Calyptus.MVC.Binding
{
	public abstract class ControllerBaseAttribute : Attribute
	{
		private struct ActionBinding
		{
			public MethodInfo Method;
			public bool IsNestable;
			public ActionAttribute Binding;
		}
		private struct PropertyBinding
		{
			public PropertyInfo Property;
			public IBindable Binding;
		}

		private ActionBinding[] _bindings;
		private PropertyBinding[] _propBindings;

		public virtual bool AutoRenderAction { get; set; }

		public virtual bool RequireExplicitActionAttributes { get; set; }

		internal virtual void Initialize(Type controllerType)
		{
			if (_bindings != null)
				return;

			List<ActionBinding> bindings = new List<ActionBinding>();

			foreach (MethodInfo m in controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
			{
				object[] actionAttributes = m.GetCustomAttributes(typeof(ActionAttribute), false);
				if (actionAttributes.Length > 0)
				{
					foreach (ActionAttribute a in actionAttributes)
					{
						a.Initialize(m);
						bindings.Add(new ActionBinding { Binding = a, Method = m, IsNestable = IsNestable(m) });
					}
				}
				else if (!RequireExplicitActionAttributes && !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_"))
				{
					ActionAttribute a = new ActionAttribute();
					a.Initialize(m);
					bindings.Add(new ActionBinding { Binding = a, Method = m, IsNestable = IsNestable(m) });
				}
			}

			if (bindings.Count > 0)
			{
				bindings.Sort((b1, b2) => b1.Binding is DefaultActionAttribute ? (b2.Binding is DefaultActionAttribute ? 0 : 1) : (b2.Binding is DefaultActionAttribute ? -1 : 0));
				_bindings = bindings.ToArray();
			}
		}

		private bool IsNestable(MethodInfo m)
		{
			return (m.ReturnType.GetCustomAttributes(typeof(ChildControllerAttribute), false).Length > 0);
		}

		internal virtual bool TryBinding(IHttpContext context, IPathStack path, out RoutingCommand command)
		{
			int index = path.Index;
			foreach (ActionBinding b in _bindings)
				if (b.Binding.TryBinding(context, path, out command) && (b.IsNestable || path.IsAtEnd))
				{
					command.Method = b.Method;
					command.IsNestable = b.IsNestable;
					return true;
				}
				else
					path.ReverseToIndex(index);

			command = null;
			return false;
		}

		internal virtual void SerializeBinding(IPathStack path, MethodInfo method, object[] arguments, out bool isNestable)
		{
			foreach(ActionBinding b in _bindings)
				if (b.Method == method)
				{
					isNestable = b.IsNestable;
					b.Binding.SerializeBinding(path, arguments);
					return;
				}

			throw new Exception(String.Format("Method \"{0}\" is not bindable.", method.Name));
		}
	}
}
