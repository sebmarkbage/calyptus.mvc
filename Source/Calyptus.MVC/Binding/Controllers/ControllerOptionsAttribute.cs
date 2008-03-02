using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC.Binding
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class ControllerOptionsAttribute : IExtension
	{
		public virtual string Master { get; set; }
		public virtual Type ViewEngine { get; set; }

		public void Initialize(MemberInfo target)
		{
		}

		public void TryMapping(IHttpContext context)
		{
		}

		public void OnPreAction(IHttpContext context, object[] parameters)
		{
		}

		public bool OnError(IHttpContext context, Exception error)
		{
		}

		public void OnPostAction(IHttpContext context, object returnValue)
		{
		}
	}
}
