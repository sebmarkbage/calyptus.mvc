using System;
using System.Linq.Expressions;
using System.Linq;

namespace Calyptus.Mvc.Tests
{
	public class BlogController
	{
		#region Private fields

		private UnitOfWork context;
		private Blog blog;
		private RootController parent;

		#endregion

		public BlogController(RootController parent, UnitOfWork context, Blog blog)
		{
			this.context = context;
			this.blog = blog;
			this.parent = parent;
		}

		/*[Get]*/
		public BlogsView Index()
		{
			if (blog.ID != 20) throw new Redirect(() => parent.ShowBlog(20).Index(), true);
			Expression<Action<Blog>> showBlog = b => parent.ShowBlog(b.ID);
			return new BlogsView
			{
				ShowSibling = () => showBlog.Compile()(this.blog.ID + 1)
			};
		}

		/*[View]*/
		public class BlogsView
		{
			public Expression<Action> ShowSibling;
		}
	}
}
