using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC.Binding
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter, AllowMultiple=true, Inherited=false)]
    public class PathAttribute : Attribute, IParameterBinding
    {
		private static IDictionary<Type, MethodInfo> _deserializeMethodCache;

		public string ValidationRegEx { get; set; }

		public string NullValue { get; set; }

		public PathAttribute()
		{
		}

		public virtual void Initialize(ParameterInfo parameter)
		{
			if (Keyword == null) Keyword = new PlainPath(parameter.Name);

			Keyword.Initialize(parameter.Member);

			_type = parameter.ParameterType;
		}

		private Type _type;

		bool IParameterBinding.TryBinding(IHttpContext context, IPathStack path, out object obj)
		{
			if (Keyword.Try(path))
			{
				return TryBinding(path, _type, out obj);
			}
			else
			{
				obj = null;
				return false;
			}
		}

		void IParameterBinding.SerializePath(IPathStack path, object obj)
		{
		}

		public virtual bool TryBinding(IPathStack path, Type type, out object obj)
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
			else if (typeof(IPathSerializable).IsAssignableFrom(type))
			{
				MethodInfo m;
				if (_deserializeMethodCache == null)
					_deserializeMethodCache = new Dictionary<Type, MethodInfo>();
				if (!_deserializeMethodCache.TryGetValue(type, out m))
				{
					lock (_deserializeMethodCache)
					{
						m = type.GetMethod("TryDeserializePath", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(IPathStack), type }, null);

						if (m == null) throw new Exception(String.Format("{0} is missing the static TryDeserializePath method.", m.Name));

						_deserializeMethodCache.Add(type, m);
					}
				}
				object[] args = new object[] { path, null };
				bool r = (bool) m.Invoke(null, args);
				obj = args[1];
				return r;
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
				}
			}
			obj = null;
			return false;
		}

		public virtual void SerializeBinding(IPathStack path, object obj)
		{
			if (obj is IPathSerializable)
				((IPathSerializable) obj).SerializeToPath(path);
			else
				path.Push(Convert.ToString(obj, System.Globalization.CultureInfo.InvariantCulture));
		}
	}
}
