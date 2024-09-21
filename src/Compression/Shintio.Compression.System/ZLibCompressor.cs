#if NETCOREAPP3_0_OR_GREATER
using System.IO.Compression;
using Shintio.Compression.Enums;

namespace Shintio.Compression.System
{
	public class ZLibCompressor : StreamCompressor<ZLibStream>
	{
		public override CompressionMethod Method => CompressionMethod.ZLib;
	}
}
#endif