using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Calyptus.MVC
{
	public abstract class RedirectBase : Exception
	{
		public bool Permanent { get; set; }

		protected void Render(IHttpContext context, string url)
		{
			context.Response.Redirect(url, false);
			if (Permanent) context.Response.StatusCode = 301;
		}
	}

	public class Redirect : RedirectBase, IView
	{
		private string url;
		public string URL { get { return url; } }

		public Redirect(string url)
		{
			this.url = url;
		}

		public Redirect(Uri uri)
		{
			this.url = uri.AbsolutePath;
		}

		public Redirect(string url, bool permanently)
		{
			this.url = url;
			this.Permanent = permanently;
		}

		public Redirect(Uri uri, bool permanently)
		{
			this.url = uri.AbsolutePath;
			this.Permanent = permanently;
		}

		void IRenderable.Render(IHttpContext context)
		{
			base.Render(context, url);
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

	public class RedirectAbsolute<TEntryController> : RedirectBase, IView where TEntryController : class, IEntryController
	{
		public Expression<Action<TEntryController>> Action { get; private set; }

		public RedirectAbsolute(Expression<Action<TEntryController>> action)
		{
			Action = action;
		}
		public RedirectAbsolute(Expression<Action<TEntryController>> action, bool permanently)
		{
			Action = action;
			Permanent = permanently;
		}

		void IRenderable.Render(IHttpContext context)
		{
			base.Render(context, context.Route.GetAbsolutePath<TEntryController>(Action));
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
