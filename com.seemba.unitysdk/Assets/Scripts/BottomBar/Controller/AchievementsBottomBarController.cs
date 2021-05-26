using UnityEngine;

namespace SeembaSDK
{
    public class AchievementsBottomBarController : MonoBehaviour
    {
        void OnEnable()
        {
            BottomMenuController.Show();
            BottomMenuController.Get.unselectHaveFun();
            BottomMenuController.Get.unselectWinMoney();
            BottomMenuController.Get.unselectSettings();
            BottomMenuController.Get.unselectHome();
            BottomMenuController.Get.unselectLeaderboard();
            BottomMenuController.Get.unselectMarket();
            BottomMenuController.Get.SelectAchievements();
        }
    }
}
