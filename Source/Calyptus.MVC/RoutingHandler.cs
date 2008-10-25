using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Calyptus.Mvc
{
	public class RoutingHandler : IHttpHandlerFactory
	{
		public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
		{
			return RoutingModule.GetHandler(context);
		}

		public void ReleaseHandler(IHttpHandler handler)
		{
		}
	}
}
