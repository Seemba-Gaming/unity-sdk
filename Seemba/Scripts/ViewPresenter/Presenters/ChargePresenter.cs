using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
struct GradientColor
{
    public Color32 startColor; public Color32 endColor;
    public GradientColor(Color32 startColor, Color32 endColor)
    {
        this.startColor = startColor;
        this.endColor = endColor;
    }
}
[CLSCompliant(false)]

public class ChargePresenter : MonoBehaviour
{
    #region Script Parameters
    public Dropdown                         Years;
    public Dropdown                         Month;
    public InputField                       CardHolder;
    public InputField                       CardNumber;
    public InputField                       CVV;
    public Animator                         CardHolderError;
    public Animator                         CardNumberError;
    public Animator                         DAEError;
    public Animator                         CVVError;
    public Text                             Amount;
    public Button                           Credit;
    public Toggle                           TermsToggel;
    public GameObject                       Card;
    public Text                             card_Number;
    public Text                             card_NumberHint;
    public InputField                       card_Name;
    public InputField                       card_ExpireDate_Month;
    public InputField                       card_ExpireDate_Year;
    public InputField                       card_CVV;
    public string                           _paymentIntentID;
    public string                           paymentCard;
    #endregion

    #region Fields
    private GradientColor                   Orca = new GradientColor(new Color32(68, 160, 141, 255), new Color32(9, 54, 55, 255));
    private GradientColor                   Favourit = new GradientColor(new Color32(55, 48, 44, 255), new Color32(75, 75, 75, 255));
    private GradientColor                   Love = new GradientColor(new Color32(32, 1, 34, 255), new Color32(111, 0, 0, 255));
    private GradientColor                   Tonight = new GradientColor(new Color32(69, 104, 220, 255), new Color32(176, 106, 179, 255));
    private GradientColor                   Lagoon = new GradientColor(new Color32(67, 198, 172, 255), new Color32(25, 22, 84, 255));
    private GradientColor                   Roseanna = new GradientColor(new Color32(255, 175, 189, 255), new Color32(255, 195, 160, 255));
    private List<GradientColor>             CardColors = new List<GradientColor>();
    #endregion

