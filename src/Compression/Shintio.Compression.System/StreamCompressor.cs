using System;
using System.IO;
using System.IO.Compression;
using Shintio.Compression.Enums;
using Shintio.Compression.Interfaces;

namespace Shintio.Compression.System
{
	public abstract class StreamCompressor<TStream> : ICompressor where TStream : Stream
	{
		public abstract CompressionMethod Method { get; }

		public byte[] Compress(byte[] data)
		{
			using var ms = new MemoryStream();

			using (var cs = Activator.CreateInstance(typeof(TStream), ms, CompressionMode.Compress) as TStream)
			{
				cs!.Write(data, 0, data.Length);
			}

			return ms.ToArray();
		}

		public byte[] Decompress(byte[] compressedData)
		{
			using var compressedMs = new MemoryStream(compressedData);
			using var decompressedMs = new MemoryStream();

			using (var ds =
			       Activator.CreateInstance(typeof(TStream), compressedMs, CompressionMode.Decompress) as TStream)
			{
				ds!.CopyTo(decompressedMs);
			}

			return decompressedMs.ToArray();
		}
	}
}