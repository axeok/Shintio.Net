using System.Linq;
using UnityEngine;

namespace Shintio.Unity.Ui.Models
{
	[CreateAssetMenu(menuName = "Shintio/Ui/Theme")]
	public class Theme : ScriptableObject
	{
		public static Color White = UnityEngine.Color.white;
		public static Color Black = UnityEngine.Color.black;

		public bool IsDark = true;

		#region Colors

		public Color Background = Color(7, 4, 15);
		public Color Surface = Color(27, 27, 39);
		public Color SurfaceVariant = Color(57, 57, 82);
		public Color Disabled = Color(190, 190, 218);

		public Color Primary = Color(111, 81, 227);
		public Color Secondary = Color(78, 138, 238);

		public Color Success = Color(178, 243, 56);
		public Color Info = Color(0, 193, 252);
		public Color Warning = Color(255, 213, 63);
		public Color Error = Color(255, 82, 82);

		public Color Cheerful = Color(161, 215, 135);
		public Color Serious = Color(124, 219, 250);
		public Color Romantic = Color(236, 168, 196);

		public Color OnBackground = Color(255, 255, 255);
		public Color OnSurface = Color(255, 255, 255);
		public Color OnSurfaceVariant = Color(255, 255, 255);
		public Color OnDisabled = Color(255, 255, 255);

		public Color OnPrimary = Color(255, 255, 255);
		public Color OnSecondary = Color(255, 255, 255);

		public Color OnSuccess = Color(0, 0, 0);
		public Color OnInfo = Color(255, 255, 255);
		public Color OnWarning = Color(0, 0, 0);
		public Color OnError = Color(255, 255, 255);

		public Color OnCheerful = Color(0, 0, 0);
		public Color OnSerious = Color(0, 0, 0);
		public Color OnRomantic = Color(0, 0, 0);

		#endregion

		#region Fonts

		#endregion

		public Color GetColor(string colorName)
		{
			var fieldInfo = typeof(Theme)
				.GetFields()
				.Where(f => f.FieldType == typeof(Color))
				.FirstOrDefault(f => f.Name == colorName);

			if (fieldInfo == null)
			{
				return IsDark ? Black : White;
			}

			return (Color)fieldInfo.GetValue(this);
		}

		private static Color Color(int r, int g, int b, int a = 255)
		{
			return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
		}
	}
}