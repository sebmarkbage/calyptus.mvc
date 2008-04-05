using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Calyptus.MVC
{
	public static class NameValueSerialization
	{
		public static bool TryDeserialize(NameValueCollection collection, string name, Type type, out object value)
		{
			value = null;
			return false;
		}

		public static void Serialize(NameValueCollection collection, string name, object value)
		{
		}

		public static bool IsComplexType(object value)
		{
			return IsComplexType(value.GetType());
		}
		public static bool IsComplexType(Type type)
		{
			return false;
		}
	}
}
