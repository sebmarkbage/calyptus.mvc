using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Reflection;

namespace Calyptus.Mvc
{
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

	public interface IParentActionHandler : IHttpHandler
	{
		bool TryBinding(IPathStack path, out IHttpHandler handler);
	}

	internal class HttpParentActionHandler : IParentActionHandler
	{
		public ParentActionHandler Handler;

		public object Controller;
		public object[] Arguments;
		public IHttpContext Context;

		private class RenderableHandler : IHttpHandler
		{
			public ParentActionHandler Handler;
			public IHttpContext Context;
			public object Value;

			public bool IsReusable
			{
				get { throw new NotImplementedException(); }
			}

			public void ProcessRequest(HttpContext context)
			{
				Handler.RenderAction(Context, Value);
			}
		}

		public bool TryBinding(IPathStack path, out IHttpHandler handler)
		{
			object value = Handler.ExecuteAction(Context, Controller, Arguments);
			
			if (value == null)
			{
				handler = null;
				return false;
			}
			else if (value is IRenderable || value is IViewTemplate)
			{
				handler = new RenderableHandler { Context = Context, Handler = Handler, Value = value };
			}
			else if ((handler = Context.Route.RoutingEngine.ParseRoute(Context, path, value)) == null)
			{
				return false;
			}
			return true;
		}

		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			throw new NotImplementedException("The routing engine is suppose to parse this action before returning an IHttpHandler.");
		}
	}

	internal class HttpAsyncActionHandler : IHttpAsyncHandler
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
			EndProcessRequest(BeginProcessRequest(context, null, null));
		}
	}
}
