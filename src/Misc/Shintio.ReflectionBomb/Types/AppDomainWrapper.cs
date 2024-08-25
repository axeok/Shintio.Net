using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public static class AppDomainWrapper
	{
		public static readonly Type AppDomainType =
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "AppDomain")!;

		private static readonly PropertyInfo CurrentDomainProperty = AppDomainType.GetProperty("CurrentDomain")!;

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
	}
}