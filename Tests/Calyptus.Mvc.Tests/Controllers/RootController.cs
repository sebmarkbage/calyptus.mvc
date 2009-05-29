using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Calyptus.Mvc.Tests
{
	public class RootController
	{
		#region Private fields

		private UnitOfWork context;
		private CultureInfo culture;

		#endregion

		#region Constructors

		public RootController(/*[ApplicationProperty("DBContext")]*/ UnitOfWork context)
		{
			this.context = context;
			this.culture = CultureInfo.InvariantCulture;
		}

		public RootController(/*[ApplicationProperty("DBContext")]*/ UnitOfWork context, /*[Path]*/ CultureInfo localization) : this(context)
		{
			this.culture = localization;
		}

		#endregion

		#region Actions

		public WelcomeView Index()
		{
			return new WelcomeView();
		}

		#endregion

		#region Paths

		//[Path]
		public BlogController ShowBlog(Blog blog)
		{
			return new BlogController(context, Master, blog);
		}

		//[Path]
		public BlogController ShowBlog(int blogID)
		{
			return ShowBlog(this, context.Blogs.Where(b => b.ID == blogID).SingleOrDefault());
		}

		#endregion

		#region HandleViews

		public RootMasterView AddMaster(BlogController.BlogsView blogsView)
		{
			blogsView.ShowBlog = b => this.ShowBlog(b);
			return new RootMasterView(blogsView)
			{
				ShowBlog = b => this.ShowBlog(b)
			};
		}

		#endregion

		#region Views

		public RootMasterView Master
		{
			get
			{
				return new RootMasterView
				{
					ShowBlog = b => this.ShowBlog(b)
				};
			}
		}

		[View]
		public class RootMasterView
		{
			public Expression<Action<Blog>> ShowBlog;
		}

		[View]
		public class WelcomeView
		{

		}

		#endregion
	}
}
