using UnityEngine;
public class WinMoneyBottomBarController : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        BottomMenuController menuController = BottomMenuController.getInstance();
        menuController.unselectHome();
        menuController.unselectHaveFun();
        menuController.unselectSettings();
        menuController.selectWinMoney();
    }
}
