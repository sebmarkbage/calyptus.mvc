using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Calyptus.MVC
{
	public interface IRouteContext : IDisposable
	{
		IRoutingEngine RoutingEngine { get; }

		void AddController(object controller, int pathIndex);
		int ControllerCount { get; }
		int Index { get; }
		void ReverseToIndex(int index);

		string GetRelativePath<TRelativeController>(int index, Expression<Action<TRelativeController>> action);
		string GetRelativePath<TRelativeController, TWithActionsFromController>(int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action);
		string GetAbsolutePath<TEntryController>(Expression<Action<TEntryController>> action) where TEntryController : class, IEntryController;
	}
}
