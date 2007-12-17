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

public class User
{
    public IQueryable<Blog> Blogs { get; set; }
}

public class Blog
{
    public IQueryable<Post> Posts { get; set; }
}

public class Post
{
    public IQueryable<Comment> Comments { get; set; }
}

public class Comment
{
}
