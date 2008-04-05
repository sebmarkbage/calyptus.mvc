using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

namespace Calyptus.MVC
{
	interface IControllerBinding
	{
		void Initialize(Type controllerType);
		bool TryBinding(IHttpContext context, IPathStack path, out IHttpHandler handler);
		void SerializeToPath(IPathStack path, MethodInfo method, object[] parameters);
	}
}
