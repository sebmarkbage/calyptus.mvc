using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC.Binding
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
	public class ContextBinding : Attribute, IPropertyBinding
	{
		public void Initialize(PropertyInfo property)
		{
		}

		public bool TryBinding(IHttpContext context, out object obj)
		{
			obj = context;
			return true;
		}

		public void OnPreAction(IHttpContext context, object value)
		{
		}

		public void OnPostAction(IHttpContext context, object value)
		{
		}
	}
}
