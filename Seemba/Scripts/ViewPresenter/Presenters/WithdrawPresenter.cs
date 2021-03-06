﻿using SimpleJSON;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class AccountStatus
    {
        public bool success;
        public string message;
        public bool payouts_enabled;
        public string verification_status;
        public string verification_link;
    }

    [CLSCompliant(false)]
    public class WithdrawPresenter : MonoBehaviour
    {
        #region Static 
        public static float WithdrawMoney;
        public static float AmountToWithdraw;
        #endregion

        #region Script Parameters
        public Text balance, TextEuro;
        public Button WithdrawButton;
        public InputField Amount;
        #endregion

        #region Fields
        private WithdrawManager withdrawManager = new WithdrawManager();
        private WithdrawController controller;
        #endregion

        #region Unity Methods

        private async void Start()
        {
            controller = new WithdrawController();
            string token = UserManager.Get.getCurrentSessionToken();

            User user = null;

            user = await UserManager.Get.getUser();
            var accountStatus = await withdrawManager.accountVerificationStatus(token);

            balance.text = user.money_credit.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;

            WithdrawButton.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendWithdrawalEvent("Click On Withdrawal", AmountToWithdraw);
                CheckAndWithdraw();
            });

            Amount.onValueChanged.AddListener(delegate
            {
                if (string.IsNullOrEmpty(Amount.text))
                {
                    WithdrawButton.interactable = false;
                    TextEuro.text = "";
                }
                else
                {
                    TextEuro.text = CurrencyManager.CURRENT_CURRENCY;
                    AmountToWithdraw = float.Parse(Amount.text);
                    if ((float.Parse(Amount.text, CultureInfo.InvariantCulture) > 0) &&
                    (float.Parse(Amount.text, CultureInfo.InvariantCulture) <= (float.Parse(UserManager.Get.GetCurrentMoneyCredit()))))
                    {
                        WithdrawButton.interactable = true;
                    }
                    else
                    {
                        WithdrawButton.interactable = false;
                    }
                }
            });
        }
        private void OnEnable()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("Go to Withdraw");
            balance.text = UserManager.Get.CurrentUser.money_credit.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        }

        private void Update()
        {
            balance.text = float.Parse(UserManager.Get.GetCurrentMoneyCredit()).ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        }
        #endregion

        #region Methods
        public bool InformationAlreadyExist(User user)
        {
            return !string.IsNullOrEmpty(user.firstname) && !string.IsNullOrEmpty(user.lastname) && !string.IsNullOrEmpty(user.birthdate) && !string.IsNullOrEmpty(user.address) && !string.IsNullOrEmpty(user.city) && !string.IsNullOrEmpty(user.zipcode) && !string.IsNullOrEmpty(user.country);
        }
        #endregion

        #region Implementation
        private async void CheckAndWithdraw()
        {
            if (CountryController.checkCountry(UserManager.Get.CurrentUser.country_code) == true)
            {
                var token = UserManager.Get.getCurrentSessionToken();
                LoaderManager.Get.LoaderController.ShowLoader();
                var accountStatus = await withdrawManager.accountVerificationStatus(token);
                LoaderManager.Get.LoaderController.HideLoader();

                if (accountStatus.payouts_enabled)
                {
                    controller.Withdraw(float.Parse(Amount.text, CultureInfo.InvariantCulture));
                    Amount.text = string.Empty;
                }
                else
                {
                    SeembaAnalyticsManager.Get.SendWithdrawalEvent("Popup Missing Iban", AmountToWithdraw);
                    PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_MISSING_INFO, PopupsText.Get.MissingInfo(), "withdraw");
                }
            }
            else
            {
                SeembaAnalyticsManager.Get.SendWithdrawalEvent("Popup prohibited location withdraw", AmountToWithdraw);
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_PROHIBITED_LOCATION_WITHDRAW, PopupsText.Get.ProhibitedLocationWithdraw());
            }
        }
        #endregion

    }
}
