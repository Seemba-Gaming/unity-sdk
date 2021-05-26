using UnityEngine;

namespace SeembaSDK
{
    public class HaveFunBottomBarController : MonoBehaviour
    {
        void OnEnable()
        {
            BottomMenuController.Show();
            BottomMenuController.Get.unselectHome();
            BottomMenuController.Get.unselectWinMoney();
            BottomMenuController.Get.unselectSettings();
            BottomMenuController.Get.unselectMarket();
            BottomMenuController.Get.unselectLeaderboard();
            BottomMenuController.Get.unselectAchievements();
            BottomMenuController.Get.selectHaveFun();
        }
    }
}
