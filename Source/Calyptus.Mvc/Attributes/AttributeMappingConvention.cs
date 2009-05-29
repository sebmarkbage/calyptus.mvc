using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Web;
using Calyptus.Mvc;
using System.Collections;
using Calyptus.Mvc.Mapping;

namespace Calyptus.Mvc
{
    public class AttributeMappingConvention : MappingConvention
    {
		public IInstanceMapping[] GetBindings(Type controllerType)
		{
			IInstanceMapping[] bindings;
			object[] attributes = controllerType.GetCustomAttributes(typeof(IInstanceMapping), false);
			if (attributes.Length == 0)
			{
				return null;
			}
			else
			{
				bindings = new IInstanceMapping[attributes.Length];
				for (int i = 0; i < attributes.Length; i++)
				{
					IInstanceMapping b = (IInstanceMapping)attributes[i];
					//TODO: b.Initialize(controllerType);
					bindings[i] = b;
				}
			}
			return bindings;
		}

		public IEntryMapping[] GetEntryBindings(Type controllerType)
		{
			IEntryMapping[] bindings;
			object[] attributes = controllerType.GetCustomAttributes(typeof(IEntryMapping), false);
			if (attributes.Length == 0)
			{
				return null;
			}
			else
			{
				bindings = new IEntryMapping[attributes.Length];
				for (int i = 0; i < attributes.Length; i++)
				{
					var b = (IEntryMapping)attributes[i];
					//TODO: b.Initialize(controllerType);
					bindings[i] = b;
				}
			}
			return bindings;
		}
	}
}
