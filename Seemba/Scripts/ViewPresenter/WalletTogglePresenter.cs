﻿using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WalletTogglePresenter : MonoBehaviour
{
    #region Script Parameters

    public Text                             SelectedText;
    public Text                             Amount;
    public Color                            SelectedOffColor;
    public Color                            AmountOffColor;
    public bool                             IsSelected;
    public InputField                       OtherAmount;

    #endregion

    #region Unity Methods
    private void Start()
    {
        if(SelectedText != null)
        {
            if(IsSelected)
            {
                SelectedText.text = "Selected";
            }
            else
            {
                SelectedText.text = "Select";
            }
        }
    }
    #endregion

    #region Methods
    public void ToggleSelected(bool selected)
    {
        if (selected)
        {
            SelectedText.text = "Selected";
            SelectedText.color = Color.white;
            Amount.color = Color.white;
            IsSelected = true;
        }
        else
        {
            SelectedText.text = "Select";
            SelectedText.color = SelectedOffColor;
            Amount.color = AmountOffColor;
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
        }
        else
        {
            IsSelected = false;
            OtherAmount.enabled = false;
            OtherAmount.text = string.Empty;
        }
    }
    public async System.Threading.Tasks.Task AddAmountAsync()
    {
        var amountToAdd = string.Join("",Amount.text.Where(char.IsDigit));
        WalletScript.LastCredit = int.Parse(amountToAdd);
        LoaderManager.Get.LoaderController.ShowLoader(null);
        User user = await UserManager.Get.getUser();

        if (EventsController.Get.checkUserBirthday(user))
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.AGE_VERIFICATION, PopupsText.Get.AgeVerification());
        }
        else
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_PAYMENT, PopupsText.Get.PopupPayment());
        }
        LoaderManager.Get.LoaderController.HideLoader();
    }
    public async System.Threading.Tasks.Task AddOtherAmountAsync()
    {
        WalletScript.LastCredit = float.Parse(OtherAmount.text.Replace(".",","),System.Globalization.NumberStyles.Float);
        Debug.LogWarning(WalletScript.LastCredit);
        LoaderManager.Get.LoaderController.ShowLoader(null);
        User user = await UserManager.Get.getUser();

        if (EventsController.Get.checkUserBirthday(user))
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.AGE_VERIFICATION, PopupsText.Get.AgeVerification());
        }
        else
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_PAYMENT, PopupsText.Get.PopupPayment());
        }
        LoaderManager.Get.LoaderController.HideLoader();
    }
    #endregion
}
