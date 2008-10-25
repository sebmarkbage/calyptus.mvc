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
	public class ETagAttribute : Attribute, IPropertyBinding, IParameterBinding
	{
		public ETagAttribute()
		{
		}

		private Type bindingType;

		public void Initialize(ParameterInfo parameter)
		{
			if (!parameter.ParameterType.IsByRef) throw new BindingException("ETag can only bind to ByRef parameters. Try using out-prefix in C#.");
			bindingType = parameter.ParameterType.GetElementType();
		}

		public void Initialize(PropertyInfo property)
		{
			if (!property.CanRead) throw new BindingException("ETag can only bind to readable properties.");
			bindingType = property.PropertyType;
		}

		public bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight)
		{
			overloadWeight = 0;
			obj = bindingType.IsValueType ? System.Activator.CreateInstance(bindingType) : null;
			return true;
		}

		public bool TryBinding(IHttpContext context, out object obj)
		{
			obj = null;
			return false;
		}

		public void StoreBinding(IHttpContext context, object value)
		{
			string v = SerializationHelper.Serialize(value);
			if (v != null)
			{
				if (v.Equals(context.Request.Headers["If-None-Match"])) throw new NotChanged();
				context.Response.AppendHeader("ETag", v);
			}
		}

		public void SerializePath(IPathStack path, object value)
		{
			// Exclude
		}
	}
}
