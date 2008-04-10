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
			try
			{
				value = Convert.ChangeType(collection[name], type);
				return true;
			}
			catch
			{
				value = null;
				return false;
			}
		}

		public static void Serialize(NameValueCollection collection, string name, object value)
		{
			if (value != null) collection.Add(name, value.ToString());
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
