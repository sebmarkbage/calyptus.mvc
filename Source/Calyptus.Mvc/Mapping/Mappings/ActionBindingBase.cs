using System.Reflection;

namespace Calyptus.Mvc.Mapping
{
	public abstract class ActionBindingBase : IActionMapping
	{
		public ActionBindingBase(MethodInfo actionMethod)
		{

		}

		public MappingResult TryBinding(IHttpContext context, IPathStack path, out object[] parameters)
		{
			throw new System.NotImplementedException();
		}


		public int OverloadWeight
		{
			get { throw new System.NotImplementedException(); }
		}
	}
}
