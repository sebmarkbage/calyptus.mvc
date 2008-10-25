using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

namespace Calyptus.Mvc
{
	interface IActionBinding
	{
		void Initialize(MethodInfo method);
		bool TryBinding(IHttpContext context, IPathStack path, out object[] parameters, out int overloadWeight);
		void OnRender(IHttpContext context, object value);
		void SerializePath(IPathStack stack, object[] parameters);
		void OnBeforeAction(IHttpContext context, BeforeActionEventArgs args);
		void OnError(IHttpContext context, ErrorEventArgs args);
		void OnAfterAction(IHttpContext context, AfterActionEventArgs args);
	}
}
