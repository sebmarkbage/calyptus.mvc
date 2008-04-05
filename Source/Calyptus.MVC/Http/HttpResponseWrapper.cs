using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Collections;

namespace Calyptus.MVC
{
	internal sealed class HttpResponseWrapper : IHttpResponse
	{
		private HttpResponse _httpResponse;

		public HttpResponseWrapper(HttpResponse httpResponse)
		{
			this._httpResponse = httpResponse;
		}

		void IHttpResponse.AddCacheDependency(params CacheDependency[] dependencies)
		{
			this._httpResponse.AddCacheDependency(dependencies);
		}

		void IHttpResponse.AddCacheItemDependencies(ArrayList cacheKeys)
		{
			this._httpResponse.AddCacheItemDependencies(cacheKeys);
		}

		void IHttpResponse.AddCacheItemDependencies(string[] cacheKeys)
		{
			this._httpResponse.AddCacheItemDependencies(cacheKeys);
		}

		void IHttpResponse.AddCacheItemDependency(string cacheKey)
		{
			this._httpResponse.AddCacheItemDependency(cacheKey);
		}

		void IHttpResponse.AddFileDependencies(string[] filenames)
		{
			this._httpResponse.AddFileDependencies(filenames);
		}

		void IHttpResponse.AddFileDependencies(ArrayList filenames)
		{
			this._httpResponse.AddFileDependencies(filenames);
		}

		void IHttpResponse.AddFileDependency(string filename)
		{
			this._httpResponse.AddFileDependency(filename);
		}

		void IHttpResponse.AppendCookie(HttpCookie cookie)
		{
			this._httpResponse.AppendCookie(cookie);
		}

		void IHttpResponse.AppendHeader(string name, string value)
		{
			this._httpResponse.AppendHeader(name, value);
		}

		void IHttpResponse.AppendToLog(string param)
		{
			this._httpResponse.AppendToLog(param);
		}

		string IHttpResponse.ApplyAppPathModifier(string virtualPath)
		{
			return this._httpResponse.ApplyAppPathModifier(virtualPath);
		}

		void IHttpResponse.BinaryWrite(byte[] buffer)
		{
			this._httpResponse.BinaryWrite(buffer);
		}

		void IHttpResponse.Clear()
		{
			this._httpResponse.Clear();
		}

		void IHttpResponse.ClearContent()
		{
			this._httpResponse.ClearContent();
		}

		void IHttpResponse.ClearHeaders()
		{
			this._httpResponse.ClearHeaders();
		}

		void IHttpResponse.DisableKernelCache()
		{
			this._httpResponse.DisableKernelCache();
		}

		void IHttpResponse.End()
		{
			this._httpResponse.End();
		}

		void IHttpResponse.Flush()
		{
			this._httpResponse.Flush();
		}

		void IHttpResponse.Pics(string value)
		{
			this._httpResponse.Pics(value);
		}

		void IHttpResponse.Redirect(string url)
		{
			this._httpResponse.Redirect(url);
		}

		void IHttpResponse.Redirect(string url, bool endResponse)
		{
			this._httpResponse.Redirect(url, endResponse);
		}

		void IHttpResponse.SetCookie(HttpCookie cookie)
		{
			this._httpResponse.SetCookie(cookie);
		}

		void IHttpResponse.TransmitFile(string filename)
		{
			this._httpResponse.TransmitFile(filename);
		}

		void IHttpResponse.TransmitFile(string filename, long offset, long length)
		{
			this._httpResponse.TransmitFile(filename, offset, length);
		}

		void IHttpResponse.Write(object obj)
		{
			this._httpResponse.Write(obj);
		}

		void IHttpResponse.Write(string s)
		{
			this._httpResponse.Write(s);
		}

		void IHttpResponse.Write(char[] buffer, int index, int count)
		{
			this._httpResponse.Write(buffer, index, count);
		}

		void IHttpResponse.WriteFile(string filename)
		{
			this._httpResponse.WriteFile(filename);
		}

		void IHttpResponse.WriteFile(string filename, bool readIntoMemory)
		{
			this._httpResponse.WriteFile(filename, readIntoMemory);
		}

		void IHttpResponse.WriteFile(IntPtr fileHandle, long offset, long size)
		{
			this._httpResponse.WriteFile(fileHandle, offset, size);
		}

		void IHttpResponse.WriteFile(string filename, long offset, long size)
		{
			this._httpResponse.WriteFile(filename, offset, size);
		}

