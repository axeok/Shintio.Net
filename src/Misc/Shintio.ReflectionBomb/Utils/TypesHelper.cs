using System;
using System.Collections.Generic;
using System.Linq;
using Shintio.ReflectionBomb.Types;

namespace Shintio.ReflectionBomb.Utils
{
	public static class TypesHelper
	{
		public static readonly Type TypeFromSystem = typeof(List<>);
		public static readonly Type TypeOfType = typeof(Type);

		public static AssemblyWrapper GetAssembly(Type type)
		{
			return new AssemblyWrapper(TypeOfType.GetProperty("Assembly")!.GetValue(type)!);
		}

		public static Type? GetType(Type neighbour, string fullName)
		{
			return GetAssembly(neighbour).GetNativeTypes().FirstOrDefault(t => t.FullName == fullName);
		}

		public static Type? GetType(Type neighbour, string prefix, string name)
		{
			return GetType(neighbour, $"{prefix}.{name}");
		}

		public static Type? GetType(Type neighbour, params string[] nameParts)
		{
			return GetType(neighbour, string.Join(".", nameParts));
		}
	}
}