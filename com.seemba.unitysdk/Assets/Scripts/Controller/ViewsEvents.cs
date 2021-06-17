using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SeembaSDK
{
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
        public ChargePresenter BankingInfo;
        public ProfilePresenter Profile;
        public PersonalInfoPresenter PersonalInfo;
        public HistoryPresenter History;
        public BugReportPresenter BugReport;
        public GiftHistoryController GiftOrderHistory;
        public GameObject Overlay;
        #endregion

        #region fields
        private GameObject mCurrentMenu;
        private Stack<GameObject> mHistory = new Stack<GameObject>();
        #endregion

        #region Shake
        private float accelerometerUpdateInterval = 1.0f / 60.0f;
        // The greater the value of LowPassKernelWidthInSeconds, the slower the
        // filtered value will converge towards current input sample (and vice versa).
        private float lowPassKernelWidthInSeconds = 2.0f;
        // This next parameter is initialized to 2.0 per Apple's recommendation,
        // or at least according to Brady! ;)
        private float shakeDetectionThreshold = 2.0f;
        private float lowPassFilterFactor;
        private Vector3 lowPassValue;
        #endregion

        private void Awake()
        {
            sInstance = this;
        }

        private async void Start()
        {
            if (Seemba.Get.DevelopmentMode)
            {
                lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
                shakeDetectionThreshold *= shakeDetectionThreshold;
                lowPassValue = Input.acceleration;
            }

            if (Seemba.Get.OverlayActivated)
            {
                Overlay.SetActive(true);
            }
            else
            {
                Overlay.SetActive(false);
            }

            mCurrentMenu = Intro.gameObject;
            mHistory.Push(Intro.gameObject);
            PopupManager.Get.PopupsTranslationController.Init();

            if (UserManager.Get.getCurrentUserId() != null)
            {
                var user = await UserManager.Get.getUser();
                UserManager.Get.CurrentUser = user;
                UserManager.Get.CurrentUser.money_credit = user.money_credit;

                SeembaAnalyticsManager.Get.SendUserEvent("Login with Token");
                GoToMenu(Menu.gameObject);
                ShowScene(Menu.Home);
            }
            else
            {
                GoToMenu(Intro.gameObject);
            }
        }
        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("space") || Input.GetKeyDown(KeyCode.A))
            //{
            //    Debug.LogWarning("here"); 
            //    if (!BugReport.gameObject.activeSelf)
            //    {
            //        StartCoroutine(OpenBugReportPanel());
            //    }
            //}

            // BUG BUG BUG 
            //if (Seemba.Get.DevelopmentMode)
            //{
            //    Vector3 acceleration = Input.acceleration;
            //    lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
            //    Vector3 deltaAcceleration = acceleration - lowPassValue;

            //    if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
            //    {
            //        // Perform your "shaking actions" here. If necessary, add suitable
            //        // guards in the if check above to avoid redundant handling during
            //        // the same shake (e.g. a minimum refractory period).
            //        if (!BugReport.gameObject.activeSelf)
            //        {
            //            StartCoroutine(OpenBugReportPanel());
            //        }
            //    }
            //}
        }
        IEnumerator OpenBugReportPanel()
        {
            yield return new WaitForEndOfFrame();
            Texture2D ScreenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            ScreenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            ScreenShot.Apply();
            var sprite = ImagesManager.getSpriteFromTexture(ScreenShot);
            BugReport.gameObject.SetActive(true);
            BugReport.Animator.SetBool("focused", true);
            BugReport.Init(sprite);
            Debug.LogWarning("Shake event .. Opening Bug Panel");
        }

        public void HideBugReportScreen()
        {
            StartCoroutine(HideBugReportPanel());
        }

        IEnumerator HideBugReportPanel()
        {
            BugReport.Animator.SetBool("focused", false);
            yield return new WaitForSeconds(0.5f);
            BugReport.gameObject.SetActive(false);
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
        public void GiftOrdersClick()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Gift Orders Click");
            GoToMenu(GiftOrderHistory.gameObject);
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
        public void GoToMenu(GameObject menu, bool popBeforeGoing = false, bool stackMenu = true)
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
            Show(mHistory.First(), menu, stackMenu);
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
