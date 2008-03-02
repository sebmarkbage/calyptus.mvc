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
    internal class AttributeRoutingEngine : IRoutingEngine
    {
		private IControllerBinding[] _controllers;

		public AttributeRoutingEngine() : this(System.Web.Compilation.BuildManager.GetReferencedAssemblies())
		{
		}

        public AttributeRoutingEngine(ICollection assemblies)
        {
			List<IControllerBinding> controllers = new List<IControllerBinding>();

			foreach (Assembly a in assemblies)
				foreach (Type t in a.GetTypes())
				{
					object[] attributes = t.GetCustomAttributes(typeof(IControllerBinding), false);
					foreach (IControllerBinding attr in attributes)
					{
						attr.Initialize(t);
						controllers.Add(attr);
					}
				}

			// Sorting hack to ensure keywords are prioritized before bindings
			if (controllers.Count > 0)
			{
				controllers.Sort((c1, c2) => c1 is DefaultControllerAttribute ? (c2 is DefaultControllerAttribute ? 0 : 1) : (c2 is DefaultControllerAttribute ? -1 : 0));
				_controllers = controllers.ToArray();
			}
		}

		public IHttpHandler ParseRoute(IHttpContext context, IPathStack path)
        {
			if (_controllers == null)
				return null;

			foreach (IControllerBinding c in _controllers)
			{
				IHttpHandler handler;
				int index = path.Index;
				if (c.TryBinding(context, path, out handler))
				{
					return handler;
				}
				else
					path.ReverseToIndex(index);
			}
			return null;
        }

		public string GetRelativePath<T>(Expression<Action<T>> action)
		{
			throw new NotImplementedException();
		}

		public string GetRelativePath<T>(int index, Expression<Action<T>> action)
		{
			throw new NotImplementedException();
		}

		public string GetReplacementPath<T>(Expression<Action<T>> action)
		{
			throw new NotImplementedException();
		}

		public string GetReplacementPath<T>(int index, Expression<Action<T>> action)
		{
			throw new NotImplementedException();
		}

		public string GetAbsolutePath<T>(Expression<Action<T>> action)
		{
			throw new NotImplementedException();
		}
	}
}
