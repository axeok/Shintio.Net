using System.Text;
using Shintio.Compression.Enums;
using Shintio.Compression.Interfaces;
using ZstdSharp;

namespace Shintio.Compression.Zstd;

public class ZstdCompressor : ICompressor
{
	public const int CompressionLevel = 0;

	public CompressionMethod Method => CompressionMethod.Zstd;

	public string Compress(string data)
	{
		var compressor = new Compressor(CompressionLevel);

		return Convert.ToBase64String(compressor.Wrap(Encoding.UTF8.GetBytes(data)));
	}

	public string Decompress(string compressedData)
	{
		var decompressor = new Decompressor();

		return Encoding.UTF8.GetString(decompressor.Unwrap(Convert.FromBase64String(compressedData)));
	}
}