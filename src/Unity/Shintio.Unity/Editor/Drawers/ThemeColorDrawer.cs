using System.Linq;
using Shintio.Unity.Ui.Attributes;
using Shintio.Unity.Ui.Models;
using UnityEditor;
using UnityEngine;

namespace Shintio.Unity.Editor.Editor.Drawers
{
	[CustomPropertyDrawer(typeof(ThemeColorAttribute))]
	public class ThemeColorDrawer : PropertyDrawer
	{
		private static readonly string[] Options = typeof(Theme)
			.GetFields()
			.Where(f => f.FieldType == typeof(Color))
			.Select(f => f.Name)
			.ToArray();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.String)
			{
				var index = Mathf.Max(0, System.Array.IndexOf(Options, property.stringValue));
				index = EditorGUI.Popup(position, label.text, index, Options);
				property.stringValue = Options[index];
			}
			else
			{
				EditorGUI.PropertyField(position, property, label);
			}
		}
	}
}