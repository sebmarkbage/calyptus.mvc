using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Calyptus.Mvc
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class AuthenticationCookieAttribute : CookieAttribute
	{
		public AuthenticationCookieAttribute(string name) : base(name) { }
		public AuthenticationCookieAttribute() : base() { }

		protected override string SerializeBinding(object value)
		{
			var name = base.SerializeBinding(value);
			var ticket = new FormsAuthenticationTicket(name, ExpiresInSeconds > 0, (ExpiresInSeconds > 60) ? (int)(ExpiresInSeconds / 60) : 1);
			return FormsAuthentication.Encrypt(ticket);
		}

		protected override bool TryBinding(string value, out object obj)
		{
			var ticket = FormsAuthentication.Decrypt(value);
			if (ticket.Expired)
			{
				obj = null;
				return false;
			}
			return base.TryBinding(ticket.Name, out obj);
		}
	}
}
