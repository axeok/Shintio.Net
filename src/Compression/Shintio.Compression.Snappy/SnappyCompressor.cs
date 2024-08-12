using System.Text;
using Shintio.Compression.Enums;
using Shintio.Compression.Interfaces;

namespace Shintio.Compression.Snappy;

public class SnappyCompressor : ICompressor
{
	public CompressionMethod Method => CompressionMethod.Snappy;

	public string Compress(string data)
	{
		return Convert.ToBase64String(Snappier.Snappy.CompressToArray(Encoding.UTF8.GetBytes(data)));
	}

	public string Decompress(string compressedData)
	{
		return Encoding.UTF8.GetString(Snappier.Snappy.DecompressToArray(Convert.FromBase64String(compressedData)));
	}
}