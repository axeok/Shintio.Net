using Shintio.Unity.Ui.Components.Binders.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Shintio.Unity.Ui.Components.Binders
{
	public class CameraBackgroundColorBinder : ColorBinder
	{
		protected override void SetColor(Color color)
		{
			var component = GetComponent<Camera>();
			if (component != null)
			{
				component.backgroundColor = color;
			}
		}
	}
}