using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Web;

namespace Calyptus.MVC.Internal
{
    internal class RoutingEngine : IRoutingEngine
    {
		private Dictionary<IKeyword, Type> _controllers;
		//private Dictionary<Type, something> _controllerBindingCache;

        private RouteNode _root;
        public RouteNode Root { get { return _root; } }

		public RoutingEngine() : this(GetAssemblies())
		{
		}

		private static IEnumerable<Assembly> GetAssemblies()
		{
			foreach (Assembly a in System.Web.Compilation.BuildManager.GetReferencedAssemblies())
				yield return a;
		}

        public RoutingEngine(IEnumerable<Assembly> assemblies)
        {
            _root = new RouteNode();
        }

		public IHttpHandler ParseRoute(PathStack path)
        {
			if (path.Current == "MVC")
				return new ControllerHandler { Stack = path, Engine = this };
			return null;
        }

		public bool TryParseSubRoute(PathStack path, object controller, out MethodInfo method, out object[] args)
		{
			if (path.Current == "MVC")
			{
				
			}
			method = null;
			args = null;
			return false;
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
