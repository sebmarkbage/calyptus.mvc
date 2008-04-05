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
using Calyptus.MVC;
using System.Net;
using System.IO;

[Controller]
public class BlogController : IController, IDisposable
{
	public RootController.MasterView Master { get; set; }

	//[Action]
	public IViewTemplate Index()
	{
		return new Redirect<RootController>(r => r.Index(), true);
	}

	//[Get]
	public IViewTemplate Edit([QueryString] int id)
	{
		return new EditView { Master = Master, ID = id, Title = "TEST Title" };
	}

	private WebRequest _request;
	private WebResponse _response;

	//[Post]
	public IAsyncResult BeginSave([QueryString] int id, [Form] string title, AsyncCallback callback, object state)
	{
		_request = WebRequest.Create("http://www.calyptus.se/");
		return _request.BeginGetResponse(callback, state);
	}

	public Stream EndSave(IAsyncResult result)
	{
		_response = _request.EndGetResponse(result);
		return _response.GetResponseStream();
	}

	public void Dispose()
	{
		_response.Close();
	}

	#region View Templates

	public class EditView : IViewTemplate<RootController.MasterView>
	{
		public int ID;
		public string Title;
		public RootController.MasterView Master { get; set; }
	}

	#endregion

}
