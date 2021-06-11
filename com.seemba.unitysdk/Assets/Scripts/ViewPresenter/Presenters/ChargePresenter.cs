using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    struct GradientColor
    {
        public Color32 startColor; public Color32 endColor;
        public GradientColor(Color32 startColor, Color32 endColor)
        {
            this.startColor = startColor;
            this.endColor = endColor;
        }
    }
    public class ChargePresenter : MonoBehaviour
    {
        #region Script Parameters
        public Dropdown Years;
        public Dropdown Month;
        public InputField CardHolder;
        public InputField CardNumber;
        public InputField CVV;
        public Animator CardHolderError;
        public Animator CardNumberError;
        public Animator DAEError;
        public Animator CVVError;
        public Text Amount;
        public Button Credit;
        public Toggle TermsToggel;
        public GameObject Card;
        public Text card_Number;
        public Text card_NumberHint;
        public InputField card_Name;
        public InputField card_ExpireDate_Month;
        public InputField card_ExpireDate_Year;
        public InputField card_CVV;
        public string _paymentIntentID;
        #endregion

        #region Fields
        private GradientColor Orca = new GradientColor(new Color32(68, 160, 141, 255), new Color32(9, 54, 55, 255));
        private GradientColor Favourit = new GradientColor(new Color32(55, 48, 44, 255), new Color32(75, 75, 75, 255));
        private GradientColor Love = new GradientColor(new Color32(32, 1, 34, 255), new Color32(111, 0, 0, 255));
        private GradientColor Tonight = new GradientColor(new Color32(69, 104, 220, 255), new Color32(176, 106, 179, 255));
        private GradientColor Lagoon = new GradientColor(new Color32(67, 198, 172, 255), new Color32(25, 22, 84, 255));
        private GradientColor Roseanna = new GradientColor(new Color32(255, 175, 189, 255), new Color32(255, 195, 160, 255));
        private List<GradientColor> CardColors = new List<GradientColor>();
        private int mChargeStatus =  0; //0 = pending, 1 = succeeded, -1 = canceled
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
                SeembaAnalyticsManager.Get.SendCreditEvent("Click on crédit", WalletScript.LastCredit);
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
                SeembaAnalyticsManager.Get.SendCreditEvent("Wrong Card Holder Name", WalletScript.LastCredit);
                confirmData = false;
            }
            if (CardNumber.text.Length != 16 || string.IsNullOrEmpty(CardNumber.text))
            {
                SeembaAnalyticsManager.Get.SendCreditEvent("Wrong Card Number", WalletScript.LastCredit);
                CardNumberError.SetBool("wrongcardnumber", true);
                confirmData = false;
            }
            if (CVV.text.Length < 3 || string.IsNullOrEmpty(CVV.text))
            {
                SeembaAnalyticsManager.Get.SendCreditEvent("Wrong CVV Number", WalletScript.LastCredit);
                CVVError.SetBool("cvverror", true);
                confirmData = false;
            }
            if (valueYears == "YYYY" || valueMonths == "MM" || (valueYears.Equals(DateTime.Today.Year.ToString()) && int.Parse(valueMonths) < DateTime.Today.Month))
            {
                SeembaAnalyticsManager.Get.SendCreditEvent("Wrong Card Exp Date", WalletScript.LastCredit);
                DAEError.SetBool("daeerror", true);
                confirmData = false;
            }
            if (confirmData == true)
            {
                mChargeStatus = 0;
                LoaderManager.Get.LoaderController.ShowLoader(null);
                await CreatePaymentMethodAsync(CardNumber.text, CVV.text, int.Parse(valueMonths), int.Parse(valueYears));
            }
        }
        public void CleatInputs()
        {
            Years.value = 0;
            Month.value = 0;
            CardHolder.text = string.Empty;
            CardNumber.text = string.Empty;
            CVV.text = string.Empty;
            TermsToggel.isOn = false;
            ViewsEvents.Get.Menu.Wallet.GetComponent<WalletScript>().ChargeAmount.text = string.Empty;
            ViewsEvents.Get.Menu.SettingsWallet.ChargeAmount.text = string.Empty;
        }
        #endregion

        #region Implementations
        private void setCardColor(int random)
        {
            Card.GetComponent<SeembaGradient>().StartColor = CardColors.ElementAt(random).startColor;
            Card.GetComponent<SeembaGradient>().EndColor = CardColors.ElementAt(random).endColor;
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
                if (i != 1 && i != 0 && i % 4 == 0)
                {
                    res += " ";
                }
                res += content.ElementAt(i);
            }
            return res;
        }
        private async System.Threading.Tasks.Task<bool> CreatePaymentMethodAsync(string card_number, string cvc, int card_expiry_month, int card_expiry_year)
        {
            string token = UserManager.Get.getCurrentSessionToken();

            string _paymentMethodID = await ChargeManager.Get.CreatePaymentMethodAsync(CardNumber.text, CVV.text, card_expiry_month, card_expiry_year);


            if (!_paymentMethodID.Equals("-1"))
            {
                var _paymentIntent = await ChargeManager.Get.CreatePaymentIntentAsync(_paymentMethodID, WalletScript.LastCredit, token);

                if (!_paymentIntent.Equals("-1"))
                {
                    _paymentIntentID = _paymentIntent.data.payment_intent.id;
                    if (_paymentIntent.success)
                    {
                        if (Is3DSecure(_paymentIntent.data.payment_intent))
                        {
                            Confirm3DSecure(_paymentIntent.data);
                        }
                        else
                        {
                            Debug.LogWarning("here");
                            if (_paymentIntent.data.payment_intent.status == ChargeManager.PAYMENT_STATUS_SUCCEEDED)
                            {
                                mChargeStatus = 1;
                                ChargeSucceeded();
                            }
                            else
                            {
                                mChargeStatus = -1;
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

        private bool Is3DSecure(PaymentIntent paymentIntent)
        {
            return paymentIntent.status == ChargeManager.PAYMENT_STATUS_REQUIRES_ACTION || paymentIntent.status == ChargeManager.PAYMENT_STATUS_REQUIRES_SOURCE_ACTION;
        }
        private void Confirm3DSecure(PaymentIntentURL paymentIntent)
        {
            var _3DSecureURL = paymentIntent.redirect_url;
            OpenBrowserFor3dSecure(_3DSecureURL);
        }
        private void OpenBrowserFor3dSecure(string url)
        {
            Application.OpenURL(url);
            StartCoroutine(IsChargeCompletedCoroutine());
        }
        public IEnumerator IsChargeCompletedCoroutine()
        {
            while(mChargeStatus == 0)
            {
                yield return new WaitForSeconds(1f);
                IsChargeCompleted();
            }
        }
        private void SelectWinMoney()
        {
            ViewsEvents.Get.WinMoneyClick();
        }

        private async void IsChargeCompleted()
        {
                string token = UserManager.Get.getCurrentSessionToken();
                string chargeConfirmed = await ChargeManager.Get.isChargeConfirmedAsync(_paymentIntentID, token);
                Debug.Log("chargeConfirmed: " + chargeConfirmed);
                if (chargeConfirmed.Equals(ChargeManager.PAYMENT_STATUS_SUCCEEDED))
                {
                    mChargeStatus = 1;
                    ChargeSucceeded();
                }
                else if (chargeConfirmed.Equals(ChargeManager.PAYMENT_STATUS_REQUIRES_PAYMENT_METHOD))
                {
                    mChargeStatus = -1;
                    ChargeCanceled();
                }
        }
        private void ChargeSucceeded()
        {
            SeembaAnalyticsManager.Get.SendCreditEvent("Credit Succeeded", WalletScript.LastCredit);
            float credit = float.Parse(UserManager.Get.GetCurrentMoneyCredit()) + WalletScript.LastCredit;
            TranslationManager._instance.scene = "Home";
            object[] _params = { TranslationManager._instance.Get("congratulations"), TranslationManager._instance.Get("transaction_accepted"), (credit * 100).ToString() + "<sprite=1>", "( +" + WalletScript.LastCredit * 100 + "<sprite=1>"  + " )", TranslationManager._instance.Get("ok") + " !" };
            LoaderManager.Get.LoaderController.HideLoader();
            PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_CONGRATS, _params);
            UserManager.Get.UpdateUserMoneyCredit(credit.ToString());
        }
        private void ChargeCanceled()
        {
            SeembaAnalyticsManager.Get.SendCreditEvent("Credit Canceled", WalletScript.LastCredit);
            UnloadBankingInfo();
        }
        private void UnloadBankingInfo()
        {
            LoaderManager.Get.LoaderController.HideLoader();
            //SelectWinMoney();
        }
        #endregion
    }
}
