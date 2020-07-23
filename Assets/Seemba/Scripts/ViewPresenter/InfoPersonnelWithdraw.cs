using SimpleJSON;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class InfoPersonnelWithdraw : MonoBehaviour
{
    public InputField LastName, FirstName, Adress, city, zip, state, country, personal_id_number, IBAN, Swift, Phone;
    public Text Birthday, WithdrawButtonText, placeholderAge;
    public Button ContinueButton, ContinueButtonIBAN, WithdrawButton, Id1, Passport, Residency;
    bool validIban;
    public static bool ibanUploaded, idProof1Uploaded, idProof2Uploaded, passportUploaded, proofResidency;
    public static string currentIBAN, currentIdProof, currentDocUploaded;
    UserManager um = new UserManager();
    WithdrawManager wm = new WithdrawManager();
    string userId, token;
    Image AcceptedIban;
    // Use this for initialization
    void OnEnable()
    {
        userId = um.getCurrentUserId();
        token = um.getCurrentSessionToken();
        WithdrawButton.interactable = false;
        AcceptedIban = GameObject.Find("AcceptedIBAN").GetComponent<Image>();
        IBAN.onValueChanged.AddListener(delegate
        {
            Image DeclinedIban = GameObject.Find("DeclinedIBAN").GetComponent<Image>();
            Image LoaderIban = GameObject.Find("LoaderIBAN").GetComponent<Image>();
            LoaderIban.transform.localScale = Vector3.one;
            validIban = wm.validateIBAN(IBAN.text);
            LoaderIban.transform.localScale = Vector3.zero;
            if (validIban)
            {
                AcceptedIban.transform.localScale = Vector3.one;
                DeclinedIban.transform.localScale = Vector3.zero;
            }
            else
            {
                AcceptedIban.transform.localScale = Vector3.zero;
                DeclinedIban.transform.localScale = Vector3.one;
            }
        });
        WithdrawButtonText.text = WithdrawPresenter.WithdrawMoney.ToString() + CurrencyManager.CURRENT_CURRENCY;
        string Id = um.getCurrentUserId();
        string Token = um.getCurrentSessionToken();
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        UnityThreadHelper.CreateThread(() =>
        {
            User user = um.getUser(Id, Token);
            var account = wm.accountVerificationJSON(Token);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadScene("Loader");
                if (user != null)
                {
                    //Check Personel Info (Individual)
                    personelInfoVerification(user);
                    //Check Docs Verification (Document + additional document)
                    docsVerification(account);
                    //check Iban Upload 
                    ibanVerification(user);
                    WithdrawButton.onClick.AddListener(() =>
                    {
                        Withdraw();
                    });
                    ContinueButtonIBAN.onClick.AddListener(() =>
                    {
                        InfoPersonnelWithdraw.currentIBAN = IBAN.text;
                        tokenizeAndAttach();
                    });
                }
                else
                {
                    try
                    {
                        SceneManager.UnloadSceneAsync("ConnectionFailed");
                    }
                    catch (ArgumentException ex) { }
                    ConnectivityController.CURRENT_ACTION = ConnectivityController.PERSONNEL_INFO_WITHDRAW_ACTION;
                    SceneManager.LoadScene("ConnectionFailed", LoadSceneMode.Additive);
                    try
                    {
                        SceneManager.UnloadSceneAsync("Loader");
                    }
                    catch (ArgumentException ex) { }
                }
            });
        });
    }
    void docsVerification(JSONNode account)
    {
        string documentFrontID = null; string documentBackID = null;
        var accountStatus = account["account"]["individual"]["verification"]["status"].Value;
        Debug.Log(accountStatus);
        try
        {
            documentFrontID = account["account"]["individual"]["verification"]["document"]["front"].Value;
        }
        catch (NullReferenceException ex) { }
        try
        {
            documentBackID = account["account"]["individual"]["verification"]["document"]["back"].Value;
        }
        catch (NullReferenceException ex) { }
        if (accountStatus.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_PENDING))
        {
            GameObject.Find("IDAddressWaiting").GetComponent<Image>().transform.localScale = Vector3.one;
            if (!string.IsNullOrEmpty(documentBackID))
            {
                GameObject.Find("IDFrontWaiting").GetComponent<Image>().transform.localScale = Vector3.one;
                GameObject.Find("IDBackWaiting").GetComponent<Image>().transform.localScale = Vector3.one;
                GameObject.Find("IDPassport").GetComponent<Button>().interactable = false;
                disable("IDPassport");
            }
            else
            {
                GameObject.Find("IDPassportWaiting").GetComponent<Image>().transform.localScale = Vector3.one;
                GameObject.Find("IDFront").GetComponent<Button>().interactable = false;
                GameObject.Find("IDBack").GetComponent<Button>().interactable = false;
                disable("IDFront");
                disable("IDBack");
            }
        }
        else if (accountStatus.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_VERIFIED))
        {
            WithdrawButton.interactable = true;
            GameObject.Find("IDAddressSuccess").GetComponent<Image>().transform.localScale = Vector3.one;
            if (!string.IsNullOrEmpty(documentBackID))
            {
                GameObject.Find("IDFrontSuccess").GetComponent<Image>().transform.localScale = Vector3.one;
                GameObject.Find("IDBackSuccess").GetComponent<Image>().transform.localScale = Vector3.one;
                GameObject.Find("IDPassport").GetComponent<Button>().interactable = false;
                disable("IDPassport");
            }
            else
            {
                GameObject.Find("IDPassportSuccess").GetComponent<Image>().transform.localScale = Vector3.one;
                GameObject.Find("IDFront").GetComponent<Button>().interactable = false;
                GameObject.Find("IDBack").GetComponent<Button>().interactable = false;
                disable("IDFront");
                disable("IDBack");
            }
        }
    }
    void personelInfoVerification(User user)
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
                Phone.text = tmp;
            else Phone.text = "+" + tmp;
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
    void ibanVerification(User user)
    {
        if (user.iban_uploaded == true)
        {
            ContinueButtonIBAN.interactable = true;
            AcceptedIban.transform.localScale = Vector3.one;
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
            return !string.IsNullOrEmpty(user.firstname) && !string.IsNullOrEmpty(user.lastname) && !string.IsNullOrEmpty(user.birthday) && !string.IsNullOrEmpty(user.adress) && !string.IsNullOrEmpty(user.city) && !string.IsNullOrEmpty(user.zipcode) && !string.IsNullOrEmpty(user.country);
    }
    void disable(string doc)
    {
        Image DocDisableimg = GameObject.Find(doc + "/disable").GetComponent<Image>();
        var tempColor = DocDisableimg.color;
        tempColor.a = 0.50f;
        DocDisableimg.color = tempColor;
    }
    public void Withdraw()
    {
        EventsController behaviourScript = new EventsController();
        behaviourScript.StepAnimation("circleEmpty3");
        WithdrawManager wm = new WithdrawManager();
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        string DocId = PlayerPrefs.GetString("DocId");
        UnityThreadHelper.CreateThread(() =>
        {
            string withdrawResult = null;
            withdrawResult = wm.payout(token, WithdrawPresenter.WithdrawMoney);
            Debug.Log("withdrawResult: " + withdrawResult);
            InfoPersonnelWithdraw.currentIdProof = null;
            InfoPersonnelWithdraw.currentIBAN = null;
            if (!string.IsNullOrEmpty(withdrawResult))
            {
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
                        behaviourScript.withdrawFailed("Withdrawal", null, WithdrawManager.WITHDRAW_INSUFFICIENT_AMOUNT_FAILED_MESSAGE);
                    });
                }
                else if (withdrawResult == WithdrawManager.WITHDRAW_ERROR_BALANCE_INSUFFICIENT)
                {
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        behaviourScript.withdrawFailed("Withdrawal", null, WithdrawManager.WITHDRAW_INSUFFICIENT_FUNDS_FAILED_MESSAGE);
                    });
                }
                else if (withdrawResult == "error")
                {
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        behaviourScript.withdrawFailed(null, null, WithdrawManager.WITHDRAW_FAILED_MESSAGE);
                    });
                }
                else if (withdrawResult == WithdrawManager.WITHDRAW_SUCCEEDED_STATUS)
                {
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        SceneManager.UnloadSceneAsync("Loader");
                        SceneManager.UnloadSceneAsync("WithdrawalInfo");
                        UserManager.CurrentMoney = (float.Parse(UserManager.CurrentMoney) - WithdrawPresenter.WithdrawMoney).ToString("N2").Replace(",", ".");
                        behaviourScript.backToWinMoney();
                        behaviourScript.ShowPopup("popupCongratWithdraw");
                    });
                }
            }
            else
            {
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    behaviourScript.withdrawFailed(null, null, WithdrawManager.WITHDRAW_FAILED_MESSAGE);
                });
            }
        });
    }
    private void tokenizeAndAttach()
    {
        if (!String.IsNullOrEmpty(InfoPersonnelWithdraw.currentIBAN))
        {
            SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
            UnityThreadHelper.CreateThread(() =>
            {
                string accounttoken = wm.TokenizeAccount(um.getUser(userId, token), InfoPersonnelWithdraw.currentIBAN);
                if (!String.IsNullOrEmpty(accounttoken))
                    if (wm.attachTokenToAccount(accounttoken, token))
                    {
                        string[] attrib = { "iban_uploaded" };
                        string[] value5 = { "true" };
                        um.UpdateUserByField(userId, token, attrib, value5);
                    }
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    SceneManager.UnloadScene("Loader");
                });
            });
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (string.IsNullOrEmpty(LastName.text) || string.IsNullOrEmpty(FirstName.text) || string.IsNullOrEmpty(Adress.text) || string.IsNullOrEmpty(city.text) || string.IsNullOrEmpty(zip.text) || string.IsNullOrEmpty(country.text) || string.IsNullOrEmpty(Birthday.text))
        {
            ContinueButton.interactable = false;
        }
        else
        {
            if (UserManager.CurrentCountryCode.ToLower().Equals("us"))
            {
                if (!string.IsNullOrEmpty(personal_id_number.text))
                    ContinueButton.interactable = true;
                else ContinueButton.interactable = false;
            }
            else ContinueButton.interactable = true;
        }
        if (validIban == true)
        {
            ContinueButtonIBAN.interactable = true;
        }
        else ContinueButtonIBAN.interactable = false;
    }
}
