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
        bool TryParseRoute(PathStack path, out IHttpHandler handler);
        string GetRelativePath(Expression<Action> action);
        string GetURL(Expression<Action> action);
        string GetAbsolutePath(Expression<Action> action);
    }
}
