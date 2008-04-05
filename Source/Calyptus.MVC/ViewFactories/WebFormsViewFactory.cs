using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.Web.UI;
using System.Web;

namespace Calyptus.MVC
{
    public class WebFormsViewFactory : IViewFactory
    {
		private static string[] _masterPatterns = new string[] { "~/Views/{0}/{1}/{2}.master", "~/Views/{1}/{2}.master", "~/Views/Shared/{0}.master" };
		private static string[] _viewPatterns = new string[] { "~/Views/{0}/{1}/{2}.aspx", "~/Views/{0}/{1}/{2}.ascx", "~/Views/{1}/{2}.aspx", "~/Views/{1}/{2}.ascx", "~/Views/Shared/{0}.aspx", "~/Views/Shared/{0}.ascx" };

		public IView CreateTemplate(IHttpContext context, IViewTemplate view)
		{
			ViewPage page = System.Activator.CreateInstance(GetPageType(view.GetType())) as ViewPage;
			if (page == null) return null;

			/*if (view.Master != null)
			{
				string path = GetMasterPath(view.Master.GetType());
				if (path == null)
					return null;
				page.MasterPageFile = path;
			}*/

			return (IView)page;
		}

		private Type GetPageType(Type t)
		{
			string name = t.Name;
			string ns = t.Namespace.Replace('.', '/');
			string assembly = t.Assembly.GetName().Name;

			foreach(string pattern in _viewPatterns)
			{
				Type type = BuildManager.GetCompiledType(String.Format(pattern, assembly, ns, name));
				if (type != null && type.IsSubclassOf(typeof(ViewPage)))
					return type;
			}
			return null;
		}

		private string GetMasterPath(Type t)
		{
			string name = t.Name;
			string ns = t.Namespace.Replace('.', '/');
			string assembly = t.Assembly.GetName().Name;

			foreach (string pattern in _masterPatterns)
			{
				string path = String.Format(pattern, assembly, ns, name);
				Type type = BuildManager.GetCompiledType(path);
				if (type != null && type.IsSubclassOf(typeof(ViewMaster)))
					return path;
			}
			return null;
		}
	}
}
