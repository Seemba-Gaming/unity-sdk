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
    public Image AcceptedIban;
    public Image DeclinedIban;
    public Image CountryFlag;
    public Button Continue;

    UserManager userManager = new UserManager();
    WithdrawManager withdrawManager = new WithdrawManager();
    WithdrawController controller;

    string user_id, token;
    private string country_code;
    private string currency;
    public bool isPaused = false;
    public bool focus = true;

    // Start is called before the first frame update
    void OnEnable()
    {
        user_id = userManager.getCurrentUserId();
        token = userManager.getCurrentSessionToken();
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

        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        UnityThreadHelper.CreateThread(() =>
        {
            User user = userManager.getUser(user_id, token);

            UnityThreadHelper.Dispatcher.Dispatch(async () =>
            {
                SceneManager.UnloadScene("Loader");
                init_iban(user);

                if (!string.IsNullOrEmpty(user.payment_account_id))
                {
                    var account = await controller.GetAccountStatus();
                    CheckAccountStatus(account);
                }

            });
        });
    }

    private void CheckAccountStatus(JSONNode account)
    {
        Debug.Log("verification_status: " + account["verification_status"].Value);
        if (account["verification_status"].Value.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_UNVERIFIED))
        {
            Unverified(account["verification_link"].Value);
        }
        if (account["verification_status"].Value.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_PENDING))
        {
            Pending();
        }
        if (account["verification_status"].Value.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_VERIFIED))
        {
            Verified();
        }
    }
    private void Unverified(string verification_link)
    {
        Debug.Log("verification_link: " + verification_link);

        if (!string.IsNullOrEmpty(verification_link))
        {
            SceneManager.UnloadScene("WithdrawalInfo");
            Application.OpenURL(verification_link);
        }
    }
    private void Pending()
    {
        Iban.placeholder.GetComponent<Text>().text = "Already Uploaded! Waiting for verification...";
    }

    private void Verified()
    {
        SceneManager.UnloadScene("WithdrawalInfo");
    }
    private async Task CreateConnectAccount()
    {
        bool res = await controller.TokenizeAndCreate(country_code, currency, Iban.text);
        if (res) Iban.interactable = false;
        var account = await controller.GetAccountStatus();
        if (account["verification_status"].Value.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_UNVERIFIED))
        {
            if (!string.IsNullOrEmpty(account["verification_link"].Value))
            {
                Application.OpenURL(account["verification_link"].Value);
            }
        }
        SceneManager.UnloadScene("WithdrawalInfo");


    }

    void init_iban(User user)
    {

        //if (!string.IsNullOrEmpty(user.payment_account_id) && !user.payment_account_id.ToLower().Equals("unsupported")) { Iban.interactable = false; }
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
                AcceptedIban.gameObject.SetActive(true);
                DeclinedIban.gameObject.SetActive(false);
            }
            else
            {
                AcceptedIban.gameObject.SetActive(false);
                DeclinedIban.gameObject.SetActive(true);
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
        string flagBytesString = await userManager.GetFlagByteAsync(country_code);
        if (string.IsNullOrEmpty(flagBytesString)) return;
        Byte[] img = Convert.FromBase64String(flagBytesString);
        Texture2D txt = new Texture2D(1, 1);
        txt.LoadImage(img);
        CountryFlag.sprite = Sprite.Create(txt as Texture2D, new Rect(0f, 0f, txt.width, txt.height), Vector2.zero);
        CountryFlag.gameObject.SetActive(true);
    }

}
