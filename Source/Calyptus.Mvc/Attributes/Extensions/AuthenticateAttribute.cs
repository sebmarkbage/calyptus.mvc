using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Web.Security;

namespace Calyptus.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthenticateAttribute : Attribute, IExtension
    {
		private string[] _allowedUsers;
		private string[] _allowedRoles;
		private string[] _deniedUsers;
		private string[] _deniedRoles;

		private bool _denyAll;

		public string AllowUsers
		{
			get { return string.Join(",", _allowedUsers); }
			set {
				_allowedUsers = value == null || value.Trim() == "*" || value.Trim() == "" ? null : value.Split(',');
				if (_allowedUsers != null) for (int i = 0; i < _allowedUsers.Length; i++) _allowedUsers[i] = _allowedUsers[i].Trim();
			}
		}
		public string AllowRoles
		{
			get { return string.Join(",", _allowedRoles); }
			set {
				_allowedRoles = value == null || value.Trim() == "*" || value.Trim() == "" ? null : value.Split(',');
				if (_allowedRoles != null) for (int i = 0; i < _allowedRoles.Length; i++) _allowedRoles[i] = _allowedRoles[i].Trim();
			}
		}
		public string DenyUsers
		{
			get { return string.Join(",", _deniedUsers); }
			set {
				if (value.Trim() == "*") _denyAll = true;
				_deniedUsers = value == null || value.Trim() == "*" || value.Trim() == "" ? null : value.Split(',');
				if (_deniedUsers != null) for (int i = 0; i < _deniedUsers.Length; i++) _deniedUsers[i] = _deniedUsers[i].Trim();
			}
		}
		public string DenyRoles
		{
			get { return string.Join(",", _deniedRoles); }
			set {
				if (value.Trim() == "*") _denyAll = true;
				_deniedRoles = value == null || value.Trim() == "*" || value.Trim() == "" ? null : value.Split(',');
				if (_deniedRoles != null) for (int i = 0; i < _deniedRoles.Length; i++) _deniedRoles[i] = _deniedRoles[i].Trim();
			}
		}

		public AuthenticateAttribute()
		{
		}

		public void Initialize(MemberInfo target) { }

		public void OnBeforeAction(IHttpContext context, BeforeActionEventArgs args)
		{
			if (context.User.Identity.IsAuthenticated)
			{
				string name = context.User.Identity.Name.Trim();

				bool allow = !_denyAll && (_allowedUsers == null && _allowedRoles == null);

				if (allow && _deniedRoles != null)
					foreach (string role in _deniedRoles)
						if (context.User.IsInRole(role))
						{
							allow = false;
							break;
						}

				if (allow && _deniedUsers != null)
					foreach (string user in _deniedUsers)
						if (user.Equals(name, StringComparison.InvariantCultureIgnoreCase))
						{
							allow = false;
							break;
						}

				if (!allow && _allowedRoles != null)
					foreach (string role in _allowedRoles)
						if (context.User.IsInRole(role))
						{
							allow = true;
							break;
						}

				if (!allow && _allowedUsers != null)
					foreach (string user in _allowedUsers)
						if (user.Equals(name, StringComparison.InvariantCultureIgnoreCase))
						{
							allow = true;
							break;
						}

				FormsAuthentication.RedirectToLoginPage();
			}
			else
				FormsAuthentication.RedirectToLoginPage();
		}

		public void OnError(IHttpContext context, ErrorEventArgs args) { }

		public void OnAfterAction(IHttpContext context, AfterActionEventArgs args) { }

		public void OnBeforeRender(IHttpContext context, BeforeRenderEventArgs args) { }

		public void OnAfterRender(IHttpContext context, AfterRenderEventArgs args) { }
	}
}
