using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.MVC.RoutingEngines;
using System.Web;

namespace Calyptus.MVC.Binding
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class ParentActionAttribute : ActionBaseAttribute
	{
		/*public ParentActionAttribute() : base() { }
		protected ParentActionAttribute(IPathSection keyword) : base(keyword) { }
		public ParentActionAttribute(string keyword) : base(keyword) { }
		public ParentActionAttribute(string keywordResourceBaseName, string keywordResourceName) : base(keywordResourceBaseName, keywordResourceName) { }
		public ParentActionAttribute(string keywordResourceAssembly, string keywordResourceBaseName, string keywordResourceName) : base(keywordResourceAssembly, keywordResourceBaseName, keywordResourceName) { }
		*/
		public override bool TryBinding(IHttpContext context, IPathStack path)
		{
			object[] arguments;
			if (TryBinding(context, path, out arguments))
			{
				/*object controller = Activator.CreateInstance(controllerType);

				if (controllerExtensions != null)
					foreach (IControllerExtension ext in controllerExtensions)
						ext.OnPreAction(context);
				if (this.Extensions != null)
					foreach (IActionExtension ext in this.Extensions)
						ext.OnPreAction(context);

				try
				{
					controller = Method.Invoke(controller, arguments);
				}
				catch (Exception e)
				{
					bool catched = false;
					if (this.Extensions != null)
						foreach (IActionExtension ext in this.Extensions)
							if (!ext.OnError(context, e))
								catched = true;
					if (controllerExtensions != null)
						foreach (IControllerExtension ext in controllerExtensions)
							if (!ext.OnError(context, e))
								catched = true;
					if (!catched)
						throw e;
				}

				if (this.Extensions != null)
					foreach (IActionExtension ext in this.Extensions)
						ext.OnPostAction(context);
				if (controllerExtensions != null)
					foreach (IControllerExtension ext in controllerExtensions)
						ext.OnPostAction(context);
				*/

				/*
				Get ChildControllerAttribute
				Try Binding on it
				*/
				/*handler = null;
				return true;*/
			}
			handler = null;
			return false;
		}
	}
}
