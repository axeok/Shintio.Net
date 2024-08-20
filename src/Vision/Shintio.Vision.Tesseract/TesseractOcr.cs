using LibGit2Sharp;
using Shintio.Git.Services;
using Shintio.Vision.Abstractions;
using Tesseract;

namespace Shintio.Vision.Tesseract;

public class TesseractOcr : IOcr
{
	private const string DataPath = "tessdata";
	private const string DataRepository = "https://github.com/tesseract-ocr/tessdata.git";

	public async Task Initialize(Action<string> log)
	{
		var service = new GitService(DataPath, DataRepository, log);

		await service.Initialize();
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