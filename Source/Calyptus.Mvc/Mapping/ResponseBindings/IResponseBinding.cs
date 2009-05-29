namespace Calyptus.Mvc.Mapping
{
	interface IResponseBinding
	{
		bool IsSupersetOf(IResponseBinding binding);
		bool IsSubsetOf(IResponseBinding binding);
		bool IsEqualTo(IResponseBinding binding);
		bool IsNegating(IResponseBinding binding);
	}

	interface IResponseBinding<TBindTo, TIn> : IResponseBinding
	{
		IResponseBinding GetResponseBinding();
		void BindToResponse(TBindTo bindTo, TIn value);
	}

	interface IResponseBinding<T> : IResponseBinding
	{
		IRequestBinding[] GetResponseBinding();
		void BindToResponse(object[] bindTo, T value);
	}

	interface IHttpResponseBinding<T> : IResponseBinding
	{
		void BindToResponse(IHttpResponse response, T value);
	}
}
