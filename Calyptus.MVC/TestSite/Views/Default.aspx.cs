using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class _Default : Calyptus.MVC.ViewPage<PageController>
{
    protected PageController Controller { get; set; }

    public void InitViewData(PageController controller)
    {
        this.Controller = controller;
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }
}

/*
public class MasterView
{
    private RootController Controller;
    public MasterView(RootController root)
    {
        this.Controller = root;
    }
}


public class DefaultView : MasterView
{
    private PageController Controller;

    public DefaultView(PageController controller) : base(controller.Root)
    {
        this.Controller = controller;
    }
}*/