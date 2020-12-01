using UnityEngine;
using UnityEngine.UI;
using System;

namespace SeembaSDK
{
	[CLSCompliant(false)]
	public class EncartPlayerPresenter : MonoBehaviour
	{
		public static Text PlayerUsername, PlayerMoney, PlayerWater;
		public static Image PlayerAvatar, PlayerAvatarDialog, PlayerFlag;
		public Image avatar;
		public Image EncartMoney;
		private static string usernameBackup;

		public static void Init()
		{
			foreach (var gameObj in FindObjectsOfType(typeof(Text)) as Text[])
			{
				if (gameObj.name == "Text_name")
				{
					if (!string.IsNullOrEmpty(UserManager.Get.CurrentUser.username))
					{
						gameObj.text = UserManager.Get.CurrentUser.username;
					}
					else
					{
						gameObj.text = PlayerPrefs.GetString("CurrentUsername");
					}
				}
			}
		}
	}
}
