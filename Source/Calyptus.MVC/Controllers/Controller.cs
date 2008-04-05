using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web;
using Calyptus.MVC;

namespace Calyptus.MVC
{
	public static class Controller
    {
		public static void Redirect(string url)
		{
			throw new Redirect(url);
		}

		public static void Redirect<T>(Expression<Action<T>> action)
		{
			throw new Redirect<T>(action);
		}

		public static void Redirect<T>(int index, Expression<Action<T>> action)
		{
			throw new Redirect<T>(index, action);
		}

		public static void RedirectReplace<T>(Expression<Action<T>> action)
		{
			throw new RedirectReplace<T>(action);
		}

		public static void RedirectReplace<T>(int index, Expression<Action<T>> action)
		{
			throw new RedirectReplace<T>(index, action);
		}

		public static void RedirectAbsolute<T>(Expression<Action<T>> action)
		{
			throw new RedirectReplace<T>(action);
		}

		public static void Redirect(this IController controller, string url)
		{
			throw new Redirect(url);
		}

		public static void Redirect<T>(this IController controller, Expression<Action<T>> action)
		{
			throw new Redirect<T>(action);
		}

		public static void Redirect<T>(this IController controller, int index, Expression<Action<T>> action)
		{
			throw new Redirect<T>(index, action);
		}

		public static void RedirectReplace<T>(this IController controller, Expression<Action<T>> action)
		{
			throw new RedirectReplace<T>(action);
		}

		public static void RedirectReplace<T>(this IController controller, int index, Expression<Action<T>> action)
		{
			throw new RedirectReplace<T>(index, action);
		}

		public static void RedirectAbsolute<T>(this IController controller, Expression<Action<T>> action)
		{
			throw new RedirectReplace<T>(action);
		}

		public static void Redirect<T>(this T controller, Expression<Action<T>> action) where T : IController
		{
			throw new Redirect<T>(action);
		}

		public static void Redirect<T>(this T controller, int index, Expression<Action<T>> action) where T : IController
		{
			throw new Redirect<T>(index, action);
		}

		public static void RedirectReplace<T>(this T controller, Expression<Action<T>> action) where T : IController
		{
			throw new RedirectReplace<T>(action);
		}

		public static void RedirectReplace<T>(this T controller, int index, Expression<Action<T>> action) where T : IController
		{
			throw new RedirectReplace<T>(index, action);
		}

		public static void RedirectAbsolute<T>(this T controller, Expression<Action<T>> action) where T : IController
		{
			throw new RedirectAbsolute<T>(action);
		}

		public static void Redirect(string url, bool permanently)
		{
			throw new Redirect(url, permanently);
		}

		public static void Redirect<T>(Expression<Action<T>> action, bool permanently)
		{
			throw new Redirect<T>(action, permanently);
		}

		public static void Redirect<T>(int index, Expression<Action<T>> action, bool permanently)
		{
			throw new Redirect<T>(index, action, permanently);
		}

		public static void RedirectReplace<T>(Expression<Action<T>> action, bool permanently)
		{
			throw new RedirectReplace<T>(action, permanently);
		}

		public static void RedirectReplace<T>(int index, Expression<Action<T>> action, bool permanently)
		{
			throw new RedirectReplace<T>(index, action, permanently);
		}

		public static void RedirectAbsolute<T>(Expression<Action<T>> action, bool permanently)
		{
			throw new RedirectReplace<T>(action, permanently);
		}

		public static void Redirect(this IController controller, string url, bool permanently)
		{
			throw new Redirect(url, permanently);
		}

		public static void Redirect<T>(this IController controller, Expression<Action<T>> action, bool permanently)
		{
			throw new Redirect<T>(action, permanently);
		}

		public static void Redirect<T>(this IController controller, int index, Expression<Action<T>> action, bool permanently)
		{
			throw new Redirect<T>(index, action, permanently);
		}

		public static void RedirectReplace<T>(this IController controller, Expression<Action<T>> action, bool permanently)
		{
			throw new RedirectReplace<T>(action, permanently);
		}

		public static void RedirectReplace<T>(this IController controller, int index, Expression<Action<T>> action, bool permanently)
		{
			throw new RedirectReplace<T>(index, action, permanently);
		}

		public static void RedirectAbsolute<T>(this IController controller, Expression<Action<T>> action, bool permanently)
		{
			throw new RedirectReplace<T>(action, permanently);
		}

		public static void Redirect<T>(this T controller, Expression<Action<T>> action, bool permanently) where T : IController
		{
			throw new Redirect<T>(action, permanently);
		}

		public static void Redirect<T>(this T controller, int index, Expression<Action<T>> action, bool permanently) where T : IController
		{
			throw new Redirect<T>(index, action, permanently);
		}

		public static void RedirectReplace<T>(this T controller, Expression<Action<T>> action, bool permanently) where T : IController
		{
			throw new RedirectReplace<T>(action, permanently);
		}

		public static void RedirectReplace<T>(this T controller, int index, Expression<Action<T>> action, bool permanently) where T : IController
		{
			throw new RedirectReplace<T>(index, action, permanently);
		}

		public static void RedirectAbsolute<T>(this T controller, Expression<Action<T>> action, bool permanently) where T : IController
		{
			throw new RedirectAbsolute<T>(action, permanently);
		}

	}
}
