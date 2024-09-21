using System;
using System.Linq;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Common
{
	public class TypeWrapper
	{
		public static readonly Type TypeType =
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "Type")!;
		
		public TypeWrapper(Type type)
		{
			Type = type;
		}

		public Type Type { get; }
		
		public static Type? GetType(string name)
		{
			return (Type)TypeType.GetMethod("GetType", new [] { typeof(string) })!.Invoke(null, new object[] { name });
		}

		public object? CreateInstance()
		{
			return Activator.CreateInstance(Type);
		}

		public object? CreateInstance(params object[] args)
		{
			return Activator.CreateInstance(Type, args);
		}

		public object? Invoke(string methodName, object from, params object[] parameters)
		{
			return Type.GetMethod(methodName, parameters.Select(p => p.GetType()).ToArray())?.Invoke(from, parameters);
		}

		public object? InvokeStatic(string methodName, params object[] parameters)
		{
			return Type.GetMethod(methodName, parameters.Select(p => p.GetType()).ToArray())?.Invoke(null, parameters);
		}
	}
}