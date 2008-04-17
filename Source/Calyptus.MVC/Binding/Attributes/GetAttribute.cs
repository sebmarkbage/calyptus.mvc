﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.MVC.Mapping;

namespace Calyptus.MVC
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class GetAttribute : BinderBaseAttribute
	{
		private IMappingBinding _verb = new VerbMapping("GET");

		public string Key { get; set; }

		public GetAttribute()
		{
			Mappings.Add(_verb);
		}

		public GetAttribute(string queryKey) : base()
		{
			Key = queryKey;
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
				string[] actions = context.Request.QueryString.GetValues("action");
				if (actions != null)
					foreach (string action in actions)
						if (action.Equals(key, StringComparison.InvariantCultureIgnoreCase))
							return true;
				return context.Request.QueryString[key] != null;
			}

			public void SerializeToPath(IPathStack path)
			{
				path.QueryString.Add("action", key);
			}
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
			return NameValueSerialization.TryDeserialize(context.Request.QueryString, Key, BindingTargetType, out value);
		}

		protected override void SerializePath(IPathStack path, object value)
		{
			NameValueSerialization.Serialize(path.QueryString, Key, value);
		}
	}

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class QueryStringAttribute : GetAttribute
	{
	}
}