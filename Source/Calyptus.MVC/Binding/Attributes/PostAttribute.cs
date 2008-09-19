using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.MVC.Mapping;
using System.Web;

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

		private class KeyMapping : IMappingBinding
		{
			private string key;

			public KeyMapping(string key)
			{
				this.key = key;
			}

			public bool TryMapping(IHttpContext context, IPathStack path)
			{
				string[] actions = context.Request.Form.GetValues("action");
				if (actions != null)
					foreach (string action in actions)
						if (action.Equals(key, StringComparison.InvariantCultureIgnoreCase))
							return true;
				return context.Request.Form[key] != null;
			}

			public void SerializeToPath(IPathStack path) { }
		}

		protected override void Initialize(System.Reflection.MethodInfo method)
		{
			if (Key != null) Mappings.Add(new KeyMapping(Key));
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
			if (BindingTargetType == typeof(HttpPostedFile))
			{
				HttpPostedFile file = context.Request.Files[Key];
				if (file.ContentLength > 0)
				{
					value = file;
					return true;
				}
				else
				{
					value = null;
					return false;
				}
			}
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

		private class KeyMapping : IMappingBinding
		{
			private string key;

			public KeyMapping(string key)
			{
				this.key = key;
			}

			public bool TryMapping(IHttpContext context, IPathStack path)
			{
				string[] actions = context.Request.Form.GetValues("action");
				if (actions != null)
					foreach (string action in actions)
						if (action.Equals(key, StringComparison.InvariantCultureIgnoreCase))
							return true;
				return context.Request.Form[key] != null;
			}

			public void SerializeToPath(IPathStack path) { }
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
			if (BindingTargetType == typeof(HttpPostedFile))
			{
				HttpPostedFile file = context.Request.Files[Key];
				if (file.ContentLength > 0)
				{
					value = file;
					return true;
				}
				else
				{
					value = null;
					return false;
				}
			}
			return NameValueSerialization.TryDeserialize(context.Request.Form, Key, BindingTargetType, out value);
		}
	}
}
