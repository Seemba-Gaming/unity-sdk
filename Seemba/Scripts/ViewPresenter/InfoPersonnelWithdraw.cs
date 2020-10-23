using SimpleJSON;
using System;
using UnityEngine;
using UnityEngine.UI;
public class InfoPersonnelWithdraw : MonoBehaviour
{
    public static bool ibanUploaded, idProof1Uploaded, idProof2Uploaded, passportUploaded;
    public static string currentIBAN, currentIdProof, currentDocUploaded;

    public InputField LastName;
    public InputField FirstName;
    public InputField Adress;
    public InputField city;
    public InputField zip;
    public InputField country;
    public InputField personal_id_number;
    public InputField IBAN;
    public InputField Swift;
    public InputField Phone;
    public Text Birthday;
    public Text WithdrawButtonText;
    public Text placeholderAge;
    public Button ContinueButton;
    public Button ContinueButtonIBAN;
    public Button WithdrawButton;

    private bool validIban;

    private WithdrawManager wm = new WithdrawManager();
    private string token;

    // Use this for initialization
    private async void OnEnable()
    {
        var IBANInputState = IBAN.GetComponent<InputfieldStateController>();
        token = UserManager.Get.getCurrentSessionToken();
        WithdrawButton.interactable = false;
        IBAN.onValueChanged.AddListener(delegate
        {
            IBANInputState.ShowLoading();
            validIban = wm.validateIBAN(IBAN.text);
            if (validIban)
            {
                IBANInputState.ShowAccepted();
            }
            else
            {
                IBANInputState.ShowDeclined();
            }
        });
        WithdrawButtonText.text = WithdrawPresenter.WithdrawMoney.ToString() + CurrencyManager.CURRENT_CURRENCY;
        string Id = UserManager.Get.getCurrentUserId();
        string Token = UserManager.Get.getCurrentSessionToken();
        LoaderManager.Get.LoaderController.ShowLoader(null);

        User user = await UserManager.Get.getUser();
        var account = wm.accountVerificationJSON(Token);

        LoaderManager.Get.LoaderController.HideLoader();
        if (user != null)
        {
            personelInfoVerification(user);
            ibanVerification(user);
            WithdrawButton.onClick.AddListener(() =>
            {
                Withdraw();
            });
            ContinueButtonIBAN.onClick.AddListener(() =>
            {
                currentIBAN = IBAN.text;
                tokenizeAndAttach();
            });
        }
        else
        {
            ConnectivityController.CURRENT_ACTION = ConnectivityController.PERSONNEL_INFO_WITHDRAW_ACTION;
            PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());

            try
            {
                LoaderManager.Get.LoaderController.HideLoader();
            }
            catch (ArgumentException) { }
        }

    }
    private void personelInfoVerification(User user)
    {
        if (!string.IsNullOrEmpty(user.lastname))
        {
            LastName.text = user.lastname;
        }
        if (!string.IsNullOrEmpty(user.firstname))
        {
            FirstName.text = user.firstname;
        }
        if (!string.IsNullOrEmpty(user.adress))
        {
            Adress.text = user.adress;
        }
        if (!string.IsNullOrEmpty(user.city))
        {
            city.text = user.city;
        }
        if (user.zipcode != null)
        {
            zip.text = user.zipcode.ToString();
        }
        if (!string.IsNullOrEmpty(user.country))
        {
            country.text = user.country;
        }
        if (user.country_code.ToLower().Equals("us"))
        {
            personal_id_number.gameObject.SetActive(true);
        }
        if (!string.IsNullOrEmpty(user.personal_id_number))
        {
            personal_id_number.text = user.personal_id_number;
        }
        if (!string.IsNullOrEmpty(user.phone))
        {
            char[] charsToTrim = { ' ' };
            string tmp = user.phone.Trim(charsToTrim);
            Debug.Log(tmp);
            if (tmp.StartsWith("+"))
            {
                Phone.text = tmp;
            }
            else
            {
                Phone.text = "+" + tmp;
            }
        }
        if (!string.IsNullOrEmpty(user.birthday))
        {
            Birthday.text = DateTime.Parse(user.birthday).ToString("yyyy-MM-dd");
            placeholderAge.transform.localScale = Vector3.zero;
        }
        else
        {
            placeholderAge.transform.localScale = Vector3.one;
        }
        if (InformationAlreadyExist(user))
        {
            ContinueButton.interactable = true;
        }
    }
    private void ibanVerification(User user)
    {
        if (user.iban_uploaded == true)
        {
            ContinueButtonIBAN.interactable = true;
            IBAN.GetComponent<InputfieldStateController>().ShowAccepted();
            IBAN.interactable = false;
            IBAN.placeholder.GetComponent<Text>().text = "Already uploaded";
            Swift.interactable = false;
            validIban = true;
            ibanUploaded = true;
        }
    }
    public bool InformationAlreadyExist(User user)
    {
        if (user.country_code.ToLower().Equals("us"))
        {
            return !string.IsNullOrEmpty(user.firstname) && !string.IsNullOrEmpty(user.lastname) && !string.IsNullOrEmpty(user.birthday) && !string.IsNullOrEmpty(user.adress) && !string.IsNullOrEmpty(user.city) && !string.IsNullOrEmpty(user.zipcode) && !string.IsNullOrEmpty(user.country) && !string.IsNullOrEmpty(user.personal_id_number);
        }
        else
        {
            return !string.IsNullOrEmpty(user.firstname) && !string.IsNullOrEmpty(user.lastname) && !string.IsNullOrEmpty(user.birthday) && !string.IsNullOrEmpty(user.adress) && !string.IsNullOrEmpty(user.city) && !string.IsNullOrEmpty(user.zipcode) && !string.IsNullOrEmpty(user.country);
        }
    }
    public void Withdraw()
    {
        WithdrawManager wm = new WithdrawManager();
        LoaderManager.Get.LoaderController.ShowLoader(null);
        string DocId = PlayerPrefs.GetString("DocId");
        UnityThreadHelper.CreateThread(async () =>
        {
            string withdrawResult = null;
            withdrawResult = await wm.Payout(token, WithdrawPresenter.WithdrawMoney);
            Debug.Log("withdrawResult: " + withdrawResult);
            currentIdProof = null;
            currentIBAN = null;
            if (!string.IsNullOrEmpty(withdrawResult))
            {
                if (withdrawResult == "ProhibitedLocation")
                {
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        LoaderManager.Get.LoaderController.HideLoader();
                        EventsController.Get.withdrawFailed("Withdrawal", null, WithdrawManager.WITHDRAW_INSUFFICIENT_AMOUNT_FAILED_MESSAGE);
                    });
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
                        UserManager.Get.UpdateUserMoneyCredit((float.Parse(UserManager.Get.GetCurrentMoneyCredit()) - WithdrawPresenter.WithdrawMoney).ToString("N2").Replace(",", "."));
                        EventsController.Get.backToWinMoney();
                    });
                }
            }
            else
            {
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    EventsController.Get.withdrawFailed(null, null, WithdrawManager.WITHDRAW_FAILED_MESSAGE);
                });
            }
        });
    }
    private async void tokenizeAndAttach()
    {
        if (!String.IsNullOrEmpty(InfoPersonnelWithdraw.currentIBAN))
        {
            LoaderManager.Get.LoaderController.ShowLoader(null);
            string accounttoken = await wm.TokenizeAccountAsync();
            if (!String.IsNullOrEmpty(accounttoken))
            {
                if (wm.attachTokenToAccount(accounttoken, token))
                {
                    string[] attrib = { "iban_uploaded" };
                    string[] value5 = { "true" };
                    UserManager.Get.UpdateUserByField(attrib, value5);
                }
            }

            LoaderManager.Get.LoaderController.HideLoader();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (string.IsNullOrEmpty(LastName.text) || string.IsNullOrEmpty(FirstName.text) || string.IsNullOrEmpty(Adress.text) || string.IsNullOrEmpty(city.text) || string.IsNullOrEmpty(zip.text) || string.IsNullOrEmpty(country.text) || string.IsNullOrEmpty(Birthday.text))
        {
            ContinueButton.interactable = false;
        }
        else
        {
            if (UserManager.Get.CurrentUser.country_code.ToLower().Equals("us"))
            {
                if (!string.IsNullOrEmpty(personal_id_number.text))
                {
                    ContinueButton.interactable = true;
                }
                else
                {
                    ContinueButton.interactable = false;
                }
            }
            else
            {
                ContinueButton.interactable = true;
            }
        }
        if (validIban == true)
        {
            ContinueButtonIBAN.interactable = true;
        }
        else
        {
            ContinueButtonIBAN.interactable = false;
        }
    }

    #region Deadcode

    //private void disable(string doc)
    //{
    //    Image DocDisableimg = GameObject.Find(doc + "/disable").GetComponent<Image>();
    //    var tempColor = DocDisableimg.color;
    //    tempColor.a = 0.50f;
    //    DocDisableimg.color = tempColor;
    //}

    //private void docsVerification(JSONNode account)
    //{
    //    string documentFrontID = null; string documentBackID = null;
    //    var accountStatus = account["account"]["individual"]["verification"]["status"].Value;
    //    Debug.Log(accountStatus);
    //    try
    //    {
    //        documentFrontID = account["account"]["individual"]["verification"]["document"]["front"].Value;
    //    }
    //    catch (NullReferenceException) { }
    //    try
    //    {
    //        documentBackID = account["account"]["individual"]["verification"]["document"]["back"].Value;
    //    }
    //    catch (NullReferenceException) { }
    //    if (accountStatus.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_PENDING))
    //    {
    //        GameObject.Find("IDAddressWaiting").GetComponent<Image>().transform.localScale = Vector3.one;
    //        if (!string.IsNullOrEmpty(documentBackID))
    //        {
    //            GameObject.Find("IDFrontWaiting").GetComponent<Image>().transform.localScale = Vector3.one;
    //            GameObject.Find("IDBackWaiting").GetComponent<Image>().transform.localScale = Vector3.one;
    //            GameObject.Find("IDPassport").GetComponent<Button>().interactable = false;
    //            disable("IDPassport");
    //        }
    //        else
    //        {
    //            GameObject.Find("IDPassportWaiting").GetComponent<Image>().transform.localScale = Vector3.one;
    //            GameObject.Find("IDFront").GetComponent<Button>().interactable = false;
    //            GameObject.Find("IDBack").GetComponent<Button>().interactable = false;
    //            disable("IDFront");
    //            disable("IDBack");
    //        }
    //    }
    //    else if (accountStatus.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_VERIFIED))
    //    {
    //        WithdrawButton.interactable = true;
    //        GameObject.Find("IDAddressSuccess").GetComponent<Image>().transform.localScale = Vector3.one;
    //        if (!string.IsNullOrEmpty(documentBackID))
    //        {
    //            GameObject.Find("IDFrontSuccess").GetComponent<Image>().transform.localScale = Vector3.one;
    //            GameObject.Find("IDBackSuccess").GetComponent<Image>().transform.localScale = Vector3.one;
    //            GameObject.Find("IDPassport").GetComponent<Button>().interactable = false;
    //            disable("IDPassport");
    //        }
    //        else
    //        {
    //            GameObject.Find("IDPassportSuccess").GetComponent<Image>().transform.localScale = Vector3.one;
    //            GameObject.Find("IDFront").GetComponent<Button>().interactable = false;
    //            GameObject.Find("IDBack").GetComponent<Button>().interactable = false;
    //            disable("IDFront");
    //            disable("IDBack");
    //        }
    //    }
    //}

    #endregion
}
