using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.MVC.Internal;

namespace Calyptus.MVC.Binding
{
    public abstract class PathBinderBase : Attribute, IKeywordBinder
    {
        private IKeyword _keyword;

        public PathBinderBase()
        {
        }

        public PathBinderBase(string keyword)
        {
            this._keyword = new PlainKeyword(keyword);
        }

        public PathBinderBase(string keywordResourceBaseName, string keywordResourceName)
        {
            this._keyword = new ResourceKeyword(System.Reflection.Assembly.GetExecutingAssembly(), keywordResourceBaseName, keywordResourceName);
        }

        public PathBinderBase(string keywordResourceAssembly, string keywordResourceBaseName, string keywordResourceName)
        {
            this._keyword = new ResourceKeyword(System.Reflection.Assembly.Load(keywordResourceAssembly), keywordResourceBaseName, keywordResourceName);
        }

        public string GetKeyword()
        {
            return _keyword.Keyword;
        }
    }
}
