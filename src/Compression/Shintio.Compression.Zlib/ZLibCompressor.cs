using Shintio.Compression.Enums;
using Shintio.Compression.Interfaces;

namespace Shintio.Compression.ZLib
{
	public class ZLibCompressor : ICompressor
	{
		public CompressionMethod Method => CompressionMethod.ZLib;

		public byte[] Compress(byte[] data)
		{
			return ZLibCompressorInternal.Compress(-1, data, 0, data.Length).ToArray();
		}

		public byte[] Decompress(byte[] compressedData)
		{
			return ZLibCompressorInternal.Decompress(compressedData, 0, compressedData.Length).ToArray();
		}
	}
}