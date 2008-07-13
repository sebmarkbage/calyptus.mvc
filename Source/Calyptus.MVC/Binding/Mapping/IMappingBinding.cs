using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Collections.Specialized;
using System.Globalization;

namespace Calyptus.MVC
{
    public interface IMappingBinding
    {
		bool TryMapping(IHttpContext context, IPathStack path);
		void SerializeToPath(IPathStack path);
	}
}
