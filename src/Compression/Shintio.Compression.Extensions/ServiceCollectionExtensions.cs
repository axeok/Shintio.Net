using Microsoft.Extensions.DependencyInjection;
using Shintio.Compression.Common;
using Shintio.Compression.Interfaces;
using Shintio.Compression.LZ4;
using Shintio.Compression.Snappy;
using Shintio.Compression.System;
using Shintio.Compression.Zstd;

namespace Shintio.Compression.Extensions;

public static class ServiceCollectionExtensions
{
	private static readonly List<Type> Compressors = new List<Type>()
	{
		typeof(ZLibCompressor),
		typeof(DeflateCompressor),
		typeof(GZipCompressor),
		typeof(BrotliCompressor),
		typeof(SnappyCompressor),
		typeof(ZstdCompressor),
		typeof(ZstdMultiThreadCompressor),
		typeof(LZ4Compressor),
	};

	public static IServiceCollection AddCompression(this IServiceCollection services)
	{
		foreach (var type in Compressors)
		{
			services.AddSingleton(type);
		}

		return services
			.AddSingleton<AutoCompressor>(provider =>
			{
				var service = new AutoCompressor();

				foreach (var type in Compressors)
				{
					service.AddCompressor((ICompressor)provider.GetRequiredService(type));
				}

				return service;
			})
			.AddSingleton<ICompressor, DeflateCompressor>();
	}
}