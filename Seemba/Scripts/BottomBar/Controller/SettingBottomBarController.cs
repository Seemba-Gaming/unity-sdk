using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class SettingBottomBarController : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnEnable()
        {
            BottomMenuController.Get.unselectHome();
            BottomMenuController.Get.unselectHaveFun();
            BottomMenuController.Get.unselectWinMoney();
            BottomMenuController.Get.unselectMarket();
            BottomMenuController.Get.selectSettings();
        }
    }
}
