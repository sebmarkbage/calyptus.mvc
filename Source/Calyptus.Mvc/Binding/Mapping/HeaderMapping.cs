using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.Mvc.Mapping
{
	public class HeaderMapping : IMappingBinding
	{
		private string header;
		private string value;

		public HeaderMapping(string header, string value)
		{
			this.header = header;
			this.value = value;
		}

		public bool TryMapping(IHttpContext context, IPathStack path)
		{
			return String.Equals(context.Request.Headers[header], value);
		}

		public void SerializeToPath(IPathStack path) { }
	}
}
