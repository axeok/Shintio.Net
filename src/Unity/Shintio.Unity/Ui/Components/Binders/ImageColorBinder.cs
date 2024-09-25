using Shintio.Unity.Ui.Components.Binders.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Shintio.Unity.Ui.Components.Binders
{
	[RequireComponent(typeof(Image))]
	public class ImageColorBinder : ColorBinder
	{
		private Image? _image;

		private void Awake()
		{
			_image = GetComponent<Image>();
		}

		protected override void SetColor(Color color)
		{
			if (_image != null)
			{
				_image.color = color;
			}
		}
	}
}