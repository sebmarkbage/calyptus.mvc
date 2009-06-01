using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.Mvc.Mapping;

namespace Calyptus.Mvc
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class QueryStringAttribute : BinderBaseAttribute
	{
		public string Key { get; set; }
		
		public QueryStringAttribute() { }
		
		public QueryStringAttribute(string queryKey) : this()
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
			return SerializationHelper.TryDeserialize(context.Request.QueryString, Key, BindingTargetType, out value);
		}

		protected override void SerializePath(IPathStack path, object value)
		{
			if (value == null && !Optional) throw new BindingException("Cannot serialize null value. Value is not Optional.");
			SerializationHelper.Serialize(value, path.QueryString, Key);
		}
	}

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class GetAttribute : QueryStringAttribute
	{
		private IMappingBinding _verb = new VerbMapping("GET");

		public GetAttribute() : base()
		{
			Mappings.Add(_verb);
		}

		public GetAttribute(string queryKey) : base(queryKey)
		{
			Mappings.Add(_verb);
		}
	}
}
