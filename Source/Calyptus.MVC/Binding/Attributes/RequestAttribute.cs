using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Security.Principal;
using System.Web.SessionState;
using System.Web.Caching;
using System.IO;
using System.Xml;

namespace Calyptus.MVC
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public class RequestAttribute : Attribute, IPropertyBinding, IParameterBinding
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

		private static Func<IHttpContext, object> GetBinder(Type type)
		{
			if (type == typeof(HttpBrowserCapabilities)) return c => c.Request.Browser;
			else if (type == typeof(IHttpRequest)) return c => c.Request;
			else if (type == typeof(Stream)) return c => c.Request.InputStream;
			else if (type == typeof(TextReader)) return c => new StreamReader(c.Request.InputStream, c.Request.ContentEncoding);
			else if (type == typeof(BinaryReader)) return c => new BinaryReader(c.Request.InputStream, c.Response.ContentEncoding);
			else if (type == typeof(XmlReader)) return c => System.Xml.XmlReader.Create(new StreamReader(c.Request.InputStream));
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
