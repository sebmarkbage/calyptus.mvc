using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC.Binding
{
	interface IPropertyBinding
	{
		void Initialize(PropertyInfo property);
		bool TryBinding(IHttpContext context, out object obj);
		void OnPreAction(IHttpContext context, object value);
		void OnPostAction(IHttpContext context, object value);
	}
}
