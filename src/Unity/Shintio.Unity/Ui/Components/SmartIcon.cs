using System.Collections;
using System.IO;
using Unity.VectorGraphics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Shintio.Unity.Ui.Components
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(SVGImage))]
	public class SmartIcon : MonoBehaviour
	{
		public static readonly string IconsPath = Path.Combine("Assets", "Resources", "Icons", "Smart");

		private SVGImage _svgImage = null!;
		private string _oldIcon = "";

		public string IconName = "";

		private void Awake()
		{
			_svgImage = GetComponent<SVGImage>();
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (_oldIcon == IconName)
			{
				return;
			}

			_oldIcon = IconName;
			StartCoroutine(LoadIcon(IconName));
		}

		private IEnumerator LoadIcon(string iconName)
		{
			var imagePath = GetImagePath(iconName);
			var path = Path.Combine(IconsPath, $"{imagePath}.svg");

			if (File.Exists(path))
			{
				SetImage(path);
				yield break;
			}

			var url = $"https://api.iconify.design/{imagePath}.svg?color=white&width=none&height=none";
			using var www = UnityWebRequest.Get(url);

			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success || www.downloadHandler.text == "404")
			{
				Debug.LogWarning($"Failed to load icon: {iconName}");
				yield break;
			}

			Directory.CreateDirectory(Path.GetDirectoryName(path)!);
			Debug.Log($"Icon {iconName} was saved to {path}");
			File.WriteAllText(path, www.downloadHandler.text);
			AssetDatabase.ImportAsset(path);

			SetImage(path);
		}
#endif

		private void SetImage(string iconPath)
		{
			_svgImage ??= GetComponent<SVGImage>();
			
			var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
			_svgImage.sprite = sprite;
		}

		private string GetImagePath(string iconName)
		{
			return iconName.Replace(":", "/");
		}
	}
}