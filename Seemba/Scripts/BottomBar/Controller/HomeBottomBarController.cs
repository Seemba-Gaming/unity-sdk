using System;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class HomeBottomBarController : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnEnable()
        {
            BottomMenuController.Show();
            BottomMenuController.Get.unselectHaveFun();
            BottomMenuController.Get.unselectWinMoney();
            BottomMenuController.Get.unselectMarket();
            BottomMenuController.Get.unselectSettings();
            BottomMenuController.Get.selectHome();
        }
    }
}
