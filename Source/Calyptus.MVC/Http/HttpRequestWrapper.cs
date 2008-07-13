using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Principal;
using System.Collections.Specialized;
using System.IO;

namespace Calyptus.MVC
{
	internal class HttpRequestWrapper : IHttpRequest
	{
		// Fields
		private HttpRequest _httpRequest;

		// Methods
		public HttpRequestWrapper(HttpRequest httpRequest)
		{
			this._httpRequest = httpRequest;
		}

		byte[] IHttpRequest.BinaryRead(int count)
		{
			return this._httpRequest.BinaryRead(count);
		}

		int[] IHttpRequest.MapImageCoordinates(string imageFieldName)
		{
			return this._httpRequest.MapImageCoordinates(imageFieldName);
		}

		string IHttpRequest.MapPath(string virtualPath)
		{
			return this._httpRequest.MapPath(virtualPath);
		}

		string IHttpRequest.MapPath(string virtualPath, string baseVirtualDir, bool allowCrossAppMapping)
		{
			return this._httpRequest.MapPath(virtualPath, baseVirtualDir, allowCrossAppMapping);
		}

		void IHttpRequest.SaveAs(string filename, bool includeHeaders)
		{
			this._httpRequest.SaveAs(filename, includeHeaders);
		}

		void IHttpRequest.ValidateInput()
		{
			this._httpRequest.ValidateInput();
		}

		// Properties
		string[] IHttpRequest.AcceptTypes
		{
			get
			{
				return this._httpRequest.AcceptTypes;
			}
		}

		string IHttpRequest.AnonymousID
		{
			get
			{
				return this._httpRequest.AnonymousID;
			}
		}

		string IHttpRequest.ApplicationPath
		{
			get
			{
				return this._httpRequest.ApplicationPath;
			}
		}

		string IHttpRequest.AppRelativeCurrentExecutionFilePath
		{
			get
			{
				return this._httpRequest.AppRelativeCurrentExecutionFilePath;
			}
		}

		HttpBrowserCapabilities IHttpRequest.Browser
		{
			get
			{
				return this._httpRequest.Browser;
			}
		}

		HttpClientCertificate IHttpRequest.ClientCertificate
		{
			get
			{
				return this._httpRequest.ClientCertificate;
			}
		}

		Encoding IHttpRequest.ContentEncoding
		{
			get
			{
				return this._httpRequest.ContentEncoding;
			}
			set
			{
				this._httpRequest.ContentEncoding = value;
			}
		}

		int IHttpRequest.ContentLength
		{
			get
			{
				return this._httpRequest.ContentLength;
			}
		}

		string IHttpRequest.ContentType
		{
			get
			{
				return this._httpRequest.ContentType;
			}
			set
			{
				this._httpRequest.ContentType = value;
			}
		}

		HttpCookieCollection IHttpRequest.Cookies
		{
			get
			{
				return this._httpRequest.Cookies;
			}
		}

		string IHttpRequest.CurrentExecutionFilePath
		{
			get
			{
				return this._httpRequest.CurrentExecutionFilePath;
			}
		}

		string IHttpRequest.FilePath
		{
			get
			{
				return this._httpRequest.FilePath;
			}
		}

		HttpFileCollection IHttpRequest.Files
		{
			get
			{
				return this._httpRequest.Files;
			}
		}

		Stream IHttpRequest.Filter
		{
			get
			{
				return this._httpRequest.Filter;
			}
			set
			{
				this._httpRequest.Filter = value;
			}
		}

		NameValueCollection IHttpRequest.Params
		{
			get
			{
				return this._httpRequest.Params;
			}
		}

		NameValueCollection IHttpRequest.Form
		{
			get
			{
				return this._httpRequest.Form;
			}
		}

		NameValueCollection IHttpRequest.Headers
		{
			get
			{
				return this._httpRequest.Headers;
			}
		}

		string IHttpRequest.HttpMethod
		{
			get
			{
				return this._httpRequest.HttpMethod;
			}
		}

		Stream IHttpRequest.InputStream
		{
			get
			{
				return this._httpRequest.InputStream;
			}
		}

		bool IHttpRequest.IsAuthenticated
		{
			get
			{
				return this._httpRequest.IsAuthenticated;
			}
		}

		bool IHttpRequest.IsSecureConnection
		{
			get
			{
				return this._httpRequest.IsSecureConnection;
			}
		}

		string IHttpRequest.this[string key]
		{
			get
			{
				return this._httpRequest[key];
			}
		}

		WindowsIdentity IHttpRequest.LogonUserIdentity
		{
			get
			{
				return this._httpRequest.LogonUserIdentity;
			}
		}

		string IHttpRequest.Path
		{
			get
			{
				return this._httpRequest.Path;
			}
		}

		string IHttpRequest.PathInfo
		{
			get
			{
				return this._httpRequest.PathInfo;
			}
		}

		string IHttpRequest.PhysicalApplicationPath
		{
			get
			{
				return this._httpRequest.PhysicalApplicationPath;
			}
		}

		string IHttpRequest.PhysicalPath
		{
			get
			{
				return this._httpRequest.PhysicalPath;
			}
		}

		NameValueCollection IHttpRequest.QueryString
		{
			get
			{
				return this._httpRequest.QueryString;
			}
		}

		string IHttpRequest.RawUrl
		{
			get
			{
				return this._httpRequest.RawUrl;
			}
		}

		string IHttpRequest.RequestType
		{
			get
			{
				return this._httpRequest.RequestType;
			}
			set
			{
				this._httpRequest.RequestType = value;
			}
		}

		NameValueCollection IHttpRequest.ServerVariables
		{
			get
			{
				return this._httpRequest.ServerVariables;
			}
		}

		int IHttpRequest.TotalBytes
		{
			get
			{
				return this._httpRequest.TotalBytes;
			}
		}

		Uri IHttpRequest.Url
		{
			get
			{
				return this._httpRequest.Url;
			}
		}

		Uri IHttpRequest.UrlReferrer
		{
			get
			{
				return this._httpRequest.Url;
			}
		}

		string IHttpRequest.UserAgent
		{
			get
			{
				return this._httpRequest.UserAgent;
			}
		}

		string IHttpRequest.UserHostAddress
		{
			get
			{
				return this._httpRequest.UserHostAddress;
			}
		}

		string IHttpRequest.UserHostName
		{
			get
			{
				return this._httpRequest.UserHostName;
			}
		}

		string[] IHttpRequest.UserLanguages
		{
			get
			{
				return this._httpRequest.UserLanguages;
			}
		}
	}
}
