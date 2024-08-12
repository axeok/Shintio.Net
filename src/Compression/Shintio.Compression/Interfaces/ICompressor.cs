using Shintio.Compression.Enums;

namespace Shintio.Compression.Interfaces
{
	public interface ICompressor
	{
		public string Compress(string data);
		public string Decompress(string compressedData);

		public CompressionMethod Method { get; }
	}
}