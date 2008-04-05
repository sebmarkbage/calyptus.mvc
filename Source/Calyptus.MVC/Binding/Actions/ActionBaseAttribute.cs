﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.ComponentModel;

namespace Calyptus.MVC
{
    public abstract class ActionBaseAttribute : Attribute, IActionBinding
    {
		protected IList<IMappingBinding> Mappings;

		protected IParameterBinding[][] Bindings;
		protected IExtension[] Extensions;

		// AsyncEndDelegate
		// OnBeforeActionDelegate
		// OnAfterActionDelegate

		protected Type DefaultParameterBinderType = typeof(ParamAttribute);

		public virtual string View { get; set; }
		public virtual string Master { get; set; }
		public virtual IViewFactory ViewEngine { get; set; }

		public ActionBaseAttribute() : this(null) { }

		public ActionBaseAttribute(IEnumerable<IMappingBinding> mappings)
		{
			this.Mappings = new List<IMappingBinding>(mappings);
		}

		public virtual void Initialize(MethodInfo method)
		{
			if (Bindings != null)
				return;

			ParameterInfo[] parameters = method.GetParameters();

			if (parameters.Length == 0)
				return;

			Bindings = new IParameterBinding[parameters.Length][];

			for (int i = 0; i < parameters.Length; i++)
			{
				IParameterBinding[] bindings;
				ParameterInfo p = parameters[i];
				object[] atts = p.GetCustomAttributes(typeof(IParameterBinding), true);
				if (atts.Length > 0)
				{
					bindings = new IParameterBinding[atts.Length];
					for (int a = 0; a < atts.Length; a++)
					{
						IParameterBinding b = (IParameterBinding)atts[a];
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
				Bindings[i] = bindings;
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
		}

		public virtual bool TryBinding(IHttpContext context, IPathStack path, out object[] arguments, out int overloadWeight)
		{
			foreach (IMappingBinding mapping in this.Mappings)
				if (!mapping.TryMapping(context, path))
				{
					arguments = null;
					overloadWeight = 0;
					return false;
				}

			// Allow IExtension to affect mapping?
			//foreach (IExtension extension in this.Extensions)
			//    if (!extension.TryMapping(context, path))
			//        return false;

			if (!path.IsAtEnd)
			{
				arguments = null;
				overloadWeight = 0;
				return false;
			}

			if (Bindings == null)
			{
				arguments = null;
				overloadWeight = 0;
				return true;
			}

			int l = Bindings.Length;
			arguments = new object[Bindings.Length];
			overloadWeight = Mappings.Count;
			for (int i = 0; i < l; i++)
			{
				bool bound = false;
				int index = path.Index;
				foreach (IParameterBinding bindable in Bindings[i])
				{
					object obj;
					int weight;
					if (bindable.TryBinding(context, path, out obj, out weight))
					{
						overloadWeight += weight;
						arguments[i] = obj;
						bound = true;
						break;
					}
					else
					{
						path.ReverseToIndex(index);
					}
				}
				if (!bound)
				{
					overloadWeight = 0;
					arguments = null;
					return false;
				}
			}
			return true;
		}

		public virtual void SerializePath(IPathStack path, object[] arguments)
		{
			int l = Bindings.Length;

			if (arguments.Length != l)
				throw new BindingException("Wrong number of arguments.");

			for (int i = 0; i < l; i++)
			{
				IParameterBinding[] b = Bindings[i];
				for (int ii = 0; ii < b.Length; ii++)
				{
					IParameterBinding bb = b[ii];
					int index = path.Index;
					bb.SerializePath(path, arguments[i]);
					if (path.Index > index)
						break;
				}
			}
		}

		public void OnBeforeAction(IHttpContext context, object[] parameters)
		{
			// Raise OnBeforeActionDelegate
		}

		public void OnAfterAction(IHttpContext context, object returnValue)
		{
			// Raise OnAfterActionDelegate
		}

		public void OnRender(IHttpContext context, object value)
		{
			IViewTemplate template = value as IViewTemplate;
			if (template != null)
			{
				// Find View
			}
			IRenderable renderable = value as IRenderable;
			if (renderable != null)
			{
				renderable.Render(context);
				return;
			}
			// Use serializer
		}

		public bool OnError(IHttpContext context, Exception error)
		{
			return true;
		}
	}
}
