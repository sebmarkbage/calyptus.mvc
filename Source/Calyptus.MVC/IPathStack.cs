using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Calyptus.MVC
{
	public interface IPathStack
	{
		NameValueCollection QueryString { get; }

		bool IsAtEnd { get; }
		string Peek();
		string Peek(int stepsForward);
		string Pop();
		void Pop(int count);

		int Index { get; }

		void Reverse(int count);
		void ReverseToIndex(int toIndex);
		
		void Push(string path);
	}
}
