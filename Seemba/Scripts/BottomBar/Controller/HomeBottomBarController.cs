using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HomeBottomBarController : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        BottomMenuController.Show();
        BottomMenuController.Get.unselectHaveFun();
        BottomMenuController.Get.unselectWinMoney();
        BottomMenuController.Get.unselectSettings();
        BottomMenuController.Get.selectHome();
    }
}
