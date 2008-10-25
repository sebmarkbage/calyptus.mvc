using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.Mvc.Mapping;
using System.Reflection;

namespace Calyptus.Mvc
{
	public abstract class BinderBaseAttribute : ActionBaseAttribute, IPropertyBinding, IParameterBinding
	{
		protected Type BindingTargetType { get; set; }

		protected IMappingBinding PathBinding;
		public string Path { get { return PathBinding == null ? null : PathBinding.ToString(); } set { if (PathBinding != null) Mappings.Remove(PathBinding); Mappings.Add(PathBinding = new PathMapping(value)); } }

		public object DefaultValue { get; set; }

		protected bool AcceptOut { get; set; }
		protected bool AcceptRef { get; set; }

		protected bool IsOut { get; private set; }
		protected bool IsIn { get; private set; }

		protected override Type DefaultParameterBinderType
		{
			get
			{
				return this.GetType();
			}
		}

		void IParameterBinding.Initialize(ParameterInfo parameter)
		{
			IsIn = !parameter.IsOut;
			IsOut = parameter.ParameterType.IsByRef;
			BindingTargetType = IsOut ? parameter.ParameterType.GetElementType() : parameter.ParameterType;
			Initialize(parameter);
		}

		protected virtual void Initialize(ParameterInfo parameter)
		{

		}

		void IPropertyBinding.Initialize(PropertyInfo property)
		{
			IsIn = property.CanWrite;
			IsOut = property.CanRead;
			BindingTargetType = property.PropertyType;
			Initialize(property);
		}

		protected virtual void Initialize(PropertyInfo property)
		{

		}

		bool IParameterBinding.TryBinding(IHttpContext context, IPathStack path, out object value, out int overloadWeight)
		{
			foreach (IMappingBinding mapping in this.Mappings)
				if (!mapping.TryMapping(context, path))
				{
					value = null;
					overloadWeight = 0;
					return false;
				}

			return TryBinding(context, path, out value, out overloadWeight);
		}

		bool IPropertyBinding.TryBinding(IHttpContext context, out object value)
		{
			foreach (IMappingBinding mapping in this.Mappings)
				if (!mapping.TryMapping(context, null))
				{
					value = null;
					return false;
				}

			return TryBinding(context, out value);
		}

		protected virtual bool TryBinding(IHttpContext context, IPathStack path, out object value, out int overloadWeight)
		{
			bool r = TryBinding(context, out value);

			if (!r) overloadWeight = 0;
			else if (value == null) overloadWeight = 50;
			else if (value.GetType() == typeof(string)) overloadWeight = 85;
			else if (value.GetType() == typeof(char[])) overloadWeight = 85;
			else if (value.GetType() == typeof(char)) overloadWeight = 86;
			else if (value.GetType() == typeof(bool)) overloadWeight = 92;
			else if (value.GetType() == typeof(int)) overloadWeight = 93;
			else if (value.GetType() == typeof(float)) overloadWeight = 94;
			else if (value.GetType() == typeof(double)) overloadWeight = 95;
			else overloadWeight = 100;

			if (!r)
				value = (DefaultValue != null ? DefaultValue : (BindingTargetType.IsValueType ? Activator.CreateInstance(BindingTargetType) : null));
			return true;
		}

		protected abstract bool TryBinding(IHttpContext context, out object value);

		void IParameterBinding.SerializePath(IPathStack path, object value)
		{
			if (Mappings != null)
				foreach (IMappingBinding mapping in Mappings)
					mapping.SerializeToPath(path);

			SerializePath(path, value);
		}

		protected virtual void SerializePath(IPathStack path, object value)
		{
		}

		void IParameterBinding.StoreBinding(IHttpContext context, object value)
		{
			StoreBinding(context, value);
		}

		void IPropertyBinding.StoreBinding(IHttpContext context, object value)
		{
			StoreBinding(context, value);
		}

		protected virtual void StoreBinding(IHttpContext context, object value) { }
	}
}
