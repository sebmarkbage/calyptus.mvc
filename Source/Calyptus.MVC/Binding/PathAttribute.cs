using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC.Binding
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter, AllowMultiple=true, Inherited=false)]
    public class PathAttribute : Attribute, IBindable
    {
		private static IDictionary<Type, MethodInfo> _deserializeMethodCache;

		public string ValidationRegEx { get; set; }

		public string NullValue { get; set; }

		internal IKeyword Keyword { get; set; }

		public PathAttribute()
		{
			this.Keyword = new DefaultKeyword();
		}
		protected PathAttribute(IKeyword keyword)
		{
			this.Keyword = keyword;
		}
		public PathAttribute(string keyword)
        {
            this.Keyword = new PlainKeyword(keyword);
        }
		public PathAttribute(string keywordResourceBaseName, string keywordResourceName)
        {
            this.Keyword = new ResourceKeyword(System.Reflection.Assembly.GetExecutingAssembly(), keywordResourceBaseName, keywordResourceName);
        }
		public PathAttribute(string keywordResourceAssembly, string keywordResourceBaseName, string keywordResourceName)
        {
            this.Keyword = new ResourceKeyword(System.Reflection.Assembly.Load(keywordResourceAssembly), keywordResourceBaseName, keywordResourceName);
        }

		public virtual void Initialize(Type type, string name)
		{
			if (Keyword == null) Keyword = new PlainKeyword(name);
			_type = type;
		}

		private Type _type;

		bool IBindable.TryBinding(IHttpContext context, IPathStack path, out object obj)
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

		void IBindable.SerializePath(IPathStack path, object obj)
		{
			Keyword.Serialize(path);
		}

		public virtual bool TryBinding(IPathStack path, Type type, out object obj)
		{
			if (NullValue != null && NullValue.Equals(path.Peek(), StringComparison.CurrentCultureIgnoreCase))
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
					obj = Convert.ChangeType(path.Peek(), type);
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
			{
				((IPathSerializable) obj).SerializeToPath(path);
			}
			else
				path.Push(obj.ToString());
		}
	}
}
