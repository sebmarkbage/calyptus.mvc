using System;
using System.Collections.Generic;
using System.Web;
using System.Reflection;
using System.Web.Configuration;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Xml;
using Calyptus.Mvc.Mapping;

namespace Calyptus.Mvc
{
    public class Routing : IHttpModule
    {
		static Routing()
		{
			_mappings = new MappingContext();
			_mappings.BindingFactories.Add(new AttributeMappingFactory());

			if (HttpContext.Current != null)
			{
				bool foundMe = false;
				foreach (object o in HttpContext.Current.ApplicationInstance.Modules)
					if (o is Routing)
					{
						foundMe = true;
						break;
					}
				if (!foundMe)
					throw new ApplicationException("The current HttpApplication hasn't configured the Routing HttpModule in Web.config."); 
			}
		}

		private static bool initialized;

		private static MappingContext _mappings;
		public static IMappingContext Mapping
		{
			get
			{
				initialized = true;
				return _mappings;
			}
		}

		public static bool RouteExistingPaths { get; set; }
		public static string RouteExtension { get; set; }

		public static void AddEntryController<T>()
		{
			AddEntryController<T>(null);
		}

		public static void AddEntryController<T>(string path)
		{
			throw new NotImplementedException();
		}

		public static void AddEntryController(IEntryMapping binding)
		{
			AddEntryController(null, binding);
		}

		public static void AddEntryController(string path, IEntryMapping binding)
		{
			throw new NotImplementedException();
		}

		public static void AddXmlMapping(string uri)
		{
			AddXmlMapping(XmlReader.Create(uri));
		}

		public static void AddXmlMapping(XmlReader reader)
		{
			throw new NotImplementedException();
		}

		private static object _requestDataKey = new object();
		//private static string[] _defaultDocuments = new string[] { "/Default.aspx" };
		private static string defaultDocument = "/Default.aspx";

		void IHttpModule.Init(HttpApplication app)
		{
			app.PostResolveRequestCache += new EventHandler(Route);
			app.PostMapRequestHandler += new EventHandler(AttachHandler);
			app.EndRequest += new EventHandler(CleanUp);
        }

		void AutoFillEntryControllers()
		{
			lock(_mappings)
			{
				AttributeMappingFactory factory = null;
				foreach (IMappingConvention f in _mappings.BindingFactories)
				{
					factory = f as AttributeMappingFactory;
					if (factory != null) break;
				}
				if (factory == null) return;
				foreach (Assembly a in System.Web.Compilation.BuildManager.GetReferencedAssemblies())
					foreach (Type t in a.GetTypes())
						foreach (var b in factory.GetEntryBindings(t))
							_mappings.EntryBindings.Add(b);
			}
		}

		private static string[] _ignorePaths;

		bool IgnorePath(HttpRequest request)
		{
			if (_ignorePaths == null)
			{
				// TODO: Use virtual directory
				DirectoryInfo dir = new DirectoryInfo(request.MapPath("~/"));
				List<string> paths = new List<string>();
				foreach (FileInfo file in dir.GetFiles())
					if (!file.Name.Equals("web.config", StringComparison.InvariantCultureIgnoreCase) && !file.Name.Equals("default.aspx", StringComparison.InvariantCultureIgnoreCase))
						paths.Add(file.Name);
				foreach (DirectoryInfo d in dir.GetDirectories())
					if (!d.Name.Equals("App_Data", StringComparison.InvariantCultureIgnoreCase) && !d.Name.Equals("App_Code", StringComparison.InvariantCultureIgnoreCase))
						paths.Add(d.Name);
				_ignorePaths = paths.ToArray();
			}
			string p = request.AppRelativeCurrentExecutionFilePath;
			int i = p.IndexOf('/', 2);
			p = i < 0 ? p.Substring(2) : p.Substring(2, i - 2);
			foreach(string s in _ignorePaths)
				if (s.Equals(p, StringComparison.InvariantCultureIgnoreCase))
					return true;
			return false;
		}

        void Route(object s, EventArgs e)
        {
			if (!initialized) AutoFillEntryControllers();

            HttpContext context = ((HttpApplication)s).Context;
            HttpRequest request = context.Request;

			if (!RouteExistingPaths && IgnorePath(request)) return;

			string appRelPath = request.AppRelativeCurrentExecutionFilePath;

			int l = appRelPath.Length - 2;
			//foreach(string defaultDocument in _defaultDocuments)
				if (appRelPath.EndsWith(defaultDocument, StringComparison.InvariantCultureIgnoreCase))
				{
					l -= defaultDocument.Length;
					//break;
				}

			string path = ((l <= 0 ? null : request.AppRelativeCurrentExecutionFilePath.Substring(2, l)) + request.PathInfo);

			/*string ext = RouteExtension;
			if (ext != null && fp.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase))
				_path[0] = fp.Substring(0, fp.Length - ext.Length);*/

            //PathStack stack = new PathStack(request.HttpMethod, path, context.Request.QueryString, true);

			IResultBinding binding = _mappings.ParseRoute(new HttpRequestWrapper(request));
			if (binding != null)
			{
				context.Items[_requestDataKey] = new RouteData {
					Binding = binding,
					OriginalPath = context.Request.Path
				};
				context.RewritePath("~/Calyptus.Mvc.axd");
			}
			else
				route.Dispose();
        }

		void AttachHandler(object sender, EventArgs e)
		{
			HttpContext context = ((HttpApplication)sender).Context;
			RouteData data = context.Items[_requestDataKey] as RouteData;
			if (data != null)
			{
				context.RewritePath(data.OriginalPath);
				IAsyncResultBinding asyncBinding = data.Binding as IAsyncResultBinding;
				context.Handler = asyncBinding != null ? new AsyncRouteBindingHttpHandler(asyncBinding) : new RouteBindingHttpHandler(data.Binding);
			}
		}

		void CleanUp(object s, EventArgs e)
		{
			HttpContext context = ((HttpApplication)s).Context;
			RouteData data = context.Items[_requestDataKey] as RouteData;
			if (data != null)
			{
				data.Route.Dispose();
				context.Items.Remove(_requestDataKey);
			}
		}

		void IHttpModule.Dispose()
		{
		}

		private class RouteData
		{
			public string OriginalPath;
			public IResultBinding Binding;
		}
	}
}
