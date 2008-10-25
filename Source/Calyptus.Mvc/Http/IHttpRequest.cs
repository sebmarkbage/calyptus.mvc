using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Security.Principal;
using System.Web.Caching;
using System.Web.Profile;
using System.Collections.Specialized;
using System.IO;

namespace Calyptus.Mvc
{
	public interface IHttpRequest
	{
		byte[] BinaryRead(int count);
		int[] MapImageCoordinates(string imageFieldName);
		string MapPath(string virtualPath);
		string MapPath(string virtualPath, string baseVirtualDir, bool allowCrossAppMapping);
		void SaveAs(string filename, bool includeHeaders);
		void ValidateInput();

		string[] AcceptTypes { get; }
		string AnonymousID { get; }
		string ApplicationPath { get; }
		string AppRelativeCurrentExecutionFilePath { get; }
		HttpBrowserCapabilities Browser { get; }
		HttpClientCertificate ClientCertificate { get; }
		Encoding ContentEncoding { get; set; }
		int ContentLength { get; }
		string ContentType { get; set; }
		HttpCookieCollection Cookies { get; }
		string CurrentExecutionFilePath { get; }
		string FilePath { get; }
		HttpFileCollection Files { get; }
		Stream Filter { get; set; }
		NameValueCollection Form { get; }
		NameValueCollection Headers { get; }
		string HttpMethod { get; }
		Stream InputStream { get; }
		bool IsAuthenticated { get; }
		bool IsSecureConnection { get; }
		string this[string key] { get; }
		WindowsIdentity LogonUserIdentity { get; }
		NameValueCollection Params { get; }
		string Path { get; }
		string PathInfo { get; }
		string PhysicalApplicationPath { get; }
		string PhysicalPath { get; }
		NameValueCollection QueryString { get; }
		string RawUrl { get; }
		string RequestType { get; set; }
		NameValueCollection ServerVariables { get; }
		int TotalBytes { get; }
		Uri Url { get; }
		Uri UrlReferrer { get; }
		string UserAgent { get; }
		string UserHostAddress { get; }
		string UserHostName { get; }
		string[] UserLanguages { get; }
	}
}
