using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Calyptus.Mvc
{
	public class NotChanged : Exception, IRenderable
	{
		void IRenderable.Render(IHttpContext context)
		{
			IHttpResponse response = context.Response;
			response.Clear();
			response.Cache.SetCacheability(HttpCacheability.NoCache);
			response.StatusCode = 304;
		}
	}
}
