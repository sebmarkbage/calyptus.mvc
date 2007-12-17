using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Linq.Expressions;

namespace Calyptus.MVC
{
    public class ViewMaster : MasterPage
    {
        private object _masterData;
        public object MasterData
        {
            get
            {
                return _masterData;
            }
            set
            {
                object[] args = value is object[] ? (object[])value : new object[] { value };
                this.GetType().InvokeMember("InitMasterData", BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, this, args);
                _masterData = value;
            }
        }

        public new ViewMaster Master { get { return (ViewMaster)base.Master; } }

        public IRoutingEngine Routing { get; set; }
        public string URL(Expression<Action> action)
        {
            return Routing.GetRelativePath(action);
        }
    }

    public class ViewMaster<T> : ViewMaster
    {
        public new T MasterData { get { return (T)base.MasterData; } set { base.MasterData = value; } }
    }
}
