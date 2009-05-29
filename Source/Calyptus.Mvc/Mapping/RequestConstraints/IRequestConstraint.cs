namespace Calyptus.Mvc.Mapping
{
    public interface IRequestConstraint
    {
		bool TryMapping(IHttpRequest request, IPathStack path);
		bool IsEqualTo(IRequestConstraint constraint);
		bool IsSubsetOf(IRequestConstraint constraint);
		bool IsSupersetOf(IRequestConstraint constraint);
		bool IsNegating(IRequestConstraint constraint);
	}
}
