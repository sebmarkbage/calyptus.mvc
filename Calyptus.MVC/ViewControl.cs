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

        public string URL(Expression<Action> action)
        {
            return Routing.GetRelativePath(action);
        }
    }
}
