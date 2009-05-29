using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.Mvc
{
	public class MappingException : Exception
	{
		public MappingException() : base() { }
		public MappingException(string message) : base(message) { }
		public MappingException(string message, Exception innerException) : base(message, innerException) { }
	}
}
