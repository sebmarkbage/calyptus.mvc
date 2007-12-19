using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC.Extensions
{
	public interface IExtension
	{
		void Initialize(MemberInfo target);

		void OnPreAction(IHttpContext context);
		bool OnError(IHttpContext context, Exception error);
		void OnPostAction(IHttpContext context);
	}
}
