using Shintio.Compression.Enums;
using Shintio.Compression.Interfaces;
using ZstdSharp;

namespace Shintio.Compression.Zstd;

public class ZstdCompressor : ICompressor
{
	public static int CompressionLevel = 0;

	public CompressionMethod Method => CompressionMethod.Zstd;

	public byte[] Compress(byte[] data)
	{
		var compressor = new Compressor(CompressionLevel);

		return compressor.Wrap(data).ToArray();
	}

	public byte[] Decompress(byte[] compressedData)
	{
		var decompressor = new Decompressor();

		return decompressor.Unwrap(compressedData).ToArray();
	}
}