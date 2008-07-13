using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

namespace Calyptus.MVC
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class CookieAttribute : Attribute, IParameterBinding, IPropertyBinding
	{
		private Type _type;
		private bool _complexType;
		private string _name;

		public string Name { get { return _name; } set { if (value == null) throw new NullReferenceException("Cookie name cannot be null"); _name = value; } }

		public bool Secure { get; set; }
		public bool HttpOnly { get; set; }
		public string Domain { get; set; }
		public double ExpiresInSeconds { get; set; }
		public double ExpiresInDays { get { return ExpiresInSeconds == Double.MaxValue ? Double.MaxValue : ExpiresInSeconds / 86400; } set { ExpiresInSeconds = value == Double.MaxValue ? Double.MaxValue : value * 86400; } }
		public bool Compress { get; set; }

		public CookieAttribute()
		{
			HttpOnly = true;
			Secure = false;
			Compress = true;
		}

		public CookieAttribute(string name) : this()
		{
			this._name = name;
		}

		public void Initialize(ParameterInfo parameter)
		{
			_type = parameter.ParameterType;
			_complexType = NameValueSerialization.IsComplexType(_type);
			MethodInfo m = (MethodInfo)parameter.Member;
			Type c = m.DeclaringType;
			if (_name == null) _name = "MvcBinding." + c.Namespace + "." + c.Name + "." + m.Name + "(" + parameter.Name + ")";
		}
		public void Initialize(PropertyInfo property)
		{
			_type = property.PropertyType;
			_complexType = NameValueSerialization.IsComplexType(_type);
			Type c = property.DeclaringType;
			if (_name == null) _name = "MvcBinding." + c.Namespace + "." + c.Name + "." + property.Name;
		}

		public virtual bool TryBinding(IHttpContext context, out object obj)
		{
			HttpCookie cookie = context.Request.Cookies[_name];
			if (cookie == null)
			{
				obj = null;
				return false;
			}
			else if (this._type == typeof(HttpCookie))
			{
				obj = cookie;
				return true;
			}
			else if (Compress)
			{
				System.Web.UI.LosFormatter formatter = new System.Web.UI.LosFormatter();
				obj = formatter.Deserialize(cookie.Value);
			}
			else
			{
				if (!_complexType)
				{
					try
					{
						obj = Convert.ChangeType(cookie.Value, _type, System.Globalization.CultureInfo.InvariantCulture);
					}
					catch
					{
						obj = null;
					}
				}
				else
				{
					throw new NotImplementedException();
				}
			}
			return obj != null && obj.GetType().IsAssignableFrom(this._type);
		}

		public bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight)
		{
			overloadWeight = 1;
			return TryBinding(context, out obj);
		}

		public void StoreBinding(IHttpContext context, object value)
		{
			HttpCookie cookie;
			if (value == null)
			{
				cookie = null;
			}
			else if (this._type == typeof(HttpCookie))
			{
				cookie = (HttpCookie)value;
			}
			else
			{
				cookie = new HttpCookie(_name);
				cookie.HttpOnly = HttpOnly;
				cookie.Secure = Secure;
				if (Domain != null)
					cookie.Domain = null;
				if (ExpiresInSeconds != 0)
					cookie.Expires = ExpiresInSeconds == Double.MaxValue ? DateTime.MaxValue : DateTime.Now.AddSeconds(ExpiresInSeconds);

				if (Compress)
				{
					System.Web.UI.LosFormatter formatter = new System.Web.UI.LosFormatter();
					System.IO.StringWriter writer = new System.IO.StringWriter();
					formatter.Serialize(writer, value);
					cookie.Value = writer.ToString();
				}
				else
				{
					if (!_complexType)
					{
						try
						{
							cookie.Value = Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);
						}
						catch(Exception e)
						{
							throw new BindingException(e.Message, e);
						}
					}
					else
					{
						throw new NotImplementedException();
					}
				}
			}
			if (cookie == null)
				context.Response.Cookies.Remove(_name);
			else
			{
				HttpCookie requestCookie = context.Request.Cookies[_name];
				if (requestCookie == null || requestCookie.Value != cookie.Value) // Only set if value has changed
					context.Response.Cookies.Add(cookie);
			}
		}

		public void SerializePath(IPathStack path, object obj)
		{
			// Exclude	
		}
	}
}
