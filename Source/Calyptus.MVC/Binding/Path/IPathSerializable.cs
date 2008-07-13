using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC
{
    interface IPathSerializable
    {
        //static bool TryDeserializePath(IPathStack stack, out object obj);
        void SerializeToPath(IPathStack stack);
    }
}
