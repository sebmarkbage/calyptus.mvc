using System;
using System.Configuration;
using System.Xml;
using System.Web.Configuration;

namespace Calyptus.MVC.Configuration
{
    public class Config : ConfigurationSection
    {
		[ConfigurationProperty("routingEngine", IsRequired = false)]
		public string RoutingEngine
		{
			get
			{
				return (string)this["routingEngine"];
			}
			set
			{
				this["routingEngine"] = value;
			}
		}

		[ConfigurationProperty("requireExplicitActions", IsRequired = false)]
		public bool RequireExplicitActions
		{
			get
			{
				return (bool)this["requireExplicitActions"];
			}
			set
			{
				this["requireExplicitActions"] = value;
			}
		}

		[ConfigurationProperty("defaultViewEngine", IsRequired = false)]
		public string DefaultViewEngine
		{
			get
			{
				return (string)this["defaultViewEngine"];
			}
			set
			{
				this["defaultViewEngine"] = value;
			}
		}
    }
}
