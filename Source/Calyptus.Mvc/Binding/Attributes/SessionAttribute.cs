using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.Mvc
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class SessionAttribute : Attribute, IParameterBinding, IPropertyBinding
	{
		private Type _type;
		private string _name;

		public string Key { get { return _name; } set { if (value == null) throw new NullReferenceException("Session key cannot be null"); _name = value; } }

		public SessionAttribute()
		{
		}

		public SessionAttribute(string key)
		{
			this._name = key;
		}

		public void Initialize(ParameterInfo parameter)
		{
			_type = parameter.ParameterType;
			MethodInfo m = (MethodInfo)parameter.Member;
			Type c = m.DeclaringType;
			if (_name == null) _name = c.Namespace + "." + c.Name + "." + m.Name + "(" + parameter.Name + ")";
		}
		public void Initialize(PropertyInfo property)
		{
			_type = property.PropertyType;
			Type c = property.DeclaringType;
			if (_name == null) _name = c.Namespace + "." + c.Name + "." + property.Name;
		}

		public virtual bool TryBinding(IHttpContext context, out object obj)
		{
			obj = context.Session[_name];
			return obj != null && obj.GetType().IsAssignableFrom(this._type);
		}

		public bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight)
		{
			overloadWeight = 10;
			return TryBinding(context, out obj);
		}

		public void StoreBinding(IHttpContext context, object value)
		{
			context.Session[_name] = value;
		}

		public void SerializePath(IPathStack path, object obj)
		{
			// Exclude	
		}
	}
}
