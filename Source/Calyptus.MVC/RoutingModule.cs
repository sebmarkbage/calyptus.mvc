using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using Calyptus.Mvc.Configuration;
using System.Web.Configuration;
using System.Configuration;
using System.Collections;
using System.IO;

namespace Calyptus.Mvc
{
    public class RoutingModule : IHttpModule
    {
		private static object _requestDataKey = new object();
		//private static string[] _defaultDocuments = new string[] { "/Default.aspx" };
		private static string defaultDocument = "/Default.aspx";

        public void Init(HttpApplication app)
        {
			app.PostResolveRequestCache += new EventHandler(Route);
			app.EndRequest += new EventHandler(CleanUp);
        }

		private static string[] _ignorePaths;

		private bool IgnorePath(HttpRequest request)
		{
			if (_ignorePaths == null)
			{
				DirectoryInfo dir = new DirectoryInfo(request.MapPath("~/"));
				List<string> paths = new List<string>();
				foreach (FileInfo file in dir.GetFiles())
					if (!file.Name.Equals("web.config", StringComparison.InvariantCultureIgnoreCase) && !file.Name.Equals("default.aspx", StringComparison.InvariantCultureIgnoreCase))
						paths.Add(file.Name);
				foreach (DirectoryInfo d in dir.GetDirectories())
					if (!d.Name.Equals("App_Data", StringComparison.InvariantCultureIgnoreCase) && !d.Name.Equals("App_Code", StringComparison.InvariantCultureIgnoreCase))
						paths.Add(d.Name);
				_ignorePaths = paths.ToArray();
			}
			string p = request.AppRelativeCurrentExecutionFilePath;
			int i = p.IndexOf('/', 2);
			p = i < 0 ? p.Substring(2) : p.Substring(2, i - 2);
			return _ignorePaths.Contains(p, StringComparer.InvariantCultureIgnoreCase);
		}

        void Route(object s, EventArgs e)
        {
            HttpContext context = ((HttpApplication)s).Context;
            HttpRequest request = context.Request;

			if (IgnorePath(request)) return;

			string appRelPath = request.AppRelativeCurrentExecutionFilePath;

			int l = appRelPath.Length - 2;
			//foreach(string defaultDocument in _defaultDocuments)
				if (appRelPath.EndsWith(defaultDocument, StringComparison.InvariantCultureIgnoreCase))
				{
					l -= defaultDocument.Length;
					//break;
				}

			string path = ((l <= 0 ? null : request.AppRelativeCurrentExecutionFilePath.Substring(2, l)) + request.PathInfo);

            PathStack stack = new PathStack(request.HttpMethod, path, context.Request.QueryString, true);

			IRoutingEngine routing = Config.GetRoutingEngine();
			IViewFactory viewFactory = Config.GetViewFactory();
			IRouteContext route = new RouteContext(routing, request.ApplicationPath, stack.Count, stack.TrailingSlash);

			IHttpHandler handler = routing.ParseRoute(new HttpContextWrapper(context, route, viewFactory), stack);
			if (handler != null)
			{
				context.Items[_requestDataKey] = new RequestData { Handler = handler, OriginalPath = context.Request.Path, Route = route };
				context.RewritePath("~/Calyptus.Mvc.axd");
			}
			else
				route.Dispose();
        }

		void CleanUp(object s, EventArgs e)
		{
			HttpContext context = ((HttpApplication)s).Context;
			RequestData data = (RequestData)context.Items[_requestDataKey];
			if (data != null)
			{
				data.Route.Dispose();
				context.Items.Remove(_requestDataKey);
			}
		}

        public void Dispose()
        {
        }

		internal static IHttpHandler GetHandler(HttpContext context)
		{
			RequestData data = (RequestData) context.Items[_requestDataKey];
			if (data != null)
			{
				context.RewritePath(data.OriginalPath);
				return data.Handler;
			}
			else
				return null;
		}

		private class RequestData
		{
			public string OriginalPath;
			public IHttpHandler Handler;
			public IRouteContext Route;
		}
    }
}
