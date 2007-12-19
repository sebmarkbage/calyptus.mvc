using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using Calyptus.MVC.Configuration;
using System.Web.Configuration;
using System.Configuration;
using System.Collections;

namespace Calyptus.MVC
{
    public class RoutingModule : IHttpModule
    {
		private static object _requestDataKey = new object();
		//private static string[] _defaultDocuments = new string[] { "/Default.aspx" };
		private static string defaultDocument = "/Default.aspx";

        public void Init(HttpApplication app)
        {
			app.PostResolveRequestCache += new EventHandler(Route);
			app.PostMapRequestHandler += new EventHandler(Reset);
        }

        void Route(object s, EventArgs e)
        {
            HttpContext context = ((HttpApplication)s).Context;
            HttpRequest request = context.Request;

			string appRelPath = request.AppRelativeCurrentExecutionFilePath;

			int l = appRelPath.Length - 2;
			//foreach(string defaultDocument in _defaultDocuments)
				if (appRelPath.EndsWith(defaultDocument, StringComparison.InvariantCultureIgnoreCase))
				{
					l -= defaultDocument.Length;
					//break;
				}

			string[] path = ((l <= 0 ? null : request.AppRelativeCurrentExecutionFilePath.Substring(2, l)) + request.PathInfo).Split('/');

            IPathStack stack = new PathStack(path, context.Request.QueryString, true);

			IHttpHandler handler = Config.GetRoutingEngine().ParseRoute(new VirtualHttpContext(context), stack);
			if (handler != null)
			{
				context.Items[_requestDataKey] = new RequestData { Handler = handler, OriginalPath = context.Request.Path };
				context.RewritePath("~/Calyptus.MVC.axd");
			}
        }

		void Reset(object s, EventArgs e)
		{
            HttpContext context = ((HttpApplication)s).Context;
			RequestData data = (RequestData) context.Items[_requestDataKey];
			if (data != null)
			{
				context.Items.Remove(_requestDataKey);
				context.RewritePath(data.OriginalPath);
				context.Handler = data.Handler;
			}
		}

        public void Dispose()
        {
        }

		private class RequestData
		{
			public string OriginalPath;
			public IHttpHandler Handler;
		}
    }
}
