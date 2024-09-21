using System.Drawing;

namespace Shintio.Formats.Interfaces
{
	public interface IPixelImage : IFormatType
	{
		public int Width { get; }
		public int Height { get; }

		public Color? GetPixel(int x, int y);
		public void SetPixel(int x, int y, Color color);

		public void Resize(int width, int height);
	}
}