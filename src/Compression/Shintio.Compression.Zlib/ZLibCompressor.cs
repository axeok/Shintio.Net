using System;
using System.Text;
using Shintio.Compression.Enums;
using Shintio.Compression.Interfaces;

namespace Shintio.Compression.ZLib
{
	public class ZLibCompressor : ICompressor
	{
		public CompressionMethod Method => CompressionMethod.ZLib;

		public string Compress(string data)
		{
			return Convert.ToBase64String(CompressBytes(data));
		}

		public string Decompress(string compressedData)
		{
			return DecompressBytes(Convert.FromBase64String(compressedData));
		}

		private byte[] CompressBytes(string data)
		{
			var bytes = Encoding.UTF8.GetBytes(data);

			return ZLibCompressorInternal.Compress(-1, bytes, 0, bytes.Length).ToArray();
		}

		private string DecompressBytes(byte[] compressedData)
		{
			var bytes = compressedData;

			return Encoding.UTF8.GetString(ZLibCompressorInternal.Decompress(bytes, 0, bytes.Length).ToArray());
		}
	}
}