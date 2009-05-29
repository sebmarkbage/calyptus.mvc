using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.Mvc.Mapping
{
	public class VerbMapping : IRequestConstraint
	{
		private string[] verbs;

		public VerbMapping(string verbs) : this(verbs == null || verbs.Trim() == "" || verbs.Trim() == "*" ? null : verbs.Split(',')) { }

		public VerbMapping(string[] verbs)
		{
			if (verbs == null) return;
			this.verbs = new string[verbs.Length];
			for (int i = 0; i < verbs.Length; i++)
				this.verbs[i] = verbs[i].Trim().ToUpper();
		}

		public bool TryMapping(IHttpContext context, IPathStack path)
		{
			if (verbs == null)
				return true;
			string rv = context.Request.HttpMethod;
			foreach (string verb in verbs)
				if (rv.Equals(verb))
					return true;
			return false;
		}

		public void SerializeToPath(IPathStack path)
		{
		}
	}
}
