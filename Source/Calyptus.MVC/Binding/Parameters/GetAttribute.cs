using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Binding
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class GetAttribute : ParamAttribute
	{
		public override bool TryBinding(IHttpContext context, IPathStack path, out object obj)
		{
			return TryBinding(context.Request.QueryString, out obj);
		}

		public override void SerializePath(IPathStack path, object obj)
		{
			SerializeBinding(path.QueryString, obj);
		}
	}

	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class QueryAttribute : GetAttribute { }
}
