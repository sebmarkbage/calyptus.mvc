using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Binding
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class PostAttribute : ParamAttribute
	{
		public override bool TryBinding(IHttpContext context, IPathStack path, out object obj)
		{
			return TryBinding(context.Request.Form, out obj);
		}

		public override void SerializePath(IPathStack path, object obj)
		{
			// Exclude
		}
	}
}
