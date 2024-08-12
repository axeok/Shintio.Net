using Shintio.Compression.Common;
using Shintio.Compression.Enums;

namespace Shintio.Compression.Extensions;

public static class AutoCompressorExtensions
{
	public const CompressionMethod DefaultCompressionMethod = CompressionMethod.Deflate;

	public static string Compress(this AutoCompressor compressor, string data)
	{
		return compressor.Compress(data, DefaultCompressionMethod);
	}

	public static string Decompress(this AutoCompressor compressor, string data)
	{
		return compressor.Decompress(data, DefaultCompressionMethod);
	}
}