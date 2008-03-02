using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC.Binding
{
	public interface IExtension
	{
		void Initialize(MemberInfo target);
		void TryMapping(IHttpContext context);

		void OnPreAction(IHttpContext context, object[] parameters);
		bool OnError(IHttpContext context, Exception error);
		void OnPostAction(IHttpContext context, object returnValue);
	}
}
