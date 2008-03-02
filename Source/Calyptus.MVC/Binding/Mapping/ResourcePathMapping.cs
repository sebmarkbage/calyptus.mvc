using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace Calyptus.MVC.Binding.Mapping
{
	public class ResourcePathMapping : IMappingBinding
	{
		private Assembly assembly;
		private string basename;
		private string name;
		private Dictionary<CultureInfo, string[]> cultureCache;

		private string[] keywords
		{
			get
			{
				string[] keywords;
				CultureInfo c = CultureInfo.CurrentUICulture;
				if (!cultureCache.TryGetValue(c, out keywords))
				{
					lock (cultureCache)
					{
						ResourceManager mgr = new ResourceManager(basename, assembly);
						keywords = mgr.GetString(name, c).Trim('/').Split('/');
						cultureCache.Add(c.LCID, keywords);
					}
				}
				return keywords;
			}
		}

		public ResourcePathMapping(Assembly assembly, string basename, string name)
		{
			this.assembly = assembly;
			this.basename = basename;
			this.name = name;

			this.cultureCache = new Dictionary<CultureInfo, string[]>();
			CultureInfo invariant = CultureInfo.InvariantCulture;
			this.cultureCache.Add(invariant, new ResourceManager(basename, assembly).GetString(name, invariant).Trim('/').Split('/'));
		}

		public bool TryMapping(IHttpContext context, IPathStack path)
		{
			int index = path.Index;
			foreach (string keyword in keywords)
				if (!keyword.Equals(path.Pop(), StringComparison.CurrentCultureIgnoreCase))
				{
					path.ReverseToIndex(index);
					return false;
				}
			return true;
		}

		public void SerializeToPath(IPathStack path)
		{
			foreach (string keyword in keywords)
				path.Push(keyword);
		}
	}
}
