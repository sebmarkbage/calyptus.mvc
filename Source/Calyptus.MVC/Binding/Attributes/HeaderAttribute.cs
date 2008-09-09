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
	public class HeaderAttribute : Attribute, IPropertyBinding, IParameterBinding
	{
		public string Key { get; set; }

		public HeaderAttribute()
		{
		}
		public HeaderAttribute(string key)
		{
			this.Key = key;
		}

		public void Initialize(ParameterInfo parameter)
		{
			Type t = parameter.ParameterType.IsByRef ? parameter.ParameterType.GetElementType() : parameter.ParameterType;
			if (t != typeof(string)) throw new BindingException(String.Format("Invalid path type. {1} can't bind to type '{0}'. Try using string.", parameter.ParameterType.Name, GetType().Name));
		}

		public void Initialize(PropertyInfo property)
		{
			Type t = property.PropertyType;
			if (t != typeof(string)) throw new BindingException(String.Format("Invalid path type. {1} can't bind to type '{0}'. Try using string", property.PropertyType.Name, GetType().Name));
		}

		public bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight)
		{
			overloadWeight = 0;
			obj = context.Request.Headers[Key];
			return obj != null;
		}

		public bool TryBinding(IHttpContext context, out object obj)
		{
			obj = context.Request.Headers[Key];
			return obj != null;
		}

		public void StoreBinding(IHttpContext context, object value)
		{
			string v = value as string;
			if (v != null)
				context.Response.AppendHeader(Key, v);
		}

		public void SerializePath(IPathStack path, object value)
		{
			// Exclude
		}
	}
}
