﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Specialized;

namespace Calyptus.MVC
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple=true, Inherited=false)]
	public class ParamAttribute : Attribute, IParameterBinding
	{
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

		public virtual bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight)
		{
			overloadWeight = 9;
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

		public void StoreBinding(IHttpContext context, object value)
		{

		}
	}
}
