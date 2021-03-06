using System;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class BottomMenuController : MonoBehaviour
    {
        #region Static
        public static BottomMenuController Get { get { return sInstance; } }
        private static BottomMenuController sInstance;
        #endregion
        private static BottomMenuController _Instance;
        public GameObject home, haveFun, WinMoney, settings, Market, Leaderboard, Achievements;
        public static int _currentPage = 0;
        public string selectedMenu = "Home";
        void Awake()
        {
            sInstance = this;
        }
        private void Start()
        {
            InvokeRepeating("hideOrShowToolbar", 0f, 0.01f);
        }
        public string menu
        {
            get { return selectedMenu; }
            set { selectedMenu = value; }
        }
        public static void Hide()
        {
            ViewsEvents.Get.Menu.BottomBar.SetActive(false);
            ViewsEvents.Get.Menu.Header.SetActive(false);
        }
        public static void Show()
        {
            ViewsEvents.Get.Menu.Header.SetActive(true);
            ViewsEvents.Get.Menu.BottomBar.SetActive(true);
        }
        public void hideOrShowToolbar()
        {
            if ((ScrollSnapRect.currentView == "Settings" && ScrollSnapRect.focusedPage != 0) || ScrollSnapRect.currentView == "Wallet")
            {
                Hide();
            }
            else
            {
                if (ScrollSnapRect.currentView != "WinMoney" && ScrollSnapRect.currentView != "HaveFun")
                {

                }
            }
        }
        public void selectHome()
        {
            home.GetComponent<Animator>().SetBool("focused", true);
            menu = "Home";
        }
        public void unselectHome()
        {
            if (menu == "Home") { home.GetComponent<Animator>().SetBool("focused", false); }
        }
        public void selectHaveFun()
        {
            haveFun.GetComponent<Animator>().SetBool("focused", true);
            menu = "HaveFun";
        }
        public void unselectHaveFun()
        {
            if (menu == "HaveFun") { haveFun.GetComponent<Animator>().SetBool("focused", false); }
        }
        public void selectWinMoney()
        {
            WinMoney.GetComponent<Animator>().SetBool("focused", true);
            menu = "WinMoney";
        }
        public void unselectWinMoney()
        {
            if (menu == "WinMoney") { WinMoney.GetComponent<Animator>().SetBool("focused", false); }
        }
        public void selectSettings()
        {
            settings.GetComponent<Animator>().SetBool("focused", true);
            menu = "Settings";
        }
        public void unselectSettings()
        {
            if (menu == "Settings") { settings.GetComponent<Animator>().SetBool("focused", false); }
        }
        public void SelectMarket()
        {
            Market.GetComponent<Animator>().SetBool("focused", true);
            menu = "Market";
        }
        public void unselectMarket()
        {
            if (menu == "Market") { Market.GetComponent<Animator>().SetBool("focused", false); }
        }
        public void SelecLeaderboard()
        {
            Leaderboard.GetComponent<Animator>().SetBool("focused", true);
            menu = "Leaderboard";
        }
        public void unselectLeaderboard()
        {
            if (menu == "Leaderboard") { Leaderboard.GetComponent<Animator>().SetBool("focused", false); }
        }
        
        public void SelectAchievements()
        {
            Achievements.GetComponent<Animator>().SetBool("focused", true);
            menu = "Achievements";
        }
        public void unselectAchievements()
        {
            if (menu == "Achievements") { Achievements.GetComponent<Animator>().SetBool("focused", false); }
        }
    }
}