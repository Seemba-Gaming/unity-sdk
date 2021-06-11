using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SeembaSDK
{
    public class WalletTogglePresenter : MonoBehaviour
    {
        #region Script Parameters

        public Text SelectedText;
        public Text Amount;
        public Text EqualsText;

        public TextMeshProUGUI CrownsValue;
        public Color SelectedOffColor;
        public Color AmountOffColor;
        public bool IsSelected;
        public InputField OtherAmount;
        public GameObject OtherAmountCrowns;

        #endregion

        #region Unity Methods
        private void Start()
        {
            if (SelectedText != null)
            {
                TranslationManager._instance.scene = "Home";
                if (IsSelected)
                {
                    SelectedText.text = TranslationManager._instance.Get("selected");
                }
                else
                {
                    SelectedText.text = TranslationManager._instance.Get("select");
                }
            }
        }
        #endregion

        #region Methods
        public void ToggleSelected(bool selected)
        {
            TranslationManager._instance.scene = "Home";
            if (selected)
            {
                SelectedText.text = TranslationManager._instance.Get("selected");
                SelectedText.color = Color.white;
                EqualsText.color = Color.white;
                CrownsValue.color = Color.white;
                Amount.color = Color.white;
                IsSelected = true;
            }
            else
            {
                SelectedText.text = TranslationManager._instance.Get("select");
                SelectedText.color = SelectedOffColor;
                Amount.color = AmountOffColor;
                EqualsText.color = AmountOffColor;
                CrownsValue.color = AmountOffColor;
                IsSelected = false;
            }
        }
        public void OtherAmountToggleSelected(bool selected)
        {
            if (selected)
            {
                IsSelected = true;
                OtherAmount.enabled = true;
                OtherAmount.ActivateInputField();
                OtherAmountCrowns.SetActive(true);
            }
            else
            {
                IsSelected = false;
                OtherAmount.enabled = false;
                OtherAmount.text = string.Empty;
                OtherAmountCrowns.SetActive(false);
            }
        }
        public void AddAmountAsync()
        {
            var amountToAdd = string.Join("", Amount.text.Where(char.IsDigit));
            WalletScript.LastCredit = int.Parse(amountToAdd);
            LoaderManager.Get.LoaderController.ShowLoader(null);
            SeembaAnalyticsManager.Get.SendCreditEvent("Try to credit", float.Parse(amountToAdd));
            if (EventsController.Get.checkUserBirthday(UserManager.Get.CurrentUser))
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.AGE_VERIFICATION, PopupsText.Get.AgeVerification());
            }
            else
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_PAYMENT, PopupsText.Get.PopupPayment());
            }
            LoaderManager.Get.LoaderController.HideLoader();
        }
        public void AddOtherAmountAsync()
        {
            WalletScript.LastCredit = float.Parse(OtherAmount.text.Replace(".", ","), System.Globalization.NumberStyles.Float);
            Debug.LogWarning(WalletScript.LastCredit);
            LoaderManager.Get.LoaderController.ShowLoader(null);

            if (EventsController.Get.checkUserBirthday(UserManager.Get.CurrentUser))
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.AGE_VERIFICATION, PopupsText.Get.AgeVerification());
            }
            else
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_PAYMENT, PopupsText.Get.PopupPayment());
            }
            LoaderManager.Get.LoaderController.HideLoader();
        }

        public void UpdateCrownValue()
        {
            if(string.IsNullOrEmpty(OtherAmount.text))
            {
                CrownsValue.text = string.Empty + " <sprite=1>";
            }
            else
            {
                CrownsValue.text = (int.Parse(OtherAmount.text) * 100).ToString() + " <sprite=1>";
            }
        }
        #endregion
    }
}
