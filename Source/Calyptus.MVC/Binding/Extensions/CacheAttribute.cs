using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace Calyptus.MVC
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CacheAttribute : Attribute, IExtension
    {
		public HttpCacheability Cacheability { get; set; }

		private bool _durationSet;
		private int _duration;
		public int Duration { get { return _duration; } set { _durationSet = true; _duration = value; } }

		public CacheAttribute()
		{
			Cacheability = HttpCacheability.Public;

		}

		public void Initialize(MemberInfo target) { }

		public void OnBeforeAction(IHttpContext context, object[] parameters) {}

		public void OnAfterAction(IHttpContext context, object returnValue)
		{
			context.Response.Cache.SetCacheability(Cacheability);
			if (_durationSet)
				context.Response.Cache.SetExpires(DateTime.Now.AddSeconds((double)_duration));
		}

		public bool OnError(IHttpContext context, Exception error) { return true; }

		public void OnBeforeRender(IHttpContext context, IView view) { }

		public void OnAfterRender(IHttpContext context, IView view) { }
	}
}
