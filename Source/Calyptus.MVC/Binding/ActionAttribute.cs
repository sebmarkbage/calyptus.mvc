using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Calyptus.MVC.Extensions;
using Calyptus.MVC.RoutingEngines;

namespace Calyptus.MVC.Binding
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true, Inherited=false)]
    public class ActionAttribute : Attribute
    {
		private IBindable[][] _bindings;

		private IExtension[] _extensions;

		internal IKeyword Keyword { get; set; }

		public ActionAttribute()
		{
		}
		protected ActionAttribute(IKeyword keyword)
		{
			this.Keyword = keyword;
		}
		public ActionAttribute(string keyword)
        {
            this.Keyword = new PlainKeyword(keyword);
        }
		public ActionAttribute(string keywordResourceBaseName, string keywordResourceName)
        {
            this.Keyword = new ResourceKeyword(System.Reflection.Assembly.GetExecutingAssembly(), keywordResourceBaseName, keywordResourceName);
        }
		public ActionAttribute(string keywordResourceAssembly, string keywordResourceBaseName, string keywordResourceName)
        {
            this.Keyword = new ResourceKeyword(System.Reflection.Assembly.Load(keywordResourceAssembly), keywordResourceBaseName, keywordResourceName);
        }

		internal virtual void Initialize(MethodInfo method)
		{
			if (_bindings != null)
				return;

			if (Keyword == null) Keyword = new PlainKeyword(method.Name);

			ParameterInfo[] parameters = method.GetParameters();

			if (parameters.Length == 0)
				return;

			_bindings = new IBindable[parameters.Length][];

			for (int i = 0; i < parameters.Length; i++)
			{
				IBindable[] bindings;
				ParameterInfo p = parameters[i];
				object[] atts = p.GetCustomAttributes(typeof(IBindable), true);
				if (atts.Length > 0)
				{
					bindings = new IBindable[atts.Length];
					for (int a = 0; a < atts.Length; a++)
					{
						IBindable b = (IBindable)atts[a];
						b.Initialize(p.ParameterType, p.Name);
						bindings[a] = b;
					}
				}
				else
				{
					IBindable b = new ParamAttribute();
					b.Initialize(p.ParameterType, p.Name);
					bindings = new IBindable[] { b };
				}
				_bindings[i] = bindings;
			}

			object[] exts = method.GetCustomAttributes(typeof(IExtension), true);
			if (exts.Length > 0)
			{
				_extensions = new IExtension[exts.Length];
				for (int i = 0; i < exts.Length; i++)
				{
					IExtension ext = (IExtension)exts[i];
					ext.Initialize(method);
					_extensions[i] = ext;
				}
			}
		}

		internal virtual bool TryBinding(IHttpContext context, IPathStack path, out RoutingCommand command)
		{
			if (!Keyword.Try(path))
			{
				command = null;
				return false;
			}

			if (_bindings == null)
			{
				command = new RoutingCommand { ActionExtensions = _extensions };
				return true;
			}

			int l = _bindings.Length;
			object[] arguments = new object[_bindings.Length];
			for (int i = 0; i < l; i++)
			{
				bool bound = false;
				int index = path.Index;
				foreach (IBindable bindable in _bindings[i])
				{
					object obj;
					if (bindable.TryBinding(context, path, out obj))
					{
						arguments[i] = obj;
						bound = true;
						break;
					}
					else
						path.ReverseToIndex(index);
				}
				if (!bound)
				{
					command = null;
					return false;
				}
			}
			command = new RoutingCommand { ActionExtensions = _extensions, Arguments = arguments };
			return true;
		}

		internal virtual void SerializeBinding(IPathStack stack, object[] arguments)
		{
			Keyword.Serialize(stack);

			int l = _bindings.Length;

			if (arguments.Length != l)
				throw new Exception("Wrong number of arguments.");

			for (int i = 0; i < l; i++)
			{
				IBindable b = _bindings[i][0];
				b.SerializePath(stack, arguments[i]);
			}
		}
	}
}
