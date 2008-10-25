using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Calyptus.Mvc.Mapping;

namespace Calyptus.Mvc
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class ActionAttribute : ActionBaseAttribute
	{
		private VerbMapping _verb;
		public string Verb { get { return _verb == null ? null : _verb.ToString(); } set { if (_verb != null) Mappings.Remove(_verb); Mappings.Add(_verb = new VerbMapping(value)); } }
		private ContentTypeMapping _contentType;
		public string RequestType { get { return _contentType == null ? null : _contentType.ToString(); } set { if (_contentType != null) Mappings.Remove(_contentType); Mappings.Add(_contentType = new ContentTypeMapping(value)); } }
		private PathMapping _path;
		public string Path { get { return _path == null ? null : _path.ToString(); } set { if (_path != null) Mappings.Remove(_path); Mappings.Add(_path = new PathMapping(value)); } }
	}
}