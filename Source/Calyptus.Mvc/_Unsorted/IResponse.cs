using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.Mvc
{
	public interface IResponse
	{
		void WriteHeaders(IHttpResponse headers);
		void WriteContent(IHttpResponse content);
	}
}
