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
	public class MapPathAttribute : Attribute, IPropertyBinding, IParameterBinding
	{
		private string path;

		public MapPathAttribute() : this(null) { }

		public MapPathAttribute(string path)
		{
			this.path = string.IsNullOrEmpty(path) ? "~/" : path;
		}

		private Func<IHttpContext, object> binder;

		public void Initialize(ParameterInfo parameter)
		{
			binder = GetBinder(parameter.ParameterType);
			if (binder == null) throw new BindingException(String.Format("Invalid path type. {1} can't bind to type '{0}'. Try using string.", parameter.ParameterType.Name, GetType().Name));
		}

		public void Initialize(PropertyInfo property)
		{
			binder = GetBinder(property.PropertyType);
			if (binder == null) throw new BindingException(String.Format("Invalid path type. {1} can't bind to type '{0}'. Try using string", property.PropertyType.Name, GetType().Name));
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

		private Func<IHttpContext, object> GetBinder(Type type)
		{
			if (type == typeof(string)) return c => c.Server.MapPath(path);
			else if (type == typeof(DirectoryInfo)) return c => new DirectoryInfo(c.Server.MapPath(path));
			else if (type == typeof(FileInfo)) return c => new FileInfo(c.Server.MapPath(path));
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
