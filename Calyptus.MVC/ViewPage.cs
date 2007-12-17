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
                object[] args = value is object[] ? (object[])value : new object[] { value };
                this.GetType().InvokeMember("InitViewData", BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, this, args);
                _viewData = value;
            }
        }

        public IRoutingEngine Routing { get; set; }

        public string URL(Expression<Action> action)
        {
            return Routing.GetRelativePath(action);
        }
    }

    public class ViewPage<T> : ViewPage
    {
        public new T ViewData { get { return (T)base.ViewData; } set { base.ViewData = value; } }
    }
}
