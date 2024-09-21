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

		public static IEnumerable<Type> GetChildrenTypes(Type type, bool allowAbstract = false)
		{
			return type.Assembly
				.GetTypes()
				.Where(child =>
					child.IsClass &&
					(allowAbstract || !child.IsAbstract) &&
					child.IsSubclassOf(type)
				) ?? Array.Empty<Type>();
		}

		public static IEnumerable<Type> GetChildrenInterfaces(Type type)
		{
			return type.Assembly
				.GetTypes()
				.Where(t => type.IsAssignableFrom(t) && t != type) ?? Array.Empty<Type>();
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

		public static object? GetPropertyValue(PropertyInfo property)
		{
			try
			{
				var assemblyBuilder =
					AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"),
						AssemblyBuilderAccess.Run);
				var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");
				var typeBuilder = moduleBuilder.DefineType("DynamicType", TypeAttributes.Public);

				typeBuilder.AddInterfaceImplementation(property.DeclaringType);

				foreach (var propertyInfo in new[] { property.DeclaringType }
					         .Concat(property.DeclaringType.GetInterfaces())
					         .SelectMany(i => i.GetProperties()))
				{
					var propertyBuilder = typeBuilder.DefineProperty(propertyInfo.Name,
						PropertyAttributes.None,
						propertyInfo.PropertyType,
						null);

					var setMethod = propertyInfo.GetSetMethod();
					if (setMethod != null)
					{
						var propertyMethod = typeBuilder.DefineMethod($"set_{propertyInfo.Name}",
							MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName |
							MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
							null,
							new[] { propertyInfo.PropertyType });

						var il = propertyMethod.GetILGenerator();
						il.Emit(OpCodes.Ldarg_0);
						il.Emit(OpCodes.Ldarg_1);
						il.Emit(OpCodes.Call, setMethod);
						il.Emit(OpCodes.Ret);
						propertyBuilder.SetSetMethod(propertyMethod);
					}
					else
					{
						var propertyMethod = typeBuilder.DefineMethod($"set_{propertyInfo.Name}",
							MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName |
							MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
							null,
							new[] { propertyInfo.PropertyType });

						var il = propertyMethod.GetILGenerator();
						il.Emit(OpCodes.Ret);
						propertyBuilder.SetSetMethod(propertyMethod);
					}

					var getMethod = propertyInfo.GetGetMethod();
					if (getMethod != null)
					{
						var propertyMethod = typeBuilder.DefineMethod($"get_{propertyInfo.Name}",
							MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName |
							MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
							propertyInfo.PropertyType,
							Type.EmptyTypes);

						var il = propertyMethod.GetILGenerator();
						il.Emit(OpCodes.Ldarg_0);
						il.Emit(OpCodes.Call, getMethod);
						il.Emit(OpCodes.Ret);
						propertyBuilder.SetGetMethod(propertyMethod);
					}
					else
					{
						var propertyMethod = typeBuilder.DefineMethod($"get_{propertyInfo.Name}",
							MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName |
							MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot,
							propertyInfo.PropertyType,
							Type.EmptyTypes);

						var il = propertyMethod.GetILGenerator();
						il.Emit(OpCodes.Ldnull);
						il.Emit(OpCodes.Ret);
						propertyBuilder.SetGetMethod(propertyMethod);
					}
				}

				var dynamicType = typeBuilder.CreateTypeInfo();

				var instance = Activator.CreateInstance(dynamicType);

				// Console.WriteLine(property.Name);
				// foreach (var propertyInfo in instance.GetType().GetProperties())
				// {
				// 	Console.WriteLine($"p {propertyInfo.Name}: {propertyInfo.GetValue(instance)}");
				// }

				return dynamicType.GetProperty(property.Name).GetValue(instance);
			}
			catch
			{
				return null;
			}
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