using System;
using System.Web;

namespace Calyptus.Mvc.Asp
{
	public class HttpConnectionStub : IHttpConnection
	{
		private static HttpConnectionStub _instance;
		public static HttpConnectionStub Instance
		{
			get
			{
				if (_instance == null) _instance = new HttpConnectionStub();
				return _instance;
			}
		}

		private static HttpSecureConnectionStub _sec;
		public static HttpSecureConnectionStub Secure
		{
			get
			{
				if (_sec == null) _sec = new HttpSecureConnectionStub();
				return _sec;
			}
		}

		public HttpConnectionStub()
		{
			LocalAddress = "127.0.0.1";
			LocalPort = 80;
			RemoteAddress = "127.0.0.1";
			RemotePort = 0;
		}

		public string LocalAddress
		{
			get;
			set;
		}

		private string lname;
		public string LocalName
		{
			get
			{
				return lname ?? LocalAddress;
			}
			set
			{
				lname = value;
			}
		}

		public int LocalPort
		{
			get;
			set;
		}

		public string RemoteAddress
		{
			get;
			set;
		}

		private string rname;
		public string RemoteName
		{
			get
			{
				return rname ?? RemoteAddress;
			}
			set
			{
				rname = value;
			}
		}

		public int RemotePort
		{
			get { return 0; }
		}

		public bool IsConnected
		{
			get { return true; }
		}
	}

	public class HttpSecureConnectionStub : HttpConnectionStub, IHttpSecureConnection
	{
		public HttpClientCertificate ClientCertificate
		{
			get;
			set;
		}

		public HttpServerCertificate ServerCertificate
		{
			get;
			set;
		}
	}
}