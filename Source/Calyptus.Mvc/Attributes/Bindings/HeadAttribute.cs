using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.Mvc.Mapping;

namespace Calyptus.Mvc
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class HeadAttribute : BinderBaseAttribute
	{
		public string Key { get; set; }

		private IMapping _verb = new VerbMapping("GET");

		public HeadAttribute()
		{
			Mappings.Add(_verb);
		}

		protected override void Initialize(System.Reflection.MethodInfo method)
		{
		}

		protected override void Initialize(System.Reflection.ParameterInfo method)
		{
		}

		protected override void Initialize(System.Reflection.PropertyInfo method)
		{
			
		}

		protected override bool TryBinding(IHttpContext context, out object value)
		{
			value = null;
			return false;
		}

		protected override void SerializePath(IPathStack path, object value)
		{
		}
	}
}
