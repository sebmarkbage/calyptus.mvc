using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calyptus.Mvc.Asp
{
	public class HttpRequestStub : IHttpRequest
	{
		public HttpRequestStub(IHttpConnection connection)
		{
			Headers = new Dictionary<string, string>();
			Connection = connection;
			Verb = "GET";
			Path = "/";
		}

		public HttpRequestStub() : this(HttpConnectionStub.Instance)
		{
		}

		public HttpRequestStub(string verb, Uri uri) : this(
			uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) ?
			new HttpSecureConnectionStub() { LocalPort = uri.Port } :
			new HttpConnectionStub() { LocalPort = uri.Port }
		)
		{
			Verb = verb;
			Headers.Add("Host", uri.Host);
			Path = uri.AbsolutePath;
			Query = uri.Query;
		}

		public HttpRequestStub(string verb, string pathAndQuery) : this()
		{
			Verb = verb;
			int i = pathAndQuery.IndexOf('?');
			if (i > 0)
			{
				Path = pathAndQuery.Substring(0, i);
				Query = pathAndQuery.Substring(i + 1);
			}
			else
			{
				Path = pathAndQuery;
			}
		}

		public HttpRequestStub(string verb, Uri uri, string body) : this(verb, uri)
		{
			Body = body;
		}

		public HttpRequestStub(string verb, string pathAndQuery, string body) : this(verb, pathAndQuery)
		{
			Body = body;
		}

		public HttpRequestStub(string verb, Uri uri, Stream body) : this(verb, uri)
		{
			data = body;
		}

		public HttpRequestStub(string verb, string pathAndQuery, Stream body) : this(verb, pathAndQuery)
		{
			data = body;
		}

		public IHttpConnection Connection
		{
			get;
			set;
		}

		private string version;
		public string Version
		{
			get { return version ?? "HTTP/1.0"; }
			set { version = value; }
		}

		public string Verb
		{
			get;
			set;
		}

		public string Path
		{
			get;
			set;
		}

		public string Query
		{
			get;
			set;
		}

		public IDictionary<string, string> Headers
		{
			get;
			set;
		}

		private Stream data;
		private string body;
		public string Body
		{
			get
			{
				return body;
			}
			set
			{
				body = value;
				data = null;
			}
		}

		string IHttpRequest.ReadHeader(string name)
		{
			if (Headers == null) return null;
			string v;
			return Headers.TryGetValue(name, out v) ? v : null;
		}

		int IHttpRequest.ReadBody(byte[] buffer, int offset, int count)
		{
			if (data == null)
			{
				if (body == null) return 0;
				Encoding encoding = Encoding.GetEncoding(28591);
				data = new MemoryStream(encoding.GetBytes(body));
			}
			return data.Read(buffer, offset, count);
		}
	}
}