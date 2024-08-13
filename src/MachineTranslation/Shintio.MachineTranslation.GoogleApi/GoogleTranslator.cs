using GTranslatorAPI;
using ITranslator = Shintio.MachineTranslation.Abstractions.ITranslator;

namespace Shintio.MachineTranslation.GoogleApi;

public class GoogleTranslator : ITranslator
{
	private readonly Translator _translator;

	public GoogleTranslator()
	{
		_translator = new Translator();
	}

	public async Task<string?> TranslateAsync(string text, string fromLanguage, string toLanguage)
	{
		try
		{
			var result = await _translator.TranslateAsync(
				fromLanguage,
				toLanguage,
				text
			);

			return result?.TranslatedText;
		}
		catch
		{
			// ignored
		}

		return null;
	}
}