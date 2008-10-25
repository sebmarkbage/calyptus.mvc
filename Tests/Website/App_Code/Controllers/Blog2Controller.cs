using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Calyptus.Mvc;

public class Blog2Controller : BlogController
{
	[Default]
	public override IViewTemplate Index(IPathStack path)
	{
		return new RootController.DefaultView { Title = "Blogs2Index", Path = path };
	}

	[Path("Index2")]
	public IViewTemplate Index2()
	{
		return new RootController.DefaultView { Title = "Blogs2Index2" };
	}
}
