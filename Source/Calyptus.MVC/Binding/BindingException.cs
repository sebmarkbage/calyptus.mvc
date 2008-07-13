using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC
{
	public class BindingException : Exception
	{
		public BindingException() : base() { }
		public BindingException(string message) : base(message) { }
		public BindingException(string message, Exception innerException) : base(message, innerException) { }
	}
}
