using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Calyptus.MVC
{
	public class RouteContext : IRouteContext
	{
		private struct TypePathIndex
		{
			public int PathIndex;
			public Type ControllerType;
		}
		private TypePathIndex[] _boundControllers;
		private Stack<IDisposable> _disposableControllers;
		private int _index;

		private int _pathCount;

		private string _appPath;

		public IRoutingEngine RoutingEngine { get; private set; }

		public RouteContext(IRoutingEngine engine, string appPath, int pathCount, bool trailingSlash)
		{
			_pathCount = pathCount;
			if (trailingSlash) _pathCount++;
			RoutingEngine = engine;
			_index = 0;
			_appPath = appPath;
			if (!_appPath.EndsWith("/")) _appPath += "/";
			_disposableControllers = new Stack<IDisposable>();
		}

		public void AddController(object controller, int pathIndex)
		{
			if (controller == null) throw new NullReferenceException("Controller cannot be null.");

			if (_boundControllers == null) _boundControllers = new TypePathIndex[1];
			if (_boundControllers.Length <= _index) Array.Resize(ref _boundControllers, _index + 1);
			_boundControllers[_index++] = new TypePathIndex { ControllerType = controller.GetType(), PathIndex = pathIndex };
			
			IDisposable dc = controller as IDisposable;
			if (dc != null && !_disposableControllers.Contains(dc)) _disposableControllers.Push(dc);
		}

		public int Index { get { return _index - 1; } }

		public int ControllerCount { get { return _index; } }

		public void ReverseToIndex(int index)
		{
			if (index < -1) index = -1;
			_index = index + 1;
		}

		private int GetPathIndexOf(Type type, int index)
		{
			int internalIndex = 0;
			if (index >= 0)
				for (int i = 0; i < _boundControllers.Length; i++)
				{
					TypePathIndex bc = _boundControllers[i];
					if (type.IsAssignableFrom(bc.ControllerType))
					{
						if (internalIndex == index) return bc.PathIndex;
						internalIndex++;
					}
				}
			else if (index < 0)
			{
				index++;
				for (int i = _boundControllers.Length - 1; i >= 0; i--)
				{
					TypePathIndex bc = _boundControllers[i];
					if (type.IsAssignableFrom(bc.ControllerType))
					{
						if (internalIndex == index) return bc.PathIndex;
						internalIndex--;
					}
				}
			}
			return -1;
		}

		private IRouteAction BuildAction(LambdaExpression expression)
		{
			if (expression.Body.NodeType == ExpressionType.Parameter) return null;
			if (expression.Body.NodeType != ExpressionType.Call) throw new BindingException("Invalid lambda expression. Cannot serialize.");

			RouteAction action = null;
			//var type = expression.Parameters[0].Type;
			var call = expression.Body as MethodCallExpression;
			while (call != null)
			{
				object[] parameters = new object[call.Arguments.Count];
				int i = 0;
				foreach (Expression paramExpr in call.Arguments)
				{
					if (paramExpr.NodeType != ExpressionType.Constant)
						parameters[i] = LambdaExpression.Lambda(paramExpr).Compile().DynamicInvoke();
					else
						parameters[i] = ((ConstantExpression)paramExpr).Value;
					i++;
				}
				// TODO: controller type casting
				//if (action != null) action.ControllerType = call.Type;
				MethodInfo method = call.Method;
				Type type;
				MethodCallExpression methodExp = call.Object as MethodCallExpression;
				if (methodExp != null)
				{
					type = methodExp.Type;
					call = methodExp;
				}
				else if (call.Object.NodeType == ExpressionType.TypeAs || call.Object.NodeType == ExpressionType.Convert || call.Object.NodeType == ExpressionType.ConvertChecked)
				{
					UnaryExpression cast = call.Object as UnaryExpression;
					call = cast.Operand as MethodCallExpression;
					type = cast.Type;
					if (call == null)
					{
						ParameterExpression firstParam = call.Object as ParameterExpression;
						if (firstParam == null || firstParam != expression.Parameters[0])
							throw new BindingException(String.Format("Only the first parameter of the lambda, casts and method call may be used. The first controller must be the first parameter of the lambda ({0}).", expression.Parameters[0].Name));
					}
				}
				else
				{
					ParameterExpression firstParam = call.Object as ParameterExpression;
					if (firstParam != null && firstParam == expression.Parameters[0])
					{
						call = null;
						type = firstParam.Type;
					}
					else
						throw new BindingException(String.Format("Only the first parameter of the lambda, casts and method call may be used. The first controller must be the first parameter of the lambda ({0}).", expression.Parameters[0].Name));
				}				

				action = new RouteAction(type, method, parameters, action);
			}
			return action;
		}

		public string GetRelativePath<TRelativeController>(int index, Expression<Action<TRelativeController>> action)
		{
			int pathIndex = GetPathIndexOf(typeof(TRelativeController), index);
			if (pathIndex == -1)
			{
				if (index == 0 || index == -1)
					return GetAbsolutePathPrivate<TRelativeController>(action);
				else
					throw new BindingException(String.Format("Index out of bounds. The controller ({0}) have been initialized fewer than {1} times.", typeof(TRelativeController).Name, index < 0 ? -index : index + 1));
			}

			var expr = action as LambdaExpression;
			if (expr == null) throw new BindingException();

			IRouteAction route = BuildAction(expr);

			StringBuilder p = new StringBuilder();

			if (action == null) return "./";

			for (int i = pathIndex; i < _pathCount - 1; i++)
				p.Append("../");

			PathStack path = new PathStack(false);
			RoutingEngine.SerializeRelativePath(route, path);
			if (path.Count == 0 && p.Length == 0) p.Append("./");

			p.Append(path.GetPath(pathIndex == 0));

			return p.ToString();
		}

		public string GetRelativePath<TRelativeController, TWithActionsFromController>(int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action)
		{
			throw new NotImplementedException();
		}

		public string GetAbsolutePath<TEntryController>(Expression<Action<TEntryController>> action) where TEntryController : class, IEntryController
		{
			return GetAbsolutePathPrivate<TEntryController>(action);
		}

		private string GetAbsolutePathPrivate<TEntryController>(Expression<Action<TEntryController>> action)
		{
			var expr = action as LambdaExpression;
			if (expr == null) throw new BindingException();

			IRouteAction route = BuildAction(expr);

			if (action == null) return _appPath;

			PathStack path = new PathStack(false);

			RoutingEngine.SerializeAbsoutePath(route, path);

			return _appPath + path.GetPath(true);
		}

		public void Dispose()
		{
			foreach (IDisposable dc in _disposableControllers)
				dc.Dispose();
		}
	}
}
