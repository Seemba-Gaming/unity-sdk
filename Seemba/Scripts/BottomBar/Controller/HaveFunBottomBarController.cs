using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
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
