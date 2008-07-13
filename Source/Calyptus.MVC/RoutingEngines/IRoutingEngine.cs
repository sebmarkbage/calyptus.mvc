using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Linq.Expressions;
using System.Reflection;

namespace Calyptus.MVC
{
    public interface IRoutingEngine
    {
		IHttpHandler ParseRoute(IHttpContext context, IPathStack path);
		IHttpHandler ParseRoute(IHttpContext context, IPathStack path, object controller);
		void SerializeAbsoutePath(IRouteAction action, IPathStack path);
		void SerializeRelativePath(IRouteAction action, IPathStack path);
	}
}
