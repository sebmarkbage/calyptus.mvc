using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.Mvc
{
	public class RouteAction : IRouteAction
	{
		public Type ControllerType { get; internal set; }

		public MethodInfo Method { get; private set; }

		public object[] Parameters { get; private set; }

		public IRouteAction ChildAction { get; private set; }

		public RouteAction(Type controllerType, MethodInfo method, object[] parameters, RouteAction childAction)
		{
			ControllerType = controllerType;
			Method = method;
			Parameters = parameters;
			ChildAction = childAction;
		}
	}
}
