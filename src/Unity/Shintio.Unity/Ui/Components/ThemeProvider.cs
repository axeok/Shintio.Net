using Shintio.Unity.Ui.Models;
using UnityEngine;
using UnityEngine.Events;

namespace Shintio.Unity.Ui.Components
{
	[ExecuteAlways]
	public class ThemeProvider : MonoBehaviour
	{
		public static readonly UnityEvent<Theme> ThemeChanged = new();

		private static ThemeProvider? _instance;

		public static ThemeProvider Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<ThemeProvider>() ??
					            new GameObject(nameof(ThemeProvider)).AddComponent<ThemeProvider>();
					
					_instance.CurrentTheme = ScriptableObject.CreateInstance<Theme>();
				}

				return _instance;
			}
		}

		[field: SerializeField]
		public Theme CurrentTheme { get; private set; } = null!;

		public void Awake()
		{
			if (_instance == null)
			{
				_instance = this;

				if (Application.IsPlaying(gameObject))
				{
					DontDestroyOnLoad(this);
				}
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public void SetTheme(Theme theme)
		{
			CurrentTheme = theme;
			ThemeChanged.Invoke(CurrentTheme);
		}
	}
}