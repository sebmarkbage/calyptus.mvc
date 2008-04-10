using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace Calyptus.MVC
{
    public class PathStack : IPathStack
    {
        private string[] _path;
        private int _currentIndex;

        private bool _readOnly;

		public NameValueCollection QueryString { get; private set; }

		private PathStack() { }

		public int Count { get { return _path.Length; } }

        internal PathStack(bool readOnly) : this(null, null, readOnly) { }

        internal PathStack(string path, NameValueCollection query, bool readOnly)
        {
			QueryString = query ?? new NameValueCollection();

			if (path == null)
				_path = new string[] { };
			else
			{
				_path = path.Split('/');
				for (int i = 0; i < _path.Length; i++)
					_path[i] = Decode(_path[i]);

				if (_path.Length > 0)
				{
					string ext = Configuration.Config.GetExtension();
					string fp = _path[0];
					if (ext != null && fp.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase))
						_path[0] = fp.Substring(0, fp.Length - ext.Length);

					if (_path[_path.Length - 1] == string.Empty)
					{
						Array.Resize(ref _path, _path.Length - 1);
						_trailingSlash = true;
					}
				}
				else
				{
					_trailingSlash = true;
				}
			}
            _currentIndex = 0;
            _readOnly = readOnly;
        }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (string path in _path)
			{
				if (sb.Length > 0) sb.Append('/');
				sb.Append(Encode(path));
			}
			if (TrailingSlash) sb.Append('/');

			if (QueryString.Count > 0)
			{
				bool first = true;
				for (int i = 0; i < QueryString.Count; i++)
				{
					string key = QueryString.Keys[i];
					key = string.IsNullOrEmpty(key) ? null : HttpUtility.UrlEncodeUnicode(key) + "=";
					
					foreach (string value in QueryString.GetValues(i))
						if (value != null)
						{
							if (first) { sb.Append('?'); first = false; } else sb.Append('&');
							sb.Append(key);
							sb.Append(HttpUtility.UrlEncodeUnicode(value));
						}
				}
			}
			return sb.ToString();
		}

		private static char[] _specialChars = new char[] { '.', '/', ':', '%', '&', '<', '>', '#', '?', '\\', '+', '\"', '|', '\r', '\n', '\t', '*' };
		//private static string[] _specialCharsEncoded = new string[] { "!2e", "!2f", "!3a", "!25", "!26", "!3c", "!3e", "!23", "!3f", "!5c", "!2b", "!22", "!7c", "!0d", "!0a", "!09", "!2a" };

		protected string Decode(string path)
		{
			path = HttpUtility.UrlDecode(path);

			for (int i = 0; i < _specialChars.Length; i++)
			{
				char c = _specialChars[i];
				string ce = ((int)c).ToString("x");
				path = path.Replace('!' + ce, c.ToString());
			}
			return path.Replace("!21", "!");
		}

		protected string Encode(string path)
		{
			path = path.Replace("!21", "!2121");
			for (int i = 0; i < _specialChars.Length; i++)
			{
				char c = _specialChars[i];
				string ce = ((int)c).ToString("x");
				path = path.Replace('!' + ce, "!21" + ce).Replace(c.ToString(), '!' + ce);
			}
			return HttpUtility.UrlPathEncode(path);
		}

		private bool _trailingSlash;

		public bool TrailingSlash
		{
			get
			{
				return _trailingSlash;
			}
			set
			{
				if (_readOnly) throw new InvalidOperationException("This PathStack is read only.");
				_trailingSlash = value;
			}
		}

		public bool IsAtEnd
		{
			get { return _currentIndex >= _path.Length || _path.Length == 0; }
		}

		public string Peek()
		{
			return IsAtEnd ? null : _path[_currentIndex];
		}

		public string Peek(int stepsForward)
		{
			int i = _currentIndex + stepsForward;
			if (i < 0 || i > _path.Length - 1)
				return null;
			return _path[i];
		}

		public string Pop()
		{
			if (IsAtEnd)
				return null;
			else
				return _path[_currentIndex++];
		}

		public void Pop(int count)
		{
			_currentIndex += count;
			if (_currentIndex >= _path.Length) _currentIndex = _path.Length;
		}

		public int Index
		{
			get
			{
				return _currentIndex;
			}
		}

		public void Reverse(int count)
		{
			_currentIndex -= count;
		}

		public void ReverseToIndex(int toIndex)
		{
			_currentIndex = toIndex;
		}

		public void Push(string path)
		{
			if (_readOnly)
				throw new InvalidOperationException("This PathStack is read only.");

			if (string.IsNullOrEmpty(path))
				throw new InvalidOperationException("The PathStack cannot Push null or empty strings.");

			if (_currentIndex == -1)
				path += Configuration.Config.GetExtension();

			if (_path.Length <= _currentIndex)
			{
				Array.Resize(ref _path, _currentIndex + 1);
			}
			_path[_currentIndex++] = path;
		}

		public void Push(IPathStack path)
		{
			if (_readOnly)
				throw new InvalidOperationException("This PathStack is read only.");

			if (path == null)
				throw new InvalidOperationException("The PathStack cannot Push null.");

			int index = path.Index;
			path.ReverseToIndex(0);
			while (!path.IsAtEnd)
				this.Push(path.Pop());

			this._trailingSlash = path.TrailingSlash;

			path.ReverseToIndex(index);

			QueryString.Add(path.QueryString);
		}
	}
}
