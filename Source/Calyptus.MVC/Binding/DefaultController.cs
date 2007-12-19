using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Binding
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class DefaultControllerAttribute : ControllerAttribute
    {
		public DefaultControllerAttribute() : base(new DefaultKeyword())
		{
		}
    }
}
