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
	public class ContentTypeAttribute : Attribute, IPropertyBinding, IParameterBinding
	{
		public ContentTypeAttribute()
		{
		}

		private Type bindingType;

		public void Initialize(ParameterInfo parameter)
		{
			bindingType = parameter.ParameterType.IsByRef ? parameter.ParameterType.GetElementType() : parameter.ParameterType;
			//if (bindingType != typeof(string)) throw new BindingException(String.Format("Invalid path type. {1} can't bind to type '{0}'. Try using string.", parameter.ParameterType.Name, GetType().Name));
		}

		public void Initialize(PropertyInfo property)
		{
			bindingType = property.PropertyType;
			//if (bindingType != typeof(string)) throw new BindingException(String.Format("Invalid path type. {1} can't bind to type '{0}'. Try using string", property.PropertyType.Name, GetType().Name));
		}

		public bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight)
		{
			overloadWeight = 0;
			return SerializationHelper.TryDeserialize(context.Request.ContentType, bindingType, out obj);
		}

		public bool TryBinding(IHttpContext context, out object obj)
		{
			return SerializationHelper.TryDeserialize(context.Request.ContentType, bindingType, out obj);
		}

		public void StoreBinding(IHttpContext context, object value)
		{
			string v = SerializationHelper.Serialize(value);
			if (v != null)
				context.Response.ContentType = v;
		}

		public void SerializePath(IPathStack path, object value)
		{
			// Exclude
		}
	}
}
