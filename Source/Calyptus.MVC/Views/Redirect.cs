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

		public void Render(IHttpContext context)
		{
			base.Render(context, url);
		}
	}

	public class Redirect<T> : RedirectBase, IView
	{
		public int Index { get; private set; }
		public Expression<Action<T>> Action { get; private set; }

		public Redirect(Expression<Action<T>> action)
		{
			Index = int.MaxValue;
			Action = action;
		}
		public Redirect(int index, Expression<Action<T>> action)
		{
			Index = index;
			Action = action;
		}
		public Redirect(Expression<Action<T>> action, bool permanently)
		{
			Index = int.MaxValue;
			Action = action;
			Permanent = permanently;
		}
		public Redirect(int index, Expression<Action<T>> action, bool permanently)
		{
			Index = index;
			Action = action;
			Permanent = permanently;
		}

		public void Render(IHttpContext context)
		{
			base.Render(context, context.RoutingEngine.GetRelativePath<T>(Index, Action));
		}
	}

	public class RedirectAbsolute<T> : RedirectBase, IView
	{
		public Expression<Action<T>> Action { get; private set; }

		public RedirectAbsolute(Expression<Action<T>> action)
		{
			Action = action;
		}
		public RedirectAbsolute(Expression<Action<T>> action, bool permanently)
		{
			Action = action;
			Permanent = permanently;
		}
		public void Render(IHttpContext context)
		{
			base.Render(context, context.RoutingEngine.GetAbsolutePath<T>(Action));
		}
	}

	public class RedirectReplace<T> : RedirectBase, IView
	{
		public int Index { get; private set; }
		public Expression<Action<T>> Action { get; private set; }

		public RedirectReplace(Expression<Action<T>> action)
		{
			Index = int.MaxValue;
			Action = action;
		}
		public RedirectReplace(int index, Expression<Action<T>> action)
		{
			Index = index;
			Action = action;
		}
		public RedirectReplace(Expression<Action<T>> action, bool permanently)
		{
			Index = int.MaxValue;
			Action = action;
			Permanent = permanently;
		}
		public RedirectReplace(int index, Expression<Action<T>> action, bool permanently)
		{
			Index = index;
			Action = action;
			Permanent = permanently;
		}
		public void Render(IHttpContext context)
		{
			base.Render(context, context.RoutingEngine.GetReplacementPath<T>(Index, Action));
		}
	}

}
