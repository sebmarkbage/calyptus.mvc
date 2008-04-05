using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Mapping
{
	public class ContentTypeMapping : IMappingBinding
	{
		private string[] contentTypes;

		public ContentTypeMapping(string contentType) : this(contentType == null || contentType.Trim() == "" || contentType.Trim() == "*" ? null : new string[] { contentType }) { }

		public ContentTypeMapping(string[] contentTypes)
		{
			if (contentTypes == null) return;
			this.contentTypes = new string[contentTypes.Length];
			for (int i = 0; i < contentTypes.Length; i++)
			{
				string v = contentTypes[i].Trim().ToLower();
				int s = v.IndexOf(';');
				this.contentTypes[i] = s > -1 ? v.Substring(0, s) : v;
			}
		}

		public bool TryMapping(IHttpContext context, IPathStack path)
		{
			string ct = context.Request.ContentType;
			int s = ct.IndexOf(';');
			if (s > -1) ct = ct.Substring(0, s);
			foreach (string contentType in contentTypes)
				if (String.Equals(contentType, ct))
					return true;
			return false;
		}

		public void SerializeToPath(IPathStack path)
		{
		}
	}
}
