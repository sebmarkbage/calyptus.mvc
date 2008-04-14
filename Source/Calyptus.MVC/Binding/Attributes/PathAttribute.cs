using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Calyptus.MVC.Mapping;

namespace Calyptus.MVC
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.GenericParameter, AllowMultiple = true, Inherited = false)]
    public class PathAttribute : BinderBaseAttribute
    {
		public string NullValue { get; set; }

		private IMappingBinding _requestType;
		public string RequestType { get { return _requestType.ToString(); } set { if (_requestType != null) Mappings.Remove(_requestType); Mappings.Add(_requestType = new ContentTypeMapping(value)); } }

		private delegate bool TryDeserialize(IPathStack path, out object obj);
		private TryDeserialize _deserializer;

		private string _pathResBaseName;
		private string _pathResKey;

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
				overloadWeight += path.Index;
				overloadWeight *= 110;
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
			else if (NullValue != null && NullValue.Equals(path.Peek(), StringComparison.CurrentCultureIgnoreCase))
			{
				path.Pop();
				obj = !BindingTargetType.IsValueType ? Activator.CreateInstance(BindingTargetType) : null;
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
					obj = Convert.ChangeType(path.Peek(), BindingTargetType, System.Globalization.CultureInfo.InvariantCulture);
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
			if (value == null || value as string == "")
			{
				if (NullValue != null) path.Push(NullValue);
				return;
			}

			IPathSerializable serializable = value as IPathSerializable;
			if (serializable != null)
				serializable.SerializeToPath(path);
			else
			{
				try
				{
					path.Push(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
				}
				catch (Exception e)
				{
					throw new BindingException(e.Message, e);
				}
			}
		}
	}
}
