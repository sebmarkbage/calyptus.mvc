using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;

namespace Calyptus.Mvc
{
	public class HttpSessionStateWrapper : IHttpSessionState
	{
		private HttpSessionState _session;

		public HttpSessionStateWrapper(HttpSessionState session)
		{
			this._session = session;
		}

		public void Abandon()
		{
			this._session.Abandon();
		}

		public void Add(string name, object value)
		{
			this._session.Add(name, value);
		}

		public void Clear()
		{
			this._session.Clear();
		}

		public int CodePage
		{
			get
			{
				return this._session.CodePage;
			}
			set
			{
				this._session.CodePage = value;
			}
		}

		public System.Web.HttpCookieMode CookieMode
		{
			get { return this._session.CookieMode; }
		}

		public void CopyTo(Array array, int index)
		{
			this._session.CopyTo(array, index);
		}

		public int Count
		{
			get { return this._session.Count; }
		}

		public System.Collections.IEnumerator GetEnumerator()
		{
			return this._session.GetEnumerator();
		}

		public bool IsCookieless
		{
			get { return this._session.IsCookieless; }
		}

		public bool IsNewSession
		{
			get { return this._session.IsNewSession; }
		}

		public bool IsReadOnly
		{
			get { return this._session.IsReadOnly; }
		}

		public bool IsSynchronized
		{
			get { return this._session.IsSynchronized; }
		}

		public System.Collections.Specialized.NameObjectCollectionBase.KeysCollection Keys
		{
			get { return this._session.Keys; }
		}

		public int LCID
		{
			get
			{
				return this._session.LCID;
			}
			set
			{
				this._session.LCID = value;;
			}
		}

		public SessionStateMode Mode
		{
			get { return this._session.Mode; }
		}

		public void Remove(string name)
		{
			this._session.Remove(name);
		}

		public void RemoveAll()
		{
			this._session.RemoveAll();
		}

		public void RemoveAt(int index)
		{
			this._session.RemoveAt(index);
		}

		public string SessionID
		{
			get { return this._session.SessionID; }
		}

		public System.Web.HttpStaticObjectsCollection StaticObjects
		{
			get { return this._session.StaticObjects; }
		}

		public object SyncRoot
		{
			get { return this._session.SyncRoot; }
		}

		public int Timeout
		{
			get
			{
				return this._session.Timeout;
			}
			set
			{
				this._session.Timeout = value;
			}
		}

		public object this[int index]
		{
			get
			{
				return this._session[index];
			}
			set
			{
				this._session[index] = value;
			}
		}

		public object this[string name]
		{
			get
			{
				return this._session[name];
			}
			set
			{
				this._session[name] = value;
			}
		}
	}
}
