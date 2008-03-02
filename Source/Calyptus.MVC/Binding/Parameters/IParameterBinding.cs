using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC.Binding
{
	public interface IParameterBinding
	{
		void Initialize(ParameterInfo parameter);
		bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight);
		void SerializePath(IPathStack path, object obj);
	}
}
