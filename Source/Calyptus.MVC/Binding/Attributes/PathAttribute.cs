using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Calyptus.MVC.Mapping;
using System.Drawing;

namespace Calyptus.MVC
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.GenericParameter, AllowMultiple = true, Inherited = false)]
    public class PathAttribute : BinderBaseAttribute
    {
		private IMappingBinding _emptyPath;
		public string EmptyPath { get { return _emptyPath == null ? null : _emptyPath.ToString(); } set { _emptyPath = new PathMapping(value); } }

		private bool _emptyValueSet;
		private object _emptyValue;
		public object EmptyValue { get { return _emptyValue; } set { _emptyValue = value; _emptyValueSet = true; } }

		private IMappingBinding _requestType;
		public string RequestType { get { return _requestType.ToString(); } set { if (_requestType != null) Mappings.Remove(_requestType); Mappings.Add(_requestType = new ContentTypeMapping(value)); } }

		private delegate bool TryDeserialize(IPathStack path, out object obj);
		private TryDeserialize _deserializer;

		private string _pathResBaseName;
		private string _pathResKey;

		public string Extension { get; set; }

		public PathAttribute()
		{
		}

		public PathAttribute(string preceedingPath)
		{
			if (preceedingPath != null)
				Path = preceedingPath;
		}

		public PathAttribute(string pathResourceBaseName, string pathResourceKey)
		{
			_pathResBaseName = pathResourceBaseName;
			_pathResKey = pathResourceKey;
		}

		public PathAttribute(string pathResourceAssembly, string pathResourceBaseName, string pathResourceKey)
		{
			Mappings.Add(new ResourcePathMapping(Assembly.Load(pathResourceAssembly), pathResourceBaseName, pathResourceKey));
		}

		protected override void Initialize(MethodInfo method)
		{
			InitializeResourcePath(method.DeclaringType);
		}

		protected override void Initialize(PropertyInfo property)
		{
			InitializeResourcePath(property.DeclaringType);
			InitializeDeserializer();
		}

		protected override void Initialize(ParameterInfo parameter)
		{
			InitializeResourcePath(parameter.Member.DeclaringType);
			InitializeDeserializer();
		}

		private void InitializeResourcePath(Type declaringType)
		{
			if (Path == null && _pathResBaseName != null && _pathResKey != null)
			{
				Mappings.Add(new ResourcePathMapping(declaringType.Assembly, _pathResBaseName, _pathResKey));
				_pathResBaseName = null;
				_pathResKey = null;
			}
		}

		private void InitializeDeserializer()
		{
			if (typeof(IPathSerializable).IsAssignableFrom(BindingTargetType))
			{
				MethodInfo m = BindingTargetType.GetMethod("TryDeserializePath", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(IPathStack), BindingTargetType }, null);
				if (m == null) throw new BindingException(String.Format("{0} is missing the static TryDeserializePath method.", BindingTargetType.FullName));
				_deserializer = delegate(IPathStack path, out object obj) {
					object[] args = new object[] { path, null };
					bool r = (bool) m.Invoke(null, args);
					obj = args[1];
					return r;
				};
			}
			else if (BindingTargetType.IsGenericType && BindingTargetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				System.ComponentModel.NullableConverter converter = new System.ComponentModel.NullableConverter(BindingTargetType);
				BindingTargetType = converter.UnderlyingType;
			}
		}


		protected override bool TryBinding(IHttpContext context, IPathStack path, out object value, out int overloadWeight)
		{
			overloadWeight = -path.Index;
			if ((PathBinding == null || PathBinding.TryMapping(context, path)) &&
				TryBinding(path, out value))
			{
				if (value == null)
					overloadWeight = 50;
				else if (value.GetType() == typeof(string)) overloadWeight = 100;
				else if (value.GetType() == typeof(char[])) overloadWeight = 100;
				else if (value.GetType() == typeof(char)) overloadWeight = 101;
				else if (value.GetType() == typeof(bool)) overloadWeight = 107;
				else if (value.GetType() == typeof(int)) overloadWeight = 108;
				else if (value.GetType() == typeof(float)) overloadWeight = 109;
				else if (value.GetType() == typeof(double)) overloadWeight = 110;
				else if (value.GetType() == typeof(DateTime)) overloadWeight = 115;
				else
				{
					overloadWeight += path.Index;
					overloadWeight *= 110;
				}
				return true;
			}
			value = null;
			overloadWeight = 0;
			return false;
		}

		private bool TryBinding(IPathStack path, out object obj)
		{
			if (path.IsAtEnd)
			{
				obj = null;
				return false;
			}
			int index = path.Index;
			if (_emptyPath != null && _emptyPath.TryMapping(null, path))
			{
				obj = _emptyValueSet ? _emptyValue : (DefaultValue != null ? DefaultValue : (BindingTargetType.IsValueType ? Activator.CreateInstance(BindingTargetType) : null));
				return true;
			}
			path.ReverseToIndex(index);
			if (_deserializer != null)
			{
				return _deserializer(path, out obj);
			}
			else
			{
				try
				{
					string nextPath = path.Peek();
					if (Extension != null)
					{
						if (!nextPath.EndsWith(Extension, StringComparison.CurrentCultureIgnoreCase))
						{
							obj = null;
							return false;
						}
						nextPath = nextPath.Substring(0, nextPath.Length - Extension.Length);
					}
					obj = ConvertFromPath(nextPath, BindingTargetType);
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

		protected override bool TryBinding(IHttpContext context, out object value)
		{
			value = null;
			return false;
		}

		protected override void SerializePath(IPathStack path, object value)
		{
			if (value == null || value as string == "" || value == DefaultValue)
			{
				_emptyPath.SerializeToPath(path);
				return;
			}

			IPathSerializable serializable = value as IPathSerializable;
			if (serializable != null)
				serializable.SerializeToPath(path);
			else
			{
				try
				{
					path.Push(ConvertToPath(value) + Extension);
				}
				catch (Exception e)
				{
					throw new BindingException(e.Message, e);
				}
			}
		}

		private string ConvertToPath(object value)
		{
			if (value is byte[])
				return Convert.ToBase64String((byte[])value, Base64FormattingOptions.None).Replace('+', '-').Replace('/', '_').Replace("=", "");
			else if (value is Size)
			{
				Size s = (Size)value;
				return s.Width.ToString() + "x" + s.Height.ToString();
			}
			else
				return Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);
		}

		private object ConvertFromPath(string path, Type targetType)
		{
			if (targetType == typeof(byte[]))
			{
				string p = path.Replace('-', '+').Replace('_', '/');
				int i = p.Length % 4;
				if (i > 0)
					p = p.PadRight(4 - i, '=');
				return Convert.FromBase64String(p);
			}
			else if (targetType == typeof(Size))
			{
				string[] s = path.Split(new char[] { 'x' }, 2);
				if (s.Length != 2) throw new Exception("Wrong format");
				return new Size(int.Parse(s[0]), int.Parse(s[1]));
			}
			else
				return Convert.ChangeType(path, BindingTargetType, System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}
