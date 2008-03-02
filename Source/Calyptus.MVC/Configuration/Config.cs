using System;
using System.Configuration;
using System.Xml;
using System.Web.Configuration;
using Calyptus.MVC.ViewEngines;
using Calyptus.MVC.RoutingEngines;

namespace Calyptus.MVC.Configuration
{
    public class Config : ConfigurationSection
	{
		public static Config Current { get; private set; }
		
		private IRoutingEngine _routingEngine;
		private IViewEngine _defaultViewEngine;
		private string _useExtension;
		private bool _useExtensionSet;

		static Config()
		{
			Current = (Config) ConfigurationManager.GetSection("calyptus.mvc");
		}

		[ConfigurationProperty("extension", IsRequired = false)]
		public string Extension
		{
			get
			{
				var r = (string)this["extension"];
				if (r == "")
					return null;
				else
					return r;
			}
			set
			{
				this["extension"] = value;
			}
		}

		[ConfigurationProperty("routingEngine", IsRequired = false)]
		public string RoutingEngine
		{
			get
			{
				var r = (string)this["routingEngine"];
				if (r == "")
					return null;
				else
					return r;
			}
			set
			{
				this["routingEngine"] = value;
			}
		}

		[ConfigurationProperty("defaultViewEngine", IsRequired = false)]
		public string DefaultViewEngine
		{
			get
			{
				var r = (string)this["defaultViewEngine"];
				if (r == "")
					return null;
				else
					return r;
			}
			set
			{
				this["defaultViewEngine"] = value;
			}
		}

		public static string GetExtension()
		{
			if (!Current._useExtensionSet)
			{
				Current._useExtension = Current.Extension;
				Current._useExtensionSet = true;
			}
			return Current._useExtension;
		}

		public static IRoutingEngine GetRoutingEngine()
		{
			if (Current._routingEngine == null)
			{
				lock (Current)
				{
					string value = Current.RoutingEngine;
					if (value == null)
						Current._routingEngine = new AttributeRoutingEngine();
					else
					{
						Type t = Type.GetType(value);
						if (t == null)
							throw new ConfigurationErrorsException(String.Format("Routing engine \"{0}\" could not be found.", value));
						if (!typeof(IRoutingEngine).IsAssignableFrom(t))
							throw new ConfigurationErrorsException(String.Format("Routing engine \"{0}\" is not an IRoutingEngine.", value));
						Current._routingEngine = (IRoutingEngine)Activator.CreateInstance(t);
					}
				}
			}
			return Current._routingEngine;
		}

		public static IViewEngine GetDefaultViewEngine()
		{
			if (Current._defaultViewEngine == null)
			{
				lock (Current)
				{
					string value = Current.DefaultViewEngine;
					if (value == null)
						Current._defaultViewEngine = new WebFormsViewEngine();
					else
					{
						Type t = Type.GetType(value);
						if (t == null)
							throw new ConfigurationErrorsException(String.Format("View engine \"{0}\" could not be found.", value));
						if (!typeof(IViewEngine).IsAssignableFrom(t))
							throw new ConfigurationErrorsException(String.Format("View engine \"{0}\" is not an IViewEngine.", value));
						Current._defaultViewEngine = (IViewEngine)Activator.CreateInstance(t);
					}
				}
			}
			return Current._defaultViewEngine;
		}
    }
}
