using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.Web.UI;
using System.Web;
using System.Web.Hosting;
using System.Reflection;

namespace Calyptus.MVC
{
    public class WebFormsViewFactory : IViewFactory
    {
		public IView FindView(IHttpContext context, IViewTemplate template)
		{
			IView view = GetPageInstance(template);
			if (view == null) return null;

			IView v = view;
			IViewTemplate m = GetMasterOf(template);
			while (m != null)
			{
				v = GetMasterInstance(m, v);
				m = GetMasterOf(m);
			}
			return view;
		}

		private IViewTemplate GetMasterOf(IViewTemplate t)
		{
			if (t == null) return null;
			PropertyInfo p = t.GetType().GetProperty("Master");
			if (p == null || !typeof(IViewTemplate).IsAssignableFrom(p.PropertyType)) return null;
			return (IViewTemplate)p.GetValue(t, null);
		}

		private VirtualPathProvider virtualPath
		{
			get
			{
				return HostingEnvironment.VirtualPathProvider;
			}
		}

		private Type GetPageType(Type t)
		{
			return GetType(t, typeof(ViewPage<>).MakeGenericType(t), "~/Views/{0}.aspx");
		}

		private Type GetControlType(Type t)
		{
			return GetType(t, new Type[] { typeof(ViewPage<>).MakeGenericType(t), typeof(ViewControl<>).MakeGenericType(t) }, new string[] { "~/Views/{0}.aspx", "~/Views/{0}.ascx" });
		}

		private string GetMasterType(Type t)
		{
			Type ofType = typeof(ViewMaster<>).MakeGenericType(t);
			string pattern = "~/Views/{0}.master";
			foreach (string path in GetPaths(t))
			{
				string vp = String.Format(pattern, path);
				if (virtualPath.FileExists(vp))
				{
					Type type = BuildManager.GetCompiledType(vp);
					if (ofType.IsAssignableFrom(type))
						return vp;
				}
			}
			return null;
		}

		private Type GetType(Type template, Type validType, params string[] filePatterns)
		{
			return GetType(template, new Type[] { validType }, filePatterns);
		}

		private Type GetType(Type template, Type[] validTypes, string[] filePatterns)
		{
			foreach (string path in GetPaths(template))
			{
				foreach (string pattern in filePatterns)
				{
					string vp = String.Format(pattern, path);
					if (virtualPath.FileExists(vp))
					{
						Type type = BuildManager.GetCompiledType(vp);
						foreach(Type ofType in validTypes)
							if (ofType.IsAssignableFrom(type))
								return type;
					}
				}
			}
			return null;
		}

		private IEnumerable<string> GetPaths(Type t)
		{
			string name = t.Name;
			string fullName = t.FullName;

			int i = fullName.LastIndexOf('.');
			string ns = i > -1 ? fullName.Substring(0, i).Replace('.', '/') + '/' : null;
			int l = fullName.Length - i - name.Length - 2;

			string controllerName = l > 0 ? fullName.Substring(i + 1, l).Replace('+', '/') + '/' : null;

			string simpleName = name.Length > 4 && name.EndsWith("View", StringComparison.InvariantCultureIgnoreCase) ? name.Substring(0, name.Length - 4) : null;
			string simpleControllerName;
			if (controllerName != null && controllerName.Contains("Controller"))
			{
				string[] cn = controllerName.Split('/');
				for (int ii = 0; ii < cn.Length; ii++)
				{
					string c = cn[ii];
					if (c.Length > 10 && c.EndsWith("Controller", StringComparison.InvariantCultureIgnoreCase))
						cn[ii] = c.Substring(0, c.Length - 10);
				}
				simpleControllerName = string.Join("/", cn);
			}
			else
				simpleControllerName = null;

			string assemblyName = t.Assembly.GetName().Name + '/';

			// Warning: The following is stoopid - Fix?

			yield return assemblyName + ns + controllerName + name;
			if (simpleName != null) yield return assemblyName + ns + controllerName + simpleName;
			if (simpleControllerName != null) yield return assemblyName + ns + simpleControllerName + name;
			if (simpleControllerName != null && simpleName != null) yield return assemblyName + ns + simpleControllerName + simpleName;

			if (ns != null)
			{
				yield return ns + controllerName + name;
				if (simpleName != null) yield return ns + controllerName + simpleName;
				if (simpleControllerName != null) yield return ns + simpleControllerName + name;
				if (simpleControllerName != null && simpleName != null) yield return ns + simpleControllerName + simpleName;
			}

			if (controllerName != null)
			{
				yield return controllerName + name;
				if (simpleName != null) yield return controllerName + simpleName;
				if (simpleControllerName != null) yield return simpleControllerName + name;
				if (simpleControllerName != null && simpleName != null) yield return simpleControllerName + simpleName;
			}

			yield return name;
			if (simpleName != null) yield return simpleName;
		}

		#region Cache
		private Dictionary<Type, Type> _pageTypeCache;
		private Dictionary<Type, Type> _controlTypeCache;
		private Dictionary<Type, string> _masterTypeCache;

		protected IView GetPageInstance(IViewTemplate template)
		{
			Type templateType = template.GetType();
			if (_pageTypeCache == null) _pageTypeCache = new Dictionary<Type, Type>();

			Type viewType;
			if (!_pageTypeCache.TryGetValue(templateType, out viewType))
			{
				lock (_pageTypeCache)
				{
					viewType = GetPageType(templateType);
					//_pageTypeCache.Add(templateType, viewType);
				}
			}
			if (viewType == null)
				return null;
			else
			{
				ViewPage p = (ViewPage)System.Activator.CreateInstance(viewType);
				p.SetTemplate(template);
				return p;
			}
		}

		protected IView GetControlInstance(IViewTemplate template)
		{
			Type templateType = template.GetType();
			if (_controlTypeCache == null) _controlTypeCache = new Dictionary<Type, Type>();

			Type viewType;
			if (!_controlTypeCache.TryGetValue(templateType, out viewType))
			{
				lock (_controlTypeCache)
				{
					viewType = GetControlType(templateType);
					_controlTypeCache.Add(templateType, viewType);
				}
			}
			if (viewType == null)
				return null;
			else
			{
				object o = System.Activator.CreateInstance(viewType);
				ViewPage p = o as ViewPage;
				if (p != null)
					p.SetTemplate(template);
				else
				{
					ViewControl c = o as ViewControl;
					if (c != null)
						c.SetTemplate(template);
				}
				return (IView)o;
			}
		}

		protected IView GetMasterInstance(IViewTemplate template, IView parent)
		{
			Type templateType = template.GetType();

			if (_masterTypeCache == null) _masterTypeCache = new Dictionary<Type, string>();

			string viewPath;
			if (!_masterTypeCache.TryGetValue(templateType, out viewPath))
			{
				lock (_masterTypeCache)
				{
					viewPath = GetMasterType(templateType);
					_masterTypeCache.Add(templateType, viewPath);
				}
			}
			if (viewPath == null)
				return null;

			ViewMaster master;
			ViewPage parentPage = parent as ViewPage;
			if (parentPage != null)
			{
				parentPage.MasterPageFile = viewPath;
				master = parentPage.Master;
			}
			else
			{
				ViewMaster parentMaster = parent as ViewMaster;
				if (parentMaster != null)
				{
					parentMaster.MasterPageFile = viewPath;
					master = parentMaster.Master;
				}
				else
					throw new Exception("Expected ViewMaster or ViewPage");
			}
			master.SetTemplate(template);
			return master;
		}
		#endregion
	}
}
