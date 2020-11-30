using System;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class MarketBottomBarController : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnEnable()
        {
            BottomMenuController.Show();
            BottomMenuController.Get.unselectHaveFun();
            BottomMenuController.Get.unselectWinMoney();
            BottomMenuController.Get.unselectSettings();
            BottomMenuController.Get.unselectHome();
            BottomMenuController.Get.SelectMarket();
        }
    }
}