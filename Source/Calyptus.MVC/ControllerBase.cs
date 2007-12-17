using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web;

namespace Calyptus.MVC
{
    public abstract class ControllerBase
    {
        private IHttpContext _context;
        public IHttpContext Context { get { if (_context == null) _context = Controller.Context; return _context; } set { _context = value; } }

        private IRoutingEngine _routing;
        public IRoutingEngine Routing { get { if (_routing == null) _routing = Controller.RoutingEngine; return _routing; } set { _routing = value; } }

        private IViewEngine _viewengine;
        public IViewEngine ViewEngine { get { if (_viewengine == null) _viewengine = Controller.ViewEngine; return _viewengine; } set { if (value == null) throw new NullReferenceException("The controller view engine cannot be null."); _viewengine = value; } }

        public virtual void RedirectToAction(Expression<Action> action)
        {
            Context.Response.Redirect(Routing.GetRelativePath(action));
        }

        public virtual string URL(Expression<Action> action)
        {
            return Routing.GetRelativePath(action);
        }

        public virtual void RenderView(string view)
        {
            ViewEngine.RenderView(Context, view);
        }

        public virtual void RenderView(string view, object viewdata)
        {
            ViewEngine.RenderView(Context, view, viewdata);
        }

        public virtual void RenderView(string view, params object[] viewdata)
        {
            ViewEngine.RenderView(Context, view, viewdata);
        }

        public HttpApplicationState Application
        {
            get { return Context.Application; }
        }

        public HttpApplication ApplicationInstance
        {
            get { return Context.ApplicationInstance; }
        }

        public System.Web.Caching.Cache Cache
        {
            get { return Context.Cache; }
        }

        public Exception Error
        {
            get { return Context.Error; }
        }

        public IHttpHandler Handler
        {
            get { return Context.Handler; }
        }

        public System.Collections.IDictionary Items
        {
            get { return Context.Items; }
        }

        public System.Web.Profile.ProfileBase Profile
        {
            get { return Context.Profile; }
        }

        public HttpResponse Response
        {
            get { return Context.Response; }
        }

        public HttpRequest Request
        {
            get { return Context.Request; }
        }

        public HttpServerUtility Server
        {
            get { return Context.Server; }
        }

        public System.Web.SessionState.HttpSessionState Session
        {
            get { return Context.Session; }
        }

        public TraceContext Trace
        {
            get { return Context.Trace; }
        }

        public System.Security.Principal.IPrincipal User
        {
            get { return Context.User; }
        }
    }
}
