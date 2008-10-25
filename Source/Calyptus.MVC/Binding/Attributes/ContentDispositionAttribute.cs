using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Security.Principal;
using System.Web.SessionState;
using System.Web.Caching;
using System.IO;
using System.Xml;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Text;

namespace Calyptus.Mvc
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public class ContentDispositionAttribute : Attribute, IPropertyBinding, IParameterBinding
	{
		private string defaultDispositionType;
		private string param;
		private object contextkey = new object();
		private Type bindingType;
		private bool complex;
		private static Regex matchValue = new Regex(";\\s*(\\w+)\\s*\\=\\s*(\"[^\"]*\"|[^\\;]+);");

		public ContentDispositionAttribute()
		{
		}

		public ContentDispositionAttribute(string param)
		{
			this.param = param;
		}

		public ContentDispositionAttribute(string defaultDispositionType, string param)
		{
			this.defaultDispositionType = defaultDispositionType;
			this.param = param;
		}

		public void Initialize(ParameterInfo parameter)
		{
			bindingType = parameter.ParameterType.IsByRef ? parameter.ParameterType.GetElementType() : parameter.ParameterType;
			if (param == null && bindingType != typeof(string)) throw new BindingException(String.Format("Invalid type. {1} can't bind to type '{0}' without param. Try using string.", parameter.ParameterType.Name, GetType().Name));
		}

		public void Initialize(PropertyInfo property)
		{
			bindingType = property.PropertyType;
			if (param == null && bindingType != typeof(string)) throw new BindingException(String.Format("Invalid type. {1} can't bind to type '{0}' without param. Try using string", property.PropertyType.Name, GetType().Name));
		}

		public bool TryBinding(IHttpContext context, IPathStack path, out object obj, out int overloadWeight)
		{
			overloadWeight = 0;
			if (!TryBinding(context, out obj) && bindingType.IsValueType) obj = System.Activator.CreateInstance(bindingType);
			return true;
		}

		public bool TryBinding(IHttpContext context, out object obj)
		{
			string contentDisposition = context.Request.Headers["Content-Disposition"];
			if (contentDisposition == null) { obj = null; return false; }

			int i = contentDisposition.IndexOf(';');
			if (param == null && !complex)
			{
				if (i > 0)
					obj = contentDisposition.Substring(0, i);
				else
					obj = contentDisposition;
				return true;
			}
			else
			{
				NameValueCollection collection = new NameValueCollection();
				if (i > 0)
				{
					collection.Add(null, contentDisposition.Substring(0, i));

					foreach (Match m in matchValue.Matches(contentDisposition))
					{
						string name = m.Groups[1].Value;
						string value = m.Groups[2].Value;
						if (value.StartsWith("\"") && value.EndsWith("\""))
							value = value.Substring(1, value.Length - 2);
						else
							value = value.TrimEnd(' ', '\t');
						collection.Add(name, value);
					}
				}
				else
					collection.Add(null, contentDisposition);
				return SerializationHelper.TryDeserialize(collection, param, bindingType, out obj);
			}
		}

		public void StoreBinding(IHttpContext context, object value)
		{
			if (value == null) return;
			NameValueCollection collection = context.Items[contextkey] as NameValueCollection;
			if (collection == null) context.Items[contextkey] = collection = new NameValueCollection();
			SerializationHelper.Serialize(value, collection, param);

			string disp = collection[null] ?? defaultDispositionType;
			if (!string.IsNullOrEmpty(disp))
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(disp);
				foreach(KeyValuePair<string, string> kv in collection)
				{
					sb.Append(';');
					sb.Append(kv.Key);
					sb.Append("=\"");
					sb.Append(kv.Value);
					sb.Append("\"");
				}
				context.Response.AppendHeader("Content-Disposition", sb.ToString());
			}
		}

		public void SerializePath(IPathStack path, object value)
		{
			// Exclude
		}
	}
}
