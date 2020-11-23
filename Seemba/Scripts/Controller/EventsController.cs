using SimpleJSON;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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

        if(www.isNetworkError)
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());
        }
        else if(www.uri.ToString().Contains("stripe"))
        {
            var responseJson = JSON.Parse(www.downloadHandler.text);
            if(responseJson["error"]["code"].ToString().Replace('"', ' ').Trim().Equals("incorrect_number"))
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.STRIPE_INCORRECT_NUMBER, PopupsText.Get.StripeIncorrectNumber());
                return;
            }
            if(responseJson["error"]["code"].ToString().Replace('"', ' ').Trim().Equals("card_declined"))
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.STRIPE_CARD_DECLINED, PopupsText.Get.StripeCardDeclined());
                return;
            }
            if(responseJson["error"]["code"].ToString().Replace('"', ' ').Trim().Equals("incorrect_cvc"))
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.STRIPE_INCORRECT_CVC, PopupsText.Get.StripeIncorrectCVC());
                return;
            }
            if(responseJson["error"]["code"].ToString().Replace('"', ' ').Trim().Equals("balance_insufficient"))
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.STRIPE_BALANCE_INSUFFICIENT, PopupsText.Get.StripeBalanceInsufficient());
                return;
            }
            if(responseJson["error"]["code"].ToString().Replace('"', ' ').Trim().Equals("insufficient_funds"))
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.STRIPE_BALANCE_INSUFFICIENT, PopupsText.Get.StripeBalanceInsufficient());
                return;
            }
            //if(responseJson["error"]["code"].ToString().Replace('"', ' ').Trim().Equals("try_again_later"))
            //{
            PopupManager.Get.PopupController.ShowPopup(PopupType.STRIPE_TRY_AGAIN_LATER, PopupsText.Get.StripeTryAgainLater());
            return;
            //}
            //else
            //{

            //}
        }
        else
        {
            var errorCode = www.responseCode;
            if(errorCode >= 500)
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_SERVER_ERROR, PopupsText.Get.ServerError());
            }
            else if(errorCode == 403 || errorCode == 401)
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
    public void IdProof()
    {
        ViewsEvents.Get.GoToMenu(ViewsEvents.Get.IdProof.gameObject);
    }
    public void CloseIdProof()
    {
        ViewsEvents.Get.IdProof.transform.GetChild(0).GetComponent<Animator>().SetBool("showView", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(500);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                ViewsEvents.Get.GoBack();
            });
        });
    }
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
    public async void UpdateAge()
    {
        User user = await UserManager.Get.getUser();
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
        Application.OpenURL("http://www.seemba.com/downloads/Privacy_Policy.pdf");
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

    public  void OnDateSelected(long val)
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
        object[] _params = { ChallengeManager.FEE_1V1_CASH_LEGEND, ChallengeManager.WIN_1V1_CASH_LEGEND, ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
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
        object[] _params = { TournamentManager.FEE_BRACKET_BUBBLE_CONFIDENT, TournamentManager.WIN_BRACKET_BUBBLE_CONFIDENT, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, TournamentManager.BRACKET_TYPE_CONFIDENT };
        PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
    }
    public void ShowTournamentBubbleChampionpopUp()
    {
        ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
        object[] _params = { TournamentManager.FEE_BRACKET_BUBBLE_CHAMPION, TournamentManager.WIN_BRACKET_BUBBLE_CHAMPION, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, TournamentManager.BRACKET_TYPE_CHAMPION };
        PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
    }
    public void ShowTournamentBubbleLegendpopUp()
    {
        ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
        object[] _params = { TournamentManager.FEE_BRACKET_BUBBLE_LEGEND, TournamentManager.WIN_BRACKET_BUBBLE_LEGEND, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, TournamentManager.BRACKET_TYPE_LEGEND };
        PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
    }
    public void ShowTournamentCashConfidentpopUp()
    {
        ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
        object[] _params = { TournamentManager.FEE_BRACKET_CASH_CONFIDENT, TournamentManager.WIN_BRACKET_CASH_CONFIDENT, ChallengeManager.CHALLENGE_WIN_TYPE_CASH, TournamentManager.BRACKET_TYPE_CONFIDENT };
        PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
    }
    public void ShowTournamentCashChampionpopUp()
    {
        ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
        object[] _params = { TournamentManager.FEE_BRACKET_CASH_CHAMPION, TournamentManager.WIN_BRACKET_CASH_CHAMPION, ChallengeManager.CHALLENGE_WIN_TYPE_CASH, TournamentManager.BRACKET_TYPE_CHAMPION };
        PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
    }
    public void ShowTournamentCashLegendpopUp()
    {
        ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
        object[] _params = { TournamentManager.FEE_BRACKET_CASH_LEGEND, TournamentManager.WIN_BRACKET_CASH_LEGEND, ChallengeManager.CHALLENGE_WIN_TYPE_CASH, TournamentManager.BRACKET_TYPE_LEGEND };
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

    #region DeadCode
    //public void StepAnimation(string animatorName)
    //{
    //    var animator = GameObject.Find(animatorName).GetComponent<Animator>();
    //    animator.SetBool("step", true);
    //}
    //IEnumerator LoadGameScene(string GameSceneName)
    //{
    //    AsyncOperation AO = SceneManager.LoadSceneAsync(GameSceneName, LoadSceneMode.Additive);
    //    AO.allowSceneActivation = false;
    //    while (AO.progress < 0.9f)
    //    {

    //        yield return new WaitForSeconds(5);

    //    }
    //    AO.allowSceneActivation = true;
    //    LoaderManager.Get.LoaderController.HideLoader();
    //}
    //public void closeCurrentProfile()
    //{
    //    var animator = GameObject.Find("PopupProfile").GetComponent<Animator>();
    //    animator.SetBool("ShowProfile", false);
    //    UnityThreading.ActionThread thread;
    //    thread = UnityThreadHelper.CreateThread(() =>
    //    {
    //        Thread.Sleep(500);
    //        UnityThreadHelper.Dispatcher.Dispatch(() =>
    //        {
    //            ViewsEvents.Get.HideOverlayMenu(ViewsEvents.Get.Profile.gameObject);
    //        });
    //    });
    //}

    //public void TryAgainPopupOps()
    //{
    //    GameObject.Find("popupOups").transform.localScale = Vector3.zero;
    //    ViewsEvents.Get.SettingsClick();
    //}

    //public void cameraClick()
    //{
    //    if (Application.platform == RuntimePlatform.Android)
    //    {
    //        #region [ Intent intent = new Intent(); ]
    //        //instantiate the class Intent
    //        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
    //        //instantiate the object Intent
    //        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
    //        #endregion [ Intent intent = new Intent(); ]
    //        #region [ intent.setAction(Intent.ACTION_GET_CONTENT); ]
    //        //call setAction setting ACTION_SEND as parameter
    //        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_GET_CONTENT"));
    //        #endregion [ intent.setAction(Intent.ACTION_GET_CONTENT); ]
    //        #region [ intent.setData(Uri.parse("content://media/internal/images/media")); ]
    //        //instantiate the class Uri
    //        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
    //        //instantiate the object Uri with the parse of the url's file
    //        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "content://media/internal/images/media");
    //        //call putExtra with the uri object of the file
    //        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
    //        #endregion [ intent.setData(Uri.parse("content://media/internal/images/media")); ]
    //        //set the type of file
    //        intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
    //        #region [ startActivityForResult(intent , 1); ]
    //        //instantiate the class UnityPlayer
    //        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    //        //instantiate the object currentActivity
    //        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
    //        //call the activity with our Intent
    //        currentActivity.Call("startActivity", intentObject);
    //        #endregion [ startActivityForResult(intent , 1); ]
    //    }
    //    else if (Application.platform == RuntimePlatform.IPhonePlayer)
    //    {
    //    }
    //}

    //public async void CreatePassword()
    //{
    //    InputField NewPassword = PopupManager.Get.PopupViewPresenter.PopupUpdatePassNewPasswordInputField;
    //    InputField ConfirmPassword = PopupManager.Get.PopupViewPresenter.PopupUpdatePassConfirmPasswordInputField;

    //    if (NewPassword.text == ConfirmPassword.text)
    //    {

    //        User user = await UserManager.Get.getUser();
    //        string[] attrib = { "password" };
    //        string[] value = { NewPassword.text };
    //        UserManager.Get.UpdateUserByField(attrib, value);
    //        await UserManager.Get.logingIn(user.email, NewPassword.text.ToString());

    //        UnityThreadHelper.CreateThread(() =>
    //        {
    //            Thread.Sleep(500);
    //            UnityThreadHelper.Dispatcher.Dispatch(() =>
    //            {
    //                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_PASSWORD_UPDATED, PopupsText.Get.PasswordUpdated());
    //            });
    //        });

    //    }
    //}

    //public void HideHistory()
    //{
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

    //public IEnumerator checkConnectionHome(Action<bool> action)
    //{
    //    WWW www = new WWW("https://www.google.fr");
    //    float timer = 0;
    //    bool failed = false;
    //    while (!www.isDone)
    //    {
    //        if (timer > 5) { failed = true; break; }
    //        timer += Time.deltaTime;
    //        Debug.Log("timer :" + timer);
    //        yield return null;
    //    }
    //    if (failed)
    //    {
    //        www.Dispose();
    //        action(false);
    //    }
    //    else
    //    {
    //        if (www.error == null)
    //        {
    //            action(true);
    //        }
    //        else
    //        {
    //            action(false);
    //        }
    //    }
    //}

    //public void hideConnectionFailedPopup()
    //{
    //    ConnectivityController.CURRENT_ACTION = "";

    //    try { LoaderManager.Get.LoaderController.HideLoader(); } catch (ArgumentException ex) { }
    //    TryAgainLastAction();
    //}
    //public void TryAgainLastAction()
    //{
    //    UnityThreadHelper.CreateThread(() =>
    //    {
    //        Thread.Sleep(400);
    //        UnityThreadHelper.Dispatcher.Dispatch(() =>
    //        {
    //            Debug.Log(ConnectivityController.CURRENT_ACTION);
    //            switch (ConnectivityController.CURRENT_ACTION)
    //            {
    //                case ConnectivityController.CHALLENGE_ACTION:
    //                    break;
    //                case ConnectivityController.HOME_ACTION:
    //                    ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Menu.gameObject);
    //                    ViewsEvents.Get.ShowScene(ViewsEvents.Get.Menu.Home);
    //                    break;
    //                case ConnectivityController.ENTER_ESPORT_TOURNAMENT_ACTION:
    //                    Seemba.Get.Enter();
    //                    break;
    //                case ConnectivityController.CREDIT_ACTION:
    //                    break;
    //                case ConnectivityController.LOGIN_ACTION:
    //                    UserService service = new UserService();
    //                    break;
    //                case ConnectivityController.PERSONNEL_INFO_ACTION:
    //                    GameObject.Find("CanvasM").GetComponent<PersonelInfoController>().enabled = false;
    //                    GameObject.Find("CanvasM").GetComponent<PersonelInfoController>().enabled = true;
    //                    break;
    //                case ConnectivityController.HISTORY_ACTION:
    //                    GameObject.Find("CanvasM").GetComponent<HistoryListController>().enabled = false;
    //                    GameObject.Find("CanvasM").GetComponent<HistoryListController>().enabled = true;
    //                    break;
    //                case ConnectivityController.PERSONNEL_INFO_WITHDRAW_ACTION:
    //                    GameObject.Find("CanvasWithdrawalInfo").GetComponent<InfoPersonnelWithdraw>().enabled = false;
    //                    GameObject.Find("CanvasWithdrawalInfo").GetComponent<InfoPersonnelWithdraw>().enabled = true;
    //                    break;
    //            }
    //        });
    //    });
    //}

    //    public bool checkDeveleperMode(string gainType)
    //    {

    //        return false;
    //#if UNITY_ANDROID
    //        if (gainType == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
    //        {
    //            using (var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
    //            {
    //                var context = actClass.GetStatic<AndroidJavaObject>("currentActivity");
    //                AndroidJavaClass systemGlobal = new AndroidJavaClass("android.provider.Settings$Global");
    //                var DevMode = 1;
    //                try
    //                {
    //                    DevMode = systemGlobal.CallStatic<int>("getInt", context.Call<AndroidJavaObject>("getContentResolver"), "development_settings_enabled");
    //                }
    //                catch (NullReferenceException ex) { }
    //                if (DevMode == 1)
    //                {
    //                    PopupManager.Get.PopupController.ShowPopup(PopupType.DEV_MODE, PopupsText.Get.dev_mode());
    //                }
    //                return (DevMode == 1) ? true : false;
    //            }
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //#else
    //		return false;
    //#endif
    //    }

    //public bool checkVPNBeforePlaying(string gainType)
    //{
    //    UnityThreading.ActionThread thread;
    //    bool isVpnConnected = false;
    //    if (gainType == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
    //    {
    //        VPNManager vpn = new VPNManager();
    //        isVpnConnected = vpn.isVpnConnected();
    //        if (isVpnConnected == true)
    //        {
    //            UnityThreading.ActionThread thread1;
    //            thread1 = UnityThreadHelper.CreateThread(() =>
    //            {
    //                Thread.Sleep(500);
    //                UnityThreadHelper.Dispatcher.Dispatch(() =>
    //                {
    //                    PopupManager.Get.PopupController.ShowPopup(PopupType.VPN, PopupsText.Get.vpn());
    //                });
    //            });
    //            return isVpnConnected;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //    else
    //    {
    //        return isVpnConnected;
    //    }
    //}
    //public bool checkCountryBeforePlaying(string code, string gainType)
    //{
    //    if (gainType == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
    //    {
    //        return CountryController.checkCountry(code);
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}

    //public void ConfirmJoiningBubbleTournament(float fee, float gain, string gain_type)
    //{
    //    if (UserManager.Get.CurrentUser.bubble_credit < fee)
    //    {
    //        UnityThreading.ActionThread thread1;
    //        thread1 = UnityThreadHelper.CreateThread(() =>
    //        {
    //            Thread.Sleep(500);
    //            UnityThreadHelper.Dispatcher.Dispatch(() =>
    //            {
    //                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_INSUFFICIENT_BUBBLES, PopupsText.Get.insufficient_bubbles());
    //            });
    //        });
    //    }
    //    else
    //    {
    //        UnityThreadHelper.CreateThread(() =>
    //        {
    //            Thread.Sleep(300);
    //            UnityThreadHelper.Dispatcher.Dispatch(() =>
    //            {
    //                joinBubbleTournament(gain, gain_type, fee);
    //            });
    //        });
    //    }
    //}
    //public void ConfirmJoiningCashTournament(float fee, float gain, string gain_type)
    //{
    //    Debug.LogWarning("here");
    //    if (float.Parse(UserManager.Get.GetCurrentMoneyCredit()) < fee)
    //    {
    //        UnityThreading.ActionThread thread1;
    //        thread1 = UnityThreadHelper.CreateThread(() =>
    //        {
    //            Thread.Sleep(500);
    //            UnityThreadHelper.Dispatcher.Dispatch(() =>
    //            {
    //                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_INSUFFICIENT_BALANCE, PopupsText.Get.insufficient_balance());
    //            });
    //        });
    //    }
    //    else
    //    {
    //        UnityThreadHelper.CreateThread(() =>
    //        {
    //            Thread.Sleep(300);
    //            UnityThreadHelper.Dispatcher.Dispatch(() =>
    //            {
    //                joinCashTournament(gain, gain_type, fee);
    //            });
    //        });
    //    }
    //}
    //public async void joinBubbleTournament(float gain, string gain_type, float fee)
    //{
    //    string userId = UserManager.Get.getCurrentUserId();
    //    string token = UserManager.Get.getCurrentSessionToken();
    //    string tournamentId = await TournamentManager.Get.JoinOrCreateTournament(TournamentManager.TOURNAMENT_8, gain, gain_type, userId, token);
    //    if (tournamentId != null)
    //    {
    //        var newBubbleCredit = UserManager.Get.CurrentUser.bubble_credit - fee;
    //        UserManager.Get.UpdateUserBubblesCredit(newBubbleCredit.ToString());
    //        TournamentController.setCurrentTournamentID(tournamentId);
    //        ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Brackets.gameObject);
    //    }
    //}
    //public void joinCashTournament(float gain, string gain_type, float fee)
    //{
    //    string code = null;
    //    try
    //    {
    //        code = UserManager.Get.CurrentUser.country_code;
    //    }
    //    catch (NullReferenceException ex)
    //    {
    //    }
    //    if (checkCountryBeforePlaying(code, gain_type) == true)
    //    {
    //        if (checkDeveleperMode(gain_type) == false)
    //        {
    //            StartCoroutine(checkInternetConnection(async (isConnected) =>
    //            {
    //                if (isConnected == true)
    //                {
    //                    LoaderManager.Get.LoaderController.HideLoader();
    //                    if (checkVPNBeforePlaying(gain_type) == false)
    //                    {
    //                        string userId = UserManager.Get.getCurrentUserId();
    //                        string token = UserManager.Get.getCurrentSessionToken();
    //                        string tournamentId = await TournamentManager.Get.JoinOrCreateTournament(TournamentManager.TOURNAMENT_8, gain, gain_type, userId, token);
    //                        if (tournamentId != null)
    //                        {
    //                            UserManager.Get.UpdateUserMoneyCredit((float.Parse(UserManager.Get.GetCurrentMoneyCredit()) - fee).ToString("N2"));
    //                            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Brackets.gameObject);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    LoaderManager.Get.LoaderController.HideLoader();
    //                    UnityThreadHelper.CreateThread(() =>
    //                    {
    //                        Thread.Sleep(300);
    //                        UnityThreadHelper.Dispatcher.Dispatch(() =>
    //                        {
    //                            PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());
    //                        });
    //                    });
    //                }
    //            }));
    //        }
    //    }
    //}

    //public void returnToHome()
    //{
    //    UnityThreading.ActionThread thread;
    //    ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Menu.gameObject);
    //    ViewsEvents.Get.ShowScene(ViewsEvents.Get.Menu.Home);
    //    thread = UnityThreadHelper.CreateThread(() =>
    //    {
    //        Thread.Sleep(2000);
    //        UnityThreadHelper.Dispatcher.Dispatch(() =>
    //        {
    //            GameObject.Find("popup").transform.localScale = new Vector3(1, 1, 1);
    //            GameObject.Find("TextMain").GetComponent<Text>().text = GameObject.Find("coach").GetComponent<Text>().text;
    //        });
    //    });
    //}

    //public void otherAmountClick()
    //{
    //    GameObject.Find("AutreButton").transform.localScale = new Vector3(0, 0, 0);
    //    GameObject.Find("Code Promo Button").transform.localScale = Vector3.one;
    //    GameObject.Find("enterAmount").transform.localScale = new Vector3(1, 1, 1);
    //    GameObject.Find("validateAmount").transform.localScale = new Vector3(1, 1, 1);
    //    GameObject.Find("enterPromoCode").transform.localScale = new Vector3(0, 0, 0);
    //    GameObject.Find("validatePromoCode").transform.localScale = new Vector3(0, 0, 0);
    //}
    //public void promoCodeClick()
    //{
    //    GameObject.Find("AutreButton").transform.localScale = Vector3.one;
    //    GameObject.Find("Code Promo Button").transform.localScale = new Vector3(0, 0, 0);
    //    GameObject.Find("enterPromoCode").transform.localScale = new Vector3(1, 1, 1);
    //    GameObject.Find("validatePromoCode").transform.localScale = new Vector3(1, 1, 1);
    //    GameObject.Find("enterAmount").transform.localScale = new Vector3(0, 0, 0);
    //    GameObject.Find("validateAmount").transform.localScale = new Vector3(0, 0, 0);
    //}

    //public async void uploadDoc(string path, string filename)
    //{
    //    string userId = UserManager.Get.getCurrentUserId();
    //    string userToken = UserManager.Get.getCurrentSessionToken();
    //    bool isAttached = false;
    //    UnityThreading.ActionThread thread;
    //    WithdrawManager wm = new WithdrawManager();
    //    LoaderManager.Get.LoaderController.ShowLoader(null);
    //    thread = UnityThreadHelper.CreateThread(() =>
    //    {
    //        string uploadRes = wm.HttpUploadFile(path, userToken);
    //        if (!string.IsNullOrEmpty(uploadRes) && uploadRes != "error")
    //        {
    //            InfoPersonnelWithdraw.currentIdProof = uploadRes;
    //            switch (filename)
    //            {
    //                case "Passport":
    //                    string[] attrib = { "passport_uploaded" };
    //                    string[] value = { "true" };
    //                    UserManager.Get.UpdateUserByField(attrib, value);
    //                    isAttached = wm.attachDocToAccount("document", uploadRes, "front", userToken);
    //                    InfoPersonnelWithdraw.passportUploaded = true;
    //                    GameObject.Find("IDFront").GetComponent<Button>().interactable = false;
    //                    GameObject.Find("IDBack").GetComponent<Button>().interactable = false;
    //                    InfoPersonnelWithdraw.currentDocUploaded = "passport_uploaded";
    //                    break;
    //                case "IDFront":
    //                    string[] attrib1 = { "id_proof_1_uploaded" };
    //                    string[] value1 = { "true" };
    //                    UserManager.Get.UpdateUserByField(attrib1, value1);
    //                    isAttached = wm.attachDocToAccount("document", uploadRes, "front", userToken);
    //                    InfoPersonnelWithdraw.currentDocUploaded = "id_proof_1_uploaded";
    //                    InfoPersonnelWithdraw.idProof1Uploaded = true;
    //                    GameObject.Find("IDPassport").GetComponent<Button>().interactable = false;
    //                    break;
    //                case "IDBack":
    //                    string[] attrib2 = { "id_proof_2_uploaded" };
    //                    string[] value2 = { "true" };
    //                    UserManager.Get.UpdateUserByField(attrib2, value2);
    //                    isAttached = wm.attachDocToAccount("document", uploadRes, "back", userToken);
    //                    InfoPersonnelWithdraw.currentDocUploaded = "id_proof_2_uploaded";
    //                    InfoPersonnelWithdraw.idProof2Uploaded = true;
    //                    GameObject.Find("IDPassport").GetComponent<Button>().interactable = false;
    //                    break;
    //                case "Address":
    //                    string[] attrib3 = { "residency_proof_uploaded" };
    //                    string[] value3 = { "true" };
    //                    UserManager.Get.UpdateUserByField(attrib3, value3);
    //                    isAttached = wm.attachDocToAccount("additional_document", uploadRes, "front", userToken);
    //                    InfoPersonnelWithdraw.currentDocUploaded = "residency_proof_uploaded";
    //                    break;
    //            }
    //            UnityThreadHelper.Dispatcher.Dispatch(() =>
    //            {
    //                LoaderManager.Get.LoaderController.HideLoader();
    //            });
    //        }
    //        else
    //        {
    //            UnityThreadHelper.Dispatcher.Dispatch(() =>
    //            {
    //                LoaderManager.Get.LoaderController.HideLoader();
    //                withdrawFailed("Upload", "Failed", "Please check your internet connection or try again later");
    //            });
    //        }
    //    });
    //}
    #endregion
}
