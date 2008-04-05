using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Calyptus.MVC.Mapping;

namespace Calyptus.MVC
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter, AllowMultiple=true, Inherited=false)]
    public class PathAttribute : Attribute, IParameterBinding
    {
		public string ValidationRegEx { get; set; }

		public string NullValue { get; set; }

		public string Path { get { return _path.ToString(); } set { _path = value == null ? null : new PathMapping(value); } }

		private PathMapping _path;

		private Type _type;

		private delegate bool TryDeserialize(IPathStack path, out object obj);
		private TryDeserialize _deserializer;

		public PathAttribute()
		{
		}

		public PathAttribute(string preceedingPath) : this()
		{
			if (preceedingPath != null)
				_path = new PathMapping(preceedingPath);
		}

		public virtual void Initialize(ParameterInfo parameter)
		{
			_type = parameter.ParameterType;

			if (typeof(IPathSerializable).IsAssignableFrom(_type))
			{
				MethodInfo m = _type.GetMethod("TryDeserializePath", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(IPathStack), _type }, null);
				if (m == null) throw new BindingException(String.Format("{0} is missing the static TryDeserializePath method.", _type.FullName));
				_deserializer = delegate(IPathStack path, out object obj) {
					object[] args = new object[] { path, null };
					bool r = (bool) m.Invoke(null, args);
					obj = args[1];
					return r;
				};
			}
			else if (_type.IsGenericType && _type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				System.ComponentModel.NullableConverter converter = new System.ComponentModel.NullableConverter(_type);
				_type = converter.UnderlyingType;
			}
		}

		public bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight)
		{
			overloadWeight = -path.Index;
			if ((_path == null || _path.TryMapping(context, path)) &&
				TryBinding(path, _type, out obj))
			{
				overloadWeight += path.Index;
				overloadWeight *= 10;
				return true;
			}
			obj = null;
			overloadWeight = 0;
			return false;
		}

		protected virtual bool TryBinding(IPathStack path, Type type, out object obj)
		{
			if (path.IsAtEnd)
			{
				obj = null;
				return false;
			}
			else if (NullValue != null && NullValue.Equals(path.Peek(), StringComparison.CurrentCultureIgnoreCase))
			{
				path.Pop();
				obj = !type.IsClass ? Activator.CreateInstance(type) : null;
				return true;
			}
			else if (_deserializer != null)
			{
				return _deserializer(path, out obj);
			}
			else
			{
				try
				{
					obj = Convert.ChangeType(path.Peek(), type, System.Globalization.CultureInfo.InvariantCulture);
					path.Pop();
					return true;
				}
				catch
				{
					obj = null;
					return false;
				}
			}
		}

		public virtual void SerializePath(IPathStack path, object obj)
		{
			if (obj is IPathSerializable)
				((IPathSerializable)obj).SerializeToPath(path);
			else
			{
				try
				{
					path.Push(Convert.ToString(obj, System.Globalization.CultureInfo.InvariantCulture));
				}
				catch (Exception e)
				{
					throw new BindingException(e.Message, e);
				}
			}
		}
	}
}
