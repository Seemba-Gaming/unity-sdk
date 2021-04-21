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
        public GlowImage glow;

        public async System.Threading.Tasks.Task InitAsync(LeaderBoardItem item)
        {
            if(!string.IsNullOrEmpty(item.user.avatar))
            {
                Avatar.sprite = await UserManager.Get.getAvatar(item.user.avatar);
            }
            Username.text = item.user.username;
            if(item.user._id == UserManager.Get.CurrentUser._id)
            {
                glow.glowSize = 4;
            }
            Score.text = item.score.ToString();
        }
    }
}
