using System;
using Calyptus.MVC;
using System.Net;
using System.IO;

[Controller]
public class BlogController : IEntryController, IDisposable
{
	public RootController.MasterView Master { get; set; }

	[Action, Path("Test")]
	public virtual IViewTemplate Index(IPathStack path)
	{
		System.Web.Security.FormsAuthenticationTicket f;
		return new Redirect<RootController>(r => r.Index());
		//return new RootController.DefaultView { Title = "BlogsIndex", Path = path };
		//return new Redirect<RootController>(r => r.Index(), true);
	}

	[Action(Verb="GET", Path="Edit1/Edit2")]
	[Get("Edit")]
	public IViewTemplate Edit([QueryString] int id)
	{
		return new EditView { Master = Master, ID = id, Title = "TEST Title" };
	}

	private WebRequest _request;
	private WebResponse _response;

	[Post]
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
		if (_response != null)
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
