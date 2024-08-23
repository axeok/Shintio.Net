using System;
using System.Collections.Generic;
using Shintio.Json.Attributes;

namespace Shintio.Essentials.Common
{
	public partial class Color : ValueObject
	{
		public const int DefaultAlpha = 255;

		[JsonConstructor]
		public Color(int r, int g, int b, int a) => (R, G, B, A) = (r, g, b, a);

		public Color(int v, int a) => (R, G, B, A) = (v, v, v, a);

		public Color(int r, int g, int b) => (R, G, B, A) = (r, g, b, DefaultAlpha);
		public Color(Color color, int a) => (R, G, B, A) = (color.R, color.G, color.B, a);
		public Color(Color color) => (R, G, B, A) = (color.R, color.G, color.B, color.A);
		public Color(System.Drawing.Color color) => (R, G, B, A) = (color.R, color.G, color.B, color.A);

		public Color(string hex) : this(System.Drawing.Color.FromArgb(Convert.ToInt32(hex, 16)))
		{
			A = 255;
		}

		public static implicit operator Color(System.Drawing.Color color) => new Color(color);

		public int R { get; private set; } = 0;
		public int G { get; private set; } = 0;
		public int B { get; private set; } = 0;
		public int A { get; private set; } = DefaultAlpha;

		public string ToHex()
		{
			var result = new char[7];
			result[0] = '#';
			result[1] = GetHexNumber((R >> 4) & 15);
			result[2] = GetHexNumber(R & 15);
			result[3] = GetHexNumber((G >> 4) & 15);
			result[4] = GetHexNumber(G & 15);
			result[5] = GetHexNumber((B >> 4) & 15);
			result[6] = GetHexNumber(B & 15);

			return new string(result);
		}

		private static char GetHexNumber(int b)
		{
			return (char)(b > 9 ? 55 + b : 48 + b);
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return R;
			yield return G;
			yield return B;
			yield return A;
		}
	}
}