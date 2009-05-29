namespace Calyptus.Mvc.Mapping
{
	public interface IMappingContext
	{
		IResultBinding Map(IHttpRequest request);
	}
}
