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
			if (expression == null) return null;
			if (expression.Body.NodeType == ExpressionType.Parameter || expression.Body.NodeType == ExpressionType.New)
				return new RouteAction(expression.Body.Type, null, null, null);
			if (expression.Body.NodeType != ExpressionType.Call)
				throw new BindingException("Invalid lambda expression. Cannot serialize. Must be only parameter or end in method call.");

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
					{
						NewExpression newParam = call.Object as NewExpression;
						if (newParam != null)
						{
							call = null;
							type = newParam.Type;
						}
						else
							throw new BindingException(String.Format("Only new-expressions, the first parameter of the lambda, casts and method call may be used. The first controller must be the first parameter of the lambda ({0}) or a new-expression.", expression.Parameters[0].Name));
						// TODO: Allow reference equals to existing controller (or bound parameter) in the route context
					}
				}				

				action = new RouteAction(type, method, parameters, action);
			}
			return action;
		}

		public string GetRelativePath<TRelativeController>(int index, Expression<Action<TRelativeController>> action)
		{
			var expr = action as LambdaExpression;

			int pathIndex = GetPathIndexOf(typeof(TRelativeController), index);
			if (pathIndex == -1)
			{
				if (index == 0 || index == -1)
					return GetAbsolutePathPrivate(action);
				else
					throw new BindingException(String.Format("Index out of bounds. The controller ({0}) have been initialized fewer than {1} times.", typeof(TRelativeController).Name, index < 0 ? -index : index + 1));
			}

			// TODO: If new expression is used, fallback to absolute path

			IRouteAction route = BuildAction(expr);

			StringBuilder p = new StringBuilder();

			for (int i = pathIndex; i < _pathCount - 1; i++)
				p.Append("../");

			PathStack path = new PathStack(false);
			
			if (route != null)
				RoutingEngine.SerializeRelativePath(route, path);

			if (path.Count == 0 && p.Length == 0)
				p.Append("./");
			else
				p.Append(path.GetPath(pathIndex == 0));

			return p.ToString();
		}

		public string GetRelativePath<TRelativeController, TWithActionsFromController>(int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action)
		{
			throw new NotImplementedException("Get relative path with actions form controller is not yet implemented.");
		}

		public string GetAbsolutePath(Expression<Action> action)
		{
			var expr = action as LambdaExpression;
			return GetAbsolutePathPrivate(expr);
		}

		private string GetAbsolutePathPrivate(LambdaExpression expr)
		{
			IRouteAction route = BuildAction(expr);

			if (route == null) return _appPath;

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
