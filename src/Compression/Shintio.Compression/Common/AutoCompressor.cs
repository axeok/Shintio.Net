using System.Collections.Generic;
using System.Linq;
using Shintio.Compression.Enums;
using Shintio.Compression.Exceptions;
using Shintio.Compression.Extensions;
using Shintio.Compression.Interfaces;

namespace Shintio.Compression.Common
{
	public class AutoCompressor
	{
		private readonly List<ICompressor> _compressors = new List<ICompressor>();

		public IEnumerable<CompressionMethod> SupportedMethods => _compressors.Select(c => c.Method).Distinct();

		public void AddCompressor(ICompressor compressor)
		{
			_compressors.Add(compressor);
		}

		public string Compress(string data, CompressionMethod method)
		{
			if (method == CompressionMethod.None)
			{
				return data;
			}

			var compressor = _compressors.FirstOrDefault(c => c.Method == method);
			if (compressor == null)
			{
				throw new UnsupportedCompressionMethod(method);
			}

			return compressor.Compress(data);
		}

		public string Decompress(string compressedData, CompressionMethod method)
		{
			if (method == CompressionMethod.None)
			{
				return compressedData;
			}

			var compressor = _compressors.FirstOrDefault(c => c.Method == method);
			if (compressor == null)
			{
				throw new UnsupportedCompressionMethod(method);
			}

			return compressor.Decompress(compressedData);
		}

		public byte[] Compress(byte[] data, CompressionMethod method)
		{
			if (method == CompressionMethod.None)
			{
				return data;
			}

			var compressor = _compressors.FirstOrDefault(c => c.Method == method);
			if (compressor == null)
			{
				throw new UnsupportedCompressionMethod(method);
			}

			return compressor.Compress(data);
		}

		public byte[] Decompress(byte[] compressedData, CompressionMethod method)
		{
			if (method == CompressionMethod.None)
			{
				return compressedData;
			}

			var compressor = _compressors.FirstOrDefault(c => c.Method == method);
			if (compressor == null)
			{
				throw new UnsupportedCompressionMethod(method);
			}

			return compressor.Decompress(compressedData);
		}
	}
}