using System;
using System.Configuration;
using System.Xml;
using System.Web.Configuration;

namespace Calyptus.MVC.Configuration
{
    public class Config : ConfigurationSection
	{
		public static Config Current { get; private set; }
		
		private IRoutingEngine _routingEngine;
		private IViewFactory _viewFactory;
		private string _useExtension;
		private bool _useExtensionSet;

		static Config()
		{
			Current = ConfigurationManager.GetSection("calyptus.mvc") as Config;
			if (Current == null) Current = new Config();
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

		[ConfigurationProperty("viewFactory", IsRequired = false)]
		public string ViewFactory
		{
			get
			{
				var r = (string)this["viewFactory"];
				if (r == "")
					return null;
				else
					return r;
			}
			set
			{
				this["viewFactory"] = value;
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

		public static IViewFactory GetViewFactory()
		{
			if (Current._viewFactory == null)
			{
				lock (Current)
				{
					string value = Current.ViewFactory;
					if (value == null)
						Current._viewFactory = new WebFormsViewFactory();
					else
					{
						Type t = Type.GetType(value);
						if (t == null)
							throw new ConfigurationErrorsException(String.Format("View engine \"{0}\" could not be found.", value));
						if (!typeof(IViewFactory).IsAssignableFrom(t))
							throw new ConfigurationErrorsException(String.Format("View engine \"{0}\" is not an IViewEngine.", value));
						Current._viewFactory = (IViewFactory)Activator.CreateInstance(t);
					}
				}
			}
			return Current._viewFactory;
		}
    }
}
