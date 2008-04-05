using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Linq.Expressions;

namespace Calyptus.MVC
{
    public abstract class ViewMaster : MasterPage
    {
        public new ViewMaster Master { get { return (ViewMaster)base.Master; } }

		public new ViewPage Page { get { return (ViewPage)base.Page; } }

		public IRoutingEngine Routing { get { return Page.Routing; } }

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
	}

    public abstract class ViewMaster<TemplateType> : ViewMaster, IView<TemplateType> where TemplateType : IViewTemplate
    {
		public new TemplateType Data { get; set; }

		public void Render(IHttpContext context, TemplateType template)
		{
			this.Data = template;
			throw new NotImplementedException();
		}

		public void Render(IHttpContext context)
		{
			throw new NotImplementedException();
		}
	}

	public abstract class ViewMaster<TemplateType, MasterType> : ViewMaster, IView<TemplateType, MasterType> where TemplateType : IViewTemplate<MasterType> where MasterType : IViewTemplate
	{
		public new TemplateType Data { get; set; }

		public void Render(IHttpContext context, TemplateType template)
		{
			this.Data = template;
			throw new NotImplementedException();
		}

		public void Render(IHttpContext context)
		{
			throw new NotImplementedException();
		}

		public new ViewMaster<MasterType> Master
		{
			get
			{
				return (ViewMaster<MasterType>)base.Master;
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
