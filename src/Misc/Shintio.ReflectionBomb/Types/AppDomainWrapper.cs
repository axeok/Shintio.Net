using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public static class AppDomainWrapper
	{
		public static readonly Type AppDomainType =
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "AppDomain")!;
		
		public static readonly Type ThreadType =
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System.Threading", "Thread")!;
		
		public static readonly Type AppContextType =
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "AppContext")!;

		public static readonly PropertyInfo CurrentDomainProperty = AppDomainType.GetProperty("CurrentDomain")!;
		
		public static readonly PropertyInfo CurrentThreadProperty = ThreadType.GetProperty("CurrentThread")!;

		public static IEnumerable<AssemblyWrapper> GetAssemblies()
		{
			var currentDomain = CurrentDomainProperty.GetValue(null)!;

#if NETCOREAPP3_0_OR_GREATER
			return ((object[])currentDomain.GetType()
					.GetMethod("GetAssemblies")!
					.Invoke(currentDomain, new object[] { }))
				.Select(a => new AssemblyWrapper(a));
#else
			return ((object[])currentDomain.GetType()
					.GetMethod("GetAssemblies",
						BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)!
					.Invoke(currentDomain, new object[] { false }))
				.Select(a => new AssemblyWrapper(a));
#endif
		}

		public static AssemblyWrapper? GetAssembly(string partOfName)
		{
			return GetAssemblies().FirstOrDefault(a => a.FullName.Contains(partOfName));
		}

		public static AssemblyWrapper? GetOrLoadAssembly(string name)
		{
			var result = GetAssembly(name);
			if (result == null)
			{
				result = AssemblyWrapper.Load(name);
			}

			return result;
		}
		
		public static int GetCurrentThreadId()
		{
			return (int)ThreadType.GetProperty("ManagedThreadId")!.GetValue(CurrentThreadProperty.GetValue(null));
		}

		public static void SubscribeToUnhandledException(UnhandledExceptionEventHandler handler)
		{
			SubscribeToCurrentDomainEvent("UnhandledException", handler);
		}
		
		public static void SubscribeToFirstChanceException(EventHandler<FirstChanceExceptionEventArgs> handler)
		{
			SubscribeToCurrentDomainEvent("FirstChanceException", handler);
		}
		
		public static void SubscribeToAssemblyResolve(ResolveEventHandler handler)
		{
			SubscribeToCurrentDomainEvent("AssemblyResolve", handler);
		}
		
		private static void SubscribeToCurrentDomainEvent(string name, Delegate handler)
		{
			var eventInfo = CurrentDomainProperty.PropertyType.GetEvent(name, BindingFlags.Instance | BindingFlags.Public)!;
			eventInfo.AddEventHandler(CurrentDomainProperty.GetValue(null), handler);
		}
	}
}