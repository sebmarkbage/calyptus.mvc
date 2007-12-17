using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Security.Principal;
using System.Web.Caching;
using System.Web.Profile;

namespace Calyptus.MVC
{
    public interface IHttpContext
    {
        HttpApplicationState Application { get; }
        HttpApplication ApplicationInstance { get; }
        Cache Cache { get; }
        Exception Error { get; }
        IHttpHandler Handler { get; }
        IDictionary Items { get; }
        ProfileBase Profile { get; }
        HttpResponse Response { get; }
        HttpRequest Request { get; }
        HttpServerUtility Server { get; }
        HttpSessionState Session { get; }
        TraceContext Trace { get; }
        IPrincipal User { get; }
    }
}
