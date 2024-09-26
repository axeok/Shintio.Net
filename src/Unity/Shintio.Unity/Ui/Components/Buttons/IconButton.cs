using Shintio.Unity.Ui.Components.Icons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shintio.Unity.Ui.Components.Buttons
{
	[RequireComponent(typeof(Button))]
	public class IconButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		private SmartToggleIcon _icon = null!;

		private void Start()
		{
			_icon = GetComponentInChildren<SmartToggleIcon>();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_icon.Toggle.Value = false;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			_icon.Toggle.Value = true;
		}
	}
}