using System.IO.Compression;
using Shintio.Compression.Enums;

namespace Shintio.Compression.System
{
	public class DeflateCompressor : StreamCompressor<DeflateStream>
	{
		public override CompressionMethod Method => CompressionMethod.Deflate;
	}
}