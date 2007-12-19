using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Calyptus.MVC.RoutingEngines;

namespace Calyptus.MVC.Binding
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor, AllowMultiple=true, Inherited=false)]
	public class ControllerAttribute : ControllerBaseAttribute
    {
		private IKeyword Keyword;

		public ControllerAttribute()
		{
		}
		protected ControllerAttribute(IKeyword keyword)
		{
			this.Keyword = keyword;
		}
		public ControllerAttribute(string keyword)
        {
            this.Keyword = new PlainKeyword(keyword);
        }
		public ControllerAttribute(string keywordResourceBaseName, string keywordResourceName)
        {
            this.Keyword = new ResourceKeyword(System.Reflection.Assembly.GetExecutingAssembly(), keywordResourceBaseName, keywordResourceName);
        }
		public ControllerAttribute(string keywordResourceAssembly, string keywordResourceBaseName, string keywordResourceName)
        {
            this.Keyword = new ResourceKeyword(System.Reflection.Assembly.Load(keywordResourceAssembly), keywordResourceBaseName, keywordResourceName);
        }

		internal override void Initialize(Type controllerType)
		{
			if (Keyword == null) Keyword = new PlainKeyword(controllerType.Name.Length > 10 && controllerType.Name.EndsWith("Controller", StringComparison.InvariantCultureIgnoreCase) ? controllerType.Name.Substring(0, controllerType.Name.Length - 10) : controllerType.Name);

			base.Initialize(controllerType);
		}

		internal override bool TryBinding(IHttpContext context, IPathStack path, out RoutingCommand command)
		{
			if (Keyword.Try(path))
				return base.TryBinding(context, path, out command);
			else
			{
				command = null;
				return false;
			}
		}

		internal override void SerializeBinding(IPathStack path, MethodInfo method, object[] arguments, out bool isNestable)
		{
			Keyword.Serialize(path);

			base.SerializeBinding(path, method, arguments, out isNestable);
		}

	}
}
