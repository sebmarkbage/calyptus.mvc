using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.MVC.Mapping;
using System.Reflection;

namespace Calyptus.MVC
{
	public abstract class BinderBaseAttribute : ActionBaseAttribute, IPropertyBinding, IParameterBinding
	{
		protected Type BindingTargetType { get; set; }

		protected IMappingBinding PathBinding;
		public string Path { get { return PathBinding == null ? null : PathBinding.ToString(); } set { if (PathBinding != null) Mappings.Remove(PathBinding); Mappings.Add(PathBinding = new PathMapping(value)); } }

		protected override Type DefaultParameterBinderType
		{
			get
			{
				return this.GetType();
			}
		}

		void IParameterBinding.Initialize(ParameterInfo parameter)
		{
			BindingTargetType = parameter.ParameterType;
			Initialize(parameter);
		}

		protected virtual void Initialize(ParameterInfo parameter)
		{

		}

		void IPropertyBinding.Initialize(PropertyInfo property)
		{
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
			overloadWeight = r ? 100 : 0;
			return r;
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
