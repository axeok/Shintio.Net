using Shintio.Compression.Enums;

namespace Shintio.Compression.Interfaces
{
	public interface ICompressor
	{
		public byte[] Compress(byte[] data);
		public byte[] Decompress(byte[] compressedData);

		public CompressionMethod Method { get; }
	}
}