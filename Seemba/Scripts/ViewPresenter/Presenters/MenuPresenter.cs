﻿using System.Collections.Generic;
using UnityEngine;

public class MenuPresenter : MonoBehaviour
{

    #region Script Parameters
    public GameObject Home;
    public GameObject HaveFun;
    public GameObject winMoney;
    public GameObject Settings;
    public GameObject Contact;
    public GameObject Security;
    public GameObject Legal;
    public GameObject Wallet;
    public GameObject BottomBar;
    public GameObject Header;
    public ScrollSnapRect ScrollSnap;
    #endregion

    #region Fields
    private List<GameObject> mSubMenus = new List<GameObject>();
    private GameObject mCurrentSubMenu;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        mCurrentSubMenu = Home;
        mSubMenus.Add(Home);
        mSubMenus.Add(HaveFun);
        mSubMenus.Add(winMoney);
        mSubMenus.Add(Settings);
        mSubMenus.Add(Contact);
        mSubMenus.Add(Security);
        mSubMenus.Add(Legal);
        mSubMenus.Add(Wallet);
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
    }

    public void ShowBottomBar(bool show)
    {
        BottomBar.SetActive(show);
    }

    public void ShowHeader(bool show)
    {
        Header.SetActive(show);
    }

    public GameObject GetCurrentSubMenu()
    {
        return mCurrentSubMenu;
    }
    #endregion
}