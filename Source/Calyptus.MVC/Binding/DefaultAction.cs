using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Binding
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
    public class DefaultActionAttribute : ActionAttribute
    {
		public DefaultActionAttribute() : base(new DefaultKeyword())
		{
		}
    }
}
