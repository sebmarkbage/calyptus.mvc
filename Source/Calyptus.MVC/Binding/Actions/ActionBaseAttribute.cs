using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Calyptus.MVC
{
    public abstract class ActionBaseAttribute : Attribute, IActionBinding
    {
		protected IList<IMappingBinding> Mappings;

		protected struct ParamBindings
		{
			public IParameterBinding[] Bindings;
			public IBindingConstraint[] Constraints;
			public bool IsIn;
			public bool IsOut;
		}

		//protected IParameterBinding[][] Bindings;
		//protected bool[] IsOutBindings;
		//protected IBindingConstraint[][] Constraints;
		protected ParamBindings[] Bindings;
		protected IExtension[] Extensions;

		public virtual string ResponseType { get; set; }

		protected virtual Type DefaultParameterBinderType { get { return typeof(ParamAttribute); } }

		public ActionBaseAttribute() : this(null) { }

		private Type _returnType;

		public ActionBaseAttribute(IEnumerable<IMappingBinding> mappings)
		{
			this.Mappings = mappings == null ? new List<IMappingBinding>() : new List<IMappingBinding>(mappings);
		}

		void IActionBinding.Initialize(MethodInfo method)
		{
			if (Bindings != null)
				return;

			_returnType = method.ReturnType;

			ParameterInfo[] parameters = method.GetParameters();

			if (parameters.Length == 0)
			{
				Initialize(method);
				return;
			}

			Bindings = new ParamBindings[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo p = parameters[i];

				IParameterBinding[] bindings;
				IBindingConstraint[] constraints;

				object[] atts = p.GetCustomAttributes(typeof(IParameterBinding), true);
				if (atts.Length > 0)
				{
					bindings = new IParameterBinding[atts.Length];
					for (int a = 0; a < atts.Length; a++)
					{
						IParameterBinding b = (IParameterBinding)atts[a];
						if (b is DefaultAttribute)
						{
							b = ContextAttribute.IsContextType(p.ParameterType) ?
								new ContextAttribute() :
								(IParameterBinding)Activator.CreateInstance(DefaultParameterBinderType);
						}
						b.Initialize(p);
						bindings[a] = b;
					}
				}
				else
				{
					IParameterBinding b = 
						ContextAttribute.IsContextType(p.ParameterType) ?
						new ContextAttribute() :
						(IParameterBinding)Activator.CreateInstance(DefaultParameterBinderType);
					b.Initialize(p);
					bindings = new IParameterBinding[] { b };
				}

				atts = p.GetCustomAttributes(typeof(IBindingConstraint), true);
				if (atts.Length > 0)
				{
					constraints = new IBindingConstraint[atts.Length];
					for (int a = 0; a < atts.Length; a++)
					{
						IBindingConstraint c = (IBindingConstraint)atts[a];
						c.Initialize(p);
						constraints[a] = c;
					}
				}
				else
				{
					constraints = null;
				}

				Bindings[i] = new ParamBindings
				{
					Bindings = bindings,
					Constraints = constraints,
					IsIn = !p.IsOut,
					IsOut = p.ParameterType.IsByRef
				};
			}

			object[] exts = method.GetCustomAttributes(typeof(IExtension), true);
			if (exts.Length > 0)
			{
				Extensions = new IExtension[exts.Length];
				for (int i = 0; i < exts.Length; i++)
				{
					IExtension ext = (IExtension)exts[i];
					ext.Initialize(method);
					Extensions[i] = ext;
				}
			}
			Initialize(method);
		}

		protected virtual void Initialize(MethodInfo method)
		{
		}

		bool IActionBinding.TryBinding(IHttpContext context, IPathStack path, out object[] parameters, out int overloadWeight)
		{
			foreach (IMappingBinding mapping in this.Mappings)
				if (!mapping.TryMapping(context, path))
				{
					parameters = null;
					overloadWeight = 0;
					return false;
				}

			// Allow IExtension to affect mapping?
			//foreach (IExtension extension in this.Extensions)
			//    if (!extension.TryMapping(context, path))
			//        return false;

			overloadWeight = Mappings.Count * 10000;

			if (Bindings == null)
			{
				parameters = null;
				return true;
			}

			int l = Bindings.Length;
			parameters = new object[Bindings.Length];
			for (int i = 0; i < l; i++)
			{
				ParamBindings pb = Bindings[i];
				if (pb.IsIn)
				{
					bool bound = false;
					int index = path.Index;
					foreach (IParameterBinding bindable in pb.Bindings)
					{
						object obj;
						int weight;
						if (bindable.TryBinding(context, path, out obj, out weight))
						{
							bool constrained = false;
							if (pb.Constraints != null)
								foreach (IBindingConstraint constraint in pb.Constraints)
									if (!constraint.TryConstraint(context, obj))
									{
										constrained = true;
										break;
									}
							if (!constrained)
							{
								overloadWeight += weight;
								parameters[i] = obj;
								bound = true;
								break;
							}
						}
						else
						{
							path.ReverseToIndex(index);
						}
					}
					if (!bound)
					{
						overloadWeight = 0;
						parameters = null;
						return false;
					}
				}
			}
			return true;
		}

		void IActionBinding.SerializePath(IPathStack path, object[] parameters)
		{
			if (Mappings != null)
				foreach (IMappingBinding mapping in Mappings)
					mapping.SerializeToPath(path);

			if (Bindings == null) return;

			int l = Bindings.Length;

			if (parameters.Length != l)
				throw new BindingException("Wrong number of parameters.");

			for (int i = 0; i < l; i++)
			{
				ParamBindings pb = Bindings[i];
				foreach(IParameterBinding b in pb.Bindings)
				{
					int index = path.Index;
					int count = path.QueryString.Count;
					b.SerializePath(path, parameters[i]);
					if (path.Index > index || path.QueryString.Count > count)
						break;
				}
			}
		}

		void IActionBinding.OnBeforeAction(IHttpContext context, BeforeActionEventArgs args)
		{
			OnBeforeAction(context, args);
		}

		void IActionBinding.OnAfterAction(IHttpContext context, AfterActionEventArgs args)
		{
			OnAfterAction(context, args);
		}

		void IActionBinding.OnError(IHttpContext context, ErrorEventArgs args)
		{
			OnError(context, args);
		}

		void IActionBinding.OnRender(IHttpContext context, object value)
		{
			OnRender(context, value);
		}

		protected virtual void OnBeforeAction(IHttpContext context, BeforeActionEventArgs args)
		{
			// Raise OnBeforeActionDelegate
		}

		protected virtual void OnAfterAction(IHttpContext context, AfterActionEventArgs args)
		{
			// Store out variables

			if (Bindings == null) return;

			int l = Bindings.Length;
			for (int i = 0; i < l; i++)
			{
				ParamBindings pb = Bindings[i];
				if (pb.IsOut)
					foreach (IParameterBinding bindable in pb.Bindings)
						bindable.StoreBinding(context, args.Parameters[i]);
			}

			// Raise OnAfterActionDelegate
		}

		protected virtual void OnRender(IHttpContext context, object value)
		{
			if (ResponseType != null) context.Response.ContentType = ResponseType;

			IViewTemplate template = value as IViewTemplate;
			if (template != null)
			{
				IView view = context.ViewFactory != null ? context.ViewFactory.FindView(context, template) : null;
				if (view != null)
					value = view;
				else if (!(template is IRenderable))
					value = MockViewFactory.CreateView(context, template);
			}

			IRenderable renderable = value as IRenderable;
			if (renderable != null)
			{
				renderable.Render(context);
				return;
			}

			Stream stream = value as Stream;
			if (stream != null)
			{
				WriteStream(stream, context.Response);
				return;
			}

			string requestType = context.Request.ContentType;
			if (IsJson(ResponseType) || (ResponseType == null && (IsJson(requestType) || "JSON".Equals(context.Request.Headers["X-Request"], StringComparison.InvariantCultureIgnoreCase) || AcceptJsonBeforeXml(context.Request.AcceptTypes))))
			{
				if (ResponseType == null) context.Response.ContentType = "application/json";
				if (value == null && _returnType == null) context.Response.Write("null");
				else
				{
					var serializer = new DataContractJsonSerializer(value == null ? _returnType : value.GetType());
					context.Response.Charset = context.Response.ContentEncoding.WebName;
					var writer = JsonReaderWriterFactory.CreateJsonWriter(context.Response.OutputStream, context.Response.ContentEncoding);
					serializer.WriteObject(writer, value);
					writer.Close();
				}
			}
			else if (ResponseType == null || IsXml(ResponseType))
			{
				if (ResponseType == null) context.Response.ContentType = "application/xml";
				if (value != null || _returnType != null)
				{
					var serializer = new DataContractSerializer(value == null ? _returnType : value.GetType());
					var writer = System.Xml.XmlDictionaryWriter.Create(context.Response.Output);
					serializer.WriteObject(writer, value);
					writer.Close();
				}
			}
		}

		private bool AcceptJsonBeforeXml(string[] acceptTypes)
		{
			foreach (string type in acceptTypes)
				if (IsJson(type))
					return true;
				else if (IsXml(type))
					return false;
			return false;
		}

		private bool IsJson(string mime)
		{
			return mime != null && (mime.EndsWith("/json", StringComparison.InvariantCultureIgnoreCase) || mime.EndsWith("+json", StringComparison.InvariantCultureIgnoreCase));
		}

		private bool IsXml(string mime)
		{
			return mime != null && (mime.EndsWith("/xml", StringComparison.InvariantCultureIgnoreCase) || mime.EndsWith("+xml", StringComparison.InvariantCultureIgnoreCase));
		}

		private void WriteStream(Stream stream, IHttpResponse response)
		{
			response.Buffer = false;
			int BufferSize = 65536;
			byte[] bytes = new byte[BufferSize];
			int numBytes;
			while ((numBytes = stream.Read(bytes, 0, BufferSize)) > 0)
				response.OutputStream.Write(bytes, 0, numBytes);
			stream.Close();
		}

		protected virtual void OnError(IHttpContext context, ErrorEventArgs args)
		{

		}
	}
}
