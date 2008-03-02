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

		public HttpContextWrapper(HttpContext context)
		{
			this._context = context;
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

		void IHttpContext.RewritePath(string path)
		{
			this._context.RewritePath(path);
		}

		void IHttpContext.RewritePath(string path, bool rebaseClientPath)
		{
			this._context.RewritePath(path, rebaseClientPath);
		}

		void IHttpContext.RewritePath(string filePath, string pathInfo, string queryString)
		{
			this._context.RewritePath(filePath, pathInfo, queryString);
		}

		void IHttpContext.RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath)
		{
			this._context.RewritePath(filePath, pathInfo, queryString, setClientFilePath);
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
				return new HttpRequestWrapper(this._context.Request);
			}
		}

		IHttpResponse IHttpContext.Response
		{
			get
			{
				return new HttpResponseWrapper(this._context.Response);
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
				return new HttpSessionStateWrapper(this._context.Session);
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
	}
}
