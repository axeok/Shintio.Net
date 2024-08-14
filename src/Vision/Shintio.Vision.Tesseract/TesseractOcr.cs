using LibGit2Sharp;
using Shintio.Vision.Abstractions;
using Tesseract;

namespace Shintio.Vision.Tesseract;

public class TesseractOcr : IOcr
{
	private const string DataPath = "tessdata";
	private const string DataRepository = "https://github.com/tesseract-ocr/tessdata.git";

	private int _lastReceivedObjects = -1;

	public Task Initialize(Action<string> log)
	{
		if (!Repository.IsValid(DataPath))
		{
			var options = new CloneOptions()
			{
				FetchOptions =
				{
					OnProgress = output =>
					{
						log(output);

						return true;
					},
					OnTransferProgress = progress =>
					{
						if (_lastReceivedObjects == progress.ReceivedObjects)
						{
							return true;
						}

						log($"Received {progress.ReceivedObjects}/{progress.TotalObjects} objects");
						_lastReceivedObjects = progress.ReceivedObjects;

						return true;
					},
				},
				OnCheckoutProgress = (path, steps, totalSteps) =>
				{
					log($"Cloning Tesseract data({steps}/{totalSteps}): {path}");
				}
			};

			log("Cloning Tesseract data...");
			Repository.Clone(DataRepository, DataPath, options);
		}

		return Task.CompletedTask;
	}

	public async Task<string> GetText(byte[] image, string language)
	{
		using var engine = new TesseractEngine(DataPath, language, EngineMode.Default);
		engine.SetVariable("debug_file", "NUL");

		var tempFile = Path.GetTempFileName();
		await File.WriteAllBytesAsync(tempFile, image);

		using var img = Pix.LoadFromFile(tempFile);
		using var page = engine.Process(img);
		var result = await Task.FromResult(page.GetText());

		File.Delete(tempFile);

		return result;
	}
}