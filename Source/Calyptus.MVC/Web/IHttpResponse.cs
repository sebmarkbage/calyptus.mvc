using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Security.Principal;
using System.Web.Caching;
using System.Web.Profile;
using System.IO;
using System.Collections.Specialized;

namespace Calyptus.MVC
{
	public interface IHttpResponse
	{
		void AddCacheDependency(params CacheDependency[] dependencies);
		void AddCacheItemDependencies(ArrayList cacheKeys);
		void AddCacheItemDependencies(string[] cacheKeys);
		void AddCacheItemDependency(string cacheKey);
		void AddFileDependencies(ArrayList filenames);
		void AddFileDependencies(string[] filenames);
		void AddFileDependency(string filename);
		void AppendCookie(HttpCookie cookie);
		void AppendHeader(string name, string value);
		void AppendToLog(string param);
		string ApplyAppPathModifier(string virtualPath);
		void BinaryWrite(byte[] buffer);
		void Clear();
		void ClearContent();
		void ClearHeaders();
		void DisableKernelCache();
		void End();
		void Flush();
		void Pics(string value);
		void Redirect(string url);
		void Redirect(string url, bool endResponse);
		void SetCookie(HttpCookie cookie);
		TextWriter SwitchWriter(TextWriter writer);
		void TransmitFile(string filename);
		void TransmitFile(string filename, long offset, long length);
		void Write(object obj);
		void Write(string s);
		void Write(char[] buffer, int index, int count);
		void WriteFile(string filename);
		void WriteFile(string filename, bool readIntoMemory);
		void WriteFile(IntPtr fileHandle, long offset, long size);
		void WriteFile(string filename, long offset, long size);
		void WriteSubstitution(HttpResponseSubstitutionCallback callback);

		bool Buffer { get; set; }
		bool BufferOutput { get; set; }
		HttpCachePolicy Cache { get; }
		string CacheControl { get; set; }
		string Charset { get; set; }
		Encoding ContentEncoding { get; set; }
		string ContentType { get; set; }
		HttpCookieCollection Cookies { get; }
		int Expires { get; set; }
		DateTime ExpiresAbsolute { get; set; }
		Stream Filter { get; set; }
		Encoding HeaderEncoding { get; set; }
		NameValueCollection Headers { get; }
		bool IsClientConnected { get; }
		bool IsRequestBeingRedirected { get; }
		TextWriter Output { get; }
		string RedirectLocation { get; set; }
		string Status { get; set; }
		int StatusCode { get; set; }
		string StatusDescription { get; set; }
		int SubStatusCode { get; set; }
		bool SuppressContent { get; set; }
		bool TrySkipIisCustomErrors { get; set; }
	}
}
