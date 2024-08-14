namespace Shintio.Vision.Abstractions;

public interface IOcr
{
	public Task<string> GetText(byte[] image, string language);
}