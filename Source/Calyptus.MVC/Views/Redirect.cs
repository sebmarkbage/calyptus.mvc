using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web;

namespace Calyptus.MVC
{
	public abstract class RedirectBase : Exception
	{
		public bool Permanent { get; set; }

		protected void Render(IHttpContext context, string url)
		{
			IHttpResponse response = context.Response;
			response.Clear();
			response.RedirectLocation = url;
			response.StatusCode = Permanent ? 301 : 302;
			response.Write("<html><head><title>The object has moved</title></head><body><h2><a href=\"" + HttpUtility.HtmlAttributeEncode(url) + "\">Click here</a></h2></body></html>");
		}
	}

	public class Redirect : RedirectBase, IView
	{
		private bool isUrl;
		private string url;
		public string URL { get { return url; } }
		public Expression<Action> Action { get; private set; }

		public Redirect(string url) : this(url, false) { }

		public Redirect(Uri uri) : this(uri, false) { }

		public Redirect(string url, bool permanently)
		{
			this.isUrl = true;
			this.url = url;
			this.Permanent = permanently;
		}

		public Redirect(Uri uri, bool permanently)
		{
			this.isUrl = true;
			this.url = uri.AbsolutePath;
			this.Permanent = permanently;
		}

		public Redirect(Expression<Action> action) : this(action, false) { }

		public Redirect(Expression<Action> action, bool permanently)
		{
			this.isUrl = false;
			Action = action;
			Permanent = permanently;
		}

		void IRenderable.Render(IHttpContext context)
		{
			if (isUrl)
				base.Render(context, url);
			else
				base.Render(context, context.Route.GetAbsolutePath(Action));
		}

		string IView.ContentType
		{
			get { return "text/html"; }
		}

		void IView.Render(System.IO.Stream stream)
		{
			throw new NotImplementedException("Can't render a redirect to a stream.");
		}
	}

	public class Redirect<TRelativeController> : RedirectBase, IView where TRelativeController : IController
	{
		public int Index { get; private set; }
		public Expression<Action<TRelativeController>> Action { get; private set; }

		public Redirect(Expression<Action<TRelativeController>> action)
		{
			Index = -1;
			Action = action;
		}
		public Redirect(int index, Expression<Action<TRelativeController>> action)
		{
			Index = index;
			Action = action;
		}
		public Redirect(Expression<Action<TRelativeController>> action, bool permanently)
		{
			Index = -1;
			Action = action;
			Permanent = permanently;
		}
		public Redirect(int index, Expression<Action<TRelativeController>> action, bool permanently)
		{
			Index = index;
			Action = action;
			Permanent = permanently;
		}

		void IRenderable.Render(IHttpContext context)
		{
			base.Render(context, context.Route.GetRelativePath<TRelativeController>(Index, Action));
		}

		string IView.ContentType
		{
			get { return "text/html"; }
		}

		void IView.Render(System.IO.Stream stream)
		{
			throw new NotImplementedException("Can't render a redirect to a stream.");
		}
	}

	public class Redirect<TRelativeController, TWithActionsFromController> : RedirectBase, IView where TRelativeController : IController where TWithActionsFromController : IController
	{
		public int Index { get; private set; }
		public int SecondIndex { get; private set; }
		public Expression<Func<TRelativeController, TWithActionsFromController>> Action { get; private set; }

		public Redirect(Expression<Func<TRelativeController, TWithActionsFromController>> action)
		{
			Index = -1;
			Action = action;
		}
		public Redirect(int index, Expression<Func<TRelativeController, TWithActionsFromController>> action)
		{
			Index = index;
			Action = action;
		}
		public Redirect(int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action)
		{
			Index = index;
			SecondIndex = secondIndex;
			Action = action;
		}
		public Redirect(Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently)
		{
			Index = -1;
			Action = action;
			Permanent = permanently;
		}
		public Redirect(int index, Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently)
		{
			Index = index;
			Action = action;
			Permanent = permanently;
		}
		public Redirect(int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently)
		{
			Index = index;
			SecondIndex = secondIndex;
			Action = action;
			Permanent = permanently;
		}

		void IRenderable.Render(IHttpContext context)
		{
			base.Render(context, context.Route.GetRelativePath<TRelativeController, TWithActionsFromController>(Index, SecondIndex, Action));
		}

		string IView.ContentType
		{
			get { return "text/html"; }
		}

		void IView.Render(System.IO.Stream stream)
		{
			throw new NotImplementedException("Can't render a redirect to a stream.");
		}
	}
}
