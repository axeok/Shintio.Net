using System;
using Shintio.Unity.Ui.Attributes;
using Shintio.Unity.Ui.Models;
using UnityEngine;

namespace Shintio.Unity.Ui.Components.Binders.Common
{
	public abstract class ColorBinder : ThemeValueBinder
	{
		private string _currentColorName = "Primary";

		[ThemeColor] public string ColorName = "Primary";
		public float Alpha = 1;

		protected abstract void SetColor(Color color);

		protected override void ApplyTheme(Theme theme)
		{
			SetColor(GetColor(theme));
		}

		private void FixedUpdate()
		{
			if (_currentColorName == ColorName)
			{
				return;
			}

			_currentColorName = ColorName;
			SetColor(GetColor(ThemeProvider.Instance.CurrentTheme));
		}

		private Color GetColor(Theme theme)
		{
			var color = theme.GetColor(ColorName);
			return new Color(color.r, color.g, color.b, Alpha);
		}
	}
}