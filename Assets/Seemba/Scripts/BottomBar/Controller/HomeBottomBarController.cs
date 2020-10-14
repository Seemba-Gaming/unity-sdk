using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HomeBottomBarController : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        try       {
            BottomMenuController menuController = BottomMenuController.getInstance();
            menuController.unselectHaveFun();
            menuController.unselectWinMoney();
            menuController.unselectSettings();
            menuController.selectHome();
        }
        catch (NullReferenceException ex) {
            };
    }
}
