using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Security.Principal;
using System.Web.SessionState;
using System.Web.Caching;

namespace Calyptus.MVC
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public class ContextAttribute : Attribute, IPropertyBinding, IParameterBinding
	{
		private Func<IHttpContext, object> binder;

		public void Initialize(ParameterInfo parameter)
		{
			binder = GetBinder(parameter.ParameterType);
			if (binder == null) throw new BindingException(String.Format("Invalid context type. ContextAttribute can't bind to type '{0}'.", parameter.ParameterType.Name));
		}

		public void Initialize(PropertyInfo property)
		{
			binder = GetBinder(property.PropertyType);
			if (binder == null) throw new BindingException(String.Format("Invalid context type. ContextAttribute can't bind to type '{0}'.", property.PropertyType.Name));
		}

		public bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight)
		{
			overloadWeight = 0;
			return TryBinding(context, out obj);
		}

		public bool TryBinding(IHttpContext context, out object obj)
		{
			obj = binder(context);
			return true;
		}

		public static bool IsContextType(Type type)
		{
			return GetBinder(type) != null;
		}

		private static Func<IHttpContext, object> GetBinder(Type type)
		{
			if (type == typeof(HttpBrowserCapabilities)) return c => c.Request.Browser;
			else if (type == typeof(Cache)) return c => c.Cache;
			else if (type == typeof(HttpApplicationState)) return c => c.Application;
			else if (type == typeof(HttpApplication)) return c => c.ApplicationInstance;
			else if (type == typeof(IHttpSessionState)) return c => c.Session;
			else if (type == typeof(IPrincipal)) return c => c.User;
			else if (type == typeof(IIdentity)) return c => c.User.Identity;
			else if (type == typeof(IHttpContext)) return c => c;
			else if (type == typeof(IHttpRequest)) return c => c.Request;
			else if (type == typeof(IHttpResponse)) return c => c.Response;
			else if (type == typeof(IRoutingEngine)) return c => c.RoutingEngine;
			else if (type == typeof(IViewFactory)) return c => c.TemplateEngine;
			else return null;
		}

		public void SerializePath(IPathStack path, object value)
		{
			// Exclude
		}

		public void StoreBinding(IHttpContext context, object value)
		{
			// Exclude
		}

	}
}
