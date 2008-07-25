using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Security.Principal;
using System.Web.SessionState;
using System.Web.Caching;
using System.IO;
using System.Xml;

namespace Calyptus.MVC
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public class ApplicationFilePathAttribute : MapPathAttribute
	{
		public ApplicationFilePathAttribute() : base()
		{
		}
	}
}
