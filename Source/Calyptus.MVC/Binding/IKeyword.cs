using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;

namespace Calyptus.MVC
{
    public interface IKeyword
    {
		bool Try(IPathStack path);
		void Serialize(IPathStack path);
    }

	internal class DefaultKeyword : IKeyword
	{
		public bool Try(IPathStack path)
		{
			return true;
		}

		public void Serialize(IPathStack path)
		{
		}
	}

    internal class PlainKeyword : IKeyword
    {
        private string keyword;

        public PlainKeyword(string keyword)
        {
            this.keyword = keyword + Configuration.Config.GetKeywordExtension();
		}

		public bool Try(IPathStack path)
		{
			if (keyword.Equals(path.Peek(), StringComparison.CurrentCultureIgnoreCase))
			{
				path.Pop();
				return true;
			}
			else
				return false;
		}

		public void Serialize(IPathStack path)
		{
			path.Push(keyword);
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

		public bool Try(IPathStack path)
		{
			string keyword = mgr.GetString(name) + Configuration.Config.GetKeywordExtension();
			if (keyword.Equals(path.Peek(), StringComparison.CurrentCultureIgnoreCase))
			{
				path.Pop();
				return true;
			}
			else
				return false;
		}

		public void Serialize(IPathStack path)
		{
			path.Push(mgr.GetString(name) + Configuration.Config.GetKeywordExtension());
		}
	}
}
