using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Reflection;
using Calyptus.MVC.RoutingEngines;
using Calyptus.MVC.Extensions;

namespace Calyptus.MVC.RoutingEngines
{
    public class ControllerHandler : IHttpHandler, IRequiresSessionState
    {
		internal IPathStack Stack;
		internal RoutingEngine Engine;
		internal Type ControllerType;
		internal RoutingCommand Command;

        public ControllerHandler()
        {
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
			ProcessRequest(new VirtualHttpContext(context));
        }

		public void ProcessRequest(IHttpContext context)
		{
			object controller = Activator.CreateInstance(ControllerType);
			while (controller != null)
			{
				if (controller is IController)
				{
					((IController)controller).Context = context;
					((IController)controller).Routing = Engine;
					((IController)controller).ViewEngine = Configuration.Config.GetDefaultViewEngine();
				}

				if (Command.ControllerExtensions != null)
					foreach (IExtension ext in Command.ControllerExtensions)
						ext.OnPreAction(context);
				if (Command.ActionExtensions != null)
					foreach (IExtension ext in Command.ActionExtensions)
						ext.OnPreAction(context);

				object returnValue;
				try
				{
					returnValue = Command.Method.Invoke(controller, Command.Arguments);
				}
				catch(Exception e)
				{
					if (Command.ControllerExtensions != null)
					{
						bool catched = false;
						foreach (IExtension ext in Command.ActionExtensions)
							if (!ext.OnError(context, e))
								catched = true;
						foreach (IExtension ext in Command.ControllerExtensions)
							if (!ext.OnError(context, e))
								catched = true;
						if (!catched)
							throw e;
					}
					break;
				}

				if (Command.ActionExtensions != null)
					foreach (IExtension ext in Command.ActionExtensions)
						ext.OnPostAction(context);
				if (Command.ControllerExtensions != null)
					foreach (IExtension ext in Command.ControllerExtensions)
						ext.OnPostAction(context);
	
				if (returnValue != null && Command.IsNestable)
				{
					if (Engine.TryParseChildRoute(returnValue.GetType(), context, Stack, out Command))
						controller = returnValue;
					else
						throw new HttpException(404, String.Format("Action not found on controller \"{0}\".", returnValue.GetType()));
				}
				else
					break;
			}
		}
	}
}
