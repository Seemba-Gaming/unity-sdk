using System;
using UnityEngine;

namespace SeembaSDK
{
    public class LeaderboardBottomBarController : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnEnable()
        {
            BottomMenuController.Show();
            BottomMenuController.Get.unselectHaveFun();
            BottomMenuController.Get.unselectWinMoney();
            BottomMenuController.Get.unselectSettings();
            BottomMenuController.Get.unselectHome();
            BottomMenuController.Get.unselectMarket();
            BottomMenuController.Get.unselectAchievements();
            BottomMenuController.Get.SelecLeaderboard();
        }
    }
}
