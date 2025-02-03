using System;
using System.Reflection;
using System.Threading.Tasks;
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

		private static readonly MethodInfo ReadLineMethod =
			StreamReaderType.GetMethod("ReadLine", new Type[] { })!;

		private static readonly MethodInfo ReadToEndAsyncMethod =
			StreamReaderType.GetMethod("ReadToEndAsync", new Type[] { })!;

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

		public Task<string> ReadToEndAsync()
		{
			return (Task<string>)ReadToEndAsyncMethod.Invoke(_streamReader, new object[] { });
		}

		public string? ReadLine()
		{
			return (string?)ReadLineMethod.Invoke(_streamReader, new object[] { });
		}
	}
}