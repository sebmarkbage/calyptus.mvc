using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.Mvc
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class SpamProtectAttribute : Attribute, IExtension
	{
		public int Hits { get; set; }
		public int Duration { get; set; }
		public int BanTimeout { get; set; }

		public SpamProtectAttribute()
		{
			Hits = 20;
			Duration = 5;
			BanTimeout = 60;
		}

		private Dictionary<string, int[]> _hits;

		public void Initialize(System.Reflection.MemberInfo target)
		{
		}

		public void OnBeforeAction(IHttpContext context, BeforeActionEventArgs args)
		{
			string ip = context.Request.UserHostName;
			
		}

		public void OnError(IHttpContext context, ErrorEventArgs args)
		{
		}

		public void OnAfterAction(IHttpContext context, AfterActionEventArgs args)
		{
		}

		public void OnBeforeRender(IHttpContext context, BeforeRenderEventArgs args)
		{
		}

		public void OnAfterRender(IHttpContext context, AfterRenderEventArgs args)
		{
		}
	}
}
