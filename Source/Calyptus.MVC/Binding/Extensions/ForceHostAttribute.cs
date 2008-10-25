using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace Calyptus.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ForceHostAttribute : Attribute, IExtension
    {
		public int Port { get; set; }
		public string[] Hosts { get; set; }

		public ForceHostAttribute(params string[] acceptableHosts)
		{
			Hosts = acceptableHosts;
		}

		public void Initialize(MemberInfo target) { }

		public void OnBeforeAction(IHttpContext context, BeforeActionEventArgs args)
		{
			Uri uri = context.Request.Url;
			bool hostAccepted = false;
			foreach (string host in Hosts)
				if (host != null && host.Equals(uri.Host, StringComparison.CurrentCultureIgnoreCase))
				{
					hostAccepted = true;
					break;
				}
			if ((Port > 0 && uri.Port != Port) || !hostAccepted)
			{
				if (context.Request.RequestType != "GET")
					return; // Skip for none GET requests which can't be redirected

				int port = Port > 0 ? Port : uri.Port;
				string host = hostAccepted ? uri.Host : Hosts[0];
				throw new Redirect((context.Request.IsSecureConnection ? "https://" : "http://") + host + (port != (context.Request.IsSecureConnection ? 443 : 80) ? ":" + Port.ToString() : null) + uri.PathAndQuery, true);
			}
		}

		public void OnError(IHttpContext context, ErrorEventArgs args) { }

		public void OnAfterAction(IHttpContext context, AfterActionEventArgs args) { }

		public void OnBeforeRender(IHttpContext context, BeforeRenderEventArgs args) { }

		public void OnAfterRender(IHttpContext context, AfterRenderEventArgs args) { }
	}
}
