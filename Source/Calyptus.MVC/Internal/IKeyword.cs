using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;

namespace Calyptus.MVC.Internal
{
    internal interface IKeyword
    {
        string Keyword { get; }
    }

    internal class PlainKeyword : IKeyword
    {
        private string keyword;

        public PlainKeyword(string keyword)
        {
            this.keyword = keyword;
        }

        public string Keyword
        {
            get
            {
                return keyword;
            }
        }
    }

    internal class ResourceKeyword : IKeyword
    {
        private ResourceManager mgr;
        private string name;

        public ResourceKeyword(System.Reflection.Assembly assembly, string basename, string name)
        {
            this.mgr = new ResourceManager(basename, assembly);
            this.name = name;
        }

        public string Keyword
        {
            get
            {
                return mgr.GetString(name);
            }
        }
    }
}
