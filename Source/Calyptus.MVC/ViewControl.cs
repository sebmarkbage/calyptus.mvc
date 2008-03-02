using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Calyptus.MVC
{
    public class ViewControl : System.Web.UI.UserControl
    {
        public new ViewPage Page { get { return (ViewPage)base.Page; } }

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
}
