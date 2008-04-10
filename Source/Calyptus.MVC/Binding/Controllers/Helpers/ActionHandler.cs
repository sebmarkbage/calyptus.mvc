using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC
{
	internal class ActionHandler
	{
		public IActionBinding Binding;
		public event Action<IHttpContext, BeforeActionEventArgs> BeforeAction;
		public event Action<IHttpContext, AfterActionEventArgs> AfterAction;
		public event Action<IHttpContext, BeforeRenderEventArgs> BeforeRender;
		public event Action<IHttpContext, AfterRenderEventArgs> AfterRender;
		public event Action<IHttpContext, ErrorEventArgs> Error;

		protected void OnBeforeAction(IHttpContext context, BeforeActionEventArgs args)
		{
			if (BeforeAction != null)
				BeforeAction(context, args);

			Binding.OnBeforeAction(context, args);
		}
		protected void OnAfterAction(IHttpContext context, AfterActionEventArgs args)
		{
			Binding.OnAfterAction(context, args);

			if (AfterAction != null)
				AfterAction(context, args);
		}
		protected void OnBeforeRender(IHttpContext context, BeforeRenderEventArgs args)
		{
			if (BeforeRender != null)
				BeforeRender(context, args);
		}
		protected void OnAfterRender(IHttpContext context, AfterRenderEventArgs args)
		{
			if (AfterRender != null)
				AfterRender(context, args);
		}
		protected void OnError(IHttpContext context, ErrorEventArgs args)
		{
			if (Error != null)
				Error(context, args);
		}

		//public Func<object, object[], object> Action;
		public System.Reflection.MethodInfo Action;
		
		public virtual object ExecuteAction(IHttpContext context, object controller, object[] parameters)
		{
			object returnValue;
			try
			{
				OnBeforeAction(context, new BeforeActionEventArgs(controller, parameters));

				returnValue = Action.Invoke(controller, parameters); //Action(controller, parameters);
			}
			catch (Exception e)
			{
				if (e is System.Reflection.TargetInvocationException) e = e.InnerException;
				returnValue = HandleException(context, e);
				if (returnValue == null) throw;
			}

			try
			{
				OnAfterAction(context, new AfterActionEventArgs(controller, parameters, returnValue));
			}
			catch (Exception e)
			{
				returnValue = HandleException(context, e);
				if (returnValue == null) throw;
			}

			return returnValue;
		}

		public virtual void RenderAction(IHttpContext context, object valueToRender)
		{
			try
			{
				OnBeforeRender(context, new BeforeRenderEventArgs(valueToRender));

				Binding.OnRender(context, valueToRender);
			}
			catch (Exception e)
			{
				object value = HandleException(context, e);
				if (value == null) throw;

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
			}

			try
			{
				OnAfterRender(context, new AfterRenderEventArgs(valueToRender));
			}
			catch (Exception e)
			{
				object value = HandleException(context, e);
				if (value == null) throw;

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
			}
		}

		protected object HandleException(IHttpContext context, Exception e)
		{
			if (e is IViewTemplate || e is IRenderable)
				return e;
			else
			{
				ErrorEventArgs exception = new ErrorEventArgs(e);
				try
				{
					OnError(context, exception);
				}
				catch (Exception ee)
				{
					if (ee is IViewTemplate || ee is IRenderable)
						return e;
					else
						throw;
				}
				if (exception.Handled)
				{
					context.Response.End();
					return null;
				}
				else
					return null;
			}
		}
	}

	internal class ParentActionHandler : ActionHandler
	{

	}

	internal class AsyncActionHandler : ActionHandler
	{
		//public Func<object, IAsyncResult, object> EndAction;
		public System.Reflection.MethodInfo EndAction;

		public override object ExecuteAction(IHttpContext context, object controller, object[] parameters)
		{
			object returnValue;
			try
			{
				OnBeforeAction(context, new BeforeActionEventArgs(controller, parameters));

				returnValue = Action.Invoke(controller, parameters); // Action(controller, parameters);
			}
			catch (Exception e)
			{
				if (e is System.Reflection.TargetInvocationException) e = e.InnerException;
				returnValue = HandleException(context, e);
				if (returnValue == null) throw;
			}

			return returnValue;
		}

		public virtual object ExecuteEndAction(IHttpContext context, object controller, object[] parameters, IAsyncResult result)
		{
			object returnValue;
			try
			{
				returnValue = EndAction.Invoke(controller, new object[] { result }); //EndAction(controller, result);

				OnAfterAction(context, new AfterActionEventArgs(controller, parameters, returnValue));
			}
			catch (Exception e)
			{
				if (e is System.Reflection.TargetInvocationException) e = e.InnerException;
				returnValue = HandleException(context, e);
				if (returnValue == null) throw;
			}
			return returnValue;
		}
	}
}
