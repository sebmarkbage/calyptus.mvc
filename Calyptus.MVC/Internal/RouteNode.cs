using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Internal
{
    internal class RouteNode
    {
        private IList<IKeyword> _keywords;
        public IList<IKeyword> Keywords { get { return _keywords; } }

        private IList<PathBinding> _pathBinding;
        public IList<PathBinding> PathBinding { get { return _pathBinding; } }

        public RouteNode()
        {
            _keywords = new List<IKeyword>();
            _pathBinding = new List<PathBinding>();
        }
    }
}
