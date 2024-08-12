using System;
using System.Text;
using Shintio.Compression.Interfaces;

namespace Shintio.Compression.Extensions
{
	public static class CompressorExtensions
	{
		public static string Compress(this ICompressor compressor, string data)
		{
			return Convert.ToBase64String(compressor.Compress(Encoding.UTF8.GetBytes(data)));
		}

		public static string Decompress(this ICompressor compressor, string compressedData)
		{
			return Encoding.UTF8.GetString(compressor.Decompress(Convert.FromBase64String(compressedData)));
		}
	}
}