using System;
using Calyptus.MVC;
using System.Security.Principal;

[EntryController]
public class RootController : IEntryController
{
	[Default, Path("TrailTest")]
	public IViewTemplate Index()
	{
		return new DefaultView { Title = "Test INDEX" };
	}

	[Path("Circular")]
	public RootController Circular()
	{
		return this;
	}


	[Path("TestObject"), Path("TestObjectJson", ResponseType="application/json")]
	public object GetTestObject()
	{
		return new TestObject {
			ID = 123,
			Title = "My <Title>",
			Children = new TestObject[] {
				new TestObject {
					ID = 123,
					Title = "My Child Title"
				}
			}
		};
	}

	[Serializable]
	public class TestObject
	{
		public int ID;
		public string Title;
		public TestObject[] Children;
	}

	[Post("Edit"), Authenticate]
	public IViewTemplate Edit()
	{
		return new Redirect<RootController>(r => r.Index());
	}

	// POST /Save
	// Content-Type: application/x-www-form...
	// id=10&name=Name

	// Content-Type: application/json
	// { id: 10, name: "Name" }

	[Get, Post(Path = "Save")]
	public void Save([Get] int id, out int id2, ref int id3)
	{
		id2 = 10;
		this.Redirect(r => r.Blogs().Edit(10));
	}

	[Path("Blogs")]
	public BlogController Blogs()
	{
		return new BlogController
		{
			Master = new MasterView { HeadTitle = "Blogs" }
		};
	}

	[Path]
	public BlogController Blog(string name)
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
		public IPathStack Path;
		public string Title;
	}
	
	#endregion
}