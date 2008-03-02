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
using System.Security.Principal;
using Calyptus.MVC.ViewEngines;
using System.IO;

[EntryController(Master="Root", ViewEngine=typeof(WebFormsViewEngine))]
[ChildController]
public class RootController : Controller
{
	// WEB  [Get] [Post] [Cookie] [Session]
	// SOA  [SOAP] [XML_RPC] [JSON_RPC] [REST?]
	// Extensions [Cache] [...?]


	[ParentAction]
	public PageController Pages()
	{
		return new PageController();
	}

	/*[Cookie("AuthCookie")]
	[Get("Auth")]*/
	public System.Web.Security.FormsIdentity User { get; set; }

	public void Login()
	{
		User = new FormsIdentity(new FormsAuthenticationTicket("Username", true, 0));
	}

	// Action
	//  /Index/bla
	
	// JsonRpcAction
	//  /
	// Content-Type: application/json
	// { method: "Index", params: { id: "bla" } }

    [DefaultAction]
	[Action(View="Default")]
	/*
	[SoapAction]
	[JsonAction]
	[JsonRpcAction]
	[XmlRpcAction]
	*/
	[Cache(Cacheability=HttpCacheability.Server, Duration=60)]
    public object Index([Get]string id, [Get]string pageName)
    {
		HttpContext.Current.Response.Write("URL:\n");
		HttpContext.Current.Response.Write(
				URLAbsolute<RootController>(r => r.Index("ID", "PageName"))
		);
		return new { ID = id, Name = pageName };
		//RenderView("Default", new { ID = id, Name = pageName });
    }

	/*[WsdlAction]   Takes a Controller Type as it's return data and renders WSDL for that controller */
	public Type WSDL()
	{
		return this.GetType();
	}

	// Seb/Blogs/GetBla


	/*	
		Path = 1,
		FormAction = 2,
		FormKeyword = 4,
		QueryStringAction = 8,
		QueryStringKeyword = 16,
		JsonAction = 32
	*/

	[GetAction("BaseName", "Index")]  //  GET /Index
	public void Index()
	{

	}

	[GetAction("Edit")]
	public void Edit()
	{

	}

	//   GET/POST/DELETE/PUT...    /Keyword
	//   GET	/?action=Keyword
	//   GET	/?keyword=whatever
	//	 POST   Content-Type: form/urlencoded    action=Keyword
	//	 POST   Content-Type: form/urlencoded    keyword=whatever
	//   POST   Content-Type: application/json   { action: 'Keyword' }
	//   POST   Content-Type: application/xml    <root action="Keyword">...</root>
	//   POST   Content-Type: application/xml    <Keyword>...</Keyword>
	

	/*[Action()]
	[Action(Verb = "POST", ContentType = "form/urlencoded", Path = "Edit")]
	[PostAction(ContentType = "form/urlencoded", Path = "Edit")]
	[FormAction("keyword", Path="Edit")]
	[QueryAction("action", "Save")]

	[PostAction()]*/
	public void Save([Path] int id, [Post] Stream file)
	{
		URL<RootController>(r => r.SelectUser("Seb", m));
	}

	[GetAction]  // GET /Default
	public void Default()
	{

	}

	[ParentAction("Users")]
	public UsersController Users()
	{
		return new UsersController(this.Culture, this.User);
	}

	[ParentAction]
	public UserController User([Path] string user)
	{
		return new UserController(user, Master("Root", this.Culture));
	}
}