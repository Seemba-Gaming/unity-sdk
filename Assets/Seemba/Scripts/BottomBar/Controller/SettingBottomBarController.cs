using UnityEngine;
public class SettingBottomBarController : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        BottomMenuController menuController = BottomMenuController.getInstance();
        menuController.unselectHome();
        menuController.unselectHaveFun();
        menuController.unselectWinMoney();
        menuController.selectSettings();
    }
}
