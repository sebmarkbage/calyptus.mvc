using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web;

namespace Calyptus.MVC
{
	public abstract class Controller : IController
    {
		private IHttpContext _context;
		public IHttpContext Context { get { return _context; } set { _context = value; } }

        private IRoutingEngine _routing;
        public IRoutingEngine Routing { get { return _routing; } set { _routing = value; } }

        private IViewEngine _viewengine;
        public IViewEngine ViewEngine { get { return _viewengine; } set { if (value == null) throw new NullReferenceException("The controller view engine cannot be null."); _viewengine = value; } }

        protected virtual void RedirectToAction(Expression<Action> action)
        {
            Context.Response.Redirect(Routing.GetRelativePath(action));
        }

		protected virtual string URL(Expression<Action> action)
        {
            return Routing.GetRelativePath(action);
        }

		protected virtual void RenderView(string view)
        {
            ViewEngine.RenderView(Context, Routing, view);
        }

		protected virtual void RenderView(string view, object viewdata)
        {
            ViewEngine.RenderView(Context, Routing, view, viewdata);
        }

		protected virtual void RenderView(string view, string master)
		{
			ViewEngine.RenderView(Context, Routing, view, master);
		}

		protected virtual void RenderView(string view, string master, object viewdata)
		{
			ViewEngine.RenderView(Context, Routing, view, master, viewdata);
		}

		protected HttpApplicationState Application
        {
            get { return Context.Application; }
        }

		protected HttpApplication ApplicationInstance
        {
            get { return Context.ApplicationInstance; }
        }

		protected System.Web.Caching.Cache Cache
        {
            get { return Context.Cache; }
        }

		protected Exception Error
        {
            get { return Context.Error; }
        }

		protected IHttpHandler Handler
        {
            get { return Context.Handler; }
        }

		protected System.Collections.IDictionary Items
        {
            get { return Context.Items; }
        }

		protected System.Web.Profile.ProfileBase Profile
        {
            get { return Context.Profile; }
        }

		protected HttpResponse Response
        {
            get { return Context.Response; }
        }

		protected HttpRequest Request
        {
            get { return Context.Request; }
        }

		protected HttpServerUtility Server
        {
            get { return Context.Server; }
        }

		protected System.Web.SessionState.HttpSessionState Session
        {
            get { return Context.Session; }
        }

		protected TraceContext Trace
        {
            get { return Context.Trace; }
        }

		protected System.Security.Principal.IPrincipal User
        {
            get { return Context.User; }
        }
    }
}
