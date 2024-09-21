namespace Shintio.Compression.Enums
{
	public enum CompressionMethod : byte
	{
		None = 0,
		Deflate = 1,
		ZLib = 2,
		GZip = 3,
		Brotli = 4, // Best compression ratio
		Snappy = 5, // Fastest, https://github.com/google/snappy
		Zstd = 6, // Near Brotli, https://github.com/facebook/zstd
		ZstdMultiThread = 7,
		LZ4 = 8,
	}
}