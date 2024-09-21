using Shintio.Compression.Enums;
using Shintio.Compression.Interfaces;

namespace Shintio.Compression.Snappy;

public class SnappyCompressor : ICompressor
{
	public CompressionMethod Method => CompressionMethod.Snappy;

	public byte[] Compress(byte[] data)
	{
		return Snappier.Snappy.CompressToArray(data);
	}

	public byte[] Decompress(byte[] compressedData)
	{
		return Snappier.Snappy.DecompressToArray(compressedData);
	}
}