using System;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public class MemoryStreamWrapper : IDisposable
	{
		public static readonly Type MemoryStreamType =
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "IO", "MemoryStream") ??
			AppDomainWrapper.GetAssembly("System." + "Private." + "CoreLib")!
				.GetNativeType("System." + "IO" + ".MemoryStream")!;

		private readonly object _memoryStream;

		public MemoryStreamWrapper()
		{
			_memoryStream = Activator.CreateInstance(MemoryStreamType)!;
		}

		public MemoryStreamWrapper(byte[] buffer)
		{
			_memoryStream = Activator.CreateInstance(MemoryStreamType, new object[] { buffer })!;
		}

		public object MemoryStream => _memoryStream;

		public void Dispose()
		{
			(_memoryStream as IDisposable)?.Dispose();
		}
	}
}
