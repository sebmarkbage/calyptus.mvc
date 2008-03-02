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
				}
			}
            _currentIndex = _path.Length > 0 ? 0 : -1;

            _readOnly = readOnly;
        }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (string path in _path)
			{
				if (sb.Length > 0) sb.Append('/');
				sb.Append(HttpUtility.UrlPathEncode(path));
			}
			return sb.ToString();
		}

		protected string Decode(string path)
		{
			return HttpUtility.UrlDecode(path);
		}

		protected string Encode(string path)
		{
			return HttpUtility.UrlPathEncode(path);
		}

		public bool IsAtEnd
		{
			get { return _currentIndex >= _path.Length || _path.Length == 0 || (_currentIndex == _path.Length -1  && _path[_currentIndex] == string.Empty); }
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
				string[] n = new string[_currentIndex + 1];
				Array.Copy(_path, n, _path.Length);
				_path = n;
			}
			_path[_currentIndex++] = path;
		}
	}
}
