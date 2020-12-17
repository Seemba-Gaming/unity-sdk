using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class LeaderboardItemController : MonoBehaviour
    {
        public Image Avatar;
        public Text Username;
        public Text Score;

        public async System.Threading.Tasks.Task InitAsync(LeaderBoardItem item)
        {
            Avatar.sprite = await UserManager.Get.getAvatar(item.user.avatar);
            Username.text = item.user.username;
            Score.text = item.score.ToString();
        }
    }
}
