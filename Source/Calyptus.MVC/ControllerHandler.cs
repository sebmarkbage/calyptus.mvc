using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;

namespace Calyptus.MVC
{
    public class ControllerHandler : IHttpHandler, IRequiresSessionState
    {
        public ControllerHandler()
        {
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write("Process Request");
            //if (!context.User.Identity.IsAuthenticated)
            //    FormsAuthentication.RedirectFromLoginPage("test", true);
            //context.Response.Write(context.Session["test"]);
            //context.Session["test"] = "test";
            //context.Response.Write(string.Join("||", RoutingModule.GetController(context)));
            //context.Response.Write("<br />");
            //context.Response.Write(context.Request.RawUrl);
            //context.RewritePath(context.Request.RawUrl);
        }
    }
}
