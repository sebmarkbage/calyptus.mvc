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

[ControllerOption]
public class UserController : Controller
{
	public User User { get; set; }
	public IMaster Master { get; set; }

    public UserController(User user, IMaster master)
    {
		this.User = user;
		this.Master = master;
    }

	[Action(View="Default", ViewEngine="WebFormsViewEngine")]
	[Action(ContentType="application/json", ViewEngine="JSONSerializer")]
	public IView Index()
	{
		RenderView("Default", User);
		return User;
		return View("Default", Master, User);
		//RenderView("Default", (object) "UserController.Index");
	}

	[Action("More")]
	public void More()
	{
		//RenderView("Default", (object)"UserController.More");

	}

	[DeleteAction]
	public void Delete()
	{

	}

	//public BlogsController Blogs()
	//{
	//    return new BlogsController();
	//}
}
