using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace Calyptus.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CacheAttribute : Attribute, IExtension
    {
		public HttpCacheability Cacheability { get; set; }

		private bool _durationSet;
		private int _duration;
		public int Duration { get { return _duration; } set { _durationSet = true; _duration = value; } }

		public string VaryByParams { get; set; }
		public string VaryByHeaders { get; set; }

		public CacheAttribute()
		{
			Cacheability = HttpCacheability.Public;

		}

		public void Initialize(MemberInfo target) { }

		public void OnBeforeAction(IHttpContext context, BeforeActionEventArgs args)
		{
			var cache = context.Response.Cache;
			if (VaryByParams != null)
			{
				if (VaryByParams.Trim().Equals("none", StringComparison.OrdinalIgnoreCase))
					cache.VaryByParams.IgnoreParams = true;
				else
				{
					cache.VaryByParams["*"] = false;
					foreach (var param in VaryByParams.Split(','))
						cache.VaryByParams[param.Trim()] = true;
				}
			}

			if (VaryByHeaders != null)
			{
				cache.SetOmitVaryStar(true);
				cache.VaryByHeaders["*"] = false;
				foreach (var param in VaryByHeaders.Split(','))
				{
					var p = param.Trim();
					if (p == "*") cache.SetOmitVaryStar(false);
					cache.VaryByHeaders[p] = true;
				}
			}

			//context.Response.Cache.VaryByParams 
			//context.Response.Cache.SetCacheability(Cacheability);
			if (_durationSet)
			{
				cache.SetExpires(DateTime.Now.AddSeconds((double)_duration));
				cache.SetMaxAge(new TimeSpan(0, 0, _duration));
			}
		}

		public void OnError(IHttpContext context, ErrorEventArgs args) { }

		public void OnAfterAction(IHttpContext context, AfterActionEventArgs args) { }

		public void OnBeforeRender(IHttpContext context, BeforeRenderEventArgs args) { }

		public void OnAfterRender(IHttpContext context, AfterRenderEventArgs args) { }
	}
}
