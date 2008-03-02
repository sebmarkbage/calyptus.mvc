using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Linq.Expressions;

namespace Calyptus.MVC
{
    public interface IRoutingEngine
    {
		IHttpHandler ParseRoute(IHttpContext context, IPathStack path);

        string GetRelativePath<T>(Expression<Action<T>> action);
		string GetRelativePath<T>(int index, Expression<Action<T>> action);
		string GetReplacementPath<T>(Expression<Action<T>> action);
		string GetReplacementPath<T>(int index, Expression<Action<T>> action);
		string GetAbsolutePath<T>(Expression<Action<T>> action);
	}
}
