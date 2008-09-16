using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Security.Principal;
using System.Web.SessionState;
using System.Web.Caching;
using System.IO;
using System.Xml;
using System.Web.Profile;
using System.Web.Security;

namespace Calyptus.MVC
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public class ContextAttribute : Attribute, IPropertyBinding, IParameterBinding
	{
		private Func<IHttpContext, object> binder;
		private bool isPrincipal;
		private bool isTicket;
		private bool isCookie;
		private bool isPathStack;
		private string name;

		public void Initialize(ParameterInfo parameter)
		{
			Type t = parameter.ParameterType.IsByRef ? parameter.ParameterType.GetElementType() : parameter.ParameterType;
			if (t == typeof(IPathStack)) { isPathStack = true; return; }
			isTicket = t == typeof(FormsAuthenticationTicket);
			isCookie = t == typeof(HttpCookie);
			if (isCookie)
				binder = c => c.Request.Cookies[name];
			else
				binder = GetBinder(t);
			name = parameter.Name;
			if (binder == null) throw new BindingException(String.Format("Invalid context type. ContextAttribute can't bind to type '{0}'.", parameter.ParameterType.Name));
		}

		public void Initialize(PropertyInfo property)
		{
			Type t = property.PropertyType;
			isPrincipal = t == typeof(IPrincipal);
			isTicket = t == typeof(FormsAuthenticationTicket);
			isCookie = t == typeof(HttpCookie);
			if (isCookie)
				binder = c => c.Request.Cookies[name];
			else
				binder = GetBinder(t);
			name = property.Name;
			if (binder == null) throw new BindingException(String.Format("Invalid context type. ContextAttribute can't bind to type '{0}'.", property.PropertyType.Name));
		}

		public bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight)
		{
			overloadWeight = 0;
			if (isPathStack) { obj = path; return true; }
			return TryBinding(context, out obj);
		}

		public bool TryBinding(IHttpContext context, out object obj)
		{
			obj = binder(context);
			return true;
		}

		public static bool IsContextType(Type type)
		{
			return (type == typeof(IPathStack) || type == typeof(HttpCookie) || GetBinder(type) != null);
		}

		private static Func<IHttpContext, object> GetBinder(Type type)
		{
			if (type == typeof(HttpBrowserCapabilities)) return c => c.Request.Browser;
			else if (type == typeof(Cache)) return c => c.Cache;
			else if (type == typeof(HttpApplicationState)) return c => c.Application;
			else if (type == typeof(HttpApplication)) return c => c.ApplicationInstance;
			else if (type == typeof(FormsAuthenticationTicket)) return c =>
			{
				if (c.Request.Cookies[FormsAuthentication.FormsCookieName] == null) return null;
				try
				{
					return FormsAuthentication.Decrypt(c.Request.Cookies[FormsAuthentication.FormsCookieName].Value);
				}
				catch (ArgumentException)
				{
					return null;
				}
			};
			else if (type == typeof(IHttpSessionState)) return c => c.Session;
			else if (type == typeof(ProfileBase)) return c => c.Profile;
			else if (type == typeof(IPrincipal)) return c => c.User;
			else if (type == typeof(IIdentity)) return c => c.User.Identity;
			else if (type == typeof(IHttpContext)) return c => c;
			else if (type == typeof(IHttpRequest)) return c => c.Request;
			else if (type == typeof(IHttpResponse)) return c => c.Response;
			else if (type == typeof(Stream)) return c => new AmbiguousInputOutputStream(c.Request, c.Response);
			else if (type == typeof(TextReader)) return c => new StreamReader(c.Request.InputStream, c.Request.ContentEncoding);
			else if (type == typeof(BinaryReader)) return c => new BinaryReader(c.Request.InputStream, c.Response.ContentEncoding);
			else if (type == typeof(XmlReader)) return c => System.Xml.XmlReader.Create(new StreamReader(c.Request.InputStream));
			else if (type == typeof(TextWriter)) return c => c.Response.Output;
			else if (type == typeof(BinaryWriter)) return c => new BinaryWriter(c.Response.OutputStream, c.Response.ContentEncoding);
			else if (type == typeof(TextWriter)) return c => c.Response.Output;
			else if (type == typeof(XmlWriter)) return c => System.Xml.XmlWriter.Create(c.Response.Output);
			else if (type == typeof(IRouteContext)) return c => c.Route;
			else if (type == typeof(IRoutingEngine)) return c => c.Route.RoutingEngine;
			else if (type == typeof(IViewFactory)) return c => c.ViewFactory;
			else return null;
		}

		public void SerializePath(IPathStack path, object value)
		{
			// Exclude
		}

		public void StoreBinding(IHttpContext context, object value)
		{
			if (isPrincipal) context.User = (IPrincipal)value;
			else if (isCookie) context.Response.SetCookie((HttpCookie)value);
			else if (isTicket)
			{
				if (value == null) context.Response.SetCookie(new HttpCookie(FormsAuthentication.FormsCookieName));
				else
				{
					HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt((FormsAuthenticationTicket)value))
					{
						Domain = FormsAuthentication.CookieDomain,
						Path = FormsAuthentication.FormsCookiePath,
						Secure = FormsAuthentication.RequireSSL
					};
					if (((FormsAuthenticationTicket)value).IsPersistent)
						cookie.Expires = ((FormsAuthenticationTicket)value).Expiration;
					context.Response.SetCookie(cookie);
				}
			}
			// Else Exclude
		}

		private class AmbiguousInputOutputStream : Stream
		{
			private IHttpResponse response;
			private IHttpRequest request;

			private Stream _input;
			private Stream _output;

			private Stream inputStream { get { return _input == null ? _input = request.InputStream : _input; } }
			private Stream outputStream { get { return _output == null ? _output = response.OutputStream : _output; } }

			public AmbiguousInputOutputStream(IHttpRequest request, IHttpResponse response)
			{
				this.request = request;
				this.response = response;
			}

			public override bool CanRead
			{
				get { return inputStream.CanRead; }
			}

			public override bool CanSeek
			{
				get { return false; }
			}

			public override bool CanWrite
			{
				get { return outputStream.CanWrite; }
			}

			public override void Flush()
			{
				outputStream.Flush();
			}

			public override long Length
			{
				get { return inputStream.Length; }
			}

			public override long Position
			{
				get
				{
					throw new NotImplementedException("Cannot seek in this stream since it's ambiguous whether it's an output or input stream. Try using the ResponseAttribute or RequestAttribute on the parameter or property.");
				}
				set
				{
					throw new NotImplementedException("Cannot seek in this stream since it's ambiguous whether it's an output or input stream. Try using the ResponseAttribute or RequestAttribute on the parameter or property.");
				}
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				return inputStream.Read(buffer, offset, count);
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				throw new NotImplementedException("Cannot seek in this stream since it's ambiguous whether it's an output or input stream. Try using the ResponseAttribute or RequestAttribute on the parameter or property.");
			}

			public override void SetLength(long value)
			{
				outputStream.SetLength(value);
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				outputStream.Write(buffer, offset, count);
			}

			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				return inputStream.BeginRead(buffer, offset, count, callback, state);
			}

			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				return outputStream.BeginWrite(buffer, offset, count, callback, state);
			}

			public override int EndRead(IAsyncResult asyncResult)
			{
				return inputStream.EndRead(asyncResult);
			}

			public override void EndWrite(IAsyncResult asyncResult)
			{
				outputStream.EndWrite(asyncResult);
			}

			public override void Close()
			{
				// Close which ever one was used or both
				if (_output != null)
					_output.Close();
				if (_input != null)
					_input.Close();
			}

			public override int ReadByte()
			{
				return inputStream.ReadByte();
			}

			public override void WriteByte(byte value)
			{
				outputStream.WriteByte(value);
			}

			public override int ReadTimeout
			{
				get
				{
					return inputStream.ReadTimeout;
				}
				set
				{
					inputStream.ReadTimeout = value;
				}
			}

			public override int WriteTimeout
			{
				get
				{
					return outputStream.WriteTimeout;
				}
				set
				{
					outputStream.WriteTimeout = value;
				}
			}
		}

	}
}
