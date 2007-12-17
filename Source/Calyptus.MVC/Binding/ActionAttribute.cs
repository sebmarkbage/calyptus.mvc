using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Binding
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionAttribute : PathBinderBase
    {
        public ActionAttribute() : base() { }

        public ActionAttribute(string keyword) : base(keyword) { }

        public ActionAttribute(string keywordResourceBaseName, string keywordResourceName) : base(keywordResourceBaseName, keywordResourceName) { }

        public ActionAttribute(string keywordResourceAssembly, string keywordResourceBaseName, string keywordResourceName) : base(keywordResourceAssembly, keywordResourceBaseName, keywordResourceName) { }
    }
}
