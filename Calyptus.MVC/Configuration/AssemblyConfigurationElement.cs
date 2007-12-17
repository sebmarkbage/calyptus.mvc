using System;
using System.Configuration;
namespace Calyptus.MVC.Configuration
{
    public class AssemblyConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("assembly", IsRequired=true, IsKey=true)]
        public string Name
        {
            get
            {
                return (string)this["assembly"];
            }
            set
            {
                this["assembly"] = value;
            }
        }
    }
}
