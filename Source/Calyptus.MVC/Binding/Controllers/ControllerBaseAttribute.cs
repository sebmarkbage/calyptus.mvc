using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Collections.Specialized;

namespace Calyptus.MVC
{
	public abstract class ControllerBaseAttribute : Attribute, IControllerBinding
	{
		private PropBinding[] _properties;
		private struct PropBinding
		{
			public PropertyInfo Property;
			public IPropertyBinding Binding;
		}

		private IExtension[] _extensions;

		private ActionHandler[] _handlers;

		private Func<object> _controllerCreator;

		// OnBeforeActionDelegate
		// OnAfterActionDelegate
		// OnBeforeSpecificActionDelegate[]
		// OnAfterSpecificActionDelegate[]
		// OnBeforeRenderDelegate
		// OnAfterRenderDelegate
		// DisposeDelegate

		public virtual void Initialize(Type controllerType)
		{
			_controllerCreator = () => System.Activator.CreateInstance(controllerType);

			List<ActionHandler> handlers = new List<ActionHandler>();
			foreach (MethodInfo m in controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
			{
				object[] actionAttributes = m.GetCustomAttributes(typeof(IActionBinding), false);
				if (actionAttributes.Length > 0)
				{
					IActionBinding[] bs = new IActionBinding[actionAttributes.Length];
					for (int i = 0; i < actionAttributes.Length; i++)
					{
						IActionBinding a = bs[i] = (IActionBinding)actionAttributes[i];
						a.Initialize(m);

						ParameterInfo[] ps = m.GetParameters();
						ParameterInfo sl = ps.Length > 1 ? ps[ps.Length - 2] : null;
						ParameterInfo l = ps.Length > 1 ? ps[ps.Length - 1] : null;
						bool isAsync = m.ReturnType == typeof(IAsyncResult) && sl != null &&
									   !sl.ParameterType.IsByRef && sl.ParameterType == typeof(AsyncCallback) &&
									   !l.ParameterType.IsByRef && l.ParameterType == typeof(object);

						bool isParentAction = !isAsync && typeof(IController).IsAssignableFrom(m.ReturnType) || m.ReturnType.GetCustomAttributes(typeof(IControllerBinding), false).Length > 0;

						ActionHandler handler = isAsync ? new AsyncActionHandler() : (isParentAction ? new ParentActionHandler() : new ActionHandler());
						handler.Action = m;
						handler.Binding = a;

						if (isAsync)
						{
							string name = m.Name.StartsWith("Begin", StringComparison.InvariantCultureIgnoreCase) ? m.Name.Substring(5) : m.Name;
							MethodInfo em = controllerType.GetMethod("End" + name, new Type[] { typeof(IAsyncResult) });
							if (em == null) controllerType.GetMethod(name, new Type[] { typeof(IAsyncResult) });
							if (em == null) throw new BindingException(String.Format("Beginning of asynchronous method '{0}' is missing the expected '{1}' end method.", m.Name, "End" + name));
							((AsyncActionHandler)handler).EndAction = em;
						}

						handlers.Add(handler);
					}
				}
			}
			if (handlers.Count > 0)
				_handlers = handlers.ToArray();

			List<PropBinding> props = new List<PropBinding>();
			foreach (PropertyInfo p in controllerType.GetProperties())
			{
				object[] propAttributes = p.GetCustomAttributes(typeof(IPropertyBinding), false);
				for (int i = 0; i < propAttributes.Length; i++)
				{
					IPropertyBinding b = (IPropertyBinding)propAttributes[i];
					b.Initialize(p);
					props.Add(new PropBinding { Binding = b, Property = p });
				}
			}
			if (props.Count > 0) _properties = props.ToArray();

			object[] exts = controllerType.GetCustomAttributes(typeof(IExtension), true);
			if (exts.Length > 0)
			{
				_extensions = new IExtension[exts.Length];
				for (int i = 0; i < exts.Length; i++)
				{
					IExtension ext = (IExtension)exts[i];
					ext.Initialize(controllerType);
					_extensions[i] = ext;
				}
			}
		}

		public virtual bool TryBinding(IHttpContext context, IPathStack path, out IHttpHandler handler)
		{
			return TryBinding(context, path, null, out handler);
		}

		public virtual bool TryBinding(IHttpContext context, IPathStack path, object controller, out IHttpHandler handler)
		{
			if (_handlers == null)
			{
				handler = null;
				return false;
			}

			int bestIndex = -1;
			int bestOverloadWeight = -1;
			object[] bestParameters = null;
			ActionHandler bestHandler = null;

			int index = path.Index;

			object[] parameters;
			int overloadWeight;
			foreach (ActionHandler h in _handlers)
			{
				if (h.Binding.TryBinding(context, path, out parameters, out overloadWeight)
					&& (overloadWeight > bestOverloadWeight || (
						overloadWeight == bestOverloadWeight && parameters.Length < bestParameters.Length  //The fewer parameters the better if same overload weight
					)) && (path.IsAtEnd || h is ParentActionHandler))
				{
					bestIndex = path.Index;
					bestHandler = h;
					bestParameters = parameters;
					bestOverloadWeight = overloadWeight;
				}
				path.ReverseToIndex(index);
			}

			if (bestHandler == null)
			{
				handler = null;
				return false;
			}

			// shouldTrailSlash = bestIndex == index; I.e. the best mapping hasn't used a path
			if (bestIndex == index ^ path.TrailingSlash && !(bestHandler is ParentActionHandler) && context.Request.HttpMethod == "GET")
			{
				// TODO: Make more efficient by simply extracting the final path section and use relative redirect: ../pathsection or ./pathsection/
				IPathStack newPath = new PathStack(false);
				newPath.Push(path);
				newPath.TrailingSlash = !path.TrailingSlash;
				context.Response.Redirect("~/" + newPath.ToString(), false);
				context.Response.StatusCode = 301;
				context.Response.End();
			}

			path.ReverseToIndex(bestIndex);

			if (controller == null) controller = _controllerCreator();

			context.Route.AddController(controller, index); // Controller is bound, add it to the route context

			if (bestHandler is ParentActionHandler)
			{
				object returnValue = bestHandler.ExecuteAction(context, controller, bestParameters);
				if (returnValue == null)
				{
					handler = null;
					return false;
				}
				IControllerBinding[] controllerBindings = returnValue is IRenderable || returnValue is IViewTemplate ? null : AttributeRoutingEngine.GetControllerBindings(returnValue.GetType());
				if (controllerBindings == null)
				{
					bestHandler.RenderAction(context, returnValue);
					context.Response.End(); // Best way to terminate response?
					handler = null;
					return true;
				}
				
				int controllerIndex = context.Route.Index;
				foreach (IControllerBinding binding in controllerBindings)
				{
					if (binding.TryBinding(context, path, returnValue, out handler))
						return true;
					else
					{
						context.Route.ReverseToIndex(controllerIndex);
						path.ReverseToIndex(bestIndex);
					}
				}

				handler = null;
				return false;
			}
			else if (bestHandler is AsyncActionHandler)
			{
				handler = new HttpAsyncActionHandler { Context = context, Controller = controller, Arguments = bestParameters, Handler = (AsyncActionHandler)bestHandler };
				return true;
			}
			else
			{
				handler = new HttpActionHandler { Context = context, Controller = controller, Arguments = bestParameters, Handler = bestHandler };
				return true;
			}
		}

		public virtual void SerializeToPath(IRouteAction action, IPathStack path)
		{
			// TODO: child action binding

			if (_handlers == null) return;

			IPathStack bestStack = null;
			foreach (ActionHandler handler in _handlers)
			{
				if (handler.Action == action.Method)
				{
					IPathStack trialStack = new PathStack(false);
					handler.Binding.SerializePath(trialStack, action.Parameters);

					IRouteAction childAction = action.ChildAction;
					if (childAction != null)
					{
						if (!(handler is ParentActionHandler)) throw new BindingException("Method \"{0}\" is not a parent action and can't handle further calls.");
						IControllerBinding[] bindings = AttributeRoutingEngine.GetControllerBindings(childAction.ControllerType);
						IPathStack bestChildStack = null;
						foreach (IControllerBinding b in bindings)
						{
							IPathStack ts = new PathStack(false);
							b.SerializeToPath(childAction, ts);
							if (bestChildStack == null || ts.Index > bestChildStack.Index || (ts.Index == bestChildStack.Index && ts.QueryString.Count > bestChildStack.QueryString.Count))
								bestChildStack = ts;
						}
						trialStack.Push(bestChildStack);
					}
					if (trialStack.Index == 0) trialStack.TrailingSlash = true;

					if (bestStack == null || trialStack.Index > bestStack.Index || (trialStack.Index == bestStack.Index && trialStack.QueryString.Count > bestStack.QueryString.Count))
						bestStack = trialStack;
				}
			}
			if (bestStack != null)
			{
				path.Push(bestStack);
			}
			else
				throw new BindingException(String.Format("Method \"{0}\" is not bindable.", action.Method.Name));
		}
	}
}
