using System.IO.Compression;
using Shintio.Compression.Enums;

namespace Shintio.Compression.System
{
	public class BrotliCompressor : StreamCompressor<BrotliStream>
	{
		public override CompressionMethod Method => CompressionMethod.Brotli;
	}
}