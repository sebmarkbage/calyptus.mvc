using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Calyptus.MVC.Binding;
using System.Linq;
using Calyptus.MVC;

[ChildController]
public class PageController : Controller
{
    [DefaultAction]
    public void Default(int hej)
    {
        RenderView("Default", hej);
    }
 }
