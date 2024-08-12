using System.Text;
using Shintio.Compression.Enums;
using Shintio.Compression.Interfaces;
using ZstdSharp;
using ZstdSharp.Unsafe;

namespace Shintio.Compression.Zstd;

public class ZstdMultiThreadCompressor : ICompressor
{
	public const int CompressionLevel = ZstdCompressor.CompressionLevel;

	public CompressionMethod Method => CompressionMethod.ZstdMultiThread;

	public string Compress(string data)
	{
		var bytes = Encoding.UTF8.GetBytes(data);

		using var ms = new MemoryStream();

		using (var cs = new CompressionStream(ms, CompressionLevel))
		{
			cs.SetParameter(ZSTD_cParameter.ZSTD_c_nbWorkers, Environment.ProcessorCount);
			cs.Write(bytes, 0, bytes.Length);
		}

		return Convert.ToBase64String(ms.ToArray());
	}

	public string Decompress(string compressedData)
	{
		var decompressor = new Decompressor();

		return Encoding.UTF8.GetString(decompressor.Unwrap(Convert.FromBase64String(compressedData)));
	}
}