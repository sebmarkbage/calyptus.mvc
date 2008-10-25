using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.Mvc
{
	public abstract class ViewFactoryBase : IViewFactory
	{
		public abstract IView FindView(IHttpContext context, IViewTemplate view);

		protected IView GetInstance(Type templateType)
		{
			if (_typeCache == null) _typeCache = new Dictionary<Type,Type>();

			Type viewType;
			if (!_typeCache.TryGetValue(templateType, out viewType))
			{
				lock(_typeCache)
				{
					viewType = GetViewType(templateType);
					_typeCache.Add(templateType, viewType);
				}
			}
			if (viewType == null)
				return null;
			else
				return (IView)System.Activator.CreateInstance(viewType);
		}

		private Dictionary<Type, Type> _typeCache;

		protected abstract Type GetViewType(Type templateType);
	}
}