    #region Unity Methods
    void OnEnable()
    {
        Amount.text = WalletScript.LastCredit.ToString("N2").Replace(",", ".") + CurrencyManager.CURRENT_CURRENCY;
    }
    void Start()
    {
        string content = "";
        AddColorsSet();
        setCardColor(new System.Random().Next(0, 5));
        Credit.onClick.AddListener(async () =>
        {
            await ChargeAsync();
        });
        TermsToggel.onValueChanged.AddListener(delegate
        {
            if (TermsToggel.isOn == true)
            {
                Credit.interactable = true;
            }
            else
            {
                Credit.interactable = false;
            }
        });
        CardHolder.onValueChanged.AddListener(delegate
        {
            card_Name.text = CardHolder.text;
            if (CardHolderError.GetBool("wrongcardholder") == true)
            {
                CardHolderError.SetBool("wrongcardholder", false);
            }
        });
        CardNumber.onValueChanged.AddListener(delegate
        {
            content = CardNumber.text;
            card_Number.text = separated(content);
            card_NumberHint.text = numberhintEditor(content);
            if (CardNumberError.GetBool("wrongcardnumber") == true)
            {
                CardNumberError.SetBool("wrongcardnumber", false);
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
            int _monthIndex = Month.value;
            string valueMonths = Month.options[_monthIndex].text;
            if (valueMonths != "MM")
            {
                card_ExpireDate_Month.text = valueMonths;
            }
            else
            {
                card_ExpireDate_Month.text = "";
            }

            if (DAEError.GetBool("daeerror") == true)
            {
                DAEError.SetBool("daeerror", false);
            }
        });
        Years.onValueChanged.AddListener(delegate
        {
            int _yearIndex = Years.value;
            string valueYears = Years.options[_yearIndex].text;
            if (valueYears != "YYYY")
            {
                card_ExpireDate_Year.text = valueYears.Substring(2, 2);
            }
            else
            {
                card_ExpireDate_Year.text = "";
            }

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
                {
                    Month.options.Add(new UnityEngine.UI.Dropdown.OptionData()
                    {
                        text = "0" + i.ToString()
                    });
                }
                else
                {
                    Month.options.Add(new UnityEngine.UI.Dropdown.OptionData()
                    {
                        text = i.ToString()
                    });
                }
            }
        }
        catch (NullReferenceException) { }
    }
    void Update()
    {
        if (CVV.isFocused)
        {
            Card.GetComponent<Animator>().SetBool("next", true);
        }
        else { Card.GetComponent<Animator>().SetBool("next", false); }
    }
    #endregion

    #region Methods
    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
    public async System.Threading.Tasks.Task ChargeAsync()
    {
        bool confirmData = true;
        int _yearIndex = Years.value;
        int _monthIndex = Month.value;
        string valueYears = Years.options[_yearIndex].text;
        string valueMonths = Month.options[_monthIndex].text;
        if (string.IsNullOrEmpty(CardHolder.text))
        {
            CardHolderError.SetBool("wrongcardholder", true);
            confirmData = false;
        }
        if (CardNumber.text.Length != 16 || string.IsNullOrEmpty(CardNumber.text))
        {
            CardNumberError.SetBool("wrongcardnumber", true);
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
            LoaderManager.Get.LoaderController.ShowLoader(null);
            await CreatePaymentMethodAsync(CardNumber.text, CVV.text, int.Parse(valueMonths), int.Parse(valueYears));
        }
    }
    #endregion

    #region Implementations
    private void setCardColor(int random)
    {
        Card.GetComponent<Gradient>().StartColor = CardColors.ElementAt(random).startColor;
        Card.GetComponent<Gradient>().EndColor = CardColors.ElementAt(random).endColor;
    }

    private void AddColorsSet()
    {
        CardColors.Add(Orca);
        CardColors.Add(Favourit);
        CardColors.Add(Love);
        CardColors.Add(Tonight);
        CardColors.Add(Lagoon);
        CardColors.Add(Roseanna);
    }

    private string numberhintEditor(string content)
    {
        string res = "";
        for (int i = 0; i < 16 - content.Length; i++)
        {
            res += "X";
        }
        return Reverse(separated(res));
    }
    private string separated(string content)
    {
        string res = "";
        for (int i = 0; i < content.Length; i++)
        {
            Debug.Log("i:" + i);
            if (i != 1 && i != 0 && i % 4 == 0)
            {
                res += " ";
            }
            res += content.ElementAt(i);
        }
        Debug.Log("res:" + res);
        return res;
    }
    private async System.Threading.Tasks.Task<bool> CreatePaymentMethodAsync(string card_number, string cvc, int card_expiry_month, int card_expiry_year)
    {
        string token = UserManager.Get.getCurrentSessionToken();

        string _paymentMethodID = await ChargeManager.Get.CreatePaymentMethodAsync(CardNumber.text, CVV.text, card_expiry_month, card_expiry_year);


        if (!_paymentMethodID.Equals("-1"))
        {
            JSONNode _paymentIntent = await ChargeManager.Get.CreatePaymentIntentAsync(_paymentMethodID, WalletScript.LastCredit, token);


            if (!_paymentIntent.Equals("-1"))
            {
                _paymentIntentID = _paymentIntent["data"]["id"].Value;
                if (_paymentIntent["success"].AsBool)
                {
                    if (Is3DSecure(_paymentIntent))
                    {
                        Confirm3DSecure(_paymentIntent);
                    }
                    else
                    {
                        if (_paymentIntent["status"].Value == ChargeManager.PAYMENT_STATUS_SUCCEEDED)
                        {
                            ChargeSucceeded();
                        }
                        else
                        {
                            ChargeCanceled();
                        }
                    }

                }
                else
                {
                    ChargeCanceled();
                }
            }
        }
        else
        {
            UnloadBankingInfo();
        }
        return true;
    }

    private bool Is3DSecure(JSONNode json)
    {
        return json["status"].Value == ChargeManager.PAYMENT_STATUS_REQUIRES_ACTION || json["status"].Value == ChargeManager.PAYMENT_STATUS_REQUIRES_SOURCE_ACTION;
    }
    private void Confirm3DSecure(JSONNode json)
    {
        var _3DSecureURL = json["redirect_url"].Value;
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

    private void SelectWinMoney()
    {
        ViewsEvents.Get.WinMoneyClick();
    }

    private void IsChargeCompleted()
    {
        string token = UserManager.Get.getCurrentSessionToken();
        UnityThreadHelper.CreateThread(async () =>
        {
            string chargeConfirmed = await ChargeManager.Get.isChargeConfirmedAsync(_paymentIntentID, token);
            Debug.Log("chargeConfirmed: " + chargeConfirmed);
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
        float credit = float.Parse(UserManager.Get.GetCurrentMoneyCredit()) + WalletScript.LastCredit;
        TranslationManager.scene = "Home";
        object[] _params = { TranslationManager.Get("congratulations"), TranslationManager.Get("transaction_accepted"), credit.ToString() + "€", "(+" + WalletScript.LastCredit + CurrencyManager.CURRENT_CURRENCY + ")", TranslationManager.Get("ok")+ " !" };
        LoaderManager.Get.LoaderController.HideLoader();
        PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_CONGRATS, _params);
        UserManager.Get.UpdateUserMoneyCredit(credit.ToString());
        //EncartPlayerPresenter.Init();
        //ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Menu.gameObject);
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
        LoaderManager.Get.LoaderController.HideLoader();
        SelectWinMoney();
    }
    #endregion
}
