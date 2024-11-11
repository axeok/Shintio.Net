namespace Shintio.Json.Interfaces
{
	public interface IJsonWriter
	{
		public void WriteValue(string value);
		public void WriteRawValue(string json);
	}
}