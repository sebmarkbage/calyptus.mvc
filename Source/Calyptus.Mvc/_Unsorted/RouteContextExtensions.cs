/*using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace Calyptus.Mvc
{
	public static class RouteContextExtensions
	{
		public static string GetPath(this IRouteContext context, Expression<Action> action)
		{
			var expr = action as LambdaExpression;
			if (expr == null) throw new BindingException("Missing action expression.");
			return GetAbsolutePathPrivate(context, expr);
		}

		private static string GetAbsolutePathPrivate(IRouteContext context, LambdaExpression expr)
		{
			IRouteAction route = BuildAction(expr);

			if (route == null) return context.AppPath;

			PathStack path = new PathStack(false);

			context.BindingContext.SerializeAbsoutePath(route, path);

			return context.AppPath + path.ToString();
		}

		public static string GetPath<TRelativeController>(this IRouteContext context, int index, Expression<Action<TRelativeController>> action)
		{
			var expr = action as LambdaExpression;

			int pathIndex = context.GetPathIndexOf(typeof(TRelativeController), index);
			if (pathIndex == -1)
			{
				if (index == 0 || index == -1)
					return GetAbsolutePathPrivate(context, action);
				else
					throw new BindingException(String.Format("Index out of bounds. The controller ({0}) have been initialized fewer than {1} times.", typeof(TRelativeController).Name, index < 0 ? -index : index + 1));
			}

			// TODO: If new expression is used, fallback to absolute path

			IRouteAction route = BuildAction(expr);

			StringBuilder p = new StringBuilder();

			for (int i = pathIndex; i < context.PathCount - 1; i++)
				p.Append("../");

			PathStack path = new PathStack(false);

			if (route != null)
				context.BindingContext.SerializeRelativePath(route, path);

			if (path.Count == 0 && p.Length == 0)
				p.Append("./");
			else
				p.Append(path.ToString());

			return p.ToString();
		}

		public static string GetPath<TRelativeController, TWithActionsFromController>(this IRouteContext context, int index, int secondIndex, Expression<Func<TRelativeController, TWithActionsFromController>> action)
		{
			throw new NotImplementedException();
		}

		private static IRouteAction BuildAction(LambdaExpression expression)
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

	}
}
*/