using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC
{
    public interface IViewEngine
    {
        void RenderView(IHttpContext context, string view);
        void RenderView(IHttpContext context, string view, object data);
        void RenderView(IHttpContext context, string view, object[] data);
    }
}