		void IHttpResponse.WriteSubstitution(HttpResponseSubstitutionCallback callback)
		{
			this._httpResponse.WriteSubstitution(callback);
		}

		bool IHttpResponse.Buffer
		{
			get
			{
				return this._httpResponse.Buffer;
			}
			set
			{
				this._httpResponse.Buffer = value;
			}
		}

		bool IHttpResponse.BufferOutput
		{
			get
			{
				return this._httpResponse.BufferOutput;
			}
			set
			{
				this._httpResponse.BufferOutput = value;
			}
		}

		HttpCachePolicy IHttpResponse.Cache
		{
			get
			{
				return this._httpResponse.Cache;
			}
		}

		string IHttpResponse.CacheControl
		{
			get
			{
				return this._httpResponse.CacheControl;
			}
			set
			{
				this._httpResponse.CacheControl = value;
			}
		}

		string IHttpResponse.Charset
		{
			get
			{
				return this._httpResponse.Charset;
			}
			set
			{
				this._httpResponse.Charset = value;
			}
		}

		Encoding IHttpResponse.ContentEncoding
		{
			get
			{
				return this._httpResponse.ContentEncoding;
			}
			set
			{
				this._httpResponse.ContentEncoding = value;
			}
		}

		string IHttpResponse.ContentType
		{
			get
			{
				return this._httpResponse.ContentType;
			}
			set
			{
				this._httpResponse.ContentType = value;
			}
		}

		HttpCookieCollection IHttpResponse.Cookies
		{
			get
			{
				return this._httpResponse.Cookies;
			}
		}

		int IHttpResponse.Expires
		{
			get
			{
				return this._httpResponse.Expires;
			}
			set
			{
				this._httpResponse.Expires = value;
			}
		}

		DateTime IHttpResponse.ExpiresAbsolute
		{
			get
			{
				return this._httpResponse.ExpiresAbsolute;
			}
			set
			{
				this._httpResponse.ExpiresAbsolute = value;
			}
		}

		Stream IHttpResponse.Filter
		{
			get
			{
				return this._httpResponse.Filter;
			}
			set
			{
				this._httpResponse.Filter = value;
			}
		}

		Encoding IHttpResponse.HeaderEncoding
		{
			get
			{
				return this._httpResponse.HeaderEncoding;
			}
			set
			{
				this._httpResponse.HeaderEncoding = value;
			}
		}

		NameValueCollection IHttpResponse.Headers
		{
			get
			{
				return this._httpResponse.Headers;
			}
		}

		bool IHttpResponse.IsClientConnected
		{
			get
			{
				return this._httpResponse.IsClientConnected;
			}
		}

		bool IHttpResponse.IsRequestBeingRedirected
		{
			get
			{
				return this._httpResponse.IsRequestBeingRedirected;
			}
		}

		TextWriter IHttpResponse.Output
		{
			get
			{
				return this._httpResponse.Output;
			}
		}

		string IHttpResponse.RedirectLocation
		{
			get
			{
				return this._httpResponse.RedirectLocation;
			}
			set
			{
				this._httpResponse.RedirectLocation = value;
			}
		}

		string IHttpResponse.Status
		{
			get
			{
				return this._httpResponse.Status;
			}
			set
			{
				this._httpResponse.Status = value;
			}
		}

		int IHttpResponse.StatusCode
		{
			get
			{
				return this._httpResponse.StatusCode;
			}
			set
			{
				this._httpResponse.StatusCode = value;
			}
		}

		string IHttpResponse.StatusDescription
		{
			get
			{
				return this._httpResponse.StatusDescription;
			}
			set
			{
				this._httpResponse.StatusDescription = value;
			}
		}

		int IHttpResponse.SubStatusCode
		{
			get
			{
				return this._httpResponse.SubStatusCode;
			}
			set
			{
				this._httpResponse.SubStatusCode = value;
			}
		}

		bool IHttpResponse.SuppressContent
		{
			get
			{
				return this._httpResponse.SuppressContent;
			}
			set
			{
				this._httpResponse.SuppressContent = value;
			}
		}

		bool IHttpResponse.TrySkipIisCustomErrors
		{
			get
			{
				return this._httpResponse.TrySkipIisCustomErrors;
			}
			set
			{
				this._httpResponse.TrySkipIisCustomErrors = value;
			}
		}
	}

}
