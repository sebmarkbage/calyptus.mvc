using System;

namespace Calyptus.Mvc.Mapping
{
	public class ControllerBindingBase : IInstanceMapping
	{
		public ControllerBindingBase(Type controllerClass, IActionMapping[] actionBindings)
		{

		}

		public bool TryBinding(IHttpContext context, IPathStack path, out IResultBinding action)
		{
			throw new NotImplementedException();
		}

		public Type ControllerType
		{
			get { throw new NotImplementedException(); }
		}
	}
}
