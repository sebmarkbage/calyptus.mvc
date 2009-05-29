using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.Mvc.Mapping;

namespace Calyptus.Mvc
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class DeleteAttribute : ActionBaseAttribute
	{
		private static IMapping verb = new VerbMapping("DELETE");

		protected override Type DefaultParameterBinderType
		{
			get { return typeof(PathAttribute); }
		}

		private IMapping _path;
		public string Path { get { return _path == null ? null : _path.ToString(); } set { if (_path != null) Mappings.Remove(_path); Mappings.Add(_path = new PathMapping(value)); } }

		public DeleteAttribute()
		{
			Mappings.Add(verb);
		}
	}
}
