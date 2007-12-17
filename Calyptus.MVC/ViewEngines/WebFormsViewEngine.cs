using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.Web.UI;
using System.Web;

namespace Calyptus.MVC.ViewEngines
{
    public class WebFormsViewEngine : IViewEngine
    {
        public void RenderView(IHttpContext context, string view)
        {
            RenderView(context, view, (object[]) null);
        }

        public void RenderView(IHttpContext context, string view, object data)
        {
            RenderView(context, view, new object[] { data });
        }

        public void RenderView(IHttpContext context, string view, object[] data)
        {
            Type viewType = BuildManager.GetCompiledType("Views/" + view + ".aspx");
            IHttpHandler handler = (IHttpHandler)System.Activator.CreateInstance(viewType, data, null);
            handler.ProcessRequest(context.ApplicationInstance.Context);
        }
    }
}
