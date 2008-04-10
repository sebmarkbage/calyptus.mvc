using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.MVC.Mapping;

namespace Calyptus.MVC
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class PostAttribute : BinderBaseAttribute
	{
		private static IMappingBinding _verb = new VerbMapping("POST");

		private IMappingBinding _requestType;
		public string RequestType { get { return _requestType.ToString(); } set { if (_requestType != null) Mappings.Remove(_requestType); Mappings.Add(_requestType = new ContentTypeMapping(value)); } }

		public string Key { get; set; }

		public PostAttribute()
		{
			Mappings.Add(_verb);
		}

		public PostAttribute(string key) : this()
		{
			Key = key;
		}

		protected override void Initialize(System.Reflection.ParameterInfo method)
		{
			if (Key == null) Key = method.Name;
		}

		protected override void Initialize(System.Reflection.PropertyInfo method)
		{
			if (Key == null) Key = method.Name;
		}

		protected override bool TryBinding(IHttpContext context, out object value)
		{
			// TODO: JSON, XML, Stream etc.
			return NameValueSerialization.TryDeserialize(context.Request.Form, Key, BindingTargetType, out value);
		}
	}

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class FormAttribute : BinderBaseAttribute
	{
		public string Key { get; set; }

		public FormAttribute()
		{
			Mappings.Add(new VerbMapping("POST"));
			Mappings.Add(new ContentTypeMapping("application/x-www-form-urlencoded", "multipart/form-data"));
		}

		public FormAttribute(string formKey) : this()
		{
			Key = formKey;
		}

		protected override void Initialize(System.Reflection.ParameterInfo method)
		{
			if (Key == null) Key = method.Name;
		}

		protected override void Initialize(System.Reflection.PropertyInfo method)
		{
			if (Key == null) Key = method.Name;
		}

		protected override bool TryBinding(IHttpContext context, out object value)
		{
			return NameValueSerialization.TryDeserialize(context.Request.Form, Key, BindingTargetType, out value);
		}
	}
}
