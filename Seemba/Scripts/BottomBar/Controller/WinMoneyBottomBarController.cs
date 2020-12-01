using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class WinMoneyBottomBarController : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnEnable()
        {
            BottomMenuController.Show();
            BottomMenuController.Get.unselectHome();
            BottomMenuController.Get.unselectHaveFun();
            BottomMenuController.Get.unselectSettings();
            BottomMenuController.Get.unselectMarket();
            BottomMenuController.Get.selectWinMoney();
        }
    }
}
