using System;
using System.Reflection;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public class StreamWriterWrapper : IDisposable
	{
		public static readonly Type StreamWriterType =
#if NETCOREAPP3_0_OR_GREATER
			TypesHelper.GetType(TypesHelper.TypeFromSystem, "System", "IO", "StreamWriter")!;
#else
			AppDomainWrapper.GetAssembly("System." + "IO" + ".FileSystem")!.GetNativeType("System." + "IO" +
				".StreamWriter")!;
#endif

		private static readonly MethodInfo WriteStringMethod =
			StreamWriterType.GetMethod("Write", new Type[] { typeof(string) })!;
		
		private static readonly MethodInfo WriteLineMethod =
			StreamWriterType.GetMethod("Write", new Type[] { typeof(string) })!;

		private readonly object _streamWriter;

		public StreamWriterWrapper(object streamWriter)
		{
			_streamWriter = streamWriter;
		}

		public object StreamWriter => _streamWriter;

		public void Dispose()
		{
			(_streamWriter as IDisposable)?.Dispose();
		}

		public void Write(string value)
		{
			WriteStringMethod.Invoke(_streamWriter, new object[] { value });
		}

		public void WriteLine(string value)
		{
			WriteLineMethod.Invoke(_streamWriter, new object[] { value });
		}
	}
}