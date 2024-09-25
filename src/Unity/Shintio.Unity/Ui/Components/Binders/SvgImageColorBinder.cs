using Shintio.Unity.Ui.Components.Binders.Common;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

namespace Shintio.Unity.Ui.Components.Binders
{
	[RequireComponent(typeof(SVGImage))]
	public class SvgImageColorBinder : ColorBinder
	{
		private SVGImage? _image;

		private void Awake()
		{
			_image = GetComponent<SVGImage>();
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