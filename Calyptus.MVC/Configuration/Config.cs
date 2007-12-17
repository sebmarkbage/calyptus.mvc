using System;
using System.Configuration;
using System.Xml;
using System.Web.Configuration;

namespace Calyptus.MVC.Configuration
{
    public class Config : ConfigurationSection
    {
        [ConfigurationProperty("assemblies")]
        public AssemblyCollection Assemblies
        {
            get
            {
                return (AssemblyCollection)this["assemblies"];
            }
        }
    }
}
