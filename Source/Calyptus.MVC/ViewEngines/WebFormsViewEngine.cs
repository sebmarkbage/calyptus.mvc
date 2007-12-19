using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.Web.UI;
using System.Web;

namespace Calyptus.MVC.ViewEngines
{
    public class WebFormsViewEngine : IViewEngine
    {
		private static string[] _masterPatterns = new string[] { "~/Views/{1}/{0}.master", "~/Views/Shared/{0}.master" };
		private static string[] _viewPatterns = new string[] { "~/Views/{1}/{0}.aspx", "~/Views/{1}/{0}.ascx", "~/Views/Shared/{0}.aspx", "~/Views/Shared/{0}.ascx" };

		public void RenderView(IHttpContext context, IRoutingEngine routing, string view)
		{
			RenderView(context, routing, view, (string)null, (object)null);
		}

		public void RenderView(IHttpContext context, IRoutingEngine routing, string view, object data)
		{
			RenderView(context, routing, view, (string)null, data);
		}

		public void RenderView(IHttpContext context, IRoutingEngine routing, string view, string master)
		{
			RenderView(context, routing, view, master, (object) null);
		}

		public void RenderView(IHttpContext context, IRoutingEngine routing, string view, string master, object data)
		{
			Type viewType = null;
			for (int i = 0; i < _viewPatterns.Length; i++)
			{
				viewType = BuildManager.GetCompiledType(String.Format(_viewPatterns[i], view, null));
				if (viewType != null)
					break;
			}

			if (viewType == null)
				throw new Exception(String.Format("View \"{0}\" could not be found.", view));

			IHttpHandler handler = (IHttpHandler)System.Activator.CreateInstance(viewType);
			if (handler is ViewPage)
				((ViewPage)handler).ViewData = data;
			handler.ProcessRequest(context.ApplicationInstance.Context);
		}
	}
}
