﻿using System.Threading.Tasks;
using UnityEngine;

namespace SeembaSDK
{
    public class WithdrawController
    {
        public async Task<bool> TokenizeAndCreate(string country_code, string currency, string iban)
        {
            WithdrawManager wm = new WithdrawManager();
            var user_token = UserManager.Get.getCurrentSessionToken();

            if (string.IsNullOrEmpty(iban) || string.IsNullOrEmpty(country_code) || string.IsNullOrEmpty(currency))
            {
                return false;
            }

            LoaderManager.Get.LoaderController.ShowLoader();
            string account_token = await wm.TokenizeAccountAsync();
            string bank_account_token = await wm.TokenizeBankAccount(country_code, currency, iban);
            Debug.LogWarning(account_token);
            if (!string.IsNullOrEmpty(bank_account_token) && !string.IsNullOrEmpty(account_token))
            {
                Debug.LogWarning("here");
                bool res = await wm.CreateConnectAccount(account_token, bank_account_token, currency, country_code, user_token);
                LoaderManager.Get.LoaderController.HideLoader();
                return res;
            }
            LoaderManager.Get.LoaderController.HideLoader();
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
            if (withdrawResult == "ProhibitedLocation") 
            {
                SeembaAnalyticsManager.Get.SendWithdrawalEvent("Withdrawal prohibited location", WithdrawPresenter.AmountToWithdraw);
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_PROHIBITED_LOCATION_WITHDRAW, PopupsText.Get.ProhibitedLocationWithdraw());
            }
            else if (withdrawResult == WithdrawManager.WITHDRAW_ERROR_AMOUNT_INSUFFICIENT)
            {
                SeembaAnalyticsManager.Get.SendWithdrawalEvent("Withdrawal amount insufficient", WithdrawPresenter.AmountToWithdraw);
                TranslationManager.scene = "Popups";
                EventsController.Get.withdrawFailed(TranslationManager.Get("withdrawal"), TranslationManager.Get("error"), WithdrawManager.WITHDRAW_INSUFFICIENT_AMOUNT_FAILED_MESSAGE);
            }
            else if (withdrawResult == WithdrawManager.WITHDRAW_ERROR_BALANCE_INSUFFICIENT)
            {
                SeembaAnalyticsManager.Get.SendWithdrawalEvent("Withdrawal balance insufficient", WithdrawPresenter.AmountToWithdraw);
                TranslationManager.scene = "Popups";
                EventsController.Get.withdrawFailed(TranslationManager.Get("withdrawal"), TranslationManager.Get("error"), WithdrawManager.WITHDRAW_INSUFFICIENT_FUNDS_FAILED_MESSAGE);
            }
            else if (withdrawResult == "error")
            {
                SeembaAnalyticsManager.Get.SendWithdrawalEvent("Withdrawal Error", WithdrawPresenter.AmountToWithdraw);
                TranslationManager.scene = "Popups";
                EventsController.Get.withdrawFailed(TranslationManager.Get("withdrawal"), TranslationManager.Get("error"), WithdrawManager.WITHDRAW_FAILED_MESSAGE);
            }
            else if (withdrawResult == WithdrawManager.WITHDRAW_SUCCEEDED_STATUS)
            {
                SeembaAnalyticsManager.Get.SendWithdrawalEvent("Withdrawal Succeeded", WithdrawPresenter.AmountToWithdraw);
                LoaderManager.Get.LoaderController.HideLoader();
                UserManager.Get.CurrentUser.money_credit = float.Parse((UserManager.Get.CurrentUser.money_credit - WithdrawPresenter.WithdrawMoney).ToString());
                EventsController.Get.withdrawSucceeded();
            }
        }
    }
}
