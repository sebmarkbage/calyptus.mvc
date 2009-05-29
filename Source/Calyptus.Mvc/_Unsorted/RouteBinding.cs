using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.Mvc
{
	public class RouteAction : IResultBinding
	{
		public Type ControllerType { get; internal set; }

		public MethodInfo Method { get; private set; }

		public object[] Parameters { get; private set; }

		public IResultBinding ParentAction { get; private set; }

		public RouteAction(Type controllerType, MethodInfo method, object[] parameters, RouteAction parentAction)
		{
			ControllerType = controllerType;
			Method = method;
			Parameters = parameters;
			ParentAction = parentAction;
		}

		public object[] Invoke()
		{
			throw new NotImplementedException();
		}

		public IResponseEngine ResultRenderer
		{
			get { throw new NotImplementedException(); }
		}

		public IResponseEngine ExceptionRenderer
		{
			get { throw new NotImplementedException(); }
		}
	}

	public class AsyncRouteAction : RouteAction, IAsyncResultBinding
	{
		public MethodInfo EndMethod { get; private set; }

		public AsyncRouteAction(Type controllerType, MethodInfo method, MethodInfo endMethod, object[] parameters, RouteAction parentAction) : base(controllerType, method, parameters, parentAction)
		{
			EndMethod = endMethod;
		}

		public IAsyncResult BeginInvoke(AsyncCallback callback, object state)
		{
			throw new NotImplementedException();
		}

		public object[] EndInvoke(IAsyncResult result)
		{
			throw new NotImplementedException();
		}
	}

}
