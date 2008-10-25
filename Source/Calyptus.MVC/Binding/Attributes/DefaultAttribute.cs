using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.Mvc
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class DefaultAttribute : BinderBaseAttribute
	{
		protected override Type DefaultParameterBinderType
		{
			get
			{
				return typeof(GetAttribute);
			}
		}

		protected override bool TryBinding(IHttpContext context, out object value)
		{
			throw new Exception("DefaultAttribute is suppose to be replaced by the Action's default implementation.");
		}
	}
}
