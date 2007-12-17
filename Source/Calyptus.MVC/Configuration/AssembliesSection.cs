using System;
using System.Configuration;
using System.Xml;

namespace Calyptus.MVC.Configuration
{
    public class AssembliesSection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssemblyConfigurationElement)element).Name;
        }
    }
}
