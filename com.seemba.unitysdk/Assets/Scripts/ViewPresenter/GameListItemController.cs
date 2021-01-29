using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace SeembaSDK
{
	[CLSCompliant(false)]
	public class GameListItemController : MonoBehaviour
	{
		public GameObject ContentPanel;
		public Text title;
		public Image icon;
		public Button Download;

		public IEnumerator ShowIcon(string url)
		{
			UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
			yield return www.SendWebRequest();
			var texture = DownloadHandlerTexture.GetContent(www);
			icon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
		}
	}
}
