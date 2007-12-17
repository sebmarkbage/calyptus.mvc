using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;

public class Page
{
    public Page(string name)
    {
        this.Name = name;
    }
    public IQueryable<Page> Children { get; set; }
    public string Name { get; set; }
}
