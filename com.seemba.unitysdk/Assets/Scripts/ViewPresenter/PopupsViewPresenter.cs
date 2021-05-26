﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SeembaSDK
{
    #pragma warning disable 649
    public class PopupsViewPresenter : MonoBehaviour
    {
        #region static
        private static PopupsViewPresenter _Instance;
        public static bool isActive;
        public static bool isBackgroundActive;
        #endregion

        #region script parameters
        public object[] _params;
        public PopupType popup;

        [Header("blur_background")]
        [SerializeField]
        private Image overlay;

        [Header("popup info")]
        public Animator PopupInfoAnimator;
        public Text PopupInfoTitle;
        public Text PopupInfoSubtitle;
        public Text PopupInfoMessage;
        public Button PopupInfoConfirmButton;
        public Text PopupInfoConfirmButtonText;
        public Button PopupInfoCancelButton;
        public Button PopupInfoSendAgainButton;
        public Toggle DoNotShowAgain;
        public Text DoNotShowAgainText;

        [Header("popup payment")]
        public Animator PopupPaymentAnimator;
        public Text PopupPaymentTitle;
        public Text PopupPaymentSubtitle;
        public Button PopupPaymentConfirmButton;
        public Text PopupPaymentConfirmButtonText;
        public Button PopupPaymentCancelButton;
        public Toggle VisaToggle;
        public Toggle MasterCardToggle;

        [Header("popup Age Verification")]
        public Animator PopupAgeAnimator;
        public Text PopupAgeSubtitle;
        public Text PopupAgeTitle;
        public Text PopupAgeMessage;
        public Text PopupAgePlaceHolder;
        public Text PopupAgeConfirmButtonText;
        public DatePickerAgeVerification PopupAgeVerification;
        public Button PopupAgeCancelButton;
        public Button PopupAgeconfirmButton;
        public Button PopupAgeDatePicker;

        [Header("popup Current Password")]
        public Animator PopupCurrentPassAnimator;
        public Text PopupCurrentPassTitle;
        public Text PopupCurrentPassSubtitle;
        public InputField PopupCurrentPassInputField;
        public Text PopupCurrentPassInputFieldPlaceholderText;
        public Button PopupCurrentPassconfirmButton;
        public Text PopupCurrentPassConfirmButtonText;
        public Button PopupCurrentPassCancelButton;

        [Header("popup Update Password")]
        public Animator PopupUpdatePassAnimator;
        public Text PopupUpdatePassTitle;
        public Text PopupUpdatePassSubtitle;
        public InputField PopupUpdatePassNewPasswordInputField;
        public InputField PopupUpdatePassConfirmPasswordInputField;
        public Text PopupUpdatePassNewPasswordInputFieldText;
        public Text PopupUpdatePassConfirmPasswordInputFieldText;
        public Button PopupUpdatePassconfirmButton;
        public Text PopupUpdatePassConfirmButtonText;
        public Button PopupUpdatePassCancelButton;

        [Header("popup Forget Password")]
        public Animator PopupForgetPassAnimator;
        public Text PopupForgetPassTitle;
        public Text PopupForgetPassSubtitle;
        public InputField PopupForgetPassEmailInputField;
        public Text PopupForgetPassEmailInputFieldPlaceholder;
        public Text PopupForgetPassMessage;
        public Button PopupForgetPassconfirmButton;
        public Text PopupForgetPassConfirmButtonText;
        public Button PopupForgetPassCancelButton;

        [Header("popup Win")]
        public Animator PopupWinAnimator;
        public TextMeshProUGUI PopupWinAmout;
        public Button PopupWinConfirmButton;
        public Text PopupWinConfirmButtonText;

        [Header("popup Tournament Draw")]
        public Animator PopupTournamentDrawAnimator;
        public TextMeshProUGUI PopupDrawTitle;
        public TextMeshProUGUI PopupDrawSubtitle;
        public Image PopupDrawMyImage;
        public Image PopupDrawOpponentImage;
        public TextMeshProUGUI PopupDrawMyScore;
        public TextMeshProUGUI PopupDrawOpponentScore;
        public TextMeshProUGUI PopupDrawTimeLimitInfo;
        public Button PopupDrawPlayNowButton;
        public Button PopupDrawPlayLaterButton;
        public TextMeshProUGUI PopupDrawPlayNowButtonText;
        public TextMeshProUGUI PopupDrawPlayLaterButtonText;

        [Header("popup Congrats")]
        public Animator PopupCongrats;
        public Text PopupCongratsTitle;
        public Text PopupCongratsSubtitle;
        public TextMeshProUGUI PopupCongratsTotal;
        public TextMeshProUGUI PopupCongratsAddedValue;
        public Button PopupCongratsConfirmButton;
        public Text PopupCongratsConfirmButtonText;

        [Header("popup Congrats Withdrawal")]
        public Animator PopupCongratsWithdrawal;
        public Text PopupCongratsWithdrawalTitle;
        public Text PopupCongratsWithdrawalSubtitle;
        public Text PopupCongratsWithdrawalDesc;
        public Text PopupCongratsWithdrawalWorkingDaysDesc;
        public Button PopupCongratsWithdrawalConfirmButton;
        public Text PopupCongratsWithdrawalConfirmButtonText;

        [Header("popup Choose Character")]
        public Animator PopupchooseCharacter;
        public Text PopupchooseCharacterTitle;
        public Transform PopupchooseCharacterToggles;

        [Header("popup duels")]
        public Animator popup_duels_animator;
        public Text entry_fee;
        //public Text gain;
        public TextMeshProUGUI Gain;
        public Text bubbles_text;
        public Image bubbles_image;
        public Text crown_text;
        public Image crown_image;
        public Button cancel_duels;
        public Button confirm_duels;

        [Header("popup gift card")]
        public Animator PopupGiftCardAnimator;
        public Text PopupGiftCardTitle;
        public Text PopupGiftCardSubtitle;
        public Text PopupGiftCardDesc;
        public Text PopupGiftCardPrice;
        public Image PopupGiftCardImage;
        public Button PopupGiftCardConfirmButton;
        public Text PopupGiftCardConfirmButtonText;

        [Header("popup balance Insufficent")]
        public Animator PopupBalanceInsufficentAnimator;
        public Text PopupBalanceInsufficentTitle;
        public Text PopupBalanceInsufficentSubtitle;
        public Text PopupBalanceInsufficentText;
        public Text PopupBalanceInsufficentConfirmButtonText;
        public Button PopupBalanceInsufficentConfirmButton;
        public Text PopupBalanceInsufficentPlayFunButtonText;
        public Button PopupBalanceInsufficentPlayFunButton;

        #endregion

        #region Unity Methods
        private void Start()
        {
            isActive = true;
            _Instance = this;
            Action();
        }
        private void OnDestroy()
        {
            isActive = false;
        }
        #endregion

        #region Methods
        public void InitPopupInfo(string title, string subtitle, string message, string buttonText)
        {
            PopupInfoTitle.text = title;
            PopupInfoSubtitle.text = subtitle;
            PopupInfoMessage.text = message;
            PopupInfoConfirmButtonText.text = buttonText;
            PopupManager.Get.PopupController.PopupInfo.SetBool("Show", true);
        }
        public void Action()
        {
            confirm_duels.onClick.AddListener(() =>
            {
                Debug.LogWarning("here");
                HidePopupContent(popup_duels_animator);
                if (_params[3].ToString().Equals(ChallengeManager.CHALLENGE_TYPE_1V1))
                {
                    StartCoroutine(StartDuels());
                }

                if (_params[3].ToString().Equals(ChallengeManager.CHALLENGE_TYPE_BRACKET))
                {
                    StartCoroutine(StartTournament());
                }
            });

            PopupAgeconfirmButton.onClick.AddListener(() =>
            {
                StartCoroutine(OnClickConfirmAegVerificationButton());
            });

            PopupPaymentConfirmButton.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendCreditEvent("Go to BankingInfo", WalletScript.LastCredit);
                EventsController.Get.continuePayment();
            });

            VisaToggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    ViewsEvents.Get.BankingInfo.paymentCard = "Visa";
                }
            });

            MasterCardToggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    ViewsEvents.Get.BankingInfo.paymentCard = "Mastercard";
                }
            });

            PopupAgeDatePicker.onClick.AddListener(() =>
            {
                EventsController.Get.ShowDatePicker("credit");
            });

        }

        public void ClearPopups()
        {
            PopupForgetPassEmailInputField.text = string.Empty;
            PopupCurrentPassInputField.text = string.Empty;
            PopupUpdatePassNewPasswordInputField.text = string.Empty;
            PopupUpdatePassConfirmPasswordInputField.text = string.Empty;
        }

        public void ShowInfoPopup(object[] _params)
        {
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_params[0].ToString(), _params[1].ToString(), _params[2].ToString(), _params[3].ToString());
            PopupInfoConfirmButton.onClick.RemoveAllListeners();
        }
        public async void ShowTournamentDrawPopupAsync(object[] _params)
        {
            ShowPopupContent(PopupManager.Get.PopupController.PopupTournamentDraw.gameObject);
            PopupDrawTitle.text = _params[0].ToString();
            PopupDrawSubtitle.text = _params[1].ToString();
            var sprite = await UserManager.Get.getAvatar(_params[2].ToString());
            if (sprite != null)
            {
                PopupDrawMyImage.sprite = sprite;

            }
            var sprite_1 = await UserManager.Get.getAvatar(_params[3].ToString());
            if (sprite_1 != null)
            {
                PopupDrawOpponentImage.sprite = sprite_1;
            }
            PopupDrawMyScore.text = _params[4].ToString();
            PopupDrawOpponentScore.text = _params[5].ToString();
            PopupDrawTimeLimitInfo.text = _params[6].ToString() + " " + _params[7].ToString();
            PopupDrawPlayNowButtonText.text = _params[8].ToString();
            PopupDrawPlayLaterButtonText.text = _params[9].ToString();
        }
        public void ShowInsufficientBalancePopup(object[] _params)
        {
 
            PopupBalanceInsufficentTitle.text = _params[0].ToString();
            PopupBalanceInsufficentSubtitle.text = _params[1].ToString();
            PopupBalanceInsufficentText.text = _params[2].ToString();
            PopupBalanceInsufficentConfirmButtonText.text = _params[3].ToString();
            PopupBalanceInsufficentPlayFunButtonText.text = _params[4].ToString();
            PopupBalanceInsufficentConfirmButton.onClick.AddListener(() =>
            {
                HidePopupContent(PopupManager.Get.PopupController.PopupInsufficentBalance);
                EventsController.Get.OpenWallet("WinMoney");
            });
            PopupBalanceInsufficentPlayFunButton.onClick.AddListener(() =>
            {
                HidePopupContent(PopupManager.Get.PopupController.PopupInsufficentBalance);
                ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Menu.HaveFun);
            });
        }
        public void ShowPasswordUpdatedPopup(object[] _param)
        {
            _params = _param;
            PopupInfoCancelButton.gameObject.SetActive(false);
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_param[0].ToString(), _param[1].ToString(), _param[2].ToString(), _param[3].ToString());
            PopupInfoConfirmButton.onClick.RemoveAllListeners();
        }
        public void ShowEqualityRefundPopup(object[] _param)
        {
            _params = _param;
            PopupInfoCancelButton.gameObject.SetActive(false);
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_param[0].ToString(), _param[1].ToString(), _param[2].ToString(), _param[3].ToString());
        }
        public void ShowNotAuthorizedPopup(object[] _param)
        {
            _params = _param;
            PopupInfoCancelButton.gameObject.SetActive(false);
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_param[0].ToString(), _param[1].ToString(), _param[2].ToString(), _param[3].ToString());

            PopupInfoConfirmButton.onClick.RemoveAllListeners();
            PopupInfoConfirmButton.onClick.AddListener(() =>
            {
                HidePopupContent(PopupManager.Get.PopupController.PopupInfo);
                ViewsEvents.Get.Login.GetComponent<UserService>().Logout();
            });
        }
        public void ShowEmailNotFoundPopup(object[] _param)
        {
            _params = _param;
            PopupInfoCancelButton.gameObject.SetActive(false);
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_param[0].ToString(), _param[1].ToString(), _param[2].ToString(), _param[3].ToString());

        }
        public void ShowInsufficientBubblesPopup(object[] _param)
        {
            _params = _param;
            PopupInfoConfirmButton.gameObject.SetActive(true);
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_param[0].ToString(), _param[1].ToString(), _param[2].ToString(), _param[3].ToString());

            PopupInfoConfirmButton.onClick.RemoveAllListeners();
            PopupInfoConfirmButton.onClick.AddListener(() =>
            {
                PopupManager.Get.PopupViewPresenter.HidePopupContent(PopupManager.Get.PopupController.PopupInfo);
            });
        }
        public void ShowConnectionFailedPopup(object[] _param)
        {
            _params = _param;
            PopupInfoCancelButton.gameObject.SetActive(false);
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_param[0].ToString(), _param[1].ToString(), _param[2].ToString(), _param[3].ToString());

            PopupInfoConfirmButton.onClick.RemoveAllListeners();
            PopupInfoConfirmButton.gameObject.SetActive(true);
            PopupInfoConfirmButton.onClick.AddListener(() =>
            {
                HidePopupContent(PopupManager.Get.PopupController.PopupInfo);
            });
        }
        public void ShowPaymentNotConfirmedPopup(object[] _param)
        {
            _params = _param;
            PopupInfoCancelButton.gameObject.SetActive(false);
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_param[0].ToString(), _param[1].ToString(), _param[2].ToString(), _param[3].ToString());
            PopupInfoConfirmButton.onClick.RemoveAllListeners();

        }
        public void ShowProhibitedLocationWalletPopup(object[] _param)
        {
            _params = _param;
            PopupInfoCancelButton.gameObject.SetActive(false);
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_param[0].ToString(), _param[1].ToString(), _param[2].ToString(), _param[3].ToString());
            PopupInfoConfirmButton.onClick.RemoveAllListeners();
        }

        public void ShowProhibitedLocationWithdrawPopup(object[] _param)
        {
            _params = _param;
            PopupInfoCancelButton.gameObject.SetActive(false);
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_param[0].ToString(), _param[1].ToString(), _param[2].ToString(), _param[3].ToString());
            PopupInfoConfirmButton.onClick.RemoveAllListeners();
        }
        public void ShowTooyoungPopup(object[] _param)
        {
            _params = _param;
            PopupInfoCancelButton.gameObject.SetActive(false);
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_param[0].ToString(), _param[1].ToString(), _param[2].ToString(), _param[3].ToString());

            PopupInfoConfirmButton.onClick.RemoveAllListeners();
            PopupInfoConfirmButton.gameObject.SetActive(true);
            PopupInfoConfirmButton.onClick.AddListener(() =>
            {
                EventsController.Get.gotItButton();
            });
        }
        public void ShowCongratsWithdrawalPopup(object[] _param)
        {
            _params = _param;
            ShowPopupContent(PopupManager.Get.PopupController.PopupCongratsWithdrawal.gameObject);
            PopupManager.Get.PopupController.PopupCongratsWithdrawal.SetBool("Show", true);
            PopupCongratsWithdrawalTitle.text = _params[0].ToString();
            PopupCongratsWithdrawalSubtitle.text = _params[1].ToString();
            PopupCongratsWithdrawalDesc.text = _params[2].ToString();
            PopupCongratsWithdrawalWorkingDaysDesc.text = _params[3].ToString();
            PopupCongratsWithdrawalConfirmButtonText.text = _params[4].ToString();
            PopupCongratsWithdrawalConfirmButton.onClick.RemoveAllListeners();
            PopupCongratsWithdrawalConfirmButton.onClick.AddListener(() =>
            {

            });
        }
        public void ShowGiftCardPopup(object[] _param, Image cover)
        {
            _params = _param;
            ShowPopupContent(PopupManager.Get.PopupController.PopupGiftCard.gameObject);
            PopupManager.Get.PopupController.PopupGiftCard.SetBool("Show", true);
            PopupGiftCardTitle.text = _params[0].ToString();
            PopupGiftCardSubtitle.text = _params[1].ToString();
            PopupGiftCardDesc.text = _params[2].ToString();
            PopupGiftCardPrice.text = _params[3].ToString();
            PopupGiftCardConfirmButtonText.text = _params[4].ToString();
            PopupGiftCardImage.sprite = cover.sprite;
            PopupGiftCardConfirmButton.onClick.RemoveAllListeners();
            PopupGiftCardConfirmButton.onClick.AddListener(async () =>
            {
                await ViewsEvents.Get.Menu.Market.GetComponent<MarketController>().BuyGiftAsync();
            });
        }
        public void ShowPrivacyPolicy()
        {
            ShowPopupContent(PopupManager.Get.PopupController.PopupPrivacyPolicy.gameObject);
            PopupManager.Get.PopupController.PopupPrivacyPolicy.SetBool("Show", true);
        }
        public void ShowDuelsPopup(object[] _param, string note = null)
        {
            _params = _param;
            ShowPopupContent(PopupManager.Get.PopupController.PopupDuels.gameObject);
            PopupManager.Get.PopupController.PopupDuels.SetBool("Show", true);
            TranslationManager._instance.scene = "Home";

            if (_param[2].Equals(ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES))
            {
                entry_fee.text = TranslationManager._instance.Get("entry_fee") + " " + float.Parse(_param[0].ToString());
                Gain.text = (float.Parse(_param[1].ToString())).ToString();
                Gain.text += " <sprite=0>";
                bubbles_image.gameObject.SetActive(true);
                crown_image.gameObject.SetActive(false);
            }
            else
            {
                entry_fee.text = TranslationManager._instance.Get("entry_fee") + " " + float.Parse(_param[0].ToString()) * 100;
                Gain.text = (float.Parse(_param[1].ToString()) * 100).ToString();
                Gain.text += " <sprite=1>";
                crown_image.gameObject.SetActive(true);
                bubbles_image.gameObject.SetActive(false);
            }
            confirm_duels.onClick.RemoveAllListeners();

            if (note != null && note.Equals("PlayAgain"))
            {
                confirm_duels.onClick.AddListener(() =>
                {
                    EventsController.Get.playAgain();
                });
            }
            else
            {
                confirm_duels.onClick.AddListener(() =>
                {
                    HidePopupContent(popup_duels_animator);
                    if (_params[3].ToString().Equals(ChallengeManager.CHALLENGE_TYPE_1V1))
                    {
                        StartCoroutine(StartDuels());
                    }

                    if (_params[3].ToString().Equals(ChallengeManager.CHALLENGE_TYPE_BRACKET))
                    {
                        StartCoroutine(StartTournament());
                    }
                });
            }
            if (_params[3].ToString().Equals(ChallengeManager.CHALLENGE_TYPE_1V1))
            {
                SeembaAnalyticsManager.Get.SendDuelInfoEvent("Duel Popup", float.Parse(_param[0].ToString()), float.Parse(_param[1].ToString()), _param[2].ToString());
            }
            else
            {
                SeembaAnalyticsManager.Get.SendTournamentInfoEvent("Tournament Popup", float.Parse(_param[0].ToString()), float.Parse(_param[1].ToString()), _param[2].ToString());
            }

        }
        public void ShowCurrentPasswordPopup(object[] _param)
        {
            _params = _param;
            ShowPopupContent(PopupManager.Get.PopupController.PopupCurrentPassword.gameObject);
            PopupManager.Get.PopupController.PopupCurrentPassword.SetBool("Show", true);
            PopupCurrentPassTitle.text = _param[0].ToString();
            PopupCurrentPassSubtitle.text = _param[1].ToString();
            PopupCurrentPassInputFieldPlaceholderText.text = _param[2].ToString();
            PopupCurrentPassConfirmButtonText.text = _param[3].ToString();
        }
        public void ShowForgetPasswordPopup(object[] _param)
        {
            _params = _param;
            ShowPopupContent(PopupManager.Get.PopupController.PopupForgetPassword.gameObject);
            PopupManager.Get.PopupController.PopupForgetPassword.SetBool("Show", true);
            PopupForgetPassTitle.text = _param[0].ToString();
            PopupForgetPassSubtitle.text = _param[1].ToString();
            PopupForgetPassEmailInputFieldPlaceholder.text = _param[2].ToString();
            PopupForgetPassMessage.text = _param[3].ToString();
            PopupForgetPassConfirmButtonText.text = _param[4].ToString();
        }
        public void ShowNewPasswordPopup(object[] _param)
        {
            _params = _param;
            ShowPopupContent(PopupManager.Get.PopupController.PopupUpdatePassword.gameObject);
            PopupManager.Get.PopupController.PopupUpdatePassword.SetBool("Show", true);
            PopupUpdatePassTitle.text = _param[0].ToString();
            PopupUpdatePassSubtitle.text = _param[1].ToString();
            PopupUpdatePassConfirmPasswordInputFieldText.text = _param[2].ToString();
            PopupUpdatePassNewPasswordInputFieldText.text = _param[3].ToString();
            PopupCurrentPassConfirmButtonText.text = _param[4].ToString();
        }
        public void ShowPaymentPopup(object[] _param)
        {
            _params = _param;
            ShowPopupContent(PopupManager.Get.PopupController.PopupPayment.gameObject);
            PopupManager.Get.PopupController.PopupPayment.SetBool("Show", true);
            PopupPaymentTitle.text = _param[0].ToString();
            PopupPaymentSubtitle.text = _param[1].ToString();
            PopupPaymentConfirmButtonText.text = _param[2].ToString();
            SeembaAnalyticsManager.Get.SendCreditEvent("Popup Payment Method", WalletScript.LastCredit);
        }
        public void ShowAgeVerificationPopup(object[] _param)
        {
            _params = _param;
            ShowPopupContent(PopupManager.Get.PopupController.PopupAgeVerification.gameObject);
            PopupManager.Get.PopupController.PopupAgeVerification.SetBool("Show", true);
            PopupAgeTitle.text = _param[0].ToString();
            PopupAgeSubtitle.text = _param[1].ToString();
            PopupAgeMessage.text = _param[2].ToString();
            PopupAgeConfirmButtonText.text = _param[3].ToString();
            PopupAgePlaceHolder.text = _param[4].ToString();
            SeembaAnalyticsManager.Get.SendCreditEvent("Popup Age verification", WalletScript.LastCredit);
        }
        public void ShowWinPopup(object[] _param, string gain)
        {
            _params = _param;
            ShowPopupContent(PopupManager.Get.PopupController.PopupWin.gameObject);
            PopupManager.Get.PopupController.PopupWin.SetBool("Show", true);
            if(_param[3].Equals("bubble"))
            {
                PopupWinAmout.text = _param[2].ToString() + " <sprite=0>";
            }
            else
            {
                PopupWinAmout.text = _param[2].ToString() + " <sprite=1>";
            }
            PopupWinConfirmButtonText.text = _param[1].ToString();
            PopupWinConfirmButton.onClick.AddListener(() =>
            {
                HidePopupContent(PopupManager.Get.PopupController.PopupWin);
            });
        }
        public void ShowCongratsPopup(object[] _param)
        {
            _params = _param;
            ShowPopupContent(PopupManager.Get.PopupController.PopupCongrats.gameObject);
            PopupManager.Get.PopupController.PopupCongrats.SetBool("Show", true);
            PopupCongratsTitle.text = _param[0].ToString();
            PopupCongratsSubtitle.text = _param[1].ToString();
            PopupCongratsTotal.text = _param[2].ToString();
            PopupCongratsAddedValue.text = _param[3].ToString();
            PopupCongratsConfirmButtonText.text = _param[4].ToString();
            PopupCongratsConfirmButton.onClick.RemoveAllListeners();
            PopupCongratsConfirmButton.onClick.AddListener(() =>
            {
                StartCoroutine(CongratsButtonClick());
            });
        }

        private IEnumerator CongratsButtonClick()
        {
            HidePopupContent(PopupManager.Get.PopupController.PopupCongrats);
            if (ViewsEvents.Get.GetCurrentMenu() != ViewsEvents.Get.Menu)
            {
                ViewsEvents.Get.GoBack();
            }
            BottomMenuController.Show();
            if (ViewsEvents.Get.Menu.GetCurrentSubMenu() == ViewsEvents.Get.Menu.Settings)
            {
                ViewsEvents.Get.Menu.ScrollSnap.CompteBackButtonClick();
                yield return new WaitForSeconds(0.15f);
                ViewsEvents.Get.WinMoneyClick();
            }
            else
            {
                ViewsEvents.Get.ShowScene(EventsController.last_view);
                yield return new WaitForSeconds(0.15f);
                ViewsEvents.Get.WinMoneyClick();
            }
            ViewsEvents.Get.BankingInfo.CleatInputs();
        }

        public void ShowUnkownDevicePopup(object[] _param)
        {
            _params = _param;
            PopupInfoCancelButton.gameObject.SetActive(false);
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_param[0].ToString(), _param[1].ToString(), _param[2].ToString(), _param[3].ToString());
            PopupInfoConfirmButton.onClick.RemoveAllListeners();
            PopupInfoConfirmButton.gameObject.SetActive(true);
            PopupInfoConfirmButton.onClick.AddListener(() =>
            {
                HidePopupContent(PopupManager.Get.PopupController.PopupInfo);
            });
        }
        public void ShowChooseCharacter(object[] _param)
        {
            _params = _param;
            ShowPopupContent(PopupManager.Get.PopupController.PopupChooseCharacter.gameObject);
            PopupManager.Get.PopupController.PopupChooseCharacter.SetBool("Show", true);
            PopupchooseCharacterTitle.text = _param[0].ToString();
            foreach (Transform child in PopupchooseCharacterToggles)
            {
                var toggle = child.GetComponent<Toggle>();
                var image = child.GetComponent<Image>();
                if(toggle != null)
                {
                    toggle.onValueChanged.AddListener((value) =>
                    {
                        ViewsEvents.Get.Signup.GetComponent<SignupPresenter>().OnToggleSelected(value, image);
                    });
                }
            }
        }
        public void ShowPopupContent(GameObject gameObject)
        {
            ShowOverlay();
            gameObject.SetActive(true);
        }
        public void HidePopupContent(Animator animator)
        {
            SeembaAnalyticsManager.Get.SendGameEvent(animator.gameObject.name + " Closed");
            StartCoroutine(WaitforAnimation(animator));
        }
        #endregion

        #region Implementation
        private IEnumerator OnClickConfirmAegVerificationButton()
        {
            HidePopupContent(PopupManager.Get.PopupController.PopupAgeVerification);
            yield return new WaitForSeconds(0.5f);
            PopupAgeconfirmButton.interactable = false;
            EventsController.Get.UpdateAge();
        }

        private void ShowOverlay() { overlay.gameObject.SetActive(true); }

        private void HideOverlay() { overlay.gameObject.SetActive(false); }

        private IEnumerator WaitforAnimation(Animator animator)
        {
            animator.SetBool("Show", false);
            yield return new WaitForSeconds(0.5f);
            HideOverlay();
        }
        private void OpenWallet(string last_view)
        {
            BottomMenuController bottomMenu = BottomMenuController.Get;
            bottomMenu.unselectHome();
            bottomMenu.unselectHaveFun();
            bottomMenu.unselectWinMoney();
            bottomMenu.unselectSettings();
            BottomMenuController.Hide();
            ViewsEvents.Get.WalletClick(last_view);
        }
        private IEnumerator StartDuels()
        {
            popup_duels_animator.SetBool("Show", false);
            yield return new WaitForSeconds(0.5f);
            ChallengeController.Get.Play(_params);
            TranslationManager._instance.scene = "Home";
            if (_params[0].ToString().Equals(TranslationManager._instance.Get("insufficient")))
            {
                yield return null;
            }
            else
            {
                SeembaAnalyticsManager.Get.SendDuelInfoEvent("Start Duel", float.Parse(_params[0].ToString()), float.Parse(_params[1].ToString()), _params[2].ToString());
            }
        }
        private IEnumerator StartTournament()
        {
            popup_duels_animator.SetBool("Show", false);
            yield return new WaitForSeconds(0.5f);
            ViewsEvents.Get.Brackets.GetComponent<TournamentController>().Play(_params);
        }
        #endregion
    }
}
