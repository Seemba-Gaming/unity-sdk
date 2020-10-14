using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WithdrawController
{
    // Start is called before the first frame update
    public async Task<bool> TokenizeAndCreate(string country_code, string currency, string iban)
    {
        WithdrawManager wm = new WithdrawManager();
        UserManager um = new UserManager();
        var user_token = um.getCurrentSessionToken();
        var user_id = um.getCurrentUserId();


        if (string.IsNullOrEmpty(iban) || string.IsNullOrEmpty(country_code) || string.IsNullOrEmpty(currency)) return false;

        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);

        string account_token = await wm.TokenizeAccount();
        string bank_account_token = await wm.TokenizeBankAccount(country_code, currency, iban);

        if (!string.IsNullOrEmpty(bank_account_token) && !string.IsNullOrEmpty(account_token))
        {
            bool res = await wm.CreateConnectAccount(account_token, bank_account_token, currency, country_code, user_token);
            SceneManager.UnloadScene("Loader");
            return res;
        }

        return false;

    }
    public async Task<JSONNode> GetAccountStatus()
    {
        UserManager um = new UserManager();
        WithdrawManager wm = new WithdrawManager();
        var user_token = um.getCurrentSessionToken();
        return await wm.accountVerificationStatus(user_token);
    }
    public async void Withdraw(float amount)
    {
        EventsController eventsController = new EventsController();
        WithdrawManager wm = new WithdrawManager();
        UserManager um = new UserManager();
        var user_token = um.getCurrentSessionToken();
        var user_id = um.getCurrentUserId();

        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);

        string withdrawResult = null;
        withdrawResult = await wm.Payout(user_token, amount);
        InfoPersonnelWithdraw.currentIdProof = null;
        InfoPersonnelWithdraw.currentIBAN = null;
        Debug.Log("withdrawResult: " + withdrawResult);
        if (withdrawResult == "ProhibitedLocation")
        {
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadSceneAsync("Loader");
                GameObject.Find("CalqueWidhraw").transform.localScale = Vector3.one;
                var animator = GameObject.Find("popupProhibitedLocation").GetComponent<Animator>();
                animator.SetBool("Show Error", true);
            });
        }
        else if (withdrawResult == WithdrawManager.WITHDRAW_ERROR_AMOUNT_INSUFFICIENT)
        {
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                eventsController.withdrawFailed("Withdrawal", null, WithdrawManager.WITHDRAW_INSUFFICIENT_AMOUNT_FAILED_MESSAGE);
            });
        }
        else if (withdrawResult == WithdrawManager.WITHDRAW_ERROR_BALANCE_INSUFFICIENT)
        {
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                eventsController.withdrawFailed("Withdrawal", null, WithdrawManager.WITHDRAW_INSUFFICIENT_FUNDS_FAILED_MESSAGE);
            });
        }
        else if (withdrawResult == "error")
        {
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                eventsController.withdrawFailed(null, null, WithdrawManager.WITHDRAW_FAILED_MESSAGE);
            });
        }
        else if (withdrawResult == WithdrawManager.WITHDRAW_SUCCEEDED_STATUS)
        {
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadSceneAsync("Loader");
                UserManager.CurrentUser.money_credit = UserManager.CurrentUser.money_credit - WithdrawPresenter.WithdrawMoney;
                eventsController.backToWinMoney();
                eventsController.ShowPopup("popupCongratWithdraw");
            });
        }


    }
}
