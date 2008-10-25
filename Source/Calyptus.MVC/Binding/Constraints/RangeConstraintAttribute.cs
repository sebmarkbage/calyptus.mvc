using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.Mvc
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter, AllowMultiple = true, Inherited = false)]
	public class RangeConstraintAttribute : Attribute, IBindingConstraint
	{
		public IComparable Min { get; set; }
		public IComparable Max { get; set; }

		public RangeConstraintAttribute()
		{
		}

		public RangeConstraintAttribute(IComparable min, IComparable max)
		{
			Min = min;
			Max = max;
		}

		public void Initialize(ParameterInfo parameter)
		{
		}

		public bool TryConstraint(IHttpContext context, object value)
		{
			return Min.CompareTo(value) < 0 && Max.CompareTo(value) > 0;
		}
	}
}
