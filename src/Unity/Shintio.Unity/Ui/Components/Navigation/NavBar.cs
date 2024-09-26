using System;
using R3;
using Shintio.Unity.Ui.Components.Buttons;
using UnityEngine;

namespace Shintio.Unity.Ui.Components.Navigation
{
	public class NavBar : MonoBehaviour
	{
		public SerializableReactiveProperty<int> CurrentIndex = new(0);

		private void Start()
		{
			CurrentIndex.Subscribe(UpdateIndex);
		}

		public void Clicked(NavIconButton button)
		{
			var items = GetItems();
			var index = Array.IndexOf(items, button);

			if (index != -1)
			{
				CurrentIndex.Value = index;
			}
		}

		private void UpdateIndex(int index)
		{
			var items = GetItems();
			index = Mathf.Clamp(index, 0, items.Length - 1);

			if (CurrentIndex.Value != index)
			{
				CurrentIndex.Value = index;
			}

			for (var i = 0; i < items.Length; i++)
			{
				items[i].IsSelected.Value = i == index;
			}
		}

		private NavIconButton[] GetItems()
		{
			return GetComponentsInChildren<NavIconButton>();
		}
	}
}