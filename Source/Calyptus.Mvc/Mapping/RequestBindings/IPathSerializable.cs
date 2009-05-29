using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.Mvc
{
    public interface IPathSerializable
    {
        //IMPLICIT: static bool TryDeserializePath(IPathStack stack, out object obj);
        void SerializeToPath(IPathStack stack);
    }
}
