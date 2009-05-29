using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Web;
using Calyptus.Mvc.Mapping;
using System.Linq;

namespace Calyptus.Mvc
{
	public sealed class MappingContext : IMappingContext
	{
		public MappingContext()
		{
			bindingFactories = new HashSet<IMappingConvention>();
			responseFactories = new List<IResponseEngine>();
			controllerBindings = new MappingSet();
			entryBindings = new EntrySet();
		}

		private HashSet<IMappingConvention> bindingFactories;
		public ICollection<IMappingConvention> BindingFactories
		{
			get
			{
				return bindingFactories;
			}
		}

		private List<IResponseEngine> responseFactories;
		public IList<IResponseEngine> ResponseFactories
		{
			get
			{
				return responseFactories;
			}
		}

		private MappingSet controllerBindings;
		public ICollection<IInstanceMapping> Bindings
		{
			get { return controllerBindings; }
		}

		private EntrySet entryBindings;
		public ICollection<IEntryMapping> EntryBindings
		{
			get { return entryBindings; }
		}

		IEnumerable<IMappingConvention> IMappingContext.BindingFactories
		{
			get { return this.BindingFactories; }
		}

		IEnumerable<IResponseEngine> IMappingContext.ResponseFactories
		{
			get { return this.ResponseFactories; }
		}

		IEnumerable<IInstanceMapping> IMappingContext.Bindings
		{
			get { return this.Bindings; }
		}

		IEnumerable<IEntryMapping> IMappingContext.EntryBindings
		{
			get { return this.EntryBindings; }
		}

		/*private IControllerBinding[] GetBindings(Type controllerType)
		{
			IControllerBinding[] b;
			if (!controllerBindings.TryGetBindings(controllerType, out b))
				lock (controllerBindings)
				{
					b = BindingFactory != null ? BindingFactory.GetBindings(controllerType) : null;
					if (b == null && !(BindingFactory is AttributeBindingFactory))
					{
						b = new AttributeBindingFactory().GetBindings(controllerType);
					}
					controllerBindings.Add(controllerType, b);
				}
			return b;
		}*/

		public IResultBinding ParseRoute(IHttpRequest request)
		{
			foreach (IEntryMapping c in entryBindings)
			{
				IResultBinding action;
				int index = path.Index;
				context.Route.ReverseToIndex(-1);
				if (c.TryBinding(context, path, out action))
				{
					/*while (action is IParentActionHandler)
						if (!(action as IParentActionHandler).TryBinding(path, out action))
							return null;*/
					return action;
				}
				else
					path.ReverseToIndex(index);
			}
			return null;
		}

		/*public IHttpHandler ParseRoute(IHttpContext context, IPathStack path, object controller)
		{
			IControllerBinding[] bindings = GetControllerBindings(controller.GetType());
			if (bindings != null)
			{
				int index = path.Index;
				int controllerIndex = context.Route.Index;
				foreach (IControllerBinding binding in bindings)
				{
					IHttpHandler handler;
					if (binding.TryBinding(context, path, controller, out handler))
						return handler;
					else
					{
						context.Route.ReverseToIndex(controllerIndex);
						path.ReverseToIndex(index);
					}
				}
			}
			return null;
		}*/

		/*public void SerializeAbsoutePath(IRouteAction action, IPathStack path)
		{
			SerializePath(action, path, true);
		}

		public void SerializeRelativePath(IRouteAction action, IPathStack path)
		{
			SerializePath(action, path, false);
		}

		private void SerializePath(IRouteAction action, IPathStack path, bool requireEntry)
		{
			IControllerBinding[] bindings = GetControllerBindings(action.ControllerType);
			if (bindings == null || bindings.Length == 0) throw new BindingException(String.Format("Type \"{0}\" is not bindable.", action.ControllerType.FullName));
			IPathStack bestStack = null;
			foreach (IControllerBinding b in bindings)
				if (!requireEntry || b is IEntryControllerBinding)
				{
					IPathStack trialStack = new PathStack(false);
					if (requireEntry)
						((IEntryControllerBinding)b).SerializeToPath(action, trialStack);
					else
						b.SerializeToPath(action, trialStack);

					IRouteAction childAction = action.ChildAction;
					if (childAction != null)
					{
						SerializePath(childAction, trialStack, false);
					}

					if (bestStack == null || trialStack.Index > bestStack.Index || (trialStack.Index == bestStack.Index && trialStack.QueryString.Count > bestStack.QueryString.Count))
						bestStack = trialStack;
				}
			if (bestStack != null)
			{
				path.Push(bestStack);
			}
			else
				throw new BindingException(String.Format("Type \"{0}\" is not a bindable EntryController.", action.ControllerType.FullName));
		}*/

