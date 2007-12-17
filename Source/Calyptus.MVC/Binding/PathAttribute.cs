using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Binding
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter)]
    public class PathAttribute : PathBinderBase
    {
        public PathAttribute() : base() { }
        public PathAttribute(string keyword) : base(keyword) { }
        public PathAttribute(string keywordResourceBaseName, string keywordResourceName) : base(keywordResourceBaseName, keywordResourceName) { }
        public PathAttribute(string keywordResourceAssembly, string keywordResourceBaseName, string keywordResourceName) : base(keywordResourceAssembly, keywordResourceBaseName, keywordResourceName) { }

        public string ValidationRegEx { get; set; }

        public bool IsNullable { get; set; }
        public string NullValue { get; set; }
    }
}
