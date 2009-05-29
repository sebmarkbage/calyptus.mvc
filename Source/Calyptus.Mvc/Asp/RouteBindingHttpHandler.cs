using System;
using System.Web;
using System.Web.SessionState;

namespace Calyptus.Mvc
{
	internal class RouteBindingHttpHandler : IHttpHandler //, IRequiresSessionState
	{
		private IResultBinding action;

		public RouteBindingHttpHandler(IResultBinding action)
		{
			this.action = action;
		}

		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			this.action.Invoke(new HttpResponseWrapper(context.Response));
		}
	}

	internal class AsyncRouteBindingHttpHandler : RouteBindingHttpHandler, IHttpAsyncHandler
	{
		private IAsyncResultBinding action;

		public AsyncRouteBindingHttpHandler(IAsyncResultBinding action) : base(action)
		{
			this.action = action;
		}

		public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback callback, object state)
		{
			return this.action.BeginInvoke(new HttpResponseWrapper(context.Response), callback, state);
		}

		public void EndProcessRequest(IAsyncResult result)
		{
			this.action.EndInvoke(result);
		}
	}
}
