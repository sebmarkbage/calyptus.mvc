using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Security.Principal;
using System.Web.SessionState;
using System.Web.Caching;
using System.IO;
using System.Xml;

namespace Calyptus.Mvc
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public class ResponseAttribute : Attribute, IPropertyBinding, IParameterBinding
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
			if (type == typeof(IHttpResponse)) return c => c.Response;
			else if (type == typeof(Stream)) return c => c.Response.OutputStream;
			else if (type == typeof(TextWriter)) return c => c.Response.Output;
			else if (type == typeof(BinaryWriter)) return c => new BinaryWriter(c.Response.OutputStream, c.Response.ContentEncoding);
			else if (type == typeof(TextWriter)) return c => c.Response.Output;
			else if (type == typeof(XmlWriter)) return c => System.Xml.XmlWriter.Create(c.Response.Output);
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
