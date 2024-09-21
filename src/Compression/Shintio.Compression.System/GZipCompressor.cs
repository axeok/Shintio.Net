using System.IO.Compression;
using Shintio.Compression.Enums;

namespace Shintio.Compression.System
{
	public class GZipCompressor : StreamCompressor<GZipStream>
	{
		public override CompressionMethod Method => CompressionMethod.GZip;
	}
}