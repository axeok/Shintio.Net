using R3;
using Shintio.Unity.Ui.Components.Binders;
using Shintio.Unity.Ui.Components.Icons;
using Shintio.Unity.Ui.Components.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace Shintio.Unity.Ui.Components.Buttons
{
	[RequireComponent(typeof(Button))]
	public class NavIconButton : MonoBehaviour
	{
		private Button _button = null!;
		
		private SmartToggleIcon _icon = null!;
		private SvgImageColorBinder _iconColor = null!;
		
		private NavBar _navBar = null!;

		public SerializableReactiveProperty<bool> IsSelected = new(false);

		private void Start()
		{
			_button = GetComponent<Button>();
			
			_icon = GetComponentInChildren<SmartToggleIcon>();
			_iconColor = GetComponentInChildren<SvgImageColorBinder>();

			_navBar = GetComponentInParent<NavBar>();

			IsSelected.Subscribe(Changed);
			_button.onClick.AddListener(Clicked);
		}

		private void Changed(bool value)
		{
			_icon.Toggle.Value = value;
			_iconColor.ColorName = _icon.Toggle.Value ? "Primary" : "Disabled";
		}

		private void Clicked()
		{
			_navBar.Clicked(this);
		}
	}
}