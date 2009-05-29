namespace Calyptus.Mvc.Mapping
{
	public interface IValueConstraint
	{
		bool TryConstraint(object value);
		bool IsEqualTo(IValueConstraint constraint);
		bool IsSupersetOf(IValueConstraint constraint);
		bool IsSubsetOf(IValueConstraint constraint);
		bool IsNegating(IValueConstraint constraint);
	}

	public interface IValueConstraint<T> : IValueConstraint
	{
		bool TryConstraint(T value);
	}
}
