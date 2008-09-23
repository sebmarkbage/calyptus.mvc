using System;
using Calyptus.MVC;
using System.Security.Principal;
using System.Runtime.Serialization;
using System.Linq;
using System.Web.Security;

[EntryController]
public class RootController : IEntryController
{
	[ApplicationPath]
	public string Path { get; set; }

	[Default]
	public IViewTemplate Index()
	{
		return new DefaultView { Title = Path };
	}

	[Get(Path = "TryBytes", ResponseType="application/json")]
	public string Get([Path] byte[] bytes)
	{
		return Convert.ToBase64String(bytes);
	}

	[Path("GoToIndex")]
	public IViewTemplate GoToIndex()
	{
		return new Redirect(() => new RootController().Circular().Index());
	}

	// bla...
	// { ID: 5, Name: 'Name' }
	// { obj: { ID: 5, Name: 'Name' }, id: 123 }

	// <root>
	//   <obj>
	//     <id>10</id>
	//   </obj>
	//   <id>10</id>
	// </root>

	// obj.id=10&obj.name=Name
	// 

	[Post(Path="postTest")]
	public IViewTemplate Save([Form] string text, [QueryString("ReturnUrl")] string q)
	{
		return new DefaultView { Title = text + (q == null ? "null" : "-" + q + "-") };
	}

	[Post]
	public void Save([Form] TestObject obj, [Get] int id)
	{

	}

	[Path("Circular")]
	public RootController Circular()
	{
		return this;
	}

	[Path("TestObject"), Path("TestObjectJson", ResponseType="application/json")]
	public object GetTestObject()
	{
		//return new System.Xml.Linq.XDocument(new System.Xml.Linq.XElement("Test", new System.Xml.Linq.XAttribute("attr", "v")));
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
		[NonSerialized]
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
	public void Save([Path(DefaultValue=-1)] int id, out int id2, ref int id3)
	{
		id2 = 10;
		//this.Redirect(r => r.Blogs().Edit(10));
	}

	[Path("Blogs")]
	public BlogController Blogs()
	{
		return new Blog2Controller
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