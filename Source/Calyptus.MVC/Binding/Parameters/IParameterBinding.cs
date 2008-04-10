using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC
{
	public interface IParameterBinding
	{
		void Initialize(ParameterInfo parameter);
		bool TryBinding(IHttpContext context, IPathStack path, out object value, out int overloadWeight);
		void SerializePath(IPathStack path, object value);
		void StoreBinding(IHttpContext context, object value);
	}
}
