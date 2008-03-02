using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Specialized;

namespace Calyptus.MVC.Binding
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple=true, Inherited=false)]
	public class ParamAttribute : Attribute, IParameterBinding
	{
		public string ValidationRegEx { get; set; }

		public ParamAttribute()
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
			return TryBinding(context.Request.Params, out obj);
		}

		protected bool TryBinding(NameValueCollection nameValues, out object obj)
		{
			string v = nameValues[_name];
			if (v != null)
				try
				{
					obj = Convert.ChangeType(v, _type);
					return true;
				}
				catch
				{
				}
			obj = !_type.IsClass ? Activator.CreateInstance(_type) : null;
			return false;
		}

		public virtual void SerializePath(IPathStack path, object obj)
		{
			SerializeBinding(path.QueryString, obj);
		}

		protected void SerializeBinding(NameValueCollection nameValues, object obj)
		{
			if (obj != null)
				nameValues.Add(_name, obj.ToString());
		}
	}
}
