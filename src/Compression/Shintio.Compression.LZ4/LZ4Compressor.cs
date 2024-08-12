using System.Text;
using K4os.Compression.LZ4.Streams;
using Shintio.Compression.Enums;
using Shintio.Compression.Interfaces;

namespace Shintio.Compression.LZ4;

public class LZ4Compressor : ICompressor
{
	public CompressionMethod Method => CompressionMethod.LZ4;

	public string Compress(string data)
	{
		var bytes = Encoding.UTF8.GetBytes(data);

		using var ms = new MemoryStream();

		using (var cs = LZ4Stream.Encode(ms))
		{
			cs.Write(bytes, 0, bytes.Length);
		}

		return Convert.ToBase64String(ms.ToArray());
	}

	public string Decompress(string compressedData)
	{
		var bytes = Convert.FromBase64String(compressedData);

		using var compressedMs = new MemoryStream(bytes);
		using var decompressedMs = new MemoryStream();

		using (var ds = LZ4Stream.Decode(compressedMs))
		{
			ds!.CopyTo(decompressedMs);
		}

		return Encoding.UTF8.GetString(decompressedMs.ToArray());
	}
}