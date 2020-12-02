using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
#pragma warning disable 0649
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
        public Text PopupWinAmout;
        public Text PopupWinCurrency;
        public Button PopupWinConfirmButton;
        public Text PopupWinConfirmButtonText;

        [Header("popup Congrats")]
        public Animator PopupCongrats;
        public Text PopupCongratsTitle;
        public Text PopupCongratsSubtitle;
        public Text PopupCongratsTotal;
        public Text PopupCongratsAddedValue;
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
        public Text gain;
        public Text bubbles_text;
        public Image bubbles_image;
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

        [Header("popup error")]
        public Animator popup_error_animator;
        public Text popup_error_title;
        public Text popup_error_subtitle;
        public Text popup_error_main_text;
        public Text popup_error_button_text;
        public Button popup_error_button;
        
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

            //popup_error_button.onClick.AddListener(() =>
            //{
            //    switch (CURRENT_POPUP)
            //    {
            //        case PopupType.INFO_INSUFFICIENT_BALANCE:
            //            OpenWallet("WinMoney");
            //            break;
            //        case PopupType.INFO_INSUFFICIENT_BUBBLES:
            //            OpenWallet("HaveFun");
            //            break;
            //        case PopupType.DOWNLOAD_FROM_STORE:
            //            EventsController.Get.DownloadFromStore();
            //            break;
            //    }
            //});

            PopupAgeconfirmButton.onClick.AddListener(() =>
            {
                StartCoroutine(OnClickConfirmAegVerificationButton());
            });

            PopupPaymentConfirmButton.onClick.AddListener(() =>
            {
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
        public void ShowErrorPopup()
        {
            popup_error_title.text = _params[0].ToString();
            popup_error_subtitle.text = _params[1].ToString();
            popup_error_main_text.text = _params[2].ToString();
            popup_error_button_text.text = _params[3].ToString();
        }
        public void ShowInfoPopup(object[] _params)
        {
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_params[0].ToString(), _params[1].ToString(), _params[2].ToString(), _params[3].ToString());
            PopupInfoConfirmButton.onClick.RemoveAllListeners();
        }
        public void ShowInsufficientBalancePopup(object[] _paras)
        {
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_paras[0].ToString(), _paras[1].ToString(), _paras[2].ToString(), _paras[3].ToString());
            PopupInfoConfirmButton.onClick.RemoveAllListeners();
            PopupInfoConfirmButton.onClick.AddListener(() =>
            {
                HidePopupContent(PopupManager.Get.PopupController.PopupInfo);
                EventsController.Get.OpenWallet("WinMoney");
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
        public void ShowMissingInfoPopup(object[] _param, string source)
        {
            _params = _param;
            PopupInfoCancelButton.gameObject.SetActive(false);
            ShowPopupContent(PopupManager.Get.PopupController.PopupInfo.gameObject);
            InitPopupInfo(_param[0].ToString(), _param[1].ToString(), _param[2].ToString(), _param[3].ToString());
            PopupInfoConfirmButton.onClick.RemoveAllListeners();
            PopupInfoConfirmButton.onClick.AddListener(() =>
            {
                if (source.Equals("Idproof"))
                {
                    ViewsEvents.Get.IdProof.GetComponent<IDProofPresenter>().missingInfoContinue();
                }
                else if (source.Equals("withdraw"))
                {
                    HidePopupContent(PopupManager.Get.PopupController.PopupInfo);
                    ViewsEvents.Get.GoToMenu(ViewsEvents.Get.WithdrawalInfo.gameObject);
                }
            });
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
            TranslationManager.scene = "Home";
            entry_fee.text = TranslationManager.Get("entry_fee") + " " + _param[0];
            gain.text = _param[1].ToString();
            if (_param[2].Equals(ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES))
            {
                bubbles_text.gameObject.SetActive(true);
                bubbles_image.gameObject.SetActive(true);
            }
            else
            {
                entry_fee.text += CurrencyManager.CURRENT_CURRENCY;
                gain.text += CurrencyManager.CURRENT_CURRENCY;
                bubbles_text.gameObject.SetActive(false);
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
            //VisaToggleText.text = _param[3].ToString();
            //MasterCardText.text = _param[4].ToString();
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
        }
        public void ShowWinPopup(object[] _param, string gain)
        {
            _params = _param;
            ShowPopupContent(PopupManager.Get.PopupController.PopupWin.gameObject);
            PopupManager.Get.PopupController.PopupWin.SetBool("Show", true);
            PopupWinAmout.text = gain;
            PopupWinCurrency.text = _param[0].ToString();
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
                toggle.onValueChanged.AddListener((value) =>
                {
                    ViewsEvents.Get.Signup.GetComponent<SignupPresenter>().OnToggleSelected(value, image);
                });
            }
        }
        public void ShowPopupContent(GameObject gameObject)
        {
            ShowOverlay();
            gameObject.SetActive(true);
        }
        public void HidePopupContent(Animator animator)
        {
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
