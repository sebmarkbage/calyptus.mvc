using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Binding
{
    interface IPathSerializable
    {
        //static bool TryDeserializePath(IPathStack stack, out object obj);
        void SerializeToPath(IPathStack stack);
    }
}
