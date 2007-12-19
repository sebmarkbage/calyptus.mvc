using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Calyptus.MVC.Extensions;

namespace Calyptus.MVC.RoutingEngines
{
	internal class RoutingCommand
	{
		public IExtension[] ControllerExtensions;
		public IExtension[] ActionExtensions;
		public MethodInfo Method;
		public object[] Arguments;
		public bool IsNestable;
	}
}
