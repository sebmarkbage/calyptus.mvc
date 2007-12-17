using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Binding
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter)]
    public class ParentAttribute : PathAttribute
    {
        public ParentAttribute() : base() { }
        public ParentAttribute(string keyword) : base(keyword) { }
        public ParentAttribute(string keywordResourceBaseName, string keywordResourceName) : base(keywordResourceBaseName, keywordResourceName) { }
        public ParentAttribute(string keywordResourceAssembly, string keywordResourceBaseName, string keywordResourceName) : base(keywordResourceAssembly, keywordResourceBaseName, keywordResourceName) { }
    }
}
