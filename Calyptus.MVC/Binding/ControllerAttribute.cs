using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Binding
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor)]
    public class ControllerAttribute : PathBinderBase
    {
        public ControllerAttribute() : base() { }

        public ControllerAttribute(string keyword) : base(keyword) { }

        public ControllerAttribute(string keywordResourceBaseName, string keywordResourceName) : base(keywordResourceBaseName, keywordResourceName) { }

        public ControllerAttribute(string keywordResourceAssembly, string keywordResourceBaseName, string keywordResourceName) : base(keywordResourceAssembly, keywordResourceBaseName, keywordResourceName) { }
    }
}
