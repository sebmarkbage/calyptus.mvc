namespace Calyptus.Mvc.Mapping
{
	public interface IActionMapping
	{
		MappingResult TryBinding(IHttpRequest context, out object[] parameters);
	}
}
