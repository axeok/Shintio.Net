using System;
using System.Linq;
using System.Reflection;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public class AssemblyLoadContextWrapper
	{
		public static readonly Type AssemblyLoadContextType =
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "Runtime", "Loader", "AssemblyLoadContext")!;

		private static readonly MethodInfo GetLoadContextMethod =
			AssemblyLoadContextType.GetMethod("GetLoadContext", new Type[] { typeof(Assembly) })!;

		private static readonly MethodInfo LoadFromStreamMethod = AssemblyLoadContextType.GetMethods()
			.FirstOrDefault(m => m.Name == "LoadFromStream" && m.GetParameters().Length == 1)!;

		private readonly object _assemblyLoadContext;

		public AssemblyLoadContextWrapper(object assemblyLoadContext)
		{
			_assemblyLoadContext = assemblyLoadContext;
		}

		public object AssemblyLoadContext => _assemblyLoadContext;

		public static AssemblyLoadContextWrapper? GetLoadContext(AssemblyWrapper assembly)
		{
			return new AssemblyLoadContextWrapper(GetLoadContextMethod.Invoke(null, new[] { assembly.Assembly })!);
		}

		public AssemblyWrapper? LoadFromStream(MemoryStreamWrapper stream)
		{
			var result = LoadFromStreamMethod.Invoke(AssemblyLoadContext, new[] { stream.MemoryStream });

			return result == null ? null : new AssemblyWrapper(result);
		}

		public AssemblyWrapper? LoadFromBytes(byte[] bytes)
		{
			using var stream = new MemoryStreamWrapper(bytes);

			return LoadFromStream(stream);
		}

		public void SubscribeToAssembly(Func<object, AssemblyName, Assembly> handler)
		{
			var eventInfo = AssemblyLoadContextType.GetEvent("Resolving", BindingFlags.Instance | BindingFlags.Public)!;
			
			eventInfo.AddEventHandler(AssemblyLoadContext, handler);
		}
	}
}