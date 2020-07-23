using UnityEngine;
public class HaveFunBottomBarController : MonoBehaviour
{
    void OnEnable()
    {
        BottomMenuController menuController = BottomMenuController.getInstance();
        menuController.unselectHome();
        menuController.unselectWinMoney();
        menuController.unselectSettings();
        menuController.selectHaveFun();
    }
}
