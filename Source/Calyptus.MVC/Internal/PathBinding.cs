using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calyptus.MVC.Binding;

namespace Calyptus.MVC.Internal
{
    internal struct PathBinding
    {
        private Type _type;
        public Type Type { get { return _type; } }

        private PathBinderBase _binder;
        public PathBinderBase Binder { get { return _binder; } }

        public PathBinding(Type type, PathBinderBase binder)
        {
            this._type = type;
            this._binder = binder;
        }
    }
}
