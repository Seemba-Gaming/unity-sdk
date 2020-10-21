using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using System.Net;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine.SceneManagement;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
public class WithdrawPresenter : MonoBehaviour
{
    public static float WithdrawMoney;

    public Text balance;
    public Text currency;
    public Button withdraw;
    public InputField amount;

    UserManager um = new UserManager();
    WithdrawManager withdrawManager = new WithdrawManager();
    WithdrawController controller;

    string userId;
    string token;
    // Update is called once per frame
    void Update()
    {
        balance.text = UserManager.CurrentUser.money_credit.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
    }
    // Use this for initialization

    private void Start()
    {
        withdraw.onClick.AddListener(delegate
        {
            CheckAndWithdraw();
        });
        amount.onValueChanged.AddListener(delegate
        {
            if (string.IsNullOrEmpty(amount.text))
            {
                withdraw.interactable = false;
                currency.text = "";
            }
            else
            {
                currency.text = CurrencyManager.CURRENT_CURRENCY;
                if ((float.Parse(amount.text, CultureInfo.InvariantCulture) > 0) && (float.Parse(amount.text, CultureInfo.InvariantCulture) <= UserManager.CurrentUser.money_credit))
                {
                    withdraw.interactable = true;
                }
                else
                {
                    withdraw.interactable = false;
                }
            }
        });
    }
    void OnEnable()
    {

        controller = new WithdrawController();
        userId = um.getCurrentUserId();
        token = um.getCurrentSessionToken();
        User user = null;
        var triggerOneTime = 0;

        //initiate balance
        UnityThreadHelper.CreateThread(() =>
        {
            user = um.getUser(userId, token);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                balance.text = user.money_credit.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
            });
        });
       
    }
    private async void CheckAndWithdraw()
    {

        if (CountryController.checkCountry(UserManager.CurrentCountryCode) == true)
        {

            SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
            JSONNode accountStatus = await withdrawManager.accountVerificationStatus(token);
            SceneManager.UnloadScene("Loader");

            if (accountStatus["payouts_enabled"].AsBool)
            {

                //Withdraw
                controller.Withdraw(float.Parse(amount.text, CultureInfo.InvariantCulture));
            }
            else
            {
                EventsController nbs = new EventsController();
                nbs.ShowPopupError("popupMissingInfo");
            }

        }
        else
        {
            EventsController nbs = new EventsController();
            nbs.ShowPopupError("popupProhibitedLocationWithdraw");
        }
    }

    public bool InformationAlreadyExist(User user)
    {
        return !string.IsNullOrEmpty(user.firstname) && !string.IsNullOrEmpty(user.lastname) && !string.IsNullOrEmpty(user.birthday) && !string.IsNullOrEmpty(user.adress) && !string.IsNullOrEmpty(user.city) && !string.IsNullOrEmpty(user.zipcode) && !string.IsNullOrEmpty(user.country);
    }

}
