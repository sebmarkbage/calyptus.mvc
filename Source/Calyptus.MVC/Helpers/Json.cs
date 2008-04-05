using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace Calyptus.MVC
{
	public static class Json
	{
		public static object GetJson<T>(this HttpRequest Request, string name) 
		{
			return Request.Form;
		}
	}
}
