using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
public class HaveFunBottomBarController : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
{
    void OnEnable()
    {
        BottomMenuController.Show();
        BottomMenuController.Get.unselectHome();
        BottomMenuController.Get.unselectWinMoney();
        BottomMenuController.Get.unselectSettings();
        BottomMenuController.Get.selectHaveFun();
    }
}
