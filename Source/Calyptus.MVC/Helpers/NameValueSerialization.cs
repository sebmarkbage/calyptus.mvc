using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Drawing;

namespace Calyptus.Mvc
{
	public static class SerializationHelper
	{
		public static bool TryDeserialize(string singleValue, Type type, out object value)
		{
			try
			{
				if (type == typeof(byte[]))
				{
					string p = singleValue.Replace('-', '+').Replace('_', '/');
					int i = p.Length % 4;
					if (i > 0)
						p = p.PadRight(p.Length + 4 - i, '=');
					value = Convert.FromBase64String(p);
				}
				else if (type == typeof(Size))
				{
					string[] s = singleValue.Split(new char[] { 'x' }, 2);
					if (s.Length != 2) { value = null; return false; }
					value = new Size(int.Parse(s[0]), int.Parse(s[1]));
				}
				else
				{
					value = Convert.ChangeType(singleValue, type, System.Globalization.CultureInfo.InvariantCulture);
				}
				return true;
			}
			catch
			{
				value = null;
				return false;
			}
		}

		public static string Serialize(object singleValue)
		{
			if (singleValue == null) return null;
			if (singleValue is byte[])
				return Convert.ToBase64String((byte[])singleValue, Base64FormattingOptions.None).Replace('+', '-').Replace('/', '_').Replace("=", "");
			else if (singleValue is Size)
			{
				Size s = (Size)singleValue;
				return s.Width.ToString() + "x" + s.Height.ToString();
			}
			else
				return Convert.ToString(singleValue, System.Globalization.CultureInfo.InvariantCulture);
		}

		public static bool TryDeserialize(NameValueCollection collection, string key, Type type, out object value)
		{
			return TryDeserialize(collection[key], type, out value);
		}

		public static void Serialize(object value, NameValueCollection targetCollection, string key)
		{
			if (value != null) targetCollection.Add(key, Serialize(value));
		}

		public static bool IsComplexType(object value)
		{
			if (value == null) return false;
			return IsComplexType(value.GetType());
		}

		public static bool IsComplexType(Type type)
		{
			return false;
		}

		public static bool IsArray(object value)
		{
			if (value == null) return false;
			return IsArray(value.GetType());
		}

		public static bool IsArray(Type type)
		{
			return false;
		}
	}
}
