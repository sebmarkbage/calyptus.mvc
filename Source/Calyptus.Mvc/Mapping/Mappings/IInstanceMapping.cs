using System;

namespace Calyptus.Mvc.Mapping
{
	public interface IInstanceMapping
	{
		bool TryBinding(object instance, IHttpRequest request, out IResultBinding action);
	}

	public interface IInstanceMapping<T> : IInstanceMapping
	{
		bool TryBinding(T instance, IHttpRequest request, out IResultBinding action);
	}
}
