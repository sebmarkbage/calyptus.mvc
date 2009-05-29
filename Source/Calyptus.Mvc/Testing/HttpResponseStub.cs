using System;
using System.Web;
using System.Collections.Generic;

namespace Calyptus.Mvc.Asp
{
	public class HttpResponseStub : IHttpResponse
	{
		private bool statusSent;
		private bool headersSent;

		public HttpResponseStub(IHttpConnection connection)
		{
			Connection = connection;
			Headers = new Dictionary<string, string>();
		}

		public IHttpConnection Connection
		{
			get;
			set;
		}

		public int StatusCode
		{
			get;
			set;
		}

		public string StatusDescription
		{
			get;
			set;
		}

		public IDictionary<string, string> Headers
		{
			get;
			private set;
		}

		void IHttpResponse.WriteStatus(int code, string description)
		{
			if (statusSent) throw new Exception("The status is already sent to the client.");
			StatusCode = code;
			StatusDescription = description;
			statusSent = true;
		}

		void IHttpResponse.WriteHeader(string name, string value)
		{
			if (!statusSent) throw new Exception("Status should be sent before the headers.");
			if (headersSent) throw new Exception("Headers are already sent to the client.");
			Headers.Add(name, value);
		}

		void IHttpResponse.WriteBody(byte[] buffer, int offset, int length)
		{
			statusSent = true;
			headersSent = true;
		}
	}
}