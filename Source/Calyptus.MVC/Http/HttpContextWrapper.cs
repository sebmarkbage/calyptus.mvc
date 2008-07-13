using System;
using System.Web;
using System.Web.Profile;
using System.Security.Principal;
using System.Web.Caching;
using System.Collections;
using System.Web.SessionState;

namespace Calyptus.MVC
{
	internal class HttpContextWrapper : IHttpContext
	{
		private HttpContext _context;
		private IHttpRequest _request;
		private IHttpResponse _response;
		private IHttpSessionState _session;

		public HttpContextWrapper(HttpContext context, IRouteContext route, IViewFactory viewFactory)
		{
			this._context = context;
			this._request = new HttpRequestWrapper(context.Request);
			this._response = new HttpResponseWrapper(context.Response);
			this._session = new HttpSessionStateWrapper(context.Session);
			this.Route = route;
			this.ViewFactory = viewFactory;
		}

		object IServiceProvider.GetService(Type serviceType)
		{
			return ((IServiceProvider)this._context).GetService(serviceType);
		}

		void IHttpContext.AddError(Exception errorInfo)
		{
			this._context.AddError(errorInfo);
		}

		void IHttpContext.ClearError()
		{
			this._context.ClearError();
		}

		object IHttpContext.GetSection(string sectionName)
		{
			return this._context.GetSection(sectionName);
		}

		Exception[] IHttpContext.AllErrors
		{
			get
			{
				return this._context.AllErrors;
			}
		}

		HttpApplicationState IHttpContext.Application
		{
			get
			{
				return this._context.Application;
			}
		}

		HttpApplication IHttpContext.ApplicationInstance
		{
			get
			{
				return this._context.ApplicationInstance;
			}
			set
			{
				this._context.ApplicationInstance = value;
			}
		}

		Cache IHttpContext.Cache
		{
			get
			{
				return this._context.Cache;
			}
		}

		IHttpHandler IHttpContext.CurrentHandler
		{
			get
			{
				return this._context.CurrentHandler;
			}
		}

		RequestNotification IHttpContext.CurrentNotification
		{
			get
			{
				return this._context.CurrentNotification;
			}
		}

		Exception IHttpContext.Error
		{
			get
			{
				return this._context.Error;
			}
		}

		IHttpHandler IHttpContext.Handler
		{
			get
			{
				return this._context.Handler;
			}
			set
			{
				this._context.Handler = value;
			}
		}

		bool IHttpContext.IsCustomErrorEnabled
		{
			get
			{
				return this._context.IsCustomErrorEnabled;
			}
		}

		bool IHttpContext.IsDebuggingEnabled
		{
			get
			{
				return this._context.IsDebuggingEnabled;
			}
		}

		bool IHttpContext.IsPostNotification
		{
			get
			{
				return this._context.IsDebuggingEnabled;
			}
		}

		IDictionary IHttpContext.Items
		{
			get
			{
				return this._context.Items;
			}
		}

		IHttpHandler IHttpContext.PreviousHandler
		{
			get
			{
				return this._context.PreviousHandler;
			}
		}

		ProfileBase IHttpContext.Profile
		{
			get
			{
				return this._context.Profile;
			}
		}

		IHttpRequest IHttpContext.Request
		{
			get
			{
				return _request;
			}
		}

		IHttpResponse IHttpContext.Response
		{
			get
			{
				return _response;
			}
		}

		HttpServerUtility IHttpContext.Server
		{
			get
			{
				return this._context.Server;
			}
		}

		IHttpSessionState IHttpContext.Session
		{
			get
			{
				return _session;
			}
		}

		bool IHttpContext.SkipAuthorization
		{
			get
			{
				return this._context.SkipAuthorization;
			}
			set
			{
				this._context.SkipAuthorization = value;
			}
		}

		DateTime IHttpContext.Timestamp
		{
			get
			{
				return this._context.Timestamp;
			}
		}

		TraceContext IHttpContext.Trace
		{
			get
			{
				return this._context.Trace;
			}
		}

		IPrincipal IHttpContext.User
		{
			get
			{
				return this._context.User;
			}
			set
			{
				this._context.User = value;
			}
		}

		public IRoutingEngine RoutingEngine { get { return Route.RoutingEngine; } }
		public IRouteContext Route { get; private set; }

		public IViewFactory ViewFactory { get; private set; }
	}
}
