using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC
{
    public class PathStack
    {
        private string[] _path;
        private int _currentIndex;

        private bool _readOnly;

        internal PathStack(bool readOnly) : this(new string[] {}, readOnly) { }

        internal PathStack(string[] path, bool readOnly)
        {
            _path = path == null ? new string[] {} : path;
            _currentIndex = _path.Length > 0 ? 0 : -1;
            _readOnly = readOnly;
        }

        public string Current
        {
            get
            {
                return _path[_currentIndex];
            }
        }

        public string Previous
        {
            get
            {
                if (IsFirst) return null;
                return _path[_currentIndex - 1];
            }
        }

        public string Next
        {
            get
            {
                if (IsLast) return null;
                return _path[_currentIndex + 1];
            }
        }

        public string Peek(int index)
        {
            int i = _currentIndex + index;
            if (i < 0 || i > _path.Length - 1)
                return null;
            return _path[i];
        }

        public string Consume()
        {
            string p = _path[_currentIndex];
            if (!IsLast) _currentIndex++;
            return p;
        }

        public bool IsLast
        {
            get
            {
                return _path.Length - 1 == _currentIndex;
            }
        }

        public bool IsFirst
        {
            get
            {
                return _currentIndex == 0;
            }
        }

        public void AddToPath(string path)
        {
            if (_readOnly)
                throw new Exception("This PathStack is read only.");
            string[] n = new string[_path.Length + 1];
            Array.Copy(_path, n, _path.Length);
            _path = n;
            _path[_path.Length - 1] = path;
        }
    }
}
