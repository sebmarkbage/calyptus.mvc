using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

namespace Calyptus.MVC.Binding
{
	interface IActionBinding
	{
		void Initialize(MethodInfo method);
		bool TryBinding(IHttpContext context, IPathStack path, out object[] parameters, out int overloadWeight);
		void OnPreAction(IHttpContext context, object[] parameters);
		void OnPostAction(IHttpContext context, object returnValue);
		void SerializePath(IPathStack stack, object[] parameters);
		int MappingWeight { get; }
	}
}
