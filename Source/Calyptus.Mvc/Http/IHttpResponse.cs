using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Security.Principal;
using System.Web.Caching;
using System.Web.Profile;
using System.IO;
using System.Collections.Specialized;

namespace Calyptus.Mvc
{
	public interface IHttpResponse
	{
		IHttpConnection Connection { get; }

		void WriteStatus(int code, string description);

		void WriteHeader(string name, string value);

		void WriteBody(byte[] buffer, int offset, int length);
	}
}
