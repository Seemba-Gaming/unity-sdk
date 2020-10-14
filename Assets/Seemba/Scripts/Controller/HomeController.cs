﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using System.Threading;
using UnityEngine.UI;
public class HomeController : MonoBehaviour
{
    public GameObject Ongoing, LastResult, Home;
    bool notConnected;
    public static bool NoLastResult, NoOngoing;
    string UserId, userToken;
    UserManager userManager;
    // Use this for initialization
    void OnEnable()
    {
        userManager = new UserManager();
        UserId = userManager.getCurrentUserId();
        userToken = userManager.getCurrentSessionToken();
        User user;

        StartCoroutine(SelectHome());

        if (UserManager.CurrentUser==null)
        {
            StartCoroutine(CheckHeader());
        }
        StartCoroutine(CheckOngoingAndLastResult());

        if (UserManager.CurrentUsername == null && !PullToRefresh.pullActivated)
        {
            UnityThreadHelper.CreateThread(() =>
            {

                user = userManager.getUser(UserId, userToken);
                UserManager.CurrentUser = user;
                Byte[] lnByte = null;
                if (user != null)
                {
                    lnByte = userManager.getAvatar(user.avatar);
                }
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {

                    if (user == null)
                    {
                        SceneManager.LoadSceneAsync("ConnectionFailed", LoadSceneMode.Additive);
                    }
                    else
                    {
                        UserManager.CurrentUser = user;
                        PlayerPrefs.SetString("CurrentUsername", user.username);
                        string country_code = userManager.GetGeoLoc();
                        UserManager.CurrentCountryCode = country_code;
                        try
                        {
                            if (country_code != user.country_code)
                            {
                                if (string.IsNullOrEmpty(UserManager.CurrentFlagBytesString))
                                {
                                    string[] attrib = { "country_code" };
                                    string[] values = { country_code };
                                    userManager.UpdateUserByField(UserId, userToken, attrib, values);
                                    UserManager.CurrentFlagBytesString = userManager.GetFlagByte(country_code);
                                    PlayerPrefs.SetString("CurrentFlagBytesString", UserManager.CurrentFlagBytesString);
                                }
                                else
                                {
                                    UserManager.CurrentFlagBytesString = PlayerPrefs.GetString("CurrentFlagBytesString");
                                    Debug.Log("CurrentFlagBytesString");
                                }
                            }
                            else
                            {
                                UserManager.CurrentFlagBytesString = userManager.GetFlagByte(country_code);
                                PlayerPrefs.SetString("CurrentFlagBytesString", UserManager.CurrentFlagBytesString);
                            }
                        }
                        catch (Exception e)
                        {
                            string[] attrib = { "country_code" };
                            string[] values = { country_code };
                            userManager.UpdateUserByField(UserId, userToken, attrib, values);
                            UserManager.CurrentFlagBytesString = userManager.GetFlagByte(country_code);
                            PlayerPrefs.SetString("CurrentFlagBytesString", UserManager.CurrentFlagBytesString);
                        }
                        try
                        {
                            UserManager.CurrentUser.bubble_credit = user.bubble_credit;
                            UserManager.CurrentUser.money_credit = user.money_credit;
                        }
                        catch (NullReferenceException ex)
                        {
                        }
                        if (user.money_credit > 0)
                        {
                            UserManager.CurrentProLabel = true;
                        }
                        else
                        {
                            UserManager.CurrentUser.money_credit = 0.00f;
                        }
                        Text PlayerWater = GameObject.Find("virtual_money").GetComponent<Text>();
                        PlayerWater.text = UserManager.CurrentUser.bubble_credit.ToString();
                        UserManager.CurrentUsername = user.username;
                        UserManager.CurrentAvatarURL = user.avatar;
                        UserManager.CurrentAvatarBytesString = ImagesManager.getSpriteFromBytes(lnByte);
                        UserManager.CurrentUsername = user.username;
                        EncartPlayerPresenter.Init();
                    }
                });
            });
        }
        else
        {
            EncartPlayerPresenter.Init();
        }
    }
    IEnumerator CheckHeader()
    {
        float timer = 0;
        bool failed = false;
        while (UserManager.CurrentUser==null)
        {
            if (timer > 12) { failed = true; break; }
            timer += Time.deltaTime;
            yield return null;
        }
        if (failed)
        {
            ConnectivityController.CURRENT_ACTION = ConnectivityController.HOME_ACTION;
            SceneManager.LoadSceneAsync("ConnectionFailed", LoadSceneMode.Additive);
        }
    }
    IEnumerator SelectHome()
    {
        notConnected = false;
        float timer = 0;

        while (BottomMenuController.getInstance() == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        ViewsEvents v = new ViewsEvents();
        BottomMenuController bottomMenu = BottomMenuController.getInstance();
        BottomMenuController.Show();

        bottomMenu.unselectSettings();
        bottomMenu.unselectHaveFun();
        bottomMenu.unselectWinMoney();
        bottomMenu.selectHome();
        v.HomeClick();
    }
    IEnumerator CheckOngoingAndLastResult()
    {
        notConnected = false;
        float timer = 0;
        while (Ongoing.transform.childCount == 0 && LastResult.transform.childCount == 0)
        {
            if (timer > 2)
            {
                if (GameObject.Find("LoadingHomeBloc").transform.localScale == Vector3.zero)
                {
                    GameObject.Find("LoadingHomeBloc").transform.localScale = Vector3.one;
                    GameObject.Find("reconnectMessage").transform.localScale = Vector3.one;
                }
                InvokeRepeating("tryingtoreconnect", 0f, 1f); ;
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }
    void tryingtoreconnect()
    {
        StartCoroutine(checkInternetConnection());
    }
    public IEnumerator checkInternetConnection()
    {
        WWW www = new WWW("https://www.google.fr");
        float timer = 0;
        while (!www.isDone)
        {
            if (timer > 5) { notConnected = true; break; }
            timer += Time.deltaTime;
            yield return null;
        }
        if (notConnected)
        {
            www.Dispose();
        }
        else
        {
            if (www.error == null)
            {
                CancelInvoke();
                GameObject.Find("reconnectMessage").transform.localScale = Vector3.zero;
                InvokeRepeating("unloadLoader", 0f, 0.5f);
            }
        }
    }
    public void unloadLoader()
    {
        if (Ongoing.transform.childCount != 0 || LastResult.transform.childCount != 0 || (NoOngoing == true && NoLastResult == true))
        {
            try
            {
                GameObject.Find("LoadingHomeBloc").transform.localScale = Vector3.zero;
            }
            catch (NullReferenceException ex) { }
            CancelInvoke();
        }
    }
}
