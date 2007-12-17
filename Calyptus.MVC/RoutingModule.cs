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
        private static RoutingEngine _engine;

        public void Init(HttpApplication app)
        {
            app.PostMapRequestHandler += new EventHandler(Route);
        }

        void Route(object s, EventArgs e)
        {
            HttpContext context = ((HttpApplication)s).Context;
            HttpRequest request = context.Request;

            if (_engine == null)
            {
                Config config = (Config)ConfigurationManager.GetSection("calyptus.mvc");

                IList app_code = System.Web.Compilation.BuildManager.CodeAssemblies;

                List<Assembly> assemblies = new List<Assembly>(config.Assemblies.Count + app_code.Count);
                foreach (AssemblyInfo a in config.Assemblies)
                    assemblies.Add(System.Reflection.Assembly.Load(a.Assembly));
                foreach (Assembly a in app_code)
                    assemblies.Add(a);

                _engine = new RoutingEngine(assemblies);
            }

            string[] path = request.Url.AbsolutePath.Length <= request.ApplicationPath.Length ? null :
                request.Url.AbsolutePath.Substring(request.ApplicationPath.Length + 1).Split('/');

            if (path != null)
                for (int i = 0; i < path.Length; i++)
                    path[i] = HttpUtility.UrlDecode(path[i]);

            PathStack stack = new PathStack(path, true);

            Assembly ass = (Assembly)System.Web.Compilation.BuildManager.CodeAssemblies[0];
            Type t = ass.GetType("PageController");
            

            object controller = System.Activator.CreateInstance(t, "test");

            //MemberInfo m = t.GetMember(".ctor", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)[0];
            //t.InvokeMember(".ctor", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, new object[] {"name"});

            IHttpHandler handler = new ControllerHandler();
            context.Handler = handler;
        }


        public void Dispose()
        {
        }
    }
}
