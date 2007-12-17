using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace Calyptus.MVC.Internal
{
    internal class RoutingEngine : IRoutingEngine
    {
        private RouteNode _root;
        public RouteNode Root { get { return _root; } }

        public RoutingEngine(IEnumerable<Assembly> assemblies)
        {
            _root = new RouteNode();
        }

        public void ParseRoute(PathStack path)
        {
            throw new NotImplementedException();
        }

        public string GetRelativePath(Expression<Action> action)
        {
            throw new NotImplementedException();
        }

        public string GetURL(Expression<Action> action)
        {
            throw new NotImplementedException();
        }

        public string GetAbsolutePath(Expression<Action> action)
        {
            throw new NotImplementedException();
        }
    }
}
