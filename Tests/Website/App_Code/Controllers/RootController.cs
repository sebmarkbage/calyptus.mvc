using System;
using Calyptus.MVC;

[EntryController]
public class RootController : IController
{
	//[Get]
	public IViewTemplate Index()
	{
		return new DefaultView { Title = "Test INDEX" };
	}

	//[Path]
	public IViewTemplate Edit()
	{
		return new Redirect<RootController>(r => r.Index());
	}

	//[Post, Post(Path="Edit")]
	public void Save()
	{
		this.Redirect(r => r.Blogs().Edit(10));
	}

	//[Path("Blogs")]
	public BlogController Blogs()
	{
		return new BlogController
		{
			Master = new MasterView { HeadTitle = "Blogs" }
		};
	}

	//[Path]
	public BlogController Blog([Path] string name)
	{
		return new BlogController
		{
			Master = new MasterView { HeadTitle = "Blog: " + name }
		};
	}

	#region View Templates

	public class MasterView : IViewTemplate
	{
		public string HeadTitle;
	}

	public class DefaultView : IViewTemplate
	{
		public string Title;
	}
	
	#endregion
}
