using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calyptus.MVC.Binding
{
	public interface IBindable
	{
		void Initialize(Type type, string name);
		bool TryBinding(IHttpContext context, IPathStack path, out object obj);
		void SerializePath(IPathStack path, object obj);
	}
}
