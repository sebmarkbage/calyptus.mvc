/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Calyptus.Mvc
{
	internal class PropertyHandler
	{
		public IPropertyBinding Binding;
		public System.Reflection.PropertyInfo Property;
	}

	internal class ActionHandler
	{
		public IActionBinding Binding;
		public IExtension[] ControllerExtensions;
		public IExtension[] ActionExtensions;
		public PropertyHandler[] Properties;

		/*public Action<IHttpContext, BeforeActionEventArgs> ControllerBeforeAction;
		public Action<IHttpContext, BeforeActionEventArgs> ControllerAfterAction;
		public Action<IHttpContext, BeforeActionEventArgs> ActionBeforeAction;
		public Action<IHttpContext, BeforeActionEventArgs> ActionAfterAction;
		public Action<IHttpContext, BeforeActionEventArgs> ControllerBeforeRender;
		public Action<IHttpContext, BeforeActionEventArgs> ControllerAfterRender;
		public Action<IHttpContext, BeforeActionEventArgs> ControllerBeforeRender;
		public Action<IHttpContext, BeforeActionEventArgs> ControllerAfterRender;* /

		//public Func<object, object[], object> Action;
		public System.Reflection.MethodInfo Action;

		protected void OnBeforeAction(IHttpContext context, BeforeActionEventArgs args)
		{
			if (ControllerExtensions != null)
				foreach(IExtension ext in ControllerExtensions)
					ext.OnBeforeAction(context, args);

			if (ActionExtensions != null)
				foreach(IExtension ext in ActionExtensions)
					ext.OnBeforeAction(context, args);

			if (Properties != null)
				foreach (PropertyHandler prop in Properties)
				{
					object v;
					if (prop.Binding.TryBinding(context, out v))
						prop.Property.SetValue(args.Controller, v, null);
				}

			Binding.OnBeforeAction(context, args);
		}
		protected void OnAfterAction(IHttpContext context, AfterActionEventArgs args)
		{
			Binding.OnAfterAction(context, args);

			if (Properties != null)
				foreach (PropertyHandler prop in Properties)
					prop.Binding.StoreBinding(context, prop.Property.GetValue(args.Controller, null));

			if (ActionExtensions != null)
				foreach(IExtension ext in ActionExtensions)
					ext.OnAfterAction(context, args);

			if (ControllerExtensions != null)
				foreach(IExtension ext in ControllerExtensions)
					ext.OnAfterAction(context, args);
		}
		protected void OnBeforeRender(IHttpContext context, BeforeRenderEventArgs args)
		{
			if (ControllerExtensions != null)
				foreach (IExtension ext in ControllerExtensions)
					ext.OnBeforeRender(context, args);
			if (ActionExtensions != null)
				foreach (IExtension ext in ActionExtensions)
					ext.OnBeforeRender(context, args);
		}
		protected void OnAfterRender(IHttpContext context, AfterRenderEventArgs args)
		{
			if (ActionExtensions != null)
				foreach (IExtension ext in ActionExtensions)
					ext.OnAfterRender(context, args);
			if (ControllerExtensions != null)
				foreach (IExtension ext in ControllerExtensions)
					ext.OnAfterRender(context, args);
		}
		protected void OnError(IHttpContext context, ErrorEventArgs args)
		{
			if (ActionExtensions != null)
				foreach (IExtension ext in ActionExtensions)
					ext.OnError(context, args);
			if (ControllerExtensions != null)
				foreach (IExtension ext in ControllerExtensions)
					ext.OnError(context, args);
		}

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
					else if (!(template is IResponse))
						value = MockViewFactory.CreateView(context, template);
				}

				IResponse renderable = value as IResponse;
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
					else if (!(template is IResponse))
						value = MockViewFactory.CreateView(context, template);
				}

				IResponse renderable = value as IResponse;
				if (renderable != null)
				{
					renderable.Render(context);
					return;
				}
			}
		}

		protected object HandleException(IHttpContext context, Exception e)
		{
			if (e is IViewTemplate || e is IResponse)
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
					if (ee is IViewTemplate || ee is IResponse)
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

		public virtual IHttpHandler GetHttpHandler(IHttpContext context, object controller, object[] parameters)
		{
			return new HttpActionHandler { Controller = controller, Arguments = parameters, Context = context, Handler = this };
		}
	}

	internal class ParentActionHandler : ActionHandler
	{
		public override IHttpHandler GetHttpHandler(IHttpContext context, object controller, object[] parameters)
		{
			return new HttpParentActionHandler { Controller = controller, Arguments = parameters, Context = context, Handler = this };
		}
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

		public override IHttpHandler GetHttpHandler(IHttpContext context, object controller, object[] parameters)
		{
			return new HttpAsyncActionHandler { Controller = controller, Arguments = parameters, Context = context, Handler = this };
		}

	}
}
*/