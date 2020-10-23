﻿using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WithdrawController
{
    public async Task<bool> TokenizeAndCreate(string country_code, string currency, string iban)
    {
        WithdrawManager wm = new WithdrawManager();
        var user_token = UserManager.Get.getCurrentSessionToken();

        if (string.IsNullOrEmpty(iban) || string.IsNullOrEmpty(country_code) || string.IsNullOrEmpty(currency)) return false;

        LoaderManager.Get.LoaderController.ShowLoader();
        string account_token = await wm.TokenizeAccountAsync();
        string bank_account_token = await wm.TokenizeAccountAsync();

        if (!string.IsNullOrEmpty(bank_account_token) && !string.IsNullOrEmpty(account_token))
        {
            bool res = await wm.CreateConnectAccount(account_token, bank_account_token, currency, country_code, user_token);
            LoaderManager.Get.LoaderController.HideLoader();
            return res;
        }
        return false;
    }
    public async Task<AccountStatus> GetAccountStatus()
    {
        WithdrawManager wm = new WithdrawManager();
        var user_token = UserManager.Get.getCurrentSessionToken();
        return await wm.accountVerificationStatus(user_token);
    }
    public async void Withdraw(float amount)
    {
        WithdrawManager wm = new WithdrawManager();
        var user_token = UserManager.Get.getCurrentSessionToken();

        string withdrawResult = null;
        withdrawResult = await wm.Payout(user_token, amount);
        InfoPersonnelWithdraw.currentIdProof = null;
        InfoPersonnelWithdraw.currentIBAN = null;
        Debug.Log("withdrawResult: " + withdrawResult);
        if (withdrawResult == "ProhibitedLocation")
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_PROHIBITED_LOCATION_WITHDRAW, PopupsText.Get.ProhibitedLocationWithdraw());
        }
        else if (withdrawResult == WithdrawManager.WITHDRAW_ERROR_AMOUNT_INSUFFICIENT)
        {
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                EventsController.Get.withdrawFailed("Withdrawal", null, WithdrawManager.WITHDRAW_INSUFFICIENT_AMOUNT_FAILED_MESSAGE);
            });
        }
        else if (withdrawResult == WithdrawManager.WITHDRAW_ERROR_BALANCE_INSUFFICIENT)
        {
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                EventsController.Get.withdrawFailed("Withdrawal", null, WithdrawManager.WITHDRAW_INSUFFICIENT_FUNDS_FAILED_MESSAGE);
            });
        }
        else if (withdrawResult == "error")
        {
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                EventsController.Get.withdrawFailed(null, null, WithdrawManager.WITHDRAW_FAILED_MESSAGE);
            });
        }
        else if (withdrawResult == WithdrawManager.WITHDRAW_SUCCEEDED_STATUS)
        {
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                LoaderManager.Get.LoaderController.HideLoader();
                UserManager.Get.CurrentUser.money_credit = float.Parse((UserManager.Get.CurrentUser.money_credit - WithdrawPresenter.WithdrawMoney).ToString().Replace(",", "."));
                EventsController.Get.backToWinMoney();
                PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_CONGRATS_WITHDRAW, PopupsText.Get.CongratsWithdraw());
            });
        }
    }
}
