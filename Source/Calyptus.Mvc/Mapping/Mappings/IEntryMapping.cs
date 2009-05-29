namespace Calyptus.Mvc.Mapping
{
	public interface IEntryMapping
	{
		bool TryBinding(IHttpRequest request, out IResultBinding binding);
	}
}
