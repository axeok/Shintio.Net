using Cysharp.Threading.Tasks;
using R3;
using Shintio.Unity.Utils;
using Unity.VectorGraphics;
using UnityEngine;

namespace Shintio.Unity.Ui.Components.Icons
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(SVGImage))]
	public class SmartToggleIcon : MonoBehaviour
	{
		private SVGImage _svgImage = null!;

		private bool _oldToggle = true;
		private string _oldIcon = "";

		public SerializableReactiveProperty<bool> Toggle = new(true);
		public SerializableReactiveProperty<string> EnabledIcon = new();
		public SerializableReactiveProperty<string> DisabledIcon = new();

		private void Awake()
		{
			_svgImage = GetComponent<SVGImage>();

			Toggle.Subscribe(_ => TryUpdateIcon());
			EnabledIcon.Subscribe(_ => TryUpdateIcon());
			DisabledIcon.Subscribe(_ => TryUpdateIcon());
		}

#if UNITY_EDITOR
		private async UniTaskVoid Update()
		{
			await TryUpdateIcon();
		}
#endif

		private async UniTask TryUpdateIcon()
		{
			var newIcon = (Toggle.Value ? EnabledIcon.Value : DisabledIcon.Value);
			
			if (_oldToggle == Toggle.Value && _oldIcon == newIcon)
			{
				return;
			}

			_oldToggle = Toggle.Value;
			_oldIcon = newIcon; 

			var sprite = await Iconify.LoadIcon(newIcon);
			if (!sprite)
			{
				return;
			}

			_svgImage ??= GetComponent<SVGImage>();
			_svgImage.sprite = sprite;
		}
	}
}