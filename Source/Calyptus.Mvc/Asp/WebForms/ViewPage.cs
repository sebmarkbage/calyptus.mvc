/*using System;
using System.Web;
using System.Web.UI;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace Calyptus.Mvc
{
    public class ViewPage : Page, IView
    {
		public new ViewMaster Master { get { return (ViewMaster)base.Master; } }

		public new ViewPage Page { get { return (ViewPage)base.Page; } }

        public IRouteContext Route { get; set; }

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
		string IView.ContentType
		{
			get
			{
				return Response.ContentType;
			}
		}

		internal virtual void SetTemplate(IViewTemplate template)
		{
		}
		private IViewTemplate _masterTemplate;

		public override string MasterPageFile
		{
			get;
			set;
		}

		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);
			base.MasterPageFile = this.MasterPageFile;
		}

		public void Render(TextWriter writer)
		{
			HttpContext context = new HttpContext(new HttpRequest("/", "http://localhost/", ""), new HttpResponse(writer));
			((IHttpHandler)this).ProcessRequest(context);
		}

		public void Render(IHttpContext context)
		{
			this.Route = context.Route;
			HttpContext c = new HttpContext(new HttpRequest(context.Request.PhysicalPath, context.Request.RawUrl, context.Request.Url.Query), new HttpResponse(context.Response.Output));
			((IHttpHandler)this).ProcessRequest(c);
		}
	}

    public abstract class ViewPage<TTemplate> : ViewPage, IView<TTemplate> where TTemplate : class, IViewTemplate
    {
		public TTemplate Data { get; set; }

		TTemplate IView<TTemplate>.Template { get { return Data; } set { Data = value; } }

		internal override void SetTemplate(IViewTemplate template)
		{
			Data = (TTemplate)template;
		}
	}

	public abstract class ViewPage<TTemplate, TMaster> : ViewPage<TTemplate>, IView<TTemplate, TMaster>
		where TTemplate : class, IViewTemplate<TMaster>
		where TMaster : class, IViewTemplate
	{
		public new ViewMaster<TMaster> Master {
			get
			{
				return (ViewMaster<TMaster>) base.Master;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		IView<TMaster> IView<TTemplate, TMaster>.Master
		{
			get
			{
				return this.Master;
			}
			set
			{
				if (value == null) throw new NullReferenceException("WebForms Master not defined.");
				ViewMaster<TMaster> master = value as ViewMaster<TMaster>;
				if (master == null) throw new Exception(String.Format("Master '{0}' is not valid WebForms Master Page.", value.GetType().FullName));
				this.Master = master;
			}
		}
	}
}
*/