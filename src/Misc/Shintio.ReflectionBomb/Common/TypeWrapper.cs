using System;
using System.Linq;

namespace Shintio.ReflectionBomb.Common
{
	public class TypeWrapper
	{
		public TypeWrapper(Type type)
		{
			Type = type;
		}

		public Type Type { get; }

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