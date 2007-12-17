using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC.Binding
{
    interface IPathSerializable<T>
    {
        //static bool TryDeserializePath(PathStack stack, out T obj);
        void SerializeToPath(PathStack stack);
    }
}
