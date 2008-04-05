using System;
using System.Web;
using System.Web.UI;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;

namespace Calyptus.MVC
{
    public abstract class ViewPage : Page, IView
    {
		public new ViewMaster Master { get { return (ViewMaster)base.Master; } }

		public new ViewPage Page { get { return (ViewPage)base.Page; } }

        public IRoutingEngine Routing { get; set; }

		protected virtual string URL<T>(Expression<Action<T>> action)
		{
			return Routing.GetRelativePath<T>(int.MaxValue, action);
		}

		protected virtual string URL<T>(int index, Expression<Action<T>> action)
		{
			return Routing.GetRelativePath<T>(index, action);
		}

		protected virtual string URLReplace<T>(Expression<Action<T>> action)
		{
			return Routing.GetReplacementPath<T>(int.MaxValue, action);
		}

		protected virtual string URLReplace<T>(int index, Expression<Action<T>> action)
		{
			return Routing.GetReplacementPath<T>(index, action);
		}

		protected virtual string URLAbsolute<T>(Expression<Action<T>> action)
		{
			return Routing.GetAbsolutePath<T>(action);
		}

		public void Render(IHttpContext context)
		{
			this.Routing = context.RoutingEngine;
			((IHttpHandler)this).ProcessRequest(context.ApplicationInstance.Context);
		}
	}

    public class ViewPage<TemplateType> : ViewPage, IView<TemplateType> where TemplateType : IViewTemplate
    {
		public TemplateType Data { get; set; }

		public void Render(IHttpContext context, TemplateType template)
		{
			this.Data = template;
			this.Render(context);
		}
	}

	public class ViewPage<TemplateType, MasterType> : ViewPage<TemplateType>, IView<TemplateType, MasterType> where TemplateType : IViewTemplate<MasterType> where MasterType : IViewTemplate
	{
		public new ViewMaster<MasterType> Master {
			get
			{
				return (ViewMaster<MasterType>) base.Master;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		IView<MasterType> IView<TemplateType, MasterType>.Master
		{
			get
			{
				return this.Master;
			}
			set
			{
				if (value == null) throw new NullReferenceException("WebForms Master not defined.");
				ViewMaster<MasterType> master = value as ViewMaster<MasterType>;
				if (master == null) throw new Exception(String.Format("Master '{0}' is not valid WebForms Master Page.", value.GetType().FullName));
				this.Master = master;
			}
		}
	}
}
