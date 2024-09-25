using Shintio.Unity.Ui.Models;
using UnityEngine;

namespace Shintio.Unity.Ui.Components.Binders.Common
{
	[ExecuteAlways]
	public abstract class ThemeValueBinder : MonoBehaviour
	{
		private void Start()
		{
			ThemeProvider.ThemeChanged.AddListener(OnThemeChanged);
			ApplyTheme(ThemeProvider.Instance.CurrentTheme);
		}

		protected abstract void ApplyTheme(Theme theme);

		private void OnThemeChanged(Theme theme)
		{
			ApplyTheme(theme);
		}

#if UNITY_EDITOR
		private void Update()
		{
			ApplyTheme(ThemeProvider.Instance.CurrentTheme);
		}
#endif
	}
}