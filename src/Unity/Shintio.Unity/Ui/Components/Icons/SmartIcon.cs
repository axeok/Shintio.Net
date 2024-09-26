using Cysharp.Threading.Tasks;
using R3;
using Shintio.Unity.Utils;
using Unity.VectorGraphics;
using UnityEngine;

namespace Shintio.Unity.Ui.Components.Icons
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(SVGImage))]
	public class SmartIcon : MonoBehaviour
	{
		private SVGImage _svgImage = null!;

		private string _oldIcon = "";

		public SerializableReactiveProperty<string> IconName = new();

		private void Awake()
		{
			_svgImage = GetComponent<SVGImage>();

			IconName.Subscribe(_ => TryUpdateIcon());
		}

#if UNITY_EDITOR
		private async UniTaskVoid Update()
		{
			await TryUpdateIcon();
		}
#endif

		private async UniTask TryUpdateIcon()
		{
			if (_oldIcon == IconName.Value)
			{
				return;
			}

			_oldIcon = IconName.Value;

			var sprite = await Iconify.LoadIcon(IconName.Value);
			if (!sprite)
			{
				return;
			}

			_svgImage ??= GetComponent<SVGImage>();
			_svgImage.sprite = sprite;
		}
	}
}