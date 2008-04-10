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

		public static void Redirect<TRelativeController>(Expression<Action<TRelativeController>> action) where TRelativeController : IController
		{
			throw new Redirect<TRelativeController>(action);
		}

		public static void Redirect<TRelativeController>(int index, Expression<Action<TRelativeController>> action) where TRelativeController : IController
		{
			throw new Redirect<TRelativeController>(index, action);
		}

		public static void Redirect<TRelativeController, TWithActionsFromController>(Expression<Func<TRelativeController, TWithActionsFromController>> action) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(action);
		}

		public static void Redirect<TRelativeController, TWithActionsFromController>(int index, Expression<Func<TRelativeController, TWithActionsFromController>> action) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(index, action);
		}

		public static void Redirect<TRelativeController, TWithActionsFromController>(int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(index, secondIndex, action);
		}

		public static void RedirectAbsolute<TEntryController>(Expression<Action<TEntryController>> action) where TEntryController : class, IEntryController
		{
			throw new RedirectAbsolute<TEntryController>(action);
		}



		public static void Redirect(this IController controller, string url)
		{
			throw new Redirect(url);
		}

		public static void Redirect<TRelativeController>(this IController controller, Expression<Action<TRelativeController>> action) where TRelativeController : IController
		{
			throw new Redirect<TRelativeController>(action);
		}

		public static void Redirect<TRelativeController>(this IController controller, int index, Expression<Action<TRelativeController>> action) where TRelativeController : IController
		{
			throw new Redirect<TRelativeController>(index, action);
		}

		public static void Redirect<TRelativeController, TWithActionsFromController>(this IController controller, Expression<Func<TRelativeController, TWithActionsFromController>> action) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(action);
		}

		public static void Redirect<TRelativeController, TWithActionsFromController>(this IController controller, int index, Expression<Func<TRelativeController, TWithActionsFromController>> action) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(index, action);
		}

		public static void RedirectAbsolute<TEntryController>(this IController controller, Expression<Action<TEntryController>> action) where TEntryController : class, IEntryController
		{
			throw new RedirectAbsolute<TEntryController>(action);
		}



		public static void Redirect<TRelativeController>(this TRelativeController controller, Expression<Action<TRelativeController>> action) where TRelativeController : IController
		{
			throw new Redirect<TRelativeController>(action);
		}

		public static void Redirect<TRelativeController>(this TRelativeController controller, int index, Expression<Action<TRelativeController>> action) where TRelativeController : IController
		{
			throw new Redirect<TRelativeController>(index, action);
		}

		/*public static void Redirect<TRelativeController, TWithActionsFromController>(this TRelativeController controller, Expression<Func<TRelativeController, TWithActionsFromController>> action) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(action);
		}

		public static void Redirect<TRelativeController, TWithActionsFromController>(this TRelativeController controller, int index, Expression<Func<TRelativeController, TWithActionsFromController>> action) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(index, action);
		}

		public static void Redirect<TRelativeController, TWithActionsFromController>(this TRelativeController controller, int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(index, secondIndex, action);
		}*/

		public static void RedirectAbsolute<TEntryController>(this TEntryController controller, Expression<Action<TEntryController>> action) where TEntryController : class, IEntryController
		{
			throw new RedirectAbsolute<TEntryController>(action);
		}





		public static void Redirect(string url, bool permanently)
		{
			throw new Redirect(url, permanently);
		}

		public static void Redirect<TRelativeController>(Expression<Action<TRelativeController>> action, bool permanently) where TRelativeController : IController
		{
			throw new Redirect<TRelativeController>(action, permanently);
		}

		public static void Redirect<TRelativeController>(int index, Expression<Action<TRelativeController>> action, bool permanently) where TRelativeController : IController
		{
			throw new Redirect<TRelativeController>(index, action, permanently);
		}

		/*public static void Redirect<TRelativeController, TWithActionsFromController>(Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(action, permanently);
		}

		public static void Redirect<TRelativeController, TWithActionsFromController>(int index, Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(index, action, permanently);
		}

		public static void Redirect<TRelativeController, TWithActionsFromController>(int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(index, secondIndex, action, permanently);
		}*/

		public static void RedirectAbsolute<TEntryController>(Expression<Action<TEntryController>> action, bool permanently) where TEntryController : class, IEntryController
		{
			throw new RedirectAbsolute<TEntryController>(action, permanently);
		}



		public static void Redirect(this IController controller, string url, bool permanently)
		{
			throw new Redirect(url, permanently);
		}

		public static void Redirect<TRelativeController>(this IController controller, Expression<Action<TRelativeController>> action, bool permanently) where TRelativeController : IController
		{
			throw new Redirect<TRelativeController>(action, permanently);
		}

		public static void Redirect<TRelativeController>(this IController controller, int index, Expression<Action<TRelativeController>> action, bool permanently) where TRelativeController : IController
		{
			throw new Redirect<TRelativeController>(index, action, permanently);
		}

		public static void Redirect<TRelativeController, TWithActionsFromController>(this IController controller, Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(action, permanently);
		}

		public static void Redirect<TRelativeController, TWithActionsFromController>(this IController controller, int index, Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(index, action, permanently);
		}

		public static void RedirectAbsolute<TEntryController>(this IController controller, Expression<Action<TEntryController>> action, bool permanently) where TEntryController : class, IEntryController
		{
			throw new RedirectAbsolute<TEntryController>(action, permanently);
		}



		public static void Redirect<TRelativeController>(this TRelativeController controller, Expression<Action<TRelativeController>> action, bool permanently) where TRelativeController : IController
		{
			throw new Redirect<TRelativeController>(action, permanently);
		}

		public static void Redirect<TRelativeController>(this TRelativeController controller, int index, Expression<Action<TRelativeController>> action, bool permanently) where TRelativeController : IController
		{
			throw new Redirect<TRelativeController>(index, action, permanently);
		}

		/*public static void Redirect<TRelativeController, TWithActionsFromController>(this TRelativeController controller, Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(action, permanently);
		}

		public static void Redirect<TRelativeController, TWithActionsFromController>(this TRelativeController controller, int index, Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(index, action, permanently);
		}*/

		public static void Redirect<TRelativeController, TWithActionsFromController>(this TRelativeController controller, int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action, bool permanently) where TRelativeController : IController where TWithActionsFromController : IController
		{
			throw new Redirect<TRelativeController, TWithActionsFromController>(index, secondIndex, action, permanently);
		}

		public static void RedirectAbsolute<TEntryController>(this TEntryController controller, Expression<Action<TEntryController>> action, bool permanently) where TEntryController : class, IEntryController
		{
			throw new RedirectAbsolute<TEntryController>(action, permanently);
		}

	}
}
