namespace Calyptus.Mvc.Mapping
{
	public interface IRequestBinding
	{
		bool IsSupersetOf(IRequestBinding binding);
		bool IsSubsetOf(IRequestBinding binding);
		bool IsEqualTo(IRequestBinding binding);
	}

	public interface IRequestBinding<TIn, TOut> : IRequestBinding
	{
		IRequestBinding GetRequestBinding();
		bool TryBinding(TIn item, out TOut value);
	}

	public interface IRequestBinding<TOut> : IRequestBinding
	{
		IRequestBinding[] GetRequestBindings();
		bool TryBinding(object[] items, out TOut value);
	}

	public interface IHttpRequestBinding<T> : IRequestBinding
	{
		bool TryBinding(IHttpRequest request, out T value);
	}
}
