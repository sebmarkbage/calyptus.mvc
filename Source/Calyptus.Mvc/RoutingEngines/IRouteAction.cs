using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.Mvc
{
	public interface IRouteAction
	{
		Type ControllerType { get; }
		MethodInfo Method { get; }
		object[] Parameters { get; }
		IRouteAction ChildAction { get; }
	}
}
