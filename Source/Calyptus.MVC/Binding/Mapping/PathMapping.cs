using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Resources;

namespace Calyptus.Mvc.Mapping
{
	public class PathMapping : IMappingBinding
	{
		private string[] keywords;

		public PathMapping(string path)
		{
			this.keywords = path.Trim('/').Split('/');
		}

		public override string ToString()
		{
			return string.Join("/", keywords);
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
