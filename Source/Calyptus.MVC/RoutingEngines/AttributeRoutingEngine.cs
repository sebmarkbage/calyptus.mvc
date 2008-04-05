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
					object[] attributes = t.GetCustomAttributes(typeof(IEntryControllerBinding), false);
					foreach (IEntryControllerBinding attr in attributes)
					{
						attr.Initialize(t);
						controllers.Add(attr);
					}
				}
		}

		public IHttpHandler ParseRoute(IHttpContext context, IPathStack path)
        {
			if (_controllers == null)
				return null;

			foreach (IEntryControllerBinding c in _controllers)
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

		public RouteTree RouteTree
		{
			get { throw new NotImplementedException(); }
		}
	}
}
