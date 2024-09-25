using Shintio.Unity.Ui.Attributes;
using Shintio.Unity.Ui.Models;
using UnityEngine;

namespace Shintio.Unity.Ui.Components.Binders.Common
{
	public abstract class ColorBinder : ThemeValueBinder
	{
		[ThemeColor] public string ColorName = "Primary";
		public float Alpha = 1;

		protected abstract void SetColor(Color color);

		protected override void ApplyTheme(Theme theme)
		{
			var color = theme.GetColor(ColorName);

			SetColor(new Color(color.r, color.g, color.b, Alpha));
		}
	}
}