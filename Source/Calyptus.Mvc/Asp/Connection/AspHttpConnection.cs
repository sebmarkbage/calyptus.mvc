using System;
using System.Web;

namespace Calyptus.Mvc.Asp
{
	public class AspHttpConnection : IHttpConnection
	{
		public static AspHttpConnection Create(HttpContext context)
		{
			HttpWorkerRequest workerRequest = ((IServiceProvider)context).GetService(typeof(HttpWorkerRequest)) as HttpWorkerRequest;
			return (workerRequest.IsSecure) ? new AspHttpSecureConnection(context, workerRequest) : new AspHttpConnection(context, workerRequest);
		}
	
		private HttpContext context;
		private HttpWorkerRequest workerRequest;

		public HttpContext Context
		{
			get
			{
				return context;
			}
		}

		public HttpWorkerRequest WorkerRequest
		{
			get
			{
				return workerRequest;
			}
		}

		internal AspHttpConnection(HttpContext context, HttpWorkerRequest request)
		{
			this.context = context;
			this.workerRequest = request;
		}

		public string LocalAddress
		{
			get { return workerRequest.GetLocalAddress(); }
		}

		public string LocalName
		{
			get { return workerRequest.GetServerName(); }
		}

		public int LocalPort
		{
			get { return workerRequest.GetLocalPort(); }
		}

		public string RemoteAddress
		{
			get { return workerRequest.GetRemoteAddress(); }
		}

		public string RemoteName
		{
			get { return workerRequest.GetRemoteName(); }
		}

		public int RemotePort
		{
			get { return workerRequest.GetRemotePort(); }
		}

		public bool IsConnected
		{
			get { return workerRequest.IsClientConnected(); }
		}
	}

	public class AspHttpSecureConnection : AspHttpConnection, IHttpSecureConnection
	{
		private HttpClientCertificate ccert;
		private HttpServerCertificate scert;
		
		internal AspHttpSecureConnection(HttpContext context, HttpWorkerRequest request)
		{
			this.context = context;
			this.workerRequest = request;
		}

		public HttpClientCertificate ClientCertificate
		{
			get
			{
				if (ccert == null)
				{
					// TODO
				}
				return ccert;
			}
		}

		public HttpServerCertificate ServerCertificate
		{
			get
			{
				if (scert == null)
				{
					// TODO
				}
				return scert;
			}
		}
	}
}