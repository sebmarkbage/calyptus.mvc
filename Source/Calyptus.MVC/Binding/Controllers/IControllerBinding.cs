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
		bool TryBinding(IHttpContext context, IPathStack path, object parentController, out IHttpHandler handler);
		void SerializeToPath(IRouteAction action, IPathStack path);
	}
}
