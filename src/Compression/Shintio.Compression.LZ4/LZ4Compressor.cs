using K4os.Compression.LZ4.Streams;
using Shintio.Compression.Enums;
using Shintio.Compression.Interfaces;

namespace Shintio.Compression.LZ4;

public class LZ4Compressor : ICompressor
{
	public CompressionMethod Method => CompressionMethod.LZ4;

	public byte[] Compress(byte[] data)
	{
		using var ms = new MemoryStream();

		using (var cs = LZ4Stream.Encode(ms))
		{
			cs.Write(data, 0, data.Length);
		}

		return ms.ToArray();
	}

	public byte[] Decompress(byte[] compressedData)
	{
		using var compressedMs = new MemoryStream(compressedData);
		using var decompressedMs = new MemoryStream();

		using (var ds = LZ4Stream.Decode(compressedMs))
		{
			ds!.CopyTo(decompressedMs);
		}

		return decompressedMs.ToArray();
	}
}