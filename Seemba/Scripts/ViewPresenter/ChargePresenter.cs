using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Linq;
public class ChargePresenter : MonoBehaviour
{
    UserManager um = new UserManager();
    ChargeManager charge = new ChargeManager();
    InputField cardHolder, cardNumber, CVV;
    Animator cardHolderError, cardNumberError, DAEError, CVVError;
    Dropdown Years, Month;
    Text Amount;
    Button Credit;
    Toggle TermsToggel;
    public string _paymentIntentID;
    public string user_id;
    public string token;
    private static bool isBackAfterPayment;
    private string current_client_secret;
    [SerializeField] private GameObject Card;
    [SerializeField] private Image card_MasterCard, card_Visa;
    [SerializeField] private Text card_Number, card_NumberHint, card_ExpireDate;
    [SerializeField] private InputField card_Name, card_ExpireDate_Month, card_ExpireDate_Year, card_CVV;
    public static string paymentCard;
    struct GradientColor
    {
        public Color32 startColor; public Color32 endColor;
        public GradientColor(Color32 startColor, Color32 endColor)
        {
            this.startColor = startColor;
            this.endColor = endColor;
        }
    }
    GradientColor Orca = new GradientColor(new Color32(68, 160, 141, 255), new Color32(9, 54, 55, 255));
    GradientColor Favourit = new GradientColor(new Color32(55, 48, 44, 255), new Color32(75, 75, 75, 255));
    GradientColor Love = new GradientColor(new Color32(32, 1, 34, 255), new Color32(111, 0, 0, 255));
    GradientColor Tonight = new GradientColor(new Color32(69, 104, 220, 255), new Color32(176, 106, 179, 255));
    GradientColor Lagoon = new GradientColor(new Color32(67, 198, 172, 255), new Color32(25, 22, 84, 255));
    GradientColor Roseanna = new GradientColor(new Color32(255, 175, 189, 255), new Color32(255, 195, 160, 255));
    List<GradientColor> CardColors = new List<GradientColor>();
    // Use this for initialization
    void Start()
    {
        user_id = um.getCurrentUserId();
        token = um.getCurrentSessionToken();


        CardColors.Add(Orca);
        CardColors.Add(Favourit);
        CardColors.Add(Love);
        CardColors.Add(Tonight);
        CardColors.Add(Lagoon);
        CardColors.Add(Roseanna);
        setCardColor(new System.Random().Next(0, 5));



        cardHolderError = GameObject.Find("CardHolderPanel").GetComponent<Animator>();
        cardNumberError = GameObject.Find("CardNumberPanel").GetComponent<Animator>();
        DAEError = GameObject.Find("DAEPanel").GetComponent<Animator>();
        CVVError = GameObject.Find("CVVPanel").GetComponent<Animator>();
        cardHolder = GameObject.Find("card holder").GetComponent<InputField>();
        cardNumber = GameObject.Find("card number").GetComponent<InputField>();
        CVV = GameObject.Find("CVV").GetComponent<InputField>();
        Years = GameObject.Find("Years").GetComponent<Dropdown>();
        Month = GameObject.Find("Months").GetComponent<Dropdown>();
        GameObject.Find("Card/Front/" + paymentCard).GetComponent<Image>().transform.localScale = Vector3.one;
        GameObject.Find("Card/Back/" + paymentCard).GetComponent<Image>().transform.localScale = Vector3.one;
        //Credit Amount
        Amount = GameObject.Find("Amount").GetComponent<Text>();
        //Toggle
        TermsToggel = GameObject.Find("ToggleTerms").GetComponent<Toggle>();
        //charge 
        Credit = GameObject.Find("Credit").GetComponent<Button>();
        Credit.onClick.AddListener(() =>
        {
            Charge();
        });
        try
        {
            Amount.text = WalletScript.LastCredit.ToString("N2").Replace(",", ".") + CurrencyManager.CURRENT_CURRENCY;
            TermsToggel.onValueChanged.AddListener(delegate
            {
                if (TermsToggel.isOn == true)
                {
                    Credit.interactable = true;
                }
                else
                    Credit.interactable = false;
            });
        }
        catch (NullReferenceException)
        {
        }
        cardHolder.onValueChanged.AddListener(delegate
        {
            card_Name.text = cardHolder.text;
            if (cardHolderError.GetBool("wrongcardholder") == true)
            {
                cardHolderError.SetBool("wrongcardholder", false);
            }
        });
        string content = "";
        cardNumber.onValueChanged.AddListener(delegate
        {
            content = cardNumber.text;
            card_Number.text = separated(content);
            Debug.Log(content);
            card_NumberHint.text = numberhintEditor(content);
            if (cardNumberError.GetBool("wrongcardnumber") == true)
            {
                cardNumberError.SetBool("wrongcardnumber", false);
            }
        });
        CVV.onValueChanged.AddListener(delegate
        {
            card_CVV.text = CVV.text;
            if (CVVError.GetBool("cvverror") == true)
            {
                CVVError.SetBool("cvverror", false);
            }
        });
        Month.onValueChanged.AddListener(delegate
        {
            int _monthIndex = GameObject.Find("Months").GetComponent<Dropdown>().value;
            string valueMonths = GameObject.Find("Months").GetComponent<Dropdown>().options[_monthIndex].text;
            if (valueMonths != "MM")
                card_ExpireDate_Month.text = valueMonths;
            else card_ExpireDate_Month.text = "";
            if (DAEError.GetBool("daeerror") == true)
            {
                DAEError.SetBool("daeerror", false);
            }
        });
        Years.onValueChanged.AddListener(delegate
        {
            int _yearIndex = GameObject.Find("Years").GetComponent<Dropdown>().value;
            string valueYears = GameObject.Find("Years").GetComponent<Dropdown>().options[_yearIndex].text;
            if (valueYears != "YYYY")
                card_ExpireDate_Year.text = valueYears.Substring(2, 2);
            else card_ExpireDate_Year.text = "";
            if (DAEError.GetBool("daeerror") == true)
            {
                DAEError.SetBool("daeerror", false);
            }
        });

        try
        {
            for (int i = DateTime.Today.Year; i < DateTime.Today.Year + 50; i++)
            {
                Years.options.Add(new UnityEngine.UI.Dropdown.OptionData()
                {
                    text = i.ToString()
                });
            }
            for (int i = 1; i <= 12; i++)
            {
                if (i < 10)
                    Month.options.Add(new UnityEngine.UI.Dropdown.OptionData()
                    {
                        text = "0" + i.ToString()
                    });
                else
                    Month.options.Add(new UnityEngine.UI.Dropdown.OptionData()
                    {
                        text = i.ToString()
                    });
            }
        }
        catch (NullReferenceException ex) { }
    }
    private void setCardColor(int random)
    {
        Debug.Log(random);
        Card.GetComponent<Gradient>().StartColor = CardColors.ElementAt(random).startColor;
        Card.GetComponent<Gradient>().EndColor = CardColors.ElementAt(random).endColor;
    }
    string numberhintEditor(string content)
    {
        string res = "";
        for (int i = 0; i < 16 - content.Length; i++)
        {
            res += "X";
        }
        return Reverse(separated(res));
    }
    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
    string separated(string content)
    {
        string res = "";
        for (int i = 0; i < content.Length; i++)
        {

            if (i != 1 && i != 0 && i % 4 == 0)
            {
                res += " ";
            }
            res += content.ElementAt(i);
        }
        return res;
    }
    public void Charge()
    {

        //If the credit is directly accepted
        const string Chargeable = "chargeable";
        //If the credit is directly accepted
        const string Succeeded = "succeeded";
        //IN Case of 3DS Card
        const string Pending = "pending";
        //Debug.Log("confirmCredit");
        bool confirmData = true;
        UnityThreading.ActionThread thread;
        cardHolderError = GameObject.Find("CardHolderPanel").GetComponent<Animator>();
        cardNumberError = GameObject.Find("CardNumberPanel").GetComponent<Animator>();
        DAEError = GameObject.Find("DAEPanel").GetComponent<Animator>();
        CVVError = GameObject.Find("CVVPanel").GetComponent<Animator>();
        int _yearIndex = GameObject.Find("Years").GetComponent<Dropdown>().value;
        int _monthIndex = GameObject.Find("Months").GetComponent<Dropdown>().value;
        string valueYears = GameObject.Find("Years").GetComponent<Dropdown>().options[_yearIndex].text;
        string valueMonths = GameObject.Find("Months").GetComponent<Dropdown>().options[_monthIndex].text;
        if (string.IsNullOrEmpty(cardHolder.text))
        {
            cardHolderError.SetBool("wrongcardholder", true);
            confirmData = false;
        }
        if (cardNumber.text.Length != 16 || string.IsNullOrEmpty(cardNumber.text))
        {
            cardNumberError.SetBool("wrongcardnumber", true);
            confirmData = false;
        }
        if (CVV.text.Length < 3 || string.IsNullOrEmpty(CVV.text))
        {
            CVVError.SetBool("cvverror", true);
            confirmData = false;
        }
        if (valueYears == "YYYY" || valueMonths == "MM" || (valueYears.Equals(DateTime.Today.Year.ToString()) && int.Parse(valueMonths) < DateTime.Today.Month))
        {
            DAEError.SetBool("daeerror", true);
            confirmData = false;
        }
        if (confirmData == true)
        {
            SceneManager.LoadScene("Loader", LoadSceneMode.Additive);

            thread = UnityThreadHelper.CreateThread(() =>
            {
                try
                {
                    string _paymentMethodID = charge.CreatePaymentMethod(cardNumber.text, CVV.text, int.Parse(valueMonths), int.Parse(valueYears));
                    
                    if (!String.IsNullOrEmpty(_paymentMethodID))
                    {
                        JSONNode _paymentIntent = charge.CreatePaymentIntent(_paymentMethodID, WalletScript.LastCredit, token);
                        _paymentIntentID = _paymentIntent["data"]["id"].Value;

                        // Response of Seemba API
                        if (_paymentIntent["success"].AsBool)
                        {
                            //check if 3D Secure needed from Stripe
                            if (Is3DSecure(_paymentIntent))
                            {
                                Debug.Log("Confirm3DSecure");
                                Confirm3DSecure(_paymentIntent);
                            }
                            else
                            {   // 3D Secure not required, directly check payment intent status
                                Debug.Log("3D Secure not required, directly check payment intent status");
                                UnityThreadHelper.Dispatcher.Dispatch(() =>
                                {
                                    InvokeRepeating("IsChargeCompleted", 0.0f, 1f);
                                });
                            }
                            //extract URL and open browser
                            UnityThreadHelper.Dispatcher.Dispatch(() =>
                            {
                                isBackAfterPayment = false;

                            });
                        }
                        else
                        {
                            //Charge Refused : not confirmed
                            ChargeCanceled();
                        }

                    }
                    else
                    {
                        UnityThreadHelper.Dispatcher.Dispatch(() =>
                        {
                            //turn back to trophy with Ouups popup
                            UnloadBankingInfo();
                        });
                    }
                }
                catch (NullReferenceException e)
                {
                    Debug.Log(e);
                }
            });
        }
    }
    private bool Is3DSecure(JSONNode json)
    {
        Debug.Log("status:" + json["status"].Value);
        return json["status"].Value == ChargeManager.PAYMENT_STATUS_REQUIRES_ACTION || json["status"].Value == ChargeManager.PAYMENT_STATUS_REQUIRES_SOURCE_ACTION;
    }
    private void Confirm3DSecure(JSONNode json)
    {
        var _3DSecureURL = json["redirect_url"].Value;
        Debug.Log("_3DSecureURL: " + json["redirect_url"].Value);
        OpenBrowserFor3dSecure(_3DSecureURL);
    }
    private void OpenBrowserFor3dSecure(string url)
    {
        UnityThreadHelper.Dispatcher.Dispatch(() =>
        {
            Application.OpenURL(url);
            InvokeRepeating("IsChargeCompleted", 0.0f, 1f);
        });
    }
    void SelectWinMoney()
    {
        ViewsEvents viewEvents = new ViewsEvents();
        viewEvents.WinMoneyClick();
        BottomMenuController bottomMenu = BottomMenuController.getInstance();
        bottomMenu.selectWinMoney();
        bottomMenu.unselectSettings();
        bottomMenu.unselectHome();
        bottomMenu.unselectHaveFun();
    }
    void IsChargeCompleted()
    {
        string token = um.getCurrentSessionToken();
        Debug.Log("chargeConfirmed:");
        UnityThreadHelper.CreateThread(() =>
        {
            string chargeConfirmed = charge.isChargeConfirmed(_paymentIntentID, token);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                if (chargeConfirmed.Equals(ChargeManager.PAYMENT_STATUS_SUCCEEDED))
                {
                    CancelInvoke();
                    ChargeSucceeded();
                }
                else if (chargeConfirmed.Equals(ChargeManager.PAYMENT_STATUS_REQUIRES_PAYMENT_METHOD))
                {
                    CancelInvoke();
                    ChargeCanceled();
                }
            });
        });
    }
    private void ChargeSucceeded()
    {

        UnityThreadHelper.CreateThread(() =>
        {
            User user = um.getUser(user_id, token);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                Debug.Log("-------- User Credit:" + user.money_credit);

                // Payment Completed 
                isBackAfterPayment = false;
                SelectWinMoney();
                EventsController nbs = new EventsController();
                nbs.ShowPopup("popupCongrat");
                UserManager.CurrentUser.money_credit = user.money_credit;
                EncartPlayerPresenter.Init();
                Text TextMain = GameObject.Find("TextMain").GetComponent<Text>();
                TextMain.text = user.money_credit.ToString() + CurrencyManager.CURRENT_CURRENCY;
                Text lastCreditValue = GameObject.Find("lastCreditValue").GetComponent<Text>();
                lastCreditValue.text = "(+" + WalletScript.LastCredit.ToString("N2") + CurrencyManager.CURRENT_CURRENCY + ")";

                try
                {
                    SceneManager.UnloadScene("Loader");
                }
                catch (Exception ex)
                {
                }
                SceneManager.UnloadScene("BankingInformation");
            });
        });
    }
    private void ChargeCanceled()
    {
        UnityThreadHelper.Dispatcher.Dispatch(() =>
        {
            UnloadBankingInfo();
        });
    }
    private void UnloadBankingInfo()
    {
        SceneManager.UnloadScene("Loader");
        SelectWinMoney();
        SceneManager.UnloadScene("BankingInformation");
        new EventsController().ShowPopupError("popupOups");
    }
    // Update is called once per frame
    void Update()
    {
        if (CVV.isFocused)
        {
            Card.GetComponent<Animator>().SetBool("next", true);
        }
        else { Card.GetComponent<Animator>().SetBool("next", false); }
    }
}
