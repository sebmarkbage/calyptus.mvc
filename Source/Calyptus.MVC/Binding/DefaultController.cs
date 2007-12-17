using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Binding
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Class)]
    public class DefaultControllerAttribute : ControllerAttribute
    {
    }
}
