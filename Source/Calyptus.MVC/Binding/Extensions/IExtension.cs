using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC
{
	public interface IExtension
	{
		void Initialize(MemberInfo target);

		void OnBeforeAction(IHttpContext context, object[] parameters);
		bool OnError(IHttpContext context, Exception error);
		void OnAfterAction(IHttpContext context, object returnValue);
		void OnBeforeRender(IHttpContext context, IView view);
		void OnAfterRender(IHttpContext context, IView view);
	}
}
