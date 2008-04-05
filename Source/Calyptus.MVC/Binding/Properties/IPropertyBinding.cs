using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC
{
	interface IPropertyBinding
	{
		void Initialize(PropertyInfo property);

		bool TryBinding(IHttpContext context, out object value);
		void StoreBinding(IHttpContext context, object value);
	}
}
