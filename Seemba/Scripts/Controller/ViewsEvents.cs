using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CLSCompliant(false)]
public class ViewsEvents : MonoBehaviour
{


    #region Static
    public static ViewsEvents Get { get { return sInstance; } }
    private static ViewsEvents sInstance;
    #endregion

    #region Script Parameters
    public SignupPresenter Signup;
    public IntroPresenter Intro;
    public MenuPresenter Menu;
    public GameObject Matchmaking;
    public TournamentPresenter Brackets;
    public ResultPresenter ResultPresenter;
    public GameObject ReplayChallenge;
    public LoginPresenter Login;
    public GameObject IdProof;
    public ChargePresenter BankingInfo;
    public ProfilePresenter Profile;
    public PersonalInfoPresenter PersonalInfo;
    public HistoryPresenter History;
    public WithdrawPresenter WithdrawalInfo;
    #endregion

    #region fields
    private GameObject mCurrentMenu;
    private Stack<GameObject> mHistory = new Stack<GameObject>();
    #endregion

    private async void Start()
    {
        sInstance = this;
        mCurrentMenu = Intro.gameObject;
        mHistory.Push(Intro.gameObject);
        //Open the first view
        if (UserManager.Get.getCurrentUserId() != null)
        {
            var user = await UserManager.Get.getUser();
            UserManager.Get.CurrentUser = user;
            UserManager.Get.CurrentAvatarBytesString = await UserManager.Get.getAvatar(user.avatar);
            GoToMenu(Menu.gameObject);
            //ShowScene(Menu.Home);
            ShowScene(Menu.HaveFun);
            PopupManager.Get.PopupsTranslationController.Init();
        }
        else
        {
            GoToMenu(Intro.gameObject);
        }
    }
    public void WalletClick(string last_view)
    {
        ShowScene(Menu.Wallet);
    }
    public void WinMoneyClick()
    {
        BottomMenuController._currentPage = 0;
        BottomMenuController.Show();
        ShowScene(Menu.winMoney);
    }
    public void HaveFunClick()
    {
        BottomMenuController.Show();
        ShowScene(Menu.HaveFun);
    }
    public void HomeClick()
    {
        BottomMenuController.Show();
        ShowScene(Menu.Home);
    }
    public void SettingsClick()
    {
        ProfilLastResultListController.profileSceneOpened = false;
        //BottomMenuController.Show();
        ShowScene(Menu.Settings);
    }
    public void SecurityClick()
    {
        ShowScene(Menu.Security);
    }
    public void PersonalInfoclick()
    {
        GoToMenu(PersonalInfo.gameObject);
    }
    public void LegalClick()
    {
        BottomMenuController.Hide();
        ShowScene(Menu.Legal);
    }
    public void ContactClick()
    {
        BottomMenuController.Hide();
        ShowScene(Menu.Contact);
    }
    public void HistoryClick()
    {
        GoToMenu(History.gameObject);
    }
    public void ShowScene(GameObject gameObject)
    {
        EventsController.last_view = Menu.GetCurrentSubMenu();
        ScrollSnapRect.currentView = gameObject.name;
        Menu.HideAllObjectsExceptOne(gameObject);
    }
    public void Show(GameObject current, GameObject next, bool stackMenu = true)
    {
        if (current != null)
        {
            current.SetActive(false);
        }

        if (next != null)
        {
            next.SetActive(true);
            if (stackMenu)
            {
                mHistory.Push(next);
            }
        }
    }
    public void ShowOverayMenu(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    public void HideOverlayMenu(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    public void ShowLastView()
    {
        ShowScene(EventsController.last_view);
    }
    public void GoBack()
    {
        var current = mHistory.Pop();
        Show(current, mHistory.First(), false);
        mCurrentMenu = mHistory.First();
    }
    public void GoToMenu(GameObject menu, bool popBeforeGoing = false)
    {
        if (menu == mHistory.First() && menu != Intro.gameObject)
        {
            if(!menu.activeSelf)
            {
                menu.SetActive(true);
            }
            return;
        }
        if (popBeforeGoing)
        {
            mHistory.Pop().SetActive(false);
        }
        Debug.LogWarning("Going From " + mHistory.First().name + "To " + menu.name);
        Show(mHistory.First(), menu);
        mCurrentMenu = menu;
    }
    public GameObject GetCurrentMenu()
    {
        return mCurrentMenu;
    }

    public void SelectHomeCoroutineLauncher()
    {
        StartCoroutine(SelectHomeCoroutine());
    }

    public IEnumerator SelectHomeCoroutine()
    {
        GoToMenu(Menu.gameObject);
        yield return new WaitForSeconds(0.2f);
        ShowScene(Menu.Home);
    }
}
