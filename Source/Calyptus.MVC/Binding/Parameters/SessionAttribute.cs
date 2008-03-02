using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC.Binding
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class SessionAttribute : Attribute, IParameterBinding
	{
		public string ValidationRegEx { get; set; }

		public SessionAttribute()
		{
		}

		public void Initialize(ParameterInfo parameter)
		{
			_type = parameter.ParameterType;
			_name = parameter.Name;
		}

		private Type _type;
		private string _name;

		public bool Required { get; set; }

		public virtual bool TryBinding(IHttpContext context, IPathStack path, out object obj)
		{
			obj = context.Session[_name];
			return obj != null && obj.GetType().IsAssignableFrom(this._type);
		}

		public virtual void SerializePath(IPathStack path, object obj)
		{
			// Exclude
		}
	}
}
