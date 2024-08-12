using System.Collections.Generic;
using System.Linq;
using ComponentAce.Compression.Libs.zlib;

namespace Shintio.Compression.ZLib
{
	internal static class ZLibCompressorInternal
	{
		private const int Flush = zlibConst.Z_NO_FLUSH;
		private const int BufferSize = 4096;

		public static List<byte> Compress(int level, byte[] buffer, int offset, int count)
		{
			var zStream = new ZStream();
			zStream.deflateInit(level);

			return Write(zStream, true, buffer, offset, count);
		}

		public static List<byte> Decompress(byte[] buffer, int offset, int count)
		{
			var zStream = new ZStream();
			zStream.inflateInit();

			return Write(zStream, false, buffer, offset, count);
		}

		private static List<byte> Write(
			ZStream zStream,
			bool isCompress,
			byte[] data,
			int offset,
			int count
		)
		{
			var buffer = new byte[BufferSize];
			var result = new List<byte>();

			if (count == 0)
			{
				return result;
			}

			int err;
			zStream.next_in = data;
			zStream.next_in_index = offset;
			zStream.avail_in = count;
			do
			{
				zStream.next_out = buffer;
				zStream.next_out_index = 0;
				zStream.avail_out = BufferSize;

				err = isCompress ? zStream.deflate(Flush) : zStream.inflate(Flush);
				if (err != zlibConst.Z_OK && err != zlibConst.Z_STREAM_END)
				{
					return result;
				}

				result.AddRange(buffer.Take(BufferSize - zStream.avail_out));
			} while (zStream.avail_in > 0 || zStream.avail_out == 0);

			do
			{
				zStream.next_out = buffer;
				zStream.next_out_index = 0;
				zStream.avail_out = BufferSize;

				err = isCompress ? zStream.deflate(zlibConst.Z_FINISH) : zStream.inflate(zlibConst.Z_FINISH);
				if (err != zlibConst.Z_STREAM_END && err != zlibConst.Z_OK)
				{
					return result;
				}

				if (BufferSize - zStream.avail_out > 0)
				{
					result.AddRange(buffer.Take(BufferSize - zStream.avail_out));
				}
			} while (zStream.avail_in > 0 || zStream.avail_out == 0);

			return result;
		}
	}
}