using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Reflection;

namespace Calyptus.MVC
{
	// AsyncEndDelegate
	// OnBeforeActionDelegate
	// OnAfterActionDelegate

	internal class HttpActionHandler : IHttpHandler, IRequiresSessionState
	{
		public ActionHandler Handler;

		public object Controller;
		public object[] Arguments;
		public IHttpContext Context;

		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			object value = Handler.ExecuteAction(Context, Controller, Arguments);
			Handler.RenderAction(Context, value);
		}
	}

	internal class HttpAsyncActionHandler :  IHttpAsyncHandler
	{
		public AsyncActionHandler Handler;

		public object Controller;
		public object[] Arguments;
		public IHttpContext Context;

		public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback callback, object state)
		{
			Arguments[Arguments.Length - 2] = callback;
			Arguments[Arguments.Length - 1] = state;

			object value = Handler.ExecuteAction(Context, Controller, Arguments);
			if (!(value is IAsyncResult))
			{
				Handler.RenderAction(Context, value);
				context.Response.End(); // TODO: How to finish off non Async?
				return null;
			}
			else
				return (IAsyncResult)value;
		}

		public void EndProcessRequest(IAsyncResult result)
		{
			object value = Handler.ExecuteEndAction(Context, Controller, Arguments, result);
			Handler.RenderAction(Context, value);
		}

		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			throw new NotImplementedException();
		}
	}
}
