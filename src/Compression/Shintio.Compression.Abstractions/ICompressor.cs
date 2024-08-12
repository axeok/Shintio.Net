using Shintio.Compression.Abstractions.Enums;

namespace Shintio.Compression.Abstractions
{
    public interface ICompressor
    {
        public string Compress(string data, CompressionMethod method);
        public string Decompress(string compressedData, CompressionMethod method);
    }
}