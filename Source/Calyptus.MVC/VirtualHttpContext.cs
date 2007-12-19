using System;
using System.Web;

namespace Calyptus.MVC
{
    internal class VirtualHttpContext : IHttpContext
    {
        private HttpContext context;

        public VirtualHttpContext(HttpContext context)
        {
            this.context = context;
        }

        public HttpApplicationState Application
        {
            get { return context.Application; }
        }

        public HttpApplication ApplicationInstance
        {
            get { return context.ApplicationInstance; }
        }

        public System.Web.Caching.Cache Cache
        {
            get { return context.Cache; }
        }

        public Exception Error
        {
            get { return context.Error; }
        }

        public IHttpHandler Handler
        {
            get { return context.Handler; }
        }

        public System.Collections.IDictionary Items
        {
            get { return context.Items; }
        }

        public System.Web.Profile.ProfileBase Profile
        {
            get { return context.Profile; }
        }

        public HttpResponse Response
        {
            get { return context.Response; }
        }

        public HttpRequest Request
        {
            get { return context.Request; }
        }

        public HttpServerUtility Server
        {
            get { return context.Server; }
        }

        public System.Web.SessionState.HttpSessionState Session
        {
            get { return context.Session; }
        }

        public TraceContext Trace
        {
            get { return context.Trace; }
        }

        public System.Security.Principal.IPrincipal User
        {
            get { return context.User; }
        }
    }
}
