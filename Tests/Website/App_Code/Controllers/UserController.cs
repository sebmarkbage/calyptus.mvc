using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Calyptus.MVC.Binding;
using Calyptus.MVC;

[Controller(RequireExplicitActionAttributes=false)]
public class UserController : Controller
{
    public UserController()
    {

    }

	[DefaultAction]
	public void Index()
	{
		RenderView("Default", 3);
	}

	public void More()
	{
		RenderView("Default", 4);
	}

	//public BlogsController Blogs()
	//{
	//    return new BlogsController();
	//}
}
