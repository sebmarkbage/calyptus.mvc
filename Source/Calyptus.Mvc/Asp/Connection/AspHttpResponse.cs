using System;
using System.Web;

namespace Calyptus.Mvc.Asp
{
	public class AspHttpResponse : IHttpResponse
	{
		private AspHttpConnection connection;
		private HttpContext context;
		private HttpWorkerRequest workerRequest;

		private bool statusSent;

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

		public AspHttpResponse(AspHttpConnection connection)
		{
			this.connection = connection;
			this.context = connection.Context;
			this.workerRequest = connection.WorkerRequest;
		}

		public IHttpConnection Connection
		{
			get { return connection; }
		}

		public void WriteStatus(int code, string description)
		{
			if (statusSent || workerRequest.HeadersSent) throw new HttpException("The status is already sent to the client.");
			if (description == null)
				description = HttpWorkerRequest.GetStatusDescription(code);
			workerRequest.SendStatus(code, description);
			statusSent = true;
		}

		public void WriteHeader(string name, string value)
		{
			if (!statusSent) throw new HttpException("Status should be sent before the headers.");
			if (workerRequest.HeadersSent) throw new HttpException("Headers are already sent to the client.");
			int i = HttpWorkerRequest.GetKnownResponseHeaderIndex(name);
			if (i == -1)
				workerRequest.SendUnknownResponseHeader(name, value);
			else
				workerRequest.SendKnownResponseHeader(i, value);
		}

		public void WriteBody(byte[] buffer, int offset, int length)
		{
			statusSent = true;
			if (offset > 0)
			{
				byte[] data = new byte[length];
				for (int i = 0; i < length; i++)
					data[i] = buffer[offset + i];
				workerRequest.SendResponseFromMemory(data, length);
			}
			else
				workerRequest.SendResponseFromMemory(buffer, length);
		}
	}
}