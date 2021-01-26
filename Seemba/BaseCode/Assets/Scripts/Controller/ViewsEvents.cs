using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SeembaSDK
{
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
        public WithdrawalInfoPresenter WithdrawalInfo;
        public LeaderboardPresenter Leaderboard;

        public GameObject Overlay;
        #endregion

        #region fields
        private GameObject mCurrentMenu;
        private Stack<GameObject> mHistory = new Stack<GameObject>();
        #endregion

        private void Awake()
        {
            sInstance = this;
        }

        private async void Start()
        {
            if(Seemba.Get.OverlayActivated)
            {
                Overlay.SetActive(true);
            }
            else
            {
                Overlay.SetActive(false);
            }

            mCurrentMenu = Intro.gameObject;
            mHistory.Push(Intro.gameObject);

            if (UserManager.Get.getCurrentUserId() != null)
            {
                var user = await UserManager.Get.getUser();
                UserManager.Get.CurrentUser = user;
                UserManager.Get.CurrentAvatarBytesString = await UserManager.Get.getAvatar(user.avatar);
                SeembaAnalyticsManager.Get.SendUserEvent("Login with Token");
                GoToMenu(Menu.gameObject);
                ShowScene(Menu.Home);
                //ShowScene(Menu.HaveFun);
                PopupManager.Get.PopupsTranslationController.Init();
            }
            else
            {
                GoToMenu(Intro.gameObject);
            }
        }
        public void WalletClick(string last_view)
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Wallet Click");
            ShowScene(Menu.Wallet);
            BottomMenuController.Hide();
        }
        public void WinMoneyClick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Win Money Click");
            BottomMenuController._currentPage = 0;
            BottomMenuController.Show();
            ShowScene(Menu.winMoney);
        }
        public void HaveFunClick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Have fun Click");
            BottomMenuController.Show();
            ShowScene(Menu.HaveFun);
        }
        public void HomeClick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Home Click");
            BottomMenuController.Show();
            ShowScene(Menu.Home);
        }
        public void MarketClick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Market Click");
            BottomMenuController.Show();
            ShowScene(Menu.Market);
        }
        public void LeaderboardClick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Leaderboard Click");
            BottomMenuController.Show();
            ShowScene(Menu.Leaderboard);
        }
        public void AchievementsClick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Achivements Click");
            BottomMenuController.Show();
            ShowScene(Menu.Achievements);
        }
        public void SettingsClick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Settings Click");
            ProfilLastResultListController.profileSceneOpened = false;
            //BottomMenuController.Show();
            ShowScene(Menu.Settings);
        }
        public void SecurityClick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Security Click");
            ShowScene(Menu.Security);
        }
        public void PersonalInfoclick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Personal Info Click");
            GoToMenu(PersonalInfo.gameObject);
        }
        public void LegalClick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Legal Click");
            BottomMenuController.Hide();
            ShowScene(Menu.Legal);
        }
        public void ContactClick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Contact Click");
            BottomMenuController.Hide();
            ShowScene(Menu.Contact);
        }
        public void HistoryClick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("History Click");
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
            SeembaAnalyticsManager.Get.SendUserEvent("Back Button");
            var current = mHistory.Pop();
            Show(current, mHistory.First(), false);
            mCurrentMenu = mHistory.First();
        }
        public void GoToMenu(GameObject menu, bool popBeforeGoing = false)
        {
            if (menu == mHistory.First() && menu != Intro.gameObject)
            {
                if (!menu.activeSelf)
                {
                    menu.SetActive(true);
                }
                return;
            }
            if (popBeforeGoing)
            {
                mHistory.Pop().SetActive(false);
            }
            //Debug.LogWarning("Going From " + mHistory.First().name + "To " + menu.name);
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
}
