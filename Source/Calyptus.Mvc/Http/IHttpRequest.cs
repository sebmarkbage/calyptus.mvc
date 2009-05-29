using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Security.Principal;
using System.Web.Caching;
using System.Web.Profile;
using System.Collections.Specialized;
using System.IO;

namespace Calyptus.Mvc
{
	public interface IHttpRequest
	{
		IHttpConnection Connection { get; }

		string Version { get; }

		string Verb { get; }

		string Path { get; }

		string Query { get; }

		string ReadHeader(string name);

		int ReadBody(byte[] buffer, int offset, int count);
	}
}
