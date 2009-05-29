namespace Calyptus.Mvc
{
	public interface IHttpConnection
	{
		string LocalAddress { get; }
		string LocalName { get; }
		int LocalPort { get; }

		string RemoteAddress { get; }
		string RemoteName { get; }
		int RemotePort { get; }

		bool IsConnected { get; }
	}

	public interface IHttpSecureConnection : IHttpConnection
	{
		HttpClientCertificate ClientCertificate { get; }
		HttpServerCertificate ServerCertificate { get; }
	}
}