using System;
using System.Collections.Generic;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class SettingsPresenter : MonoBehaviour
    {
        #region Script Parameters
        public GameObject Settings;
        public GameObject Account;
        public GameObject Withdraw;
        public GameObject Credit;
        public GameObject HelpCenter;
        public WalletScript SettingsWallet;
        #endregion

        #region Fields
        private List<GameObject> mSubMenus = new List<GameObject>();
        private GameObject mCurrentSubMenu;
        private GameObject mPreviousSubMenu;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            mCurrentSubMenu = Settings;
            mPreviousSubMenu = Settings;
            mSubMenus.Add(Settings);
            mSubMenus.Add(Account);
            mSubMenus.Add(Withdraw);
            mSubMenus.Add(Credit);
            mSubMenus.Add(HelpCenter);
        }
        #endregion

        #region Methods
        public void HideAllObjectsExceptOne(GameObject menuGameObject)
        {
            foreach (GameObject gameobject in mSubMenus)
            {
                if (gameobject.name != menuGameObject.name)
                {
                    gameobject.SetActive(false);
                }
                menuGameObject.SetActive(true);
                mCurrentSubMenu = menuGameObject;
            }

            if (mCurrentSubMenu == Settings)
            {
                ViewsEvents.Get.Menu.ShowHeader(true);
                ViewsEvents.Get.Menu.ShowBottomBar(true);
            }
            else
            {
                ViewsEvents.Get.Menu.ShowBottomBar(false);
                ViewsEvents.Get.Menu.ShowHeader(false);
            }
        }
        public void ShowLastView()
        {
            mCurrentSubMenu.SetActive(false);
            mPreviousSubMenu.SetActive(true);
            mCurrentSubMenu = mPreviousSubMenu;
            if(mCurrentSubMenu == Settings)
            {
                ViewsEvents.Get.Menu.ShowHeader(true);
                ViewsEvents.Get.Menu.ShowBottomBar(true);
            }
            else
            {
                ViewsEvents.Get.Menu.ShowBottomBar(false);
                ViewsEvents.Get.Menu.ShowHeader(false);
            }
        }

        public GameObject GetCurrentSubMenu()
        {
            return mCurrentSubMenu;
        }

        public void Show(GameObject gameObject)
        {
            mPreviousSubMenu = mCurrentSubMenu;
            mCurrentSubMenu = gameObject;
            HideAllObjectsExceptOne(gameObject);
        }
        #endregion

    }
}
