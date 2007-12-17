using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using Calyptus.MVC.Internal;
using Calyptus.MVC.Configuration;
using System.Web.Configuration;
using System.Configuration;
using System.Collections;

namespace Calyptus.MVC
{
    public class RoutingModule : IHttpModule
    {
        private static IRoutingEngine _engine;
		private static object _routingEngineLock = new object();
		private static object _requestDataKey = new object();

        public void Init(HttpApplication app)
        {
			app.PostResolveRequestCache += new EventHandler(Route);
			app.PostMapRequestHandler += new EventHandler(Reset);
        }

        void Route(object s, EventArgs e)
        {
            HttpContext context = ((HttpApplication)s).Context;
            HttpRequest request = context.Request;

            if (_engine == null)
            {
				lock (_routingEngineLock)
				{
					Config config = (Config)ConfigurationManager.GetSection("calyptus.mvc");
					if (string.IsNullOrEmpty(config.RoutingEngine))
						_engine = new RoutingEngine();
					else
					{
						Type t = Type.GetType(config.RoutingEngine);
						if (t == null)
							throw new ConfigurationErrorsException(String.Format("Routing engine \"{0}\" could not be found.", config.RoutingEngine));
						if (!typeof(IRoutingEngine).IsAssignableFrom(t))
							throw new ConfigurationErrorsException(String.Format("Routing engine \"{0}\" is not an IRoutingEngine.", config.RoutingEngine));
						_engine = (IRoutingEngine)Activator.CreateInstance(t);
					}
				}
            }

			string[] path = (request.AppRelativeCurrentExecutionFilePath.Substring(2) + request.PathInfo).Split('/');

            if (path != null)
                for (int i = 0; i < path.Length; i++)
                    path[i] = HttpUtility.UrlDecode(path[i]);

            PathStack stack = new PathStack(path, true);

			IHttpHandler handler = _engine.ParseRoute(stack);
			if (handler != null)
			{
				context.Items[_requestDataKey] = new RequestData { Handler = handler, OriginalPath = context.Request.Path };
				context.RewritePath("~/Calyptus.MVC.axd");
			}

            //Assembly ass = (Assembly)System.Web.Compilation.BuildManager.CodeAssemblies[0];
            //Type t = ass.GetType("PageController");
            
            //object controller = System.Activator.CreateInstance(t, "test");

            //MemberInfo m = t.GetMember(".ctor", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)[0];
            //t.InvokeMember(".ctor", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, new object[] {"name"});

            /*IHttpHandler handler = new ControllerHandler();
            context.Handler = handler;*/

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
