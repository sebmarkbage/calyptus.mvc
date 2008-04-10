using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace Calyptus.MVC
{
	interface IEntryControllerBinding : IControllerBinding
	{
		bool TryBinding(IHttpContext context, IPathStack path, out IHttpHandler handler);
		new void SerializeToPath(IRouteAction action, IPathStack path);
	}
}
