using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Calyptus.Mvc;
using System.Web;
using Calyptus.Mvc.Mapping;

namespace Calyptus.Mvc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=false)]
	public class EntryControllerAttribute : ControllerBaseAttribute, IEntryControllerBinding
    {
		protected IList<IMappingBinding> Mappings;

		private IMappingBinding _path;
		public string Path { get { return _path == null ? null : _path.ToString(); } set { if (_path != null) Mappings.Remove(_path); Mappings.Add(_path = new PathMapping(value)); } }

		private string _pathResBaseName;
		private string _pathResKey;

		public EntryControllerAttribute()
		{
			Mappings = new List<IMappingBinding>();
		}

		public EntryControllerAttribute(string path) : this()
		{
			if (path != null)
				Path = path;
		}

		public EntryControllerAttribute(string pathResourceBaseName, string pathResourceKey) : this()
		{
			_pathResBaseName = pathResourceBaseName;
			_pathResKey = pathResourceKey;
		}

		public EntryControllerAttribute(string pathResourceAssembly, string pathResourceBaseName, string pathResourceKey) : this()
		{
			_path = new ResourcePathMapping(Assembly.Load(pathResourceAssembly), pathResourceBaseName, pathResourceKey);
		}

		public override void Initialize(Type controllerType)
		{
			if (_path == null && _pathResBaseName != null && _pathResKey != null)
			{
				_path = new ResourcePathMapping(controllerType.Assembly, _pathResBaseName, _pathResKey);
				Mappings.Add(_path);
				_pathResBaseName = null;
				_pathResKey = null;
			}

			base.Initialize(controllerType);
			if (Mappings.Count == 0) Mappings = null;
		}

		/*public override void Initialize(Type controllerType)
		{
			//if (Keyword == null) Keyword = new PlainPath(controllerType.Name.Length > 10 && controllerType.Name.EndsWith("Controller", StringComparison.InvariantCultureIgnoreCase) ? controllerType.Name.Substring(0, controllerType.Name.Length - 10) : controllerType.Name);

			base.Initialize(controllerType);
		}*/

		bool IEntryControllerBinding.TryBinding(IHttpContext context, IPathStack path, out IHttpHandler handler)
		{
			if (Mappings != null)
				foreach (IMappingBinding mapping in this.Mappings)
					if (!mapping.TryMapping(context, path))
					{
						handler = null;
						return false;
					}

			return TryBinding(context, path, out handler);
		}

		void IEntryControllerBinding.SerializeToPath(IRouteAction action, IPathStack path)
		{
			if (Mappings != null)
				foreach (IMappingBinding mapping in this.Mappings)
					mapping.SerializeToPath(path);

			base.SerializeToPath(action, path);
		}
	}
}
