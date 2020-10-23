using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WithdrawalInfoPresenter : MonoBehaviour
{
    public InputField Iban;
    public Image CountryFlag;
    public Button Continue;

    WithdrawManager withdrawManager = new WithdrawManager();
    WithdrawController controller;

    private string country_code;
    private string currency;
    public bool isPaused = false;
    public bool focus = true;

    // Start is called before the first frame update
    void OnEnable()
    {
        controller = new WithdrawController();
        init();
    }
    async void init()
    {
        Continue.interactable = false;
        Continue.onClick.AddListener(async delegate
        {
            await CreateConnectAccount();
        });

        LoaderManager.Get.LoaderController.ShowLoader();
        User user = await UserManager.Get.getUser();
        LoaderManager.Get.LoaderController.HideLoader();

        init_iban(user);

        if (!string.IsNullOrEmpty(user.payment_account_id))
        {
            var account = await controller.GetAccountStatus();
            CheckAccountStatus(account);
        }
    }
    private void CheckAccountStatus(AccountStatus account)
    {
        Debug.Log("verification_status: " + account.verification_status);
        if (account.verification_status.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_UNVERIFIED))
        {
            Unverified(account.verification_link);
        }
        if (account.verification_status.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_PENDING))
        {
            Pending();
        }
        if (account.verification_status.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_VERIFIED))
        {
            Verified();
        }
    }
    private void Unverified(string verification_link)
    {
        if (!string.IsNullOrEmpty(verification_link))
        {
            ViewsEvents.Get.GoBack();
            Application.OpenURL(verification_link);
        }
    }
    private void Pending()
    {
        Iban.placeholder.GetComponent<Text>().text = "Already Uploaded! Waiting for verification...";
    }
    private void Verified()
    {
        ViewsEvents.Get.GoBack();
    }
    private async Task CreateConnectAccount()
    {
        bool res = await controller.TokenizeAndCreate(country_code, currency, Iban.text);
        if (res) Iban.interactable = false;
        var account = await controller.GetAccountStatus();
        if (account.verification_status.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_UNVERIFIED))
        {
            if (!string.IsNullOrEmpty(account.verification_link))
            {
                Application.OpenURL(account.verification_link);
            }
        }
        ViewsEvents.Get.GoBack();
    }
    void init_iban(User user)
    {
        Iban.onValueChanged.AddListener(delegate
        {
            CountryFlag.gameObject.SetActive(false);
            Continue.interactable = false;
            currency = null;
            country_code = null;

            if (withdrawManager.validateIBAN(Iban.text))
            {
                Continue.interactable = true;
                SetCountryCurrency();
                Iban.GetComponent<InputfieldStateController>().ShowAccepted();
            }
            else
            {
                Iban.GetComponent<InputfieldStateController>().ShowDeclined();
            }
        });
    }
    void SetCountryCurrency()
    {
        country_code = GetCountryFromIban();
        RegionInfo myRI1 = new RegionInfo(country_code);
        currency = myRI1.ISOCurrencySymbol;

        SetSelectedCountryFlag(country_code.ToLower());
    }
    private string GetCountryFromIban()
    {
        return Iban.text.Substring(0, 2);
    }
    private async void SetSelectedCountryFlag(string country_code)
    {
        var mTexture = await UserManager.Get.GetFlagBytes(country_code);
        if (mTexture == null) return;
        CountryFlag.sprite = Sprite.Create(mTexture, new Rect(0f, 0f, mTexture.width, mTexture.height), Vector2.zero);
        CountryFlag.gameObject.SetActive(true);
    }
}
