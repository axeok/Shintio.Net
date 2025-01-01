using System;
using System.Reflection;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public class StreamReaderWrapper : IDisposable
	{
		public static readonly Type StreamReaderType =
#if NETCOREAPP3_0_OR_GREATER
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "IO", "StreamReader")!;
#else
			AppDomainWrapper.GetAssembly("System." + "IO" + ".FileSystem")!.GetNativeType("System." + "IO" +
				".StreamReader")!;
#endif

		private static readonly MethodInfo ReadToEndMethod =
			StreamReaderType.GetMethod("ReadToEnd", new Type[] { })!;

		private readonly object _streamReader;

		public StreamReaderWrapper(object streamReader)
		{
			_streamReader = streamReader;
		}

		public object StreamReader => _streamReader;

		public void Dispose()
		{
			(_streamReader as IDisposable)?.Dispose();
		}

		public string ReadToEnd()
		{
			return (string)ReadToEndMethod.Invoke(_streamReader, new object[] { });
		}
	}
}