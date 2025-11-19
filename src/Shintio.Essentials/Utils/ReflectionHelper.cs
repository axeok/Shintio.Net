using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Shintio.Essentials.Utils
{
	public static class ReflectionHelper
	{
		public const BindingFlags PrivateFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
		public const BindingFlags StaticFlags = PrivateFlags | BindingFlags.Static;

		public static bool IsSubclassOfGeneric(Type type, Type generic)
		{
			var tempType = type;

			while (tempType != null && tempType != typeof(object))
			{
				var current = tempType.IsGenericType ? tempType.GetGenericTypeDefinition() : tempType;
				if (generic == current)
				{
					return true;
				}

				tempType = tempType.BaseType;
			}

			return false;
		}

		public static void SetProperty<T>(T obj, string name, object value) where T : notnull
		{
			obj.GetType().GetProperty(name, PrivateFlags)?.SetValue(obj, value);
		}

		public static IEnumerable<MethodInfo> GetMethodsWithoutBase(Type type)
		{
			return type.GetMethods()
				.Where(m =>
					!typeof(object)
						.GetMethods()
						.Select(me => me.Name)
						.Contains(m.Name));
		}

		public static IEnumerable<MethodInfo> GetMethodsWithoutBase(Type type, BindingFlags flags)
		{
			return type.GetMethods(flags)
				.Where(m =>
					!typeof(object)
						.GetMethods()
						.Select(me => me.Name)
						.Contains(m.Name));
		}

		public static bool IsComputed(this PropertyInfo propertyInfo)
		{
			return propertyInfo.GetMethod != null &&
			       propertyInfo.GetMethod.GetCustomAttribute(typeof(CompilerGeneratedAttribute)) == null;
		}

		public static MemberInfo GetMemberInfo(LambdaExpression expression)
		{
			if (expression.Body is UnaryExpression unaryExpression)
			{
				var operand = unaryExpression.Operand;
				if (operand is MethodCallExpression methodCallExpression)
				{
					return (MethodInfo)((ConstantExpression)methodCallExpression.Object!).Value;
				}

				return ((MemberExpression)operand).Member;
			}
			else if (expression.Body is MethodCallExpression methodCallExpression)
			{
				return methodCallExpression.Method;
			}

			return ((MemberExpression)expression.Body).Member;
		}
		
		public static Delegate CreateDelegate(MethodInfo methodInfo, object? target)
		{
			Func<Type[], Type> getType = Expression.GetDelegateType;
			var isAction = methodInfo.ReturnType == typeof(void);
			var types = methodInfo.GetParameters().Select(p => p.ParameterType);

			if (isAction)
			{
				types = types.Concat(new[] { typeof(void) });
			}
			else
			{
				types = types.Concat(new[] { methodInfo.ReturnType });
			}

			if (methodInfo.IsStatic)
			{
				return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);
			}

			return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo);
		}
	}
}