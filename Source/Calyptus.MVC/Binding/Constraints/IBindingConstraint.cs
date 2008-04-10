using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC
{
	public interface IBindingConstraint
	{
		void Initialize(ParameterInfo parameter);
		bool TryConstraint(IHttpContext context, object value);
	}
}
