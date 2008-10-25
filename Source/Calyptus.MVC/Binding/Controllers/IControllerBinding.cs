using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

namespace Calyptus.Mvc
{
	interface IControllerBinding
	{
		void Initialize(Type controllerType);
		bool TryBinding(IHttpContext context, IPathStack path, object controller, out IHttpHandler handler);
		void SerializeToPath(IRouteAction action, IPathStack path);
	}
}
