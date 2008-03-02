using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC.Binding.Parameters
{
	public abstract class ParamBaseAttribute : Attribute, IParameterBinding
	{
		protected IList<IMappingBinding> Mappings;

		public bool Required { get; set; }

		public ParamBaseAttribute() : base(null) { }

		public ParamBaseAttribute(IEnumerable<IMappingBinding> mappings)
		{
			Mappings = new List<IMappingBinding>(mappings);
		}

		public void Initialize(ParameterInfo parameter)
		{

		}

		public bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight)
		{

		}

		public void SerializePath(IPathStack path, object obj)
		{
		}
	}
}
