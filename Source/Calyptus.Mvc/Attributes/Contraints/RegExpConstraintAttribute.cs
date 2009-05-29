using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Calyptus.Mvc
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter, AllowMultiple = true, Inherited = false)]
	public class RegExpConstraintAttribute : Attribute, IBindingConstraint
	{
		private Regex regex;

		public RegExpConstraintAttribute(string pattern)
		{
			this.regex = new Regex(pattern, RegexOptions.Compiled);
		}

		public RegExpConstraintAttribute(string pattern, RegexOptions options)
		{
			this.regex = new Regex(pattern, options | RegexOptions.Compiled);
		}

		public void Initialize(ParameterInfo parameter)
		{
			
		}

		public bool TryConstraint(IHttpContext context, object value)
		{
			return value == null || regex.IsMatch(value.ToString());
		}
	}
}
