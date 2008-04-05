using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Calyptus.MVC;
using System.Web;

namespace Calyptus.MVC
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=false)]
	public class EntryControllerAttribute : ControllerBaseAttribute, IEntryControllerBinding
    {
		public string Path { set { this.Mappings.Add(new Mapping.PathMapping(value)); } }

		public EntryControllerAttribute()
		{
		}

		/*public override void Initialize(Type controllerType)
		{
			//if (Keyword == null) Keyword = new PlainPath(controllerType.Name.Length > 10 && controllerType.Name.EndsWith("Controller", StringComparison.InvariantCultureIgnoreCase) ? controllerType.Name.Substring(0, controllerType.Name.Length - 10) : controllerType.Name);

			base.Initialize(controllerType);
		}*/

		public override bool TryBinding(IHttpContext context, IPathStack path, out IHttpHandler handler)
		{
			foreach (IMappingBinding mapping in this.Mappings)
				if (!mapping.TryMapping(context, path))
				{
					handler = null;
					return false;
				}

			return base.TryBinding(context, path, out handler);
		}

		public override void SerializeToPath(IPathStack path, MethodInfo method, object[] arguments)
		{
			foreach (IMappingBinding mapping in this.Mappings)
				mapping.SerializeToPath(path);

			base.SerializeToPath(path, method, arguments);
		}

	}
}
