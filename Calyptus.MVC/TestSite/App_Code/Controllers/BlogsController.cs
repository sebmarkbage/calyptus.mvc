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

public class BlogsController
{
    IQueryable<Blog> Blogs;

    public BlogsController()
    {

    }

    public PostsController Index()
    {
        return new PostsController();
    }
}

public class PostsController
{
    IQueryable<Post> Posts;

    public PostsController()
    {
    }

    public void List(int year, int month, int date, string[] tags)
    {
        RenderView("List", Posts);
    }

    public void View(int year, int month, int date, string name)
    {
        RenderView("View", Posts.Where(p => p.Year == year && p.Month == month && p.Date == date && p.Title == name).First());
    }

    public CommentsController View(int year, int month, int date, string name)
    {
        return new CommentsController(Posts.Where(p => p.Year == year && p.Month == month && p.Date == date && p.Title == name).First().Comments);
    }

    public void Edit(int year, int month, int date, string name)
    {
        RenderView("Edit", Posts.Where(p => p.Year == year && p.Month == month && p.Date == date && p.Title == name).First());
    }
}

public class CommentsController
{
    public CommentsController()
    {
    }

    public void Edit()
    {
    }
}
/*
/User/Profile.aspx
/User/EditProfile.aspx
/User/Blogs/List.aspx
/User/Blogs/Edit.aspx
/User/Blogs/Posts/List.aspx
/User/Blogs/Posts/View.aspx
/User/Blogs/Posts/Edit.aspx
*/