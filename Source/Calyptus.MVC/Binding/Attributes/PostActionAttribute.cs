using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.MVC.Mapping;

namespace Calyptus.MVC.Actions
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class PostActionAttribute : ActionBaseAttribute
	{
		public string ContentType { set { Mappings.Add(new ContentTypeMapping(value)); } }
		public string Path { set { Mappings.Add(new PathMapping(value)); } }

		public PostActionAttribute()
		{
			Mappings.Add(new VerbMapping("POST"));
		}
	}
}
