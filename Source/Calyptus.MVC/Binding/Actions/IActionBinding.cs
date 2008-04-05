using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

namespace Calyptus.MVC
{
	interface IActionBinding
	{
		void Initialize(MethodInfo method);
		bool TryBinding(IHttpContext context, IPathStack path, out object[] parameters, out int overloadWeight);
		void SerializePath(IPathStack stack, object[] parameters);
		void OnBeforeAction(IHttpContext context, object[] parameters);
		bool OnError(IHttpContext context, Exception error);
		void OnAfterAction(IHttpContext context, object returnValue);
		void OnRender(IHttpContext context, object value);
	}
}
