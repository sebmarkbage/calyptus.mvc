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

[Controller("keyword")]
public class RootController : ControllerBase, IPagesHolder
{
    public IPagesHolder Parent { get; set; }

    public RootController()
    {
    }

    public RootController(CultureInfo culture)
    {
    }

    public RootController(CultureInfo culture, string somestring)
    {
        
    }

    [Action]
    public void Index()
    {
        RenderView("Default");
    }

    public IQueryable<Page> Children
    {
        get
        {
            return db.Pages.Where(p => p.Parent == null);
        }
        set
        {
            throw new NotImplementedException();
        }
    }
}