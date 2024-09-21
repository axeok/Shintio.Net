using Shintio.Essentials.Common;
using Shintio.Math.Utils;

namespace Shintio.Math.Extensions
{
	public class ColorExtensions
	{
		public static Color Lerp(Color a, Color b, float t)
		{
			return LerpUnclamped(a, b, Mathf.Clamp01(t));
		}

		public static Color LerpUnclamped(Color a, Color b, float t)
		{
			return new Color(
				(int)(a.R + (b.R - a.R) * t),
				(int)(a.G + (b.G - a.G) * t),
				(int)(a.B + (b.B - a.B) * t),
				(int)(a.A + (b.A - a.A) * t)
			);
		}
	}
}