		private class EntrySet : ICollection<IEntryMapping>
		{
			private IEntryMapping[] list;

			public bool IsSet
			{
				get
				{
					return list != null;
				}
			}

			private void SetStore()
			{
				if (list == null) list = new IEntryMapping[0];
			}

			public void Add(IEntryMapping item)
			{
				SetStore();
				if (!Contains(item))
				{
					// TODO: Sort to weight
					Array.Resize(ref list, list.Length + 1);
					list[list.Length - 1] = item;
				}
			}

			public void Clear()
			{
				list = new IEntryMapping[0];
			}

			public bool Contains(IEntryMapping item)
			{
				if (list != null)
					foreach (IEntryMapping b in list)
						if (item == b) return true;
				return false;
			}

			public void CopyTo(IEntryMapping[] array, int arrayIndex)
			{
				list.CopyTo(array, arrayIndex);
			}

			public int Count
			{
				get
				{
					return list.Length;
				}
			}

			public bool IsReadOnly
			{
				get { return false; }
			}

			public bool Remove(IEntryMapping item)
			{
				int o = 0;
				for (int i = 0; i < list.Length; i++)
				{
					if (list[i].Equals(item))
						o++;
					else if (o > 0)
						list[i - o] = list[i];
				}
				if (o > 0)
					Array.Resize(ref list, list.Length - o);
				return o > 0;
			}

			public IEnumerator<IEntryMapping> GetEnumerator()
			{
				return ((IEnumerable<IEntryMapping>)list).GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
		
		private class MappingSet : ICollection<IInstanceMapping>
		{
			public bool IsSet
			{
				get
				{
					return typeControllers != null;
				}
			}

			public bool TryGetBindings(Type controllerType, out IInstanceMapping[] bindings)
			{
				if (typeControllers == null) { bindings = null; return false; }
				return typeControllers.TryGetValue(controllerType, out bindings);
			}

			private Dictionary<Type, IInstanceMapping[]> typeControllers;

			private void SetStore()
			{
				if (typeControllers == null) typeControllers = new Dictionary<Type, IInstanceMapping[]>();
			}

			public void Add(Type controllerType, IInstanceMapping[] bindings)
			{
				typeControllers.Add(controllerType, bindings);
			}

			public void Add(IInstanceMapping item)
			{
				if (item == null) throw new NullReferenceException();
				SetStore();
				IInstanceMapping[] b;
				if (!typeControllers.TryGetValue(item.ControllerType, out b))
					typeControllers.Add(item.ControllerType, new IInstanceMapping[] { item });
				else if (b == null || !b.Contains(item))
				{
					Array.Resize(ref b, b.Length + 1);
					b[b.Length - 1] = item;
				}
			}

			public void Clear()
			{
				SetStore();
				typeControllers.Clear();
			}

			public bool Contains(IInstanceMapping item)
			{
				if (typeControllers == null) return false;
				IInstanceMapping[] b;
				if (!typeControllers.TryGetValue(item.ControllerType, out b)) return false;
				return b != null && b.Contains(item);
			}

			public void CopyTo(IInstanceMapping[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			public int Count
			{
				get
				{
					if (typeControllers == null) return 0;
					int i = 0;
					foreach (IInstanceMapping[] b in typeControllers.Values)
						i += b.Length;
					return i;
				}
			}

			public bool IsReadOnly
			{
				get { return false; }
			}

			public bool Remove(IInstanceMapping item)
			{
				SetStore();
				IInstanceMapping[] b;
				if (typeControllers.TryGetValue(item.ControllerType, out b) && b != null)
				{
					int o = 0;
					for (int i = 0; i < b.Length; i++)
					{
						if (b[i].Equals(item))
							o++;
						else if (o > 0)
							b[i - o] = b[i];
					}
					if (o > 0)
						Array.Resize(ref b, b.Length - o);
				}
				return true;
			}

			public IEnumerator<IInstanceMapping> GetEnumerator()
			{
				if (typeControllers == null) yield break;
				foreach (IInstanceMapping[] bs in typeControllers.Values)
					if (bs != null)
						foreach (IInstanceMapping b in bs)
							yield return b;
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}
