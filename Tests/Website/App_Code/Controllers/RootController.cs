using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Calyptus.MVC;
using Calyptus.MVC.Binding;
using System.Globalization;
using System.Linq;
using Calyptus.MVC.Extensions;

[DefaultController]
public class RootController : Controller
{
	//   [Get] [Post] [Cookie] [Session] [SOAP] [JSON]

	[Action("Pages")]
	public PageController Pages()
	{
		return new PageController();
	}

    [DefaultAction]
	[Action]
	[Cache(Cacheability=HttpCacheability.Server, Expires=3)]
    public void Index([Path]string id, [Get]string pageName)
    {
		RenderView("Default", new { ID = id, Name = pageName });
    }
}