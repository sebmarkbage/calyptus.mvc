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
using System.IO;

public class Test
{
	public Test()
	{
		RootController r = new RootController();
		using (BlogController b = r.Blogs())
		{
			IAsyncResult ar = b.BeginSave(0, "Title", null, null);
			// Async ...
			Stream s = b.EndSave(ar);
			// Render stream ...
		}
	}
}
