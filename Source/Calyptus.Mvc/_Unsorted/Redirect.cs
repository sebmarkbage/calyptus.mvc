﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web;
using System.IO;

namespace Calyptus.Mvc
{
	public abstract class RedirectBase : Exception, IView
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

		protected abstract void Render(IHttpContext context);

		public override string Message
		{
			get
			{
				return "A redirect was issued.";
			}
		}

		string IView.ContentType
		{
			get { return "text/html"; }
		}

		void IView.Render(TextWriter writer)
		{
			throw new NotImplementedException("Can't render a redirect to a stream or writer.");
		}

		void IResponse.Render(IHttpContext context)
		{
			Render(context);
		}
	}

	public class Redirect : RedirectBase
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

		public override string Message
		{
			get
			{
				return "A redirect was issued to: " + (isUrl ? url : Action.ToString());
			}
		}

		protected override void Render(IHttpContext context)
		{
			if (isUrl)
				base.Render(context, url);
			else
				throw new NotImplementedException();
				// TODO: base.Render(context, context.Route.GetPath(Action));
		}
	}

	public class Redirect<TRelativeController> : RedirectBase
	{
		public int Index { get; private set; }
		public Expression<Action<TRelativeController>> Action { get; private set; }

		public Redirect() : this(-1, null, false) { }
		public Redirect(int index) : this(index, null, false) { }
		public Redirect(bool permanently) : this(-1, null, permanently) { }
		public Redirect(int index, bool permanently) : this(-1, null, permanently) { }
		public Redirect(Expression<Action<TRelativeController>> action) : this(-1, action, false) {}
		public Redirect(int index, Expression<Action<TRelativeController>> action) : this(index, action, false) { }
		public Redirect(Expression<Action<TRelativeController>> action, bool permanently) : this(-1, action, permanently) { }
		public Redirect(int index, Expression<Action<TRelativeController>> action, bool permanently)
		{
			Index = index;
			Action = action;
			Permanent = permanently;
		}

		protected override void Render(IHttpContext context)
		{
			// TODO: base.Render(context, context.Route.GetPath<TRelativeController>(Index, Action));
		}

		public override string Message
		{
			get
			{
				return "A redirect was issued to: " + Action.ToString();
			}
		}
	}

	public class Redirect<TRelativeController, TWithActionsFromController> : RedirectBase
	{
		public int Index { get; private set; }
		public int SecondIndex { get; private set; }
		public Expression<Func<TRelativeController, TWithActionsFromController>> Action { get; private set; }

		public Redirect() : this(-1, 0, null, false) { }
		public Redirect(int index) : this(index, 0, null, false) { }
		public Redirect(int index, int secondIndex) : this(index, secondIndex, null, false) { }
		public Redirect(bool permanently) : this(-1, 0, null, permanently) { }
		public Redirect(int index, bool permanently) : this(index, 0, null, permanently) { }
		public Redirect(int index, int secondIndex, bool permanently) : this(index, secondIndex, null, permanently) {  }

		public Redirect(Expression<Func<TRelativeController, TWithActionsFromController>> action) : this(-1, 0, action, false) { }
		public Redirect(int index, Expression<Func<TRelativeController, TWithActionsFromController>> action) : this(index, 0, action, false) { }
		public Redirect(int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action) : this(index, secondIndex, action, false) { }
		public Redirect(Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently) : this(-1, 0, action, permanently) { }
		public Redirect(int index, Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently) : this(index, 0, action, permanently) { }
		public Redirect(int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently)
		{
			Index = index;
			SecondIndex = secondIndex;
			Action = action;
			Permanent = permanently;
		}

		protected override void Render(IHttpContext context)
		{
			// TODO: base.Render(context, context.Route.GetPath<TRelativeController, TWithActionsFromController>(Index, SecondIndex, Action));
		}

		public override string Message
		{
			get
			{
				return "A redirect was issued to: " + Action.ToString();
			}
		}
	}
}