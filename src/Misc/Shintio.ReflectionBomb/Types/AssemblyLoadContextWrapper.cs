using System;
using System.Linq;
using System.Linq.Expressions;
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

		public void SubscribeToAssembly(Func<object, AssemblyName, object> handler)
		{
			var eventInfo = AssemblyLoadContextType.GetEvent("Resolving", BindingFlags.Instance | BindingFlags.Public)!;

			var asmNameType = typeof(AssemblyName);
			var asmType = TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "Reflection", "Assembly")!;

			var ctxParam = Expression.Parameter(typeof(object), "ctx");
			var nameParam = Expression.Parameter(asmNameType, "name");

			var handlerConst = Expression.Constant(handler);

			var call = Expression.Call(
				handlerConst,
				handler.GetType().GetMethod("Invoke")!,
				ctxParam,
				nameParam
			);

			var cast = Expression.Convert(call, asmType);

			var lambdaType = typeof(Func<,,>).MakeGenericType(typeof(object), asmNameType, asmType);
			var lambda = Expression.Lambda(lambdaType, cast, ctxParam, nameParam);

			var compiled = lambda.Compile();

			eventInfo.AddEventHandler(AssemblyLoadContext, compiled);
		}
	}
}