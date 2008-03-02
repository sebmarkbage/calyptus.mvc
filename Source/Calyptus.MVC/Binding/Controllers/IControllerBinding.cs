using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace Calyptus.MVC.Binding
{
	interface IControllerBinding
	{
		void Initialize(Type controllerType);
		bool TryBinding(IHttpContext context, IPathStack path, out IHttpHandler handler);
		void SerializeToPath(IPathStack path, MethodInfo method, object[] arguments);
	}
}
