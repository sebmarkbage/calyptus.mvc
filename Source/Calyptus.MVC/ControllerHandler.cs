using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using Calyptus.MVC.Internal;
using System.Reflection;

namespace Calyptus.MVC
{
    public class ControllerHandler : IHttpHandler, IRequiresSessionState
    {
		internal PathStack Stack;
		internal RoutingEngine Engine;

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
			context.Response.Write("Process Request");

			Type controllerType;
			MethodInfo method;
			object[] arguments;
			
			object controller = Activator.CreateInstance(controllerType);
			object returnValue = method.Invoke(controller, arguments);

			while (returnValue is IController)
			{
				returnValue = null;
			}

			// Create controller
			// Execute controller

			// If controller returns another controller execute that one with the remaining stack
		}
	}
}
