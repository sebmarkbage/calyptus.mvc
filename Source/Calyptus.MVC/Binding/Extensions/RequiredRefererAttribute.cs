using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace Calyptus.MVC
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RequiredRefererAttribute : Attribute, IExtension
    {
		public string Host { get; set; }

		public RequiredRefererAttribute()
		{
		}

		public RequiredRefererAttribute(string host)
		{
			this.Host = host;
		}

		public void Initialize(MemberInfo target) { }

		public void OnBeforeAction(IHttpContext context, BeforeActionEventArgs args)
		{
			string host = Host ?? context.Request.Url.Host;
			string referer = context.Request.UrlReferrer.Host;
			if (referer != null && !host.Equals(referer, StringComparison.CurrentCultureIgnoreCase))
				throw new HttpException(403, "The request cannot only be called from the refering host.");
		}

		public void OnError(IHttpContext context, ErrorEventArgs args) { }

		public void OnAfterAction(IHttpContext context, AfterActionEventArgs args) { }

		public void OnBeforeRender(IHttpContext context, BeforeRenderEventArgs args) { }

		public void OnAfterRender(IHttpContext context, AfterRenderEventArgs args) { }

	}
}
