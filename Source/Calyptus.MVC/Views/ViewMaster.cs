using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Linq.Expressions;

namespace Calyptus.Mvc
{
    public abstract class ViewMaster : MasterPage, IView
    {

        public new ViewMaster Master { get { return (ViewMaster)base.Master; } }

		public new ViewPage Page { get { return (ViewPage)base.Page; } }

		protected IRouteContext Route { get { return Page.Route; } }

		protected virtual string URL()
		{
			return Route.GetAbsolutePath(null);
		}

		protected virtual string URL<TRelativeController>()
		{
			return Route.GetRelativePath<TRelativeController>(-1, null);
		}

		protected virtual string URL<TRelativeController>(int index)
		{
			return Route.GetRelativePath<TRelativeController>(index, null);
		}

		protected virtual string URL<TRelativeController, TWithActionsFromController>()
		{
			return Route.GetRelativePath<TRelativeController, TWithActionsFromController>(-1, 0, null);
		}

		protected virtual string URL<TRelativeController, TWithActionsFromController>(int index)
		{
			return Route.GetRelativePath<TRelativeController, TWithActionsFromController>(index, 0, null);
		}

		protected virtual string URL<TRelativeController, TWithActionsFromController>(int index, int secondIndex)
		{
			return Route.GetRelativePath<TRelativeController, TWithActionsFromController>(index, secondIndex, null);
		}

		protected virtual string URL<TRelativeController>(Expression<Action<TRelativeController>> action)
		{
			return Route.GetRelativePath<TRelativeController>(-1, action);
		}

		protected virtual string URL<TRelativeController>(int index, Expression<Action<TRelativeController>> action)
		{
			return Route.GetRelativePath<TRelativeController>(index, action);
		}

		protected virtual string URL<TRelativeController, TWithActionsFromController>(Expression<Func<TRelativeController, TWithActionsFromController>> action)
		{
			return Route.GetRelativePath<TRelativeController, TWithActionsFromController>(-1, 0, action);
		}

		protected virtual string URL<TRelativeController, TWithActionsFromController>(int index, Expression<Func<TRelativeController, TWithActionsFromController>> action)
		{
			return Route.GetRelativePath<TRelativeController, TWithActionsFromController>(index, 0, action);
		}

		protected virtual string URL<TRelativeController, TWithActionsFromController>(int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action)
		{
			return Route.GetRelativePath<TRelativeController, TWithActionsFromController>(index, secondIndex, action);
		}

		protected virtual string URL(Expression<Action> action)
		{
			return Route.GetAbsolutePath(action);
		}

		internal virtual void SetTemplate(IViewTemplate template)
		{
		}

		string IView.ContentType
		{
			get { return ((IView)Page).ContentType; }
		}

		void IView.Render(System.IO.Stream stream)
		{
			throw new NotImplementedException();
		}

		public void Render(IHttpContext context)
		{
			throw new NotImplementedException();
		}

	}

	public abstract class ViewMaster<TTemplate> : ViewMaster, IView<TTemplate> where TTemplate : class, IViewTemplate
    {
		private TTemplate _data;

		public TTemplate Data { get { return _data; } }

		TTemplate IView<TTemplate>.Template { get { return _data; } set { SetTemplate(value); } }

		internal override void SetTemplate(IViewTemplate template)
		{
			_data = (TTemplate)template;
			base.SetTemplate(template);
		}
	}

	public abstract class ViewMaster<TTemplate, TMaster> : ViewMaster<TTemplate>, IView<TTemplate, TMaster>
		where TTemplate : class, IViewTemplate<TMaster>
		where TMaster : class, IViewTemplate
	{
		public void Render(IHttpContext context, TTemplate template)
		{
			throw new NotImplementedException();
		}

		public new ViewMaster<TMaster> Master
		{
			get
			{
				return (ViewMaster<TMaster>)base.Master;
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
