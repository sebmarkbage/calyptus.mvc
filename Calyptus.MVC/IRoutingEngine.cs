using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Calyptus.MVC
{
    public interface IRoutingEngine
    {
        void ParseRoute(PathStack path);
        string GetRelativePath(Expression<Action> action);
        string GetURL(Expression<Action> action);
        string GetAbsolutePath(Expression<Action> action);
    }
}
