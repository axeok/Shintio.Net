using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shintio.Vision.Abstractions;
using Shintio.Vision.Tesseract;

namespace Shintio.Vision.Extensions;

public class TesseractService : IHostedService
{
	private readonly IOcr _ocr;
	private readonly ILogger<TesseractService> _logger;

	public TesseractService(IOcr ocr, ILogger<TesseractService> logger)
	{
		_ocr = ocr;
		_logger = logger;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		if (_ocr is TesseractOcr tesseractOcr)
		{
			_logger.LogInformation("Initializing Tesseract...");

			await tesseractOcr.Initialize(Log);
		}

		await Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	private void Log(string message)
	{
		_logger.LogInformation("[Tesseract] {Message}", message);
	}
}