using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PopupType
{
    DUELS,
    DEV_MODE,
    VPN,
    PROHIBITED_LOCATION,
    DOWNLOAD_FROM_STORE,
    AGE_VERIFICATION,
    POPUP_PAYMENT,
    POPUP_PAYMENT_FAILED,
    LOGOUT,
    POPUP,
    POPUP_UPDATE_PASSWORD,
    POPUP_CURRENT_PASSWORD,
    POPUP_FORGET_PASSWORD,
    POPUP_CONGRATS_WITHDRAW,
    POPUP_CONGRATS,
    POPUP_WIN,
    POPUP_PROFILE,
    POPUP_CHOOSE_CHARACTER,
    POPUP_OUPS,
    INFO_INSUFFICIENT_BALANCE,
    INFO_INSUFFICIENT_BUBBLES,
    INFO_POPUP_ERROR,
    INFO_POPUP_3D_SECURE_CONFIRMATION,
    INFO_POPUP_TOO_YOUNG,
    INFO_POPUP_PROHIBITED_LOCATION_WALLET,
    INFO_POPUP_PROHIBITED_LOCATION_WITHDRAW,
    INFO_POPUP_PASSWORD_UPDATED,
    INFO_POPUP_MISSING_INFO,
    INFO_POPUP_MISSING,
    INFO_POPUP_CONNECTION_FAILED,
    INFO_POPUP_SERVER_ERROR,
    INFO_POPUP_UNAUTHORIZED,
    INFO_POPUP_EMAIL_NOT_FOUND,
    INFO_POPUP_PAYMENT_NOT_CONFIRMED,
    INFO_POPUP_EQUALITY_REFUND,
    INFO_POPUP_NOT_AVAILABLE,
    SESSION_EXPIRED
}

[CLSCompliant(false)]
public class PopupsController : MonoBehaviour
{
    #region static
    public static PopupType                         CURRENT_POPUP;
    #endregion

    #region Script Parameters
    public Animator                                 PopupDuels;
    public Animator                                 PopupAds;
    public Animator                                 PopupError;
    public Animator                                 PopupInfo;
    public Animator                                 PopupAgeVerification;
    public Animator                                 PopupUpdatePassword;
    public Animator                                 PopupForgetPassword;
    public Animator                                 PopupCurrentPassword;
    public Animator                                 PopupWin;
    public Animator                                 PopupCongrats;
    public Animator                                 PopupPayment;
    public Animator                                 PopupChooseCharacter;
    #endregion

    #region Fields
    private GameObject                              mPopupContent;
    #endregion

    #region Unity Methods
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        mPopupContent = transform.GetChild(0).gameObject;
    }
    #endregion

    #region Methods
    public void ShowPopup(PopupType popupType, object[] _params, string note = null)
    {
        //LoadPopup();
        CURRENT_POPUP = popupType;
        switch (popupType)
        {
            case PopupType.DUELS:
                PopupManager.Get.PopupViewPresenter.ShowDuelsPopup(_params, note);
                break;
            case PopupType.AGE_VERIFICATION:
                PopupManager.Get.PopupViewPresenter.ShowAgeVerificationPopup(_params);
                break;
            case PopupType.INFO_INSUFFICIENT_BALANCE:
                PopupManager.Get.PopupViewPresenter.ShowInsufficientBalancePopup(_params);
                break;
            case PopupType.INFO_INSUFFICIENT_BUBBLES:
                PopupManager.Get.PopupViewPresenter.ShowInsufficientBubblesPopup(_params);
                break;
            case PopupType.DOWNLOAD_FROM_STORE:
                PopupManager.Get.PopupViewPresenter.ShowInfoPopup(_params);
                break;
            case PopupType.PROHIBITED_LOCATION:
                PopupManager.Get.PopupViewPresenter.ShowProhibitedLocationWalletPopup(_params);
                break;
            case PopupType.INFO_POPUP_CONNECTION_FAILED:
                PopupManager.Get.PopupViewPresenter.ShowConnectionFailedPopup(_params);
                break;
            case PopupType.POPUP_CURRENT_PASSWORD:
                PopupManager.Get.PopupViewPresenter.ShowCurrentPasswordPopup(_params);
                break;
            case PopupType.POPUP_UPDATE_PASSWORD:
                PopupManager.Get.PopupViewPresenter.ShowNewPasswordPopup(_params);
                break;
            case PopupType.POPUP_FORGET_PASSWORD:
                PopupManager.Get.PopupViewPresenter.ShowForgetPasswordPopup(_params);
                break;
            case PopupType.POPUP_PAYMENT:
                PopupManager.Get.PopupViewPresenter.ShowPaymentPopup(_params);
                break;
            case PopupType.INFO_POPUP_TOO_YOUNG:
                PopupManager.Get.PopupViewPresenter.ShowTooyoungPopup(_params);
                break;
            case PopupType.INFO_POPUP_MISSING_INFO:
                PopupManager.Get.PopupViewPresenter.ShowMissingInfoPopup(_params, note);
                break;
            case PopupType.POPUP_CONGRATS:
                PopupManager.Get.PopupViewPresenter.ShowCongratsPopup(_params);
                break;
            case PopupType.POPUP_CHOOSE_CHARACTER:
                PopupManager.Get.PopupViewPresenter.ShowChooseCharacter(_params);
                break;
            case PopupType.POPUP_WIN:
                PopupManager.Get.PopupViewPresenter.ShowWinPopup(_params, note);
                break;
            case PopupType.POPUP_PAYMENT_FAILED:
                PopupManager.Get.PopupViewPresenter.ShowInfoPopup(_params);
                break;
            case PopupType.INFO_POPUP_PASSWORD_UPDATED:
                PopupManager.Get.PopupViewPresenter.ShowInfoPopup(_params);
                break;
            case PopupType.VPN:
                PopupManager.Get.PopupViewPresenter.ShowInfoPopup(_params);
                break;
            case PopupType.INFO_POPUP_SERVER_ERROR:
                PopupManager.Get.PopupViewPresenter.ShowInfoPopup(_params);
                break;
            case PopupType.INFO_POPUP_UNAUTHORIZED:
                PopupManager.Get.PopupViewPresenter.ShowNotAuthorizedPopup(_params);
                break;
            default:
                Debug.LogWarning(popupType);
                UnloadPopup();
                break;
        }
    }

    public void UnloadPopup()
    {
        mPopupContent.SetActive(false);
    }
    #endregion


}
