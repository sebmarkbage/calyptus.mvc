using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.Web.UI;
using System.Web;
using System.Web.Hosting;

namespace Calyptus.MVC
{
    public class WebFormsViewFactory : IViewFactory
    {
		public IView FindView(IHttpContext context, IViewTemplate template)
		{
			IView view = GetPageInstance(template);
			if (view == null) return null;

			/*if (view.Master != null)
			{
				IView master = GetMasterInstance(view.Master);
				//if (path == null)
				//	return null;
				//page.MasterPageFile = path;
			}*/

			return view;
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

		private Type GetMasterType(Type t)
		{
			return GetType(t, typeof(ViewMaster<>).MakeGenericType(t), "~/Views/{0}.master");
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
			if (controllerName.Contains("Controller"))
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
		private Dictionary<Type, Type> _masterTypeCache;

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
				return Create(viewType, template);
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
				return Create(viewType, template);
		}

		protected IView GetMasterInstance(IViewTemplate template)
		{
			Type templateType = template.GetType();

			if (_masterTypeCache == null) _masterTypeCache = new Dictionary<Type, Type>();

			Type viewType;
			if (!_masterTypeCache.TryGetValue(templateType, out viewType))
			{
				lock (_masterTypeCache)
				{
					viewType = GetMasterType(templateType);
					_masterTypeCache.Add(templateType, viewType);
				}
			}
			if (viewType == null)
				return null;
			else
				return Create(viewType, template);
		}
		#endregion

		private IView Create(Type type, IViewTemplate template)
		{
			Type t = typeof(IView<>).MakeGenericType(template.GetType());
			IView v = (IView)Activator.CreateInstance(type);
			System.Reflection.PropertyInfo p = t.GetProperty("Template");
			p.SetValue(v, template, null);
			return v;
		}
	}
}
