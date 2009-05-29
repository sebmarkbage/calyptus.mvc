using System;

namespace Calyptus.Mvc
{
	public interface IResultBinding : IDisposable
	{
		void Send(IHttpResponse response);
	}

	public interface IAsyncResultBinding : IResultBinding
	{
		IAsyncResult BeginSend(IHttpResponse response, AsyncCallback callback, object state);
		void EndSend(IAsyncResult result);
	}
}
