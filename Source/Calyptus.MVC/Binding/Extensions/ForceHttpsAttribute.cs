using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace Calyptus.MVC
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ForceHttpsAttribute : Attribute, IExtension
    {
		public int Port { get; set; }
		public string Host { get; set; }

		public ForceHttpsAttribute()
		{
		}

		public void Initialize(MemberInfo target) { }

		public void OnBeforeAction(IHttpContext context, BeforeActionEventArgs args)
		{
			Uri uri = context.Request.Url;
			if (!context.Request.IsSecureConnection || (Port > 0 && uri.Port != Port) || (Host != null && !Host.Equals(uri.Host, StringComparison.CurrentCultureIgnoreCase)))
			{
				if (context.Request.RequestType != "GET")
					throw new HttpException(403, "The request can only be processed via the HTTPS protocol.");

				int port = Port > 0 ? Port : 443;
				string host = Host ?? uri.Host;
				throw new Redirect("https://" + host + (port != 443 ? ":" + port.ToString() : null) + uri.PathAndQuery, true);
			}
		}

		public void OnError(IHttpContext context, ErrorEventArgs args) { }

		public void OnAfterAction(IHttpContext context, AfterActionEventArgs args) { }

		public void OnBeforeRender(IHttpContext context, BeforeRenderEventArgs args) { }

		public void OnAfterRender(IHttpContext context, AfterRenderEventArgs args) { }
	}
}
