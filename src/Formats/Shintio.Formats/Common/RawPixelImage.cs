using System.Drawing;
using Shintio.Formats.Interfaces;

namespace Shintio.Formats.Common
{
	public class RawPixelImage : IPixelImage
	{
		private Color[,] _pixels;

		public RawPixelImage(int width, int height)
		{
			_pixels = new Color[width, height];
		}

		public int Width => _pixels.GetLength(0);
		public int Height => _pixels.GetLength(1);

		public Color? GetPixel(int x, int y)
		{
			return _pixels[x, y];
		}

		public void SetPixel(int x, int y, Color color)
		{
			_pixels[x, y] = color;
		}

		public void Resize(int width, int height)
		{
			var oldPixels = _pixels;

			_pixels = new Color[width, height];

			for (var i = 0; i < oldPixels.GetLength(0); i++)
			{
				for (var j = 0; j < oldPixels.GetLength(1); j++)
				{
					_pixels[i, j] = oldPixels[i, j];
				}
			}
		}
	}
}