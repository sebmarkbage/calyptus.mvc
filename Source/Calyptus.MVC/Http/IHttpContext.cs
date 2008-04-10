using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Security.Principal;
using System.Web.Caching;
using System.Web.Profile;

namespace Calyptus.MVC
{
	public interface IHttpContext : IServiceProvider
	{
		void AddError(Exception errorInfo);
		void ClearError();
		object GetSection(string sectionName);
		Exception[] AllErrors { get; }
		HttpApplicationState Application { get; }
		HttpApplication ApplicationInstance { get; set; }
		Cache Cache { get; }
		IHttpHandler CurrentHandler { get; }
		RequestNotification CurrentNotification { get; }
		Exception Error { get; }
		IHttpHandler Handler { get; set; }
		bool IsCustomErrorEnabled { get; }
		bool IsDebuggingEnabled { get; }
		bool IsPostNotification { get; }
		IDictionary Items { get; }
		IHttpHandler PreviousHandler { get; }
		ProfileBase Profile { get; }
		IHttpRequest Request { get; }
		IHttpResponse Response { get; }
		HttpServerUtility Server { get; }
		IHttpSessionState Session { get; }
		bool SkipAuthorization { get; set; }
		DateTime Timestamp { get; }
		TraceContext Trace { get; }
		IPrincipal User { get; set; }

		IViewFactory ViewFactory { get; }
		IRouteContext Route { get; }
	}
}
