using System;
using System.Web;

namespace Calyptus.Mvc.Asp
{
	public class AspHttpRequest : IHttpRequest
	{
		private AspHttpConnection connection;
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

		public AspHttpRequest(AspHttpConnection connection)
		{
			this.connection = connection;
			this.context = connection.Context;
			this.workerRequest = connection.WorkerRequest;
		}

		public IHttpConnection Connection
		{
			get { return connection; }
		}

		public string Version
		{
			get { return workerRequest.GetHttpVersion(); }
		}

		public string Verb
		{
			get { return workerRequest.GetHttpVerbName(); }
		}

		public string Path
		{
			get { return workerRequest.GetUriPath(); }
		}

		public string Query
		{
			get { return workerRequest.GetQueryString(); }
		}

		public string ReadHeader(string name)
		{
			int i = HttpWorkerRequest.GetKnownRequestHeaderIndex(name);
			if (i == -1)
				return workerRequest.GetUnknownRequestHeader(name);
			else
				return workerRequest.GetKnownRequestHeader(i);
		}

		private int position;

		public int ReadBody(byte[] buffer, int offset, int count)
		{
			byte[] preloaded = workerRequest.GetPreloadedEntityBody();
			if (preloaded != null)
			{
				int readBytes = preloaded.Length - position;
				if (readBytes > 0)
				{
					if (position + count <= preloaded.Length)
					{
						Buffer.BlockCopy(preloaded, position, buffer, offset, count);
						position += count;
						return count;
					}
					else
					{
						Buffer.BlockCopy(preloaded, position, buffer, offset, readBytes);
						position += readBytes;
						int bytesRead = workerRequest.ReadEntityBody(buffer, position, count - readBytes);
						return readBytes + bytesRead;
					}
				}
				if (workerRequest.IsEntireEntityBodyIsPreloaded()) return 0;
			}
			return workerRequest.ReadEntityBody(buffer, offset, count);
		}
	}
}