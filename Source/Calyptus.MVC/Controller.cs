using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web;
using Calyptus.MVC.Binding;

namespace Calyptus.MVC
{
	public abstract class Controller
    {
		private IHttpContext _context;
		[ContextBinding]
		public IHttpContext Context { get { return _context; } set { _context = value; } }

        private IRoutingEngine _routing;
        public IRoutingEngine Routing { get { return _routing; } set { _routing = value; } }

        private IViewEngine _viewengine;
        public IViewEngine ViewEngine { get { return _viewengine; } set { if (value == null) throw new NullReferenceException("The controller view engine cannot be null."); _viewengine = value; } }

		protected virtual void Redirect<T>(Expression<Action<T>> action)
		{
			Context.Response.Redirect(URL<T>(action));
		}

		protected virtual void Redirect<T>(int index, Expression<Action<T>> action)
		{
			Context.Response.Redirect(URL<T>(index, action));
		}

		protected virtual void RedirectReplace<T>(Expression<Action<T>> action)
		{
			Context.Response.Redirect(URLReplace<T>(action));
		}

		protected virtual void RedirectReplace<T>(int index, Expression<Action<T>> action)
		{
			Context.Response.Redirect(URLReplace<T>(index, action));
		}

		protected virtual void RedirectAbsolute<T>(Expression<Action<T>> action)
		{
			Context.Response.Redirect(URLAbsolute<T>(action));
		}

		protected virtual string URL<T>(Expression<Action<T>> action)
        {
            return Routing.GetRelativePath<T>(action);
        }

		protected virtual string URL<T>(int index, Expression<Action<T>> action)
        {
            return Routing.GetRelativePath<T>(index, action);
        }

		protected virtual string URLReplace<T>(Expression<Action<T>> action)
		{
			return Routing.GetReplacementPath<T>(action);
		}

		protected virtual string URLReplace<T>(int index, Expression<Action<T>> action)
        {
            return Routing.GetReplacementPath<T>(index, action);
        }

		protected virtual string URLAbsolute<T>(Expression<Action<T>> action)
		{
			return Routing.GetAbsolutePath<T>(action);
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
