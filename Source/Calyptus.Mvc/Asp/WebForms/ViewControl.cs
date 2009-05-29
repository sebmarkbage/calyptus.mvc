/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.IO;

namespace Calyptus.Mvc
{
    public class ViewControl : System.Web.UI.UserControl, IView
    {
        public new ViewPage Page { get { return (ViewPage)base.Page; } }

		public IRouteContext Route { get { return this.Page.Route; } }

		/*
		protected virtual string URL()
		{
			return Route.GetPath(null);
		}

		protected virtual string URL<TRelativeController>()
		{
			return Route.GetPath<TRelativeController>(-1, null);
		}

		protected virtual string URL<TRelativeController>(int index)
		{
			return Route.GetPath<TRelativeController>(index, null);
		}

		protected virtual string URL<TRelativeController, TWithActionsFromController>()
		{
			return Route.GetPath<TRelativeController, TWithActionsFromController>(-1, 0, null);
		}

		protected virtual string URL<TRelativeController, TWithActionsFromController>(int index)
		{
			return Route.GetPath<TRelativeController, TWithActionsFromController>(index, 0, null);
		}

		protected virtual string URL<TRelativeController, TWithActionsFromController>(int index, int secondIndex)
		{
			return Route.GetPath<TRelativeController, TWithActionsFromController>(index, secondIndex, null);
		}

		protected virtual string URL<TRelativeController>(Expression<Action<TRelativeController>> action)
		{
			return Route.GetPath<TRelativeController>(-1, action);
		}

		protected virtual string URL<TRelativeController>(int index, Expression<Action<TRelativeController>> action)
		{
			return Route.GetPath<TRelativeController>(index, action);
		}

		protected virtual string URL<TRelativeController, TWithActionsFromController>(Expression<Func<TRelativeController, TWithActionsFromController>> action)
		{
			return Route.GetPath<TRelativeController, TWithActionsFromController>(-1, 0, action);
		}

		protected virtual string URL<TRelativeController, TWithActionsFromController>(int index, Expression<Func<TRelativeController, TWithActionsFromController>> action)
		{
			return Route.GetPath<TRelativeController, TWithActionsFromController>(index, 0, action);
		}

		protected virtual string URL<TRelativeController, TWithActionsFromController>(int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action)
		{
			return Route.GetPath<TRelativeController, TWithActionsFromController>(index, secondIndex, action);
		}

		protected virtual string URL(Expression<Action> action)
		{
			return Route.GetPath(action);
		}
		* /

		public string ContentType
		{
			get { return ((IView)Page).ContentType; }
		}

		public void Render(TextWriter stream)
		{
			ViewPage p = new ViewPage();
			p.Controls.Add(this);
			p.Render(stream);
		}

		public void Render(IHttpContext context)
		{
			ViewPage p = new ViewPage();
			p.Controls.Add(this);
			p.Render(context);
		}

		internal virtual void SetTemplate(IViewTemplate template)
		{
		}
	}

	public class ViewControl<TTemplate> : ViewControl, IView<TTemplate> where TTemplate : class, IViewTemplate
	{
		public TTemplate Data { get; set; }

		TTemplate IView<TTemplate>.Template { get { return Data; } set { Data = value; } }

		internal override void SetTemplate(IViewTemplate template)
		{
			Data = (TTemplate)template;
		}
	}
}
*/