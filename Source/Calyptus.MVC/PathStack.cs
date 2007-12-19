using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

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

        internal PathStack(string[] path, NameValueCollection query, bool readOnly)
        {
			QueryString = query ?? new NameValueCollection();

            _path = path ?? new string[] {};
            _currentIndex = _path.Length > 0 ? 0 : -1;

            _readOnly = readOnly;
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
