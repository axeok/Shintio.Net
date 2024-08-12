using System;
using Shintio.Compression.Enums;

namespace Shintio.Compression.Exceptions
{
	public class UnsupportedCompressionMethod : Exception
	{
		public UnsupportedCompressionMethod(CompressionMethod compressionMethod)
		{
			CompressionMethod = compressionMethod;
		}

		public CompressionMethod CompressionMethod { get; }
	}
}