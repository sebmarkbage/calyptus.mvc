using System;
using System.Web;
using System.Web.UI;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;

namespace Calyptus.MVC
{
    public class ViewPage : Page
    {
        public new ViewMaster Master { get { return (ViewMaster)base.Master; } }

        private object _viewData;
        public object ViewData
        {
            get
            {
                return _viewData;
            }
            set
            {
                _viewData = value;
            }
        }

        public IRoutingEngine Routing { get; set; }

		protected virtual string URL<T>(Expression<Action<T>> action)
		{
			return Routing.GetRelativePath<T>(action);
		}

		protected virtual string URL<T>(int index, Expression<Action<T>> action)
		{
			return Routing.GetRelativePath<T>(index, action);
		}

		protected virtual string URLReplace<T>(Expression<Action<T>> action)
		{
			return Routing.GetReplacementPath<T>(action);
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

    public class ViewPage<T> : ViewPage
    {
        public new T ViewData { get { return (T)base.ViewData; } set { base.ViewData = value; } }
    }
}
