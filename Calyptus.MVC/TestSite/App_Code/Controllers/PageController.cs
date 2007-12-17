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

public class PageController : ControllerBase, IPagesHolder
{
    public RootController Root;

    public IPagesHolder Parent { get; set; }
    public Page Page { get; private set; }

    public IQueryable<Page> Children { get; set; }

    /*public PageController([Path] string pageName)
    {
        Page = new Page(pageName);
    }*/

    [DefaultController]
    public PageController([Parent] IPagesHolder parent, [Path] string pageName)
    {
        if (parent is RootController)
            Root = parent;
        else
            Root = (parent as PagesController).Root;

        Parent = parent;
        Page = parent.Children.Where(p => p.Name == pageName).First();
        this.Children = Page.Children;
        
    }

    //   [Get] [Post] [Cookie] [Session] [SOAP] [JSON]

    [DefaultAction]
    public void Default([Path] int hej)
    {
        //   /hej/controllerKeyword/pagename/actionKeyword
        //   /sv-SE/name/

        //   /sv-SE/firstpage/subpage/
        //   /sv-SE/firstpage/second_subpage/
        //string name = this.Parent.Children.First();
        //RedirectToAction(() => new PageController(this.Parent, name).Default());

        RenderView("Default", this);
    }
 }
