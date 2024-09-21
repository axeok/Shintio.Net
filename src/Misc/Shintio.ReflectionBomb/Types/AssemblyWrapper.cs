using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shintio.ReflectionBomb.Common;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public class AssemblyWrapper
	{
		public static readonly Type AssemblyType = GetAssemblyType();

		private readonly object _assembly;

		private static readonly MethodInfo LoadPathMethod = AssemblyType.GetMethod("Load", new[] { typeof(string) })!;
		private static readonly MethodInfo LoadBytesMethod = AssemblyType.GetMethod("Load", new[] { typeof(byte[]) })!;

		public AssemblyWrapper(object assembly)
		{
			_assembly = assembly;
			Type = _assembly.GetType();
			FullName = Type.GetProperty("FullName")!.GetValue(assembly).ToString();
		}

		public Type Type { get; }
		public string FullName { get; }
		public object Assembly => _assembly;

		public static AssemblyWrapper Load(string path)
		{
			return new AssemblyWrapper(LoadPathMethod.Invoke(null, new object[] { path }));
		}

		public static AssemblyWrapper Load(byte[] rawAssembly)
		{
			return new AssemblyWrapper(LoadBytesMethod.Invoke(null, new object[] { rawAssembly }));
		}

		public static AssemblyWrapper LoadFromBase64(string data)
		{
			return Load(Convert.FromBase64String(data));
		}

		public IEnumerable<TypeWrapper> GetTypes()
		{
			return GetNativeTypes().Select(t => new TypeWrapper(t));
		}

		public IEnumerable<Type> GetNativeTypes()
		{
			return (IEnumerable<Type>)AssemblyType.GetMethod("GetTypes")!.Invoke(_assembly, null);
		}

		public TypeWrapper? GetType(string fullName)
		{
			var type = GetNativeType(fullName);
			return type != null ? new TypeWrapper(type) : null;
		}

		public Type? GetNativeType(string fullName)
		{
			return GetNativeTypes().FirstOrDefault(t => t.FullName == fullName);
		}

		private static Type GetAssemblyType()
		{
			// ReSharper disable once PossibleMistakenCallToGetType
			var assembly = TypesHelper.TypeFromSystem.GetType()
				.GetProperty("Assembly")!
				.GetValue(TypesHelper.TypeOfType)!;

			return ((IEnumerable<Type>)assembly.GetType()!.GetMethod("GetTypes")!.Invoke(assembly, null))
				.FirstOrDefault(t => t.Name == "Assembly")!;
		}
	}
}