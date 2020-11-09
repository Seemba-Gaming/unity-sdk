using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
public class WinMoneyBottomBarController : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
{
    // Start is called before the first frame update
    void OnEnable()
    {
        BottomMenuController.Show();
        BottomMenuController.Get.unselectHome();
        BottomMenuController.Get.unselectHaveFun();
        BottomMenuController.Get.unselectSettings();
        BottomMenuController.Get.selectWinMoney();
    }
}
