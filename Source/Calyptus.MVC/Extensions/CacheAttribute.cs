using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace Calyptus.MVC.Extensions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CacheAttribute : Attribute, IExtension
    {
		public HttpCacheability Cacheability { get; set; }

		private bool _expiresSet;
		private int _expires;
		public int Expires { get { return _expires; } set { _expiresSet = true; _expires = value; } }

		public CacheAttribute()
		{
			Cacheability = HttpCacheability.Server;
		}

		public void Initialize(MemberInfo target)
		{
		}

		public void OnPreAction(IHttpContext context)
		{
			context.Response.Write("Cache.OnPreAction");
			context.Response.Cache.SetCacheability(Cacheability);
			if (_expiresSet)
				context.Response.Cache.SetExpires(DateTime.Now.AddMinutes((double) _expires));
		}

		public void OnPostAction(IHttpContext context)
		{
		}

		public bool OnError(IHttpContext context, Exception error)
		{
			return true;
		}
	}
}
