using Shintio.Compression.Enums;
using Shintio.Compression.Interfaces;
using ZstdSharp;
using ZstdSharp.Unsafe;

namespace Shintio.Compression.Zstd;

public class ZstdMultiThreadCompressor : ICompressor
{
	public CompressionMethod Method => CompressionMethod.ZstdMultiThread;

	public byte[] Compress(byte[] data)
	{
		using var ms = new MemoryStream();

		using (var cs = new CompressionStream(ms, ZstdCompressor.CompressionLevel))
		{
			cs.SetParameter(ZSTD_cParameter.ZSTD_c_nbWorkers, Environment.ProcessorCount);
			cs.Write(data, 0, data.Length);
		}

		return ms.ToArray();
	}

	public byte[] Decompress(byte[] compressedData)
	{
		var decompressor = new Decompressor();

		return decompressor.Unwrap(compressedData).ToArray();
	}
}