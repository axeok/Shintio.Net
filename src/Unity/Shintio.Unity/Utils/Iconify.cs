using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Shintio.Unity.Utils
{
	public static class Iconify
	{
		public static readonly string IconsPath = "Assets/Resources/Icons/Smart";

		public static async UniTask<Sprite?> LoadIcon(string iconName)
		{
			var imagePath = GetImagePath(iconName);
			var path = $"{IconsPath}/{imagePath}.svg";

			var sprite = LoadSprite(path);
			if (sprite != null)
			{
				return sprite;
			}

#if UNITY_EDITOR
			var url = $"https://api.iconify.design/{imagePath}.svg?color=white&width=none&height=none";
			using var www = UnityWebRequest.Get(url);

			await www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success || www.downloadHandler.text == "404")
			{
				Debug.LogWarning($"Failed to load icon: {iconName}");
				return null;
			}

			Directory.CreateDirectory(Path.GetDirectoryName(path)!);
			await File.WriteAllTextAsync(path, www.downloadHandler.text);

			Debug.Log($"Icon {iconName} was saved to {path}");
			AssetDatabase.ImportAsset(path);

			return LoadSprite(path);
#else
			return null;
#endif
		}

		private static Sprite? LoadSprite(string path)
		{
			path = path.Replace("Assets/Resources/", "").Replace(".svg", "");

			return Resources.Load<Sprite>(path);
		}

		private static string GetImagePath(string iconName)
		{
			return iconName.Replace(":", "/");
		}
	}
}