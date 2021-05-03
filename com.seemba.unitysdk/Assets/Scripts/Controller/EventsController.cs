using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class StripeError
    {
        public StripeErrorInfo error; 
    }
    [CLSCompliant(false)]
    public class StripeErrorInfo
    {
        public string code;
        public string doc_url;
        public string message;
        public string type;
    }
    [CLSCompliant(false)]
    public class EventsController : MonoBehaviour
    {
        #region Static
        public static EventsController Get { get { return sInstance; } }

        private static EventsController sInstance;
        public static GameObject last_view;
        #endregion

        #region Script Parameters
        public static bool advFound;
        public static string ChallengeType, ChallengeName;
        public static int MoneyToAdd;
        public AudioListener AudioListener;
        #endregion

        #region Fields
        private string _selectedDateString2 = "1995-09-15";
        private string mCaller;
        #endregion

        private String SelectedDateString2
        {
            get
            {
                return _selectedDateString2;
            }
            set
            {
                _selectedDateString2 = value;
                UserManager.Get.CurrentUser.birthdate = value;
            }
        }
        private void Awake()
        {
            sInstance = this;
            //GameAnalytics.Initialize();
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SeembaWebRequest.Get.OnSeembaErrorEvent += OnSeembaError;
        }

        private void OnSeembaError(UnityWebRequest www)
        {
            Debug.LogWarning(www.uri);
            Debug.LogWarning(www.downloadHandler.text);
            Debug.LogWarning(www.error);

            if (www.isNetworkError)
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());
            }
            else if (www.uri.ToString().Contains("stripe"))
            {
                Debug.LogWarning(www.downloadHandler.text);
                StripeError response = JsonConvert.DeserializeObject<StripeError>(www.downloadHandler.text);

                if (response.error.code.Replace('"', ' ').Trim().Equals("incorrect_number"))
                {
                    SeembaAnalyticsManager.Get.SendCreditEvent("Incorrect Card Number", WalletScript.LastCredit);
                    PopupManager.Get.PopupController.ShowPopup(PopupType.STRIPE_INCORRECT_NUMBER, PopupsText.Get.StripeIncorrectNumber());
                    return;
                }
                if (response.error.code.Replace('"', ' ').Trim().Equals("card_declined"))
                {
                    SeembaAnalyticsManager.Get.SendCreditEvent("Card Declined", WalletScript.LastCredit);
                    PopupManager.Get.PopupController.ShowPopup(PopupType.STRIPE_CARD_DECLINED, PopupsText.Get.StripeCardDeclined());
                    return;
                }
                if (response.error.code.Replace('"', ' ').Trim().Equals("incorrect_cvc"))
                {
                    SeembaAnalyticsManager.Get.SendCreditEvent("Incorrect CVC", WalletScript.LastCredit);
                    PopupManager.Get.PopupController.ShowPopup(PopupType.STRIPE_INCORRECT_CVC, PopupsText.Get.StripeIncorrectCVC());
                    return;
                }
                if (response.error.code.Replace('"', ' ').Trim().Equals("balance_insufficient"))
                {
                    SeembaAnalyticsManager.Get.SendCreditEvent("Card Balance Insufficient", WalletScript.LastCredit);
                    PopupManager.Get.PopupController.ShowPopup(PopupType.STRIPE_BALANCE_INSUFFICIENT, PopupsText.Get.StripeBalanceInsufficient());
                    return;
                }
                if (response.error.code.Replace('"', ' ').Trim().Equals("insufficient_funds"))
                {
                    SeembaAnalyticsManager.Get.SendCreditEvent("Card funds Insufficient", WalletScript.LastCredit);
                    PopupManager.Get.PopupController.ShowPopup(PopupType.STRIPE_BALANCE_INSUFFICIENT, PopupsText.Get.StripeBalanceInsufficient());
                    return;
                }

                PopupManager.Get.PopupController.ShowPopup(PopupType.STRIPE_TRY_AGAIN_LATER, PopupsText.Get.StripeTryAgainLater());
                SeembaAnalyticsManager.Get.SendCreditEvent("Stripe Error Try Later", WalletScript.LastCredit);
                return;
            }
            else
            {
                var errorCode = www.responseCode;
                if (errorCode >= 500)
                {
                    PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_SERVER_ERROR, PopupsText.Get.ServerError());
                }
                else if (errorCode == 403 || errorCode == 401)
                {
                    PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_UNAUTHORIZED, PopupsText.Get.Unauthorized());
                }
            }
            LoaderManager.Get.LoaderController.HideLoader();
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            LoaderManager.Get.LoaderController.HideLoader();
        }

        public void SendEmail()
        {
            string email = UserService.Seemba_Email;
            Application.OpenURL("mailto:" + email);
        }
        public void ContinueEquality()
        {
            string dntshowagain = PlayerPrefs.GetString("DontShowAgainPopupEqualityRefund");
            if (string.IsNullOrEmpty(dntshowagain))
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_EQUALITY_REFUND, PopupsText.Get.EqualityRefund());
            }
        }
        //public void IdProof()
        //{
        //    ViewsEvents.Get.GoToMenu(ViewsEvents.Get.IdProof.gameObject);
        //}
        //public void CloseIdProof()
        //{
        //    ViewsEvents.Get.IdProof.transform.GetChild(0).GetComponent<Animator>().SetBool("showView", false);
        //    UnityThreading.ActionThread thread;
        //    thread = UnityThreadHelper.CreateThread(() =>
        //    {
        //        Thread.Sleep(500);
        //        UnityThreadHelper.Dispatcher.Dispatch(() =>
        //        {
        //            ViewsEvents.Get.GoBack();
        //        });
        //    });
        //}
        public void OpenWallet(string last_view)
        {
            ViewsEvents.Get.ShowScene(ViewsEvents.Get.Menu.Wallet);
        }
        public void HidePopupProfile()
        {
            UnityThreading.ActionThread thread;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                Thread.Sleep(500);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    try
                    {
                        ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
                    }
                    catch (Exception) { }
                });
            });
        }
        public void gotItButton()
        {
            StartCoroutine(OnClickGoItTooYoung());
        }
        public void DownloadFromStore()
        {
            Application.OpenURL("https://seemba-game-store.herokuapp.com/");
        }

        IEnumerator OnClickGoItTooYoung()
        {
            PopupManager.Get.PopupViewPresenter.HidePopupContent(PopupManager.Get.PopupController.PopupInfo);
            yield return new WaitForSeconds(0.5f);
            WalletBack();
            yield return new WaitForSeconds(0.15f);
            ViewsEvents.Get.HaveFunClick();
        }
        public void ReconnectExipiredSession()
        {
            UnityThreading.ActionThread thread;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                Thread.Sleep(100);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Login.gameObject);
                });
            });
        }
        public void missingInfowithdrawContinue()
        {
            UnityThreading.ActionThread thread;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                Thread.Sleep(300);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    ViewsEvents.Get.GoToMenu(ViewsEvents.Get.IdProof.gameObject);
                });
            });
        }

        public void withdrawFailed(string headertext, string headertext2, string msg)
        {
            StartCoroutine(WithdrawFailed(headertext, headertext2, msg));
        }

        IEnumerator WithdrawFailed(string headertext, string headertext2, string msg)
        {
            ViewsEvents.Get.Menu.ScrollSnap.LerpToPage(0);
            BottomMenuController.Get.selectSettings();
            ViewsEvents.Get.SettingsClick();
            yield return new WaitForSeconds(0.2f);
            ViewsEvents.Get.WinMoneyClick();
            LoaderManager.Get.LoaderController.HideLoader();
            object[] _params = { headertext, headertext2, msg, TranslationManager.Get("continue") };
            PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_PAYMENT_FAILED, _params);
        }
        public void withdrawSucceeded()
        {
            StartCoroutine(WithdrawSucceeded());
        }
        IEnumerator WithdrawSucceeded()
        {
            ViewsEvents.Get.Menu.ScrollSnap.LerpToPage(0);
            BottomMenuController.Get.selectSettings();
            ViewsEvents.Get.SettingsClick();
            yield return new WaitForSeconds(0.2f);
            ViewsEvents.Get.WinMoneyClick();
            LoaderManager.Get.LoaderController.HideLoader();
            PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_CONGRATS_WITHDRAW, PopupsText.Get.CongratsWithdraw());
        }
        public void backToWinMoney()
        {
            ViewsEvents.Get.WinMoneyClick();
            BottomMenuController bottomMenu = BottomMenuController.Get;
            bottomMenu.selectWinMoney();
            bottomMenu.unselectSettings();
            bottomMenu.unselectHome();
            bottomMenu.unselectHaveFun();
        }
        public void closeProfil()
        {
            HidePopupProfile();
        }
        public void openCurrentProfile()
        {
            ViewsEvents.Get.Profile.InitProfile(UserManager.Get.CurrentUser);
            ViewsEvents.Get.ShowOverayMenu(ViewsEvents.Get.Profile.gameObject);
        }
        public void playAgain()
        {
            OpponentFound.adversaireName = null;
            OpponentFound.Avatar = null;
            OpponentFound.AdvCountryCode = null;
            advFound = false;
            switch (ChallengeManager.CurrentChallenge.gain_type)
            {
                case ChallengeManager.CHALLENGE_WIN_TYPE_CASH:
                    if (float.Parse(ChallengeManager.CurrentChallenge.gain) == ChallengeManager.WIN_1V1_CASH_CONFIDENT)
                    {
                        object[] _params = { ChallengeManager.FEE_1V1_CASH_CONFIDENT, ChallengeManager.WIN_1V1_CASH_CONFIDENT, ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
                        PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
                    }
                    else if (float.Parse(ChallengeManager.CurrentChallenge.gain) == ChallengeManager.WIN_1V1_CASH_CHAMPION)
                    {
                        object[] _params = { ChallengeManager.FEE_1V1_CASH_CHAMPION, ChallengeManager.WIN_1V1_CASH_CHAMPION, ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
                        PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
                    }
                    else if (float.Parse(ChallengeManager.CurrentChallenge.gain) == ChallengeManager.WIN_1V1_CASH_LEGEND)
                    {
                        object[] _params = { ChallengeManager.FEE_1V1_CASH_LEGEND, ChallengeManager.WIN_1V1_CASH_LEGEND, ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
                        PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
                    }
                    break;
                case ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES:


                    if (float.Parse(ChallengeManager.CurrentChallenge.gain) == ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT)
                    {
                        object[] _params = { ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT, ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_1V1 };
                        PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
                        ChallengeName = "NoviceGoutte";
                    }
                    else if (float.Parse(ChallengeManager.CurrentChallenge.gain) == ChallengeManager.WIN_1V1_BUBBLES_CHAMPION)
                    {
                        object[] _params = { ChallengeManager.FEE_1V1_BUBBLES_CHAMPION, ChallengeManager.WIN_1V1_BUBBLES_CHAMPION, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_1V1 };
                        PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
                        ChallengeName = "AmateurGoutte";
                    }
                    else if (float.Parse(ChallengeManager.CurrentChallenge.gain) == ChallengeManager.WIN_1V1_BUBBLES_LEGEND)
                    {
                        object[] _params = { ChallengeManager.FEE_1V1_BUBBLES_LEGEND, ChallengeManager.WIN_1V1_BUBBLES_LEGEND, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_1V1 };
                        PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
                        ChallengeName = "ConfirmedGoutte";
                    }
                    break;
            }
        }
        public void QuitToFirstScene()
        {
            BackgroundController.CurrentBackground = null;
            ChallengeType = null;
            SceneManager.LoadScene(0);
        }
        public void Play()
        {
            ViewsEvents.Get.GoBack();
            BackgroundController.CurrentBackground = null;
            advFound = false;
            LoaderManager.Get.LoaderController.ShowLoader(null);
            ViewsEvents.Get.GetCurrentMenu().SetActive(false);
            AudioListener.enabled = false;
            ViewsEvents.Get.Matchmaking.GetComponent<OpponentFound>().ResetOpponent();
            var fee = ChallengeManager.Get.GetChallengeFee(float.Parse(ChallengeManager.CurrentChallengeGain), ChallengeManager.CurrentChallengeGainType);
            SeembaAnalyticsManager.Get.SendDuelInfoEvent("Play Duel Now", fee, float.Parse(ChallengeManager.CurrentChallengeGain), ChallengeManager.CurrentChallengeGainType);
            SceneManager.LoadSceneAsync(GamesManager.GAME_SCENE_NAME, LoadSceneMode.Additive);
        }
        public bool checkUserBirthday(User user)
        {
            DateTime birthdate;
            try
            {
                birthdate = DateTime.Parse(user.birthdate);
            }
            catch (Exception)
            {
                return true;
            }
            if (!string.IsNullOrEmpty(user.birthdate))
            {
                if (DateTime.UtcNow.Year - birthdate.Year >= 18)
                    return false;
                else
                    return true;
            }
            else
                return true;
        }
        public void UpdateAge()
        {
            Text mAge = PopupManager.Get.PopupViewPresenter.PopupAgePlaceHolder;
            string[] date = mAge.text.Split(new char[] { '-' }, 3);
            string Years = date[0];
            string Days = date[2];
            string Months = date[1];
            LoaderManager.Get.LoaderController.ShowLoader(null);

            string[] attrib = { "birthdate" };
            string[] value = { Years + "-" + Months + "-" + Days };
            UserManager.Get.UpdateUserByField(attrib, value);

            if (DateTime.UtcNow.Year - int.Parse(Years) >= 18)
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_PAYMENT, PopupsText.Get.PopupPayment());
            }
            else
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_TOO_YOUNG, PopupsText.Get.TooYoung());
            }
            LoaderManager.Get.LoaderController.HideLoader();
        }
        public void OpenInfoPersonel()
        {
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.PersonalInfo.gameObject);
        }
        public IEnumerator checkInternetConnection(Action<bool?> action)
        {
            UnityWebRequest www = new UnityWebRequest("https://www.google.fr");
            yield return www.SendWebRequest();

            if (www.error == null)
            {
                action(true);
            }
            else
            {
                action(false);
            }
        }
        public async void startFirstChallenge(string token)
        {
            ChallengeManager.CurrentChallengeGain = ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT.ToString();
            ChallengeManager.CurrentChallengeGainType = ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES;
            SearchingForPlayerPresenter.nbPlayer = "duel";
            ChallengeType = ChallengeManager.CHALLENGE_TYPE_1V1;
            LoaderManager.Get.LoaderController.ShowLoader(LoaderManager.FIRST_CHALLENGE);
            ChallengeManager.CurrentChallenge = await ChallengeManager.Get.AddChallenge("headTohead", ChallengeManager.CurrentChallengeGain, ChallengeManager.CurrentChallengeGainType, 0, token);
            LoaderManager.Get.LoaderController.HideLoader();
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Matchmaking.gameObject);
            SeembaAnalyticsManager.Get.SendUserEvent("Start First Challenge");
        }
        public void WalletBack()
        {
            ViewsEvents.Get.Menu.ShowBottomBar(true);
            ViewsEvents.Get.Menu.ShowHeader(true);
            ViewsEvents.Get.ShowScene(last_view);
        }
        public void continuePayment()
        {
            PopupManager.Get.PopupViewPresenter.HidePopupContent(PopupManager.Get.PopupController.PopupPayment);
            UnityThreading.ActionThread myThread;
            myThread = UnityThreadHelper.CreateThread(() =>
            {
                Thread.Sleep(700);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    ViewsEvents.Get.GoToMenu(ViewsEvents.Get.BankingInfo.gameObject);
                });
            });
        }
        public void yesSoldeInsuffisant(string last_view)
        {
            UnityThreading.ActionThread thread;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                Thread.Sleep(500);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    BottomMenuController bottomMenu = BottomMenuController.Get;
                    bottomMenu.unselectWinMoney();
                    bottomMenu.selectSettings();
                    bottomMenu.unselectHome();
                    bottomMenu.unselectHaveFun();
                    ViewsEvents.Get.WalletClick(last_view);
                });
            });
        }
        public bool IsValidEmailAddress(string s)
        {
            var regex = new Regex(@"[a-z0-9!#$%&amp;'*+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&amp;'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            return regex.IsMatch(s);
        }
        public void QuitSeemba()
        {
            SceneManager.LoadScene(0);
        }
        public void TCs()
        {
            Application.OpenURL("http://www.seemba.com/downloads/T&Cs_Seemba.pdf");
        }
        public void PrivacyPolicy()
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_PRIVACY_POLICY, null);
            //Application.OpenURL("http://www.seemba.com/downloads/Privacy_Policy.pdf");
        }

        private void OnApplicationQuit()
        {
            //if(!string.IsNullOrEmpty(ViewsEvents.Get.Matchmaking.GetComponent<OpponentFound>().opponent_username.text))
            //{
            //    Seemba.Get.setResult(0);
            //}
        }

        #region DatePicker
        public void ShowDatePicker(string caller)
        {
            Debug.Log("ShowDatePicker " + caller);
            mCaller = caller;
            NativePicker.Instance.ShowDatePicker(new Rect(0, 0, 0, 0), (long val) =>
            {
                Debug.Log(caller);
                OnDateSelected(val);
            }
            , () =>
            {
                Debug.Log(caller);
                OnDateCanceled();
            }
            );
        }
        private void OnDateCanceled()
        {
            Debug.Log("OnDateCanceled ");
            if (mCaller.Equals("PersonalInfo"))
            {
                Debug.Log("OnDateCanceled personal info ");
            }
            else
            {
                SelectedDateString2 = DateTime.Now.ToString("yyyy-MM-dd");
                PopupManager.Get.PopupViewPresenter.PopupAgeconfirmButton.interactable = false;
                PopupManager.Get.PopupViewPresenter.PopupAgePlaceHolder.text = "Select Date";
            }
        }

        public void OnDateSelected(long val)
        {
            Debug.Log("OnDateSelected " + val);
            if (mCaller.Equals("personalinfo"))
            {
                SelectedDateString2 = NativePicker.ConvertToDateTime(val).ToString("yyyy-MM-dd");
                Debug.Log("OnDateSelected personal info " + val + " " + SelectedDateString2);
                var mBirthDate = ViewsEvents.Get.PersonalInfo.GetComponent<PersonelInfoController>().Birthdate;
                var mBirthDatePlaceHolder = ViewsEvents.Get.PersonalInfo.GetComponent<PersonelInfoController>().PlaceHolderAge;
                mBirthDatePlaceHolder.gameObject.SetActive(false);
                mBirthDate.text = SelectedDateString2;
                string[] attrib = { "birthdate" };
                string[] values = { mBirthDate.text };
                ViewsEvents.Get.PersonalInfo.GetComponent<PersonelInfoController>().UpdateAge(attrib, values);
            }
            else
            {
                SelectedDateString2 = NativePicker.ConvertToDateTime(val).ToString("yyyy-MM-dd");
                PopupManager.Get.PopupViewPresenter.PopupAgePlaceHolder.text = SelectedDateString2;
                PopupManager.Get.PopupViewPresenter.PopupAgeconfirmButton.interactable = true;
            }
        }

        #endregion

        #region ShowPopups
        public void ShowNoviceArgentpopUp()
        {
            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
            object[] _params = { ChallengeManager.FEE_1V1_CASH_CONFIDENT, ChallengeManager.WIN_1V1_CASH_CONFIDENT, ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        public void ShowAmateurArgentpopUp()
        {
            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
            object[] _params = { ChallengeManager.FEE_1V1_CASH_CHAMPION, ChallengeManager.WIN_1V1_CASH_CHAMPION, ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        public void ShowConfirmedArgentpopUp()
        {
            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
            object[] _params = { ChallengeManager.FEE_1V1_CASH_LEGEND , ChallengeManager.WIN_1V1_CASH_LEGEND, ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        public void ShowNoviceGouttepopUp()
        {
            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
            object[] _params = { ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT, ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        public void ShowAmateurGouttepopUp()
        {
            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
            object[] _params = { ChallengeManager.FEE_1V1_BUBBLES_CHAMPION, ChallengeManager.WIN_1V1_BUBBLES_CHAMPION, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        public void ShowConfirmedGouttepopUp()
        {
            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
            object[] _params = { ChallengeManager.FEE_1V1_BUBBLES_LEGEND, ChallengeManager.WIN_1V1_BUBBLES_LEGEND, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        public void ShowTournamentBubbleConfidentpopUp()
        {
            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
            object[] _params = { TournamentManager.FEE_BRACKET_BUBBLE_AMATEUR, TournamentManager.WIN_BRACKET_BUBBLE_AMATEUR, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, TournamentManager.BRACKET_TYPE_CONFIDENT };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        public void ShowTournamentBubbleChampionpopUp()
        {
            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
            object[] _params = { TournamentManager.FEE_BRACKET_BUBBLE_NOVICE, TournamentManager.WIN_BRACKET_BUBBLE_NOVICE, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, TournamentManager.BRACKET_TYPE_CHAMPION };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        public void ShowTournamentBubbleLegendpopUp()
        {
            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
            object[] _params = { TournamentManager.FEE_BRACKET_BUBBLE_CONFIRMED, TournamentManager.WIN_BRACKET_BUBBLE_CONFIRMED, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, TournamentManager.BRACKET_TYPE_LEGEND };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        public void ShowTournamentCashConfidentpopUp()
        {
            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
            object[] _params = { TournamentManager.FEE_BRACKET_CASH_AMATEUR, TournamentManager.WIN_BRACKET_CASH_AMATEUR , ChallengeManager.CHALLENGE_WIN_TYPE_CASH, TournamentManager.BRACKET_TYPE_CONFIDENT };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        public void ShowTournamentCashChampionpopUp()
        {
            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
            object[] _params = { TournamentManager.FEE_BRACKET_CASH_NOVICE, TournamentManager.WIN_BRACKET_CASH_NOVICE, ChallengeManager.CHALLENGE_WIN_TYPE_CASH, TournamentManager.BRACKET_TYPE_CHAMPION };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        public void ShowTournamentCashLegendpopUp()
        {
            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
            object[] _params = { TournamentManager.FEE_BRACKET_CASH_CONFIRMED, TournamentManager.WIN_BRACKET_CASH_CONFIRMED, ChallengeManager.CHALLENGE_WIN_TYPE_CASH, TournamentManager.BRACKET_TYPE_LEGEND };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        #endregion

        #region Ads
        public void ContinueToShowAdsPopup(GameObject gameObject)
        {
            OpponentFound.adversaireName = null;//usr.username
            OpponentFound.Avatar = null;
            OpponentFound.AdvCountryCode = null;//.country_code;
            advFound = false;
            gameObject.SetActive(false);
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Menu.gameObject);
            ViewsEvents.Get.ShowScene(ViewsEvents.Get.Menu.Home);
            int GamePlayed = PlayerPrefs.GetInt("GamePlayed", 0);

            if (GamePlayed == 0)
            {
            }
            else
            {
                if (PlayerPrefs.GetInt("GamePlayed") % 10 == 0) { }
            }
            PlayerPrefs.SetInt("GamePlayed", ++GamePlayed);
        }
        #endregion
    }
}
