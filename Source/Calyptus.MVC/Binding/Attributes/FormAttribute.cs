﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class FormAttribute : ParamAttribute
	{
		public override bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight)
		{
			overloadWeight = 10;
			return TryBinding(context.Request.Form, out obj);
		}

		public override void SerializePath(IPathStack path, object obj)
		{
			// Exclude
		}
	}
}
