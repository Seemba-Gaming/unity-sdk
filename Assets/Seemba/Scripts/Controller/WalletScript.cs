using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class WalletScript : MonoBehaviour
{
    UserManager um = new UserManager();
    public static float LastCredit;
    public static bool showPopupMail;
    public static string moneyToAdd;
    public Button Continue, ContinuePayment, ContinueAgeVerification, closePopupWallet, ContinueCredit;
    public InputField Email, password, amount;
    public GameObject Loading, popupWallet;
    public Toggle masterCard, visa, paypal, AgeToggle, euro5, euro10, euro15, euro20, otherAmount;
    public Text creditText, OtherAmount, selected5, selected10, selected15, selected20, text5, text10, text15, text20;
    EventsController nbs = new EventsController();
    //public GoogleAnalyticsV4 googleAnalytics; 
    Text textDatePredefined;
    // Use this for initialization
    public void creditClick()
    {
        Debug.Log(moneyToAdd);
        Debug.Log("credit+ UserManager.CurrentCountryCode: " + UserManager.CurrentCountryCode);
        UserManager um = new UserManager();
        if (string.IsNullOrEmpty(UserManager.CurrentCountryCode))
        {
            UserManager.CurrentCountryCode = um.GetGeoLoc();
        }
        if (CountryController.checkCountry(UserManager.CurrentCountryCode) == true)
        {
            StartCoroutine(nbs.checkInternetConnection((isConnected) =>
            {
                Debug.Log("checked country");
                SceneManager.UnloadSceneAsync("Loader");
                if (isConnected == true)
                {



                    switch (moneyToAdd)
                    {
                        case "5":
                            nbs.Add5Euro();
                            WalletScript.LastCredit = 5;
                            break;
                        case "10":
                            nbs.Add10Euro();
                            WalletScript.LastCredit = 10;
                            break;
                        case "15":
                            nbs.Add15Euro();
                            WalletScript.LastCredit = 15;
                            break;
                        case "20":
                            nbs.Add20Euro();
                            WalletScript.LastCredit = 20;
                            break;
                        default:
                            nbs.AddxEuro(float.Parse(moneyToAdd, CultureInfo.InvariantCulture.NumberFormat));
                            WalletScript.LastCredit = float.Parse(moneyToAdd, CultureInfo.InvariantCulture.NumberFormat);
                            break;
                    }
                }
                else
                {
                    try
                    {
                        SceneManager.UnloadSceneAsync("ConnectionFailed");
                    }
                    catch (ArgumentException ex) { }
                    ConnectivityController.CURRENT_ACTION = ConnectivityController.CREDIT_ACTION;
                    SceneManager.LoadScene("ConnectionFailed", LoadSceneMode.Additive);
                }
            }));
        }
        else
        {
            UnityThreading.ActionThread thread, thr;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                Thread.Sleep(300);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    EventsController nbs = new EventsController();
                    nbs.ShowPopupError("popupProhibitedLocationWallet");
                });
            });
        }
    }
    void OnDisable()
    {
    }
    public void OnEnable()
    {
        string UserId = um.getCurrentUserId();
        string UserToken = um.getCurrentSessionToken();
        try
        {
            closePopupWallet.onClick.AddListener(() =>
            {
                popupWallet.transform.localScale = Vector3.zero;
            });
        }
        catch (NullReferenceException ex)
        {
        }
        try
        {
            Continue.interactable = false;
            ContinuePayment.interactable = false;
        }
        catch (NullReferenceException ex)
        {
        }
        try
        {
            textDatePredefined = GameObject.Find("textDatePredefined").GetComponent<Text>();
        }
        catch (NullReferenceException ex)
        {
        }
        try
        {
            amount.onValueChanged.AddListener(delegate
            {
                if (string.IsNullOrEmpty(amount.text))
                {
                    GameObject.Find("EuroLabel").GetComponent<Text>().text = "";
                }
                else
                {
                    GameObject.Find("EuroLabel").GetComponent<Text>().text = CurrencyManager.CURRENT_CURRENCY;
                }
            });
        }
        catch (NullReferenceException ex)
        {
        }
        if (LastCredit != 0)
        {
            try
            {
                GameObject.Find("TextMain").GetComponent<Text>().text = float.Parse(LastCredit.ToString()).ToString("N2");
                LastCredit = 0;
            }
            catch (NullReferenceException ex)
            {
            }
        }
        else
        {
        }
    }
    private bool ValidMail(string mail_address)
    {
        Regex myRegex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.IgnoreCase);
        return myRegex.IsMatch(mail_address);
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            if (visa.isOn)
            {
                ChargePresenter.paymentCard = "Visa";
            }
        }
        catch (NullReferenceException ex) { }
        try
        {
            if (masterCard.isOn)
            {
                ChargePresenter.paymentCard = "Mastercard";
            }
        }
        catch (NullReferenceException ex) { }
        try
        {
            if (paypal.isOn == true)
                ContinuePayment.interactable = false;
            else
                ContinuePayment.interactable = true;
        }
        catch (NullReferenceException ex)
        {
        }
        try
        {
            if (textDatePredefined.text != "Enter Date" && AgeToggle.isOn == true)
                ContinueAgeVerification.interactable = true;
            else
                ContinueAgeVerification.interactable = false;
        }
        catch (NullReferenceException)
        {
        }
        try
        {
            if (euro5.isOn == true)
            {
                creditText.text = "credit 5,00" + CurrencyManager.CURRENT_CURRENCY;
                text5.color = new Color(1f, 1f, 1f);
                selected5.text = "selected";
                selected5.color = new Color(1f, 1f, 1f);
                amount.text = "";
                text10.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected10.text = "select";
                selected10.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                text15.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected15.text = "select";
                selected15.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                text20.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected20.text = "select";
                selected20.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                moneyToAdd = "5";
            }
            if (euro10.isOn == true)
            {
                creditText.text = "credit 10,00" + CurrencyManager.CURRENT_CURRENCY;
                text10.color = new Color(1f, 1f, 1f);
                selected10.color = new Color(1f, 1f, 1f);
                selected10.text = "selected";
                amount.text = "";
                text5.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected5.text = "select";
                selected5.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                text15.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected15.text = "select";
                selected15.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                text20.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected20.text = "select";
                selected20.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                moneyToAdd = "10";
            }
            if (euro15.isOn == true)
            {
                creditText.text = "credit 15,00" + CurrencyManager.CURRENT_CURRENCY;
                text15.color = new Color(1f, 1f, 1f);
                selected15.color = new Color(1f, 1f, 1f);
                selected15.text = "selected";
                //amount.text="";
                text5.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected5.text = "select";
                selected5.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                text10.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected10.text = "select";
                selected10.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                text20.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected20.text = "select";
                selected20.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                moneyToAdd = "15";
            }
            if (euro20.isOn == true)
            {
                creditText.text = "credit 20,00" + CurrencyManager.CURRENT_CURRENCY;
                text20.color = new Color(1f, 1f, 1f);
                selected20.color = new Color(1f, 1f, 1f);
                selected20.text = "selected";
                amount.text = "";
                text5.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected5.text = "select";
                selected5.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                text10.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected10.text = "select";
                selected10.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                text15.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected15.text = "select";
                selected15.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                moneyToAdd = "20";
            }
            if (otherAmount.isOn == true)
            {
                text20.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                selected20.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                selected20.text = "select";
                text5.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected5.text = "select";
                selected5.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                text10.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected10.text = "select";
                selected10.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                text15.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected15.text = "select";
                selected15.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                text20.color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
                selected20.text = "select";
                selected20.color = new Color(188 / 255f, 182 / 255f, 194 / 255f);
                if (amount.text != "" && float.Parse(OtherAmount.text, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture) > 0)
                {
                    creditText.text = "credit " + float.Parse(OtherAmount.text, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture).ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                    ContinueCredit.interactable = true;
                    moneyToAdd = amount.text;
                }
                else
                {
                    creditText.text = "credit";
                    ContinueCredit.interactable = false;
                }
            }
            if (amount.isFocused == true)
            {
                otherAmount.isOn = true;
            }
            if (euro5.isOn == true || euro10.isOn == true || euro15.isOn == true || euro20.isOn == true)
                ContinueCredit.interactable = true;
        }
        catch (NullReferenceException ex)
        {
        }
    }
}