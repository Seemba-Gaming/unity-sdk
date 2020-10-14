using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
using SimpleJSON;
using System;
using System.Text.RegularExpressions;
using System.Globalization;
public class EventsController : MonoBehaviour
{
    //public string currentScene;
    public static bool advFound;
    public static InputField username;
    public static InputField conf_username;
    public static string ChallengeType, ChallengeName;
    public static string TournoiName;
    public static string adversaireEgalité;
    public ToggleGroup toggleGroup;
    public string currentScene;
    public string CurrentChallengeId;
    ChallengeManager cm = new ChallengeManager();
    UserManager um = new UserManager();
    public UnityEngine.UI.Image mImage;
    public static int MoneyToAdd;
    public string Tournoi8Gain, Tournoi8GainType, CurrentTourInTournoi8;
    public Text NbPlayer;
    public GameObject CoachPanel;
    public GameObject Pro1vs1More;
    public GameObject Pro1BracketMore;
    public GameObject Pro1vs1Less;
    public GameObject Pro1BracketLess, Loading;
    public Text AgeText;
    public static string backTo;
    private Text _textDatePredefined;
    private string _selectedDateString2 = "1995-09-15";
    public static string last_view;
    private String SelectedDateString2
    {
        get
        {
            return _selectedDateString2;
        }
        set
        {
            _selectedDateString2 = value;
            UserManager.currentBirthdate = value;
            try
            {
                //Debug.Log("NBS:69");
                _textDatePredefined.text = SelectedDateString2;
            }
            catch (NullReferenceException ex) { }
        }
    }

    public void Awake()
    {

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
            ShowPopup("popupEqualityRefund");
        }
        else
        {
            ContinueToShowAdsPopup();
        }
    }
    public void ContinueEqualityRefund()
    {
        Toggle dntshowagain = GameObject.Find("DontShowAgain").GetComponent<Toggle>();
        if (dntshowagain.isOn)
        {
            PlayerPrefs.SetString("DontShowAgainPopupEqualityRefund", "true");
        }
        UnityThreading.ActionThread thread;
        HidePopup("popupEqualityRefund", true);
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(500);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                ContinueToShowAdsPopup();
            });
        });
    }
    public void FinancialInfo()
    {
        SceneManager.LoadScene("FinancialInfo", LoadSceneMode.Additive);
    }
    public void CloseFinancialInfo()
    {
        GameObject.Find("Canvas").GetComponent<Animator>().SetBool("showView", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(500);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadScene("FinancialInfo");
            });
        });
    }
    public void IdProof()
    {
        SceneManager.LoadScene("IDProof", LoadSceneMode.Additive);
    }
    public void CloseIdProof()
    {
        GameObject.Find("Container_IDProof").GetComponent<Animator>().SetBool("showView", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(500);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadScene("IDProof");
            });
        });
    }
    public void OpenWallet(string last_view)
    {
        ViewsEvents v = new ViewsEvents();
        BottomMenuController bottomMenu = BottomMenuController.getInstance();
        bottomMenu.unselectHome();
        bottomMenu.unselectHaveFun();
        bottomMenu.unselectWinMoney();
        bottomMenu.selectSettings();
        v.WalletClick(last_view);
    }
    //Live
    public void ShowReplayNotAvailablePopup()
    {
        GameObject.Find("popupNotAvailable").transform.localScale = Vector3.one;
    }
    public void HideReplayNotAvailablePopup()
    {
        GameObject.Find("popupNotAvailable").transform.localScale = Vector3.zero;
    }
    public void ShowPopupError(string popupname)
    {
        BottomMenuController.Hide();
        if (popupname == "popupMissingInfo" || popupname == "popupProhibitedLocationWithdraw")
        {
            try
            {
                GameObject.Find("CalqueWithdraw").transform.localScale = Vector3.one;
            }
            catch (Exception e)
            {
            }
        }
        else if (popupname == "popupConnectionFailed")
        {
            GameObject.Find("CalqueConnectionFailed").transform.localScale = Vector3.one;
        }
        else if (popupname == "popupProhibitedLocationWallet")
        {
            GameObject.Find("CalqueWallet").transform.localScale = Vector3.one;
        }
        else
        {
            try
            {
                GameObject.Find("Calque").transform.localScale = Vector3.one;
            }
            catch (Exception e)
            {
            }
        }
        var animator = GameObject.Find(popupname).GetComponent<Animator>();
        animator.SetBool("Show Error", true);
    }
    public void ShowPopup(string popupname)
    {
        BottomMenuController.Hide();
        try
        {
            GameObject.Find("Calque").transform.localScale = Vector3.one;
        }
        catch (Exception e) { }
        var animator = GameObject.Find(popupname).GetComponent<Animator>();
        animator.SetBool("showSucces", true);
    }
    public void HidePopup(string popupname, bool hideCalque)
    {
        var animator = GameObject.Find(popupname).GetComponent<Animator>();
        animator.SetBool("showSucces", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(300);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
           {
               if (hideCalque == true)
               {
                   try
                   {
                       GameObject.Find("Calque").transform.localScale = Vector3.zero;
                   }
                   catch (Exception e)
                   {
                   }
                   BottomMenuController.Show();
               }
           });
        });
    }
    public void ShowPopupProfile()
    {
        //var animator = GameObject.Find("PopupProfile").GetComponent<Animator>();
        //animator.SetBool("ShowProfile",true);
    }
    public void HidePopupProfile()
    {
        var animator = GameObject.Find("PopupProfile").GetComponent<Animator>();
        animator.SetBool("ShowProfile", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(500);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                try
                {
                    SceneManager.UnloadScene("Profile");
                }
                catch (Exception e) { }
            });
        });
    }
    public void HidePopupError(string popupname, bool hideCalque)
    {
        var animator = GameObject.Find(popupname).GetComponent<Animator>();
        animator.SetBool("Show Error", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(400);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                BottomMenuController.Show();
                if (hideCalque == true)
                {
                    if (popupname == "popupMissingInfo")
                    {
                        GameObject.Find("CalqueWithdraw").transform.localScale = Vector3.zero;
                        BottomMenuController.Hide();
                    }
                    if (popupname == "popupConnectionFailed")
                    {
                        GameObject.Find("CalqueConnectionFailed").transform.localScale = Vector3.zero;
                    }
                    else
                    {
                        GameObject.Find("Calque").transform.localScale = Vector3.zero;
                    }
                    
                }
            });
        });
    }
    public void gotItButton()
    {
        ViewsEvents ve = new ViewsEvents();
        HidePopupError("popupAgeTooYoung", true);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(450);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                ve.HaveFunClick();
            });
        });
    }
    public void ReconnectExipiredSession()
    {
        HidePopupError("popupSessionExpired", true);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(100);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.LoadSceneAsync("Login");
            });
        });
    }
    public void refreshButton()
    {
        HidePopupError("popupOups", true);
    }
    public void missingInfoWithdrawHide()
    {
        HidePopupError("popupMissingInfo", true);
    }
    public void missingInfowithdrawContinue()
    {
        HidePopupError("popupMissingInfo", true);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(300);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.LoadScene("WithdrawalInfo", LoadSceneMode.Additive);
            });
        });
    }
    public void uploadDoc(string path, string filename)
    {
        string userId = um.getCurrentUserId();
        string userToken = um.getCurrentSessionToken();
        bool isAttached = false;
        UnityThreading.ActionThread thread;
        WithdrawManager wm = new WithdrawManager();
        //GameObject.Find (filename).GetComponent<Image> ().transform.localScale = Vector3.one;
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        thread = UnityThreadHelper.CreateThread(() =>
        {
            string uploadRes = wm.HttpUploadFile(path, userToken);
            if (!string.IsNullOrEmpty(uploadRes) && uploadRes != "error")
            {
                InfoPersonnelWithdraw.currentIdProof = uploadRes;
                switch (filename.ToUpper())
                {
                    case "PASSPORT":
                        string[] attrib = { "passport_uploaded" };
                        string[] value = { "true" };
                        um.UpdateUserByField(userId, userToken, attrib, value);
                        isAttached = wm.attachDocToAccount("document", uploadRes, "front", userToken);
                        InfoPersonnelWithdraw.passportUploaded = true;
                        GameObject.Find("IDFront").GetComponent<Button>().interactable = false;
                        GameObject.Find("IDBack").GetComponent<Button>().interactable = false;
                        InfoPersonnelWithdraw.currentDocUploaded = "passport_uploaded";
                        break;
                    case "IDFRONT":
                        string[] attrib1 = { "id_proof_1_uploaded" };
                        string[] value1 = { "true" };
                        um.UpdateUserByField(userId, userToken, attrib1, value1);
                        isAttached = wm.attachDocToAccount("document", uploadRes, "front", userToken);
                        InfoPersonnelWithdraw.currentDocUploaded = "id_proof_1_uploaded";
                        InfoPersonnelWithdraw.idProof1Uploaded = true;
                        GameObject.Find("IDPassport").GetComponent<Button>().interactable = false;
                        break;
                    case "IDBACK":
                        string[] attrib2 = { "id_proof_2_uploaded" };
                        string[] value2 = { "true" };
                        um.UpdateUserByField(userId, userToken, attrib2, value2);
                        isAttached = wm.attachDocToAccount("document", uploadRes, "back", userToken);
                        InfoPersonnelWithdraw.currentDocUploaded = "id_proof_2_uploaded";
                        InfoPersonnelWithdraw.idProof2Uploaded = true;
                        GameObject.Find("IDPassport").GetComponent<Button>().interactable = false;
                        break;
                    case "ADDRESS":
                        string[] attrib3 = { "residency_proof_uploaded" };
                        string[] value3 = { "true" };
                        um.UpdateUserByField(userId, userToken, attrib3, value3);
                        isAttached = wm.attachDocToAccount("additional_document", uploadRes, "front", userToken);
                        InfoPersonnelWithdraw.currentDocUploaded = "residency_proof_uploaded";
                        break;
                }
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    SceneManager.UnloadScene("Loader");
                });
            }
            else
            {
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    SceneManager.UnloadSceneAsync("Loader");
                    try
                    {
                        SceneManager.UnloadSceneAsync("FinancialInfo");
                    }
                    catch (ArgumentException ex) { }
                    try
                    {
                        SceneManager.UnloadSceneAsync("IDProof");
                    }
                    catch (ArgumentException ex) { }
                    try
                    {
                        SceneManager.UnloadSceneAsync("WithdrawalInfo");
                    }
                    catch (ArgumentException ex) { }
                    withdrawFailed("Upload", "Failed", "Please check your internet connection or try again later");
                });
            }
        });
    }
    public void withdrawFailed(string headertext, string headertext2, string msg)
    {
        UnityThreadHelper.Dispatcher.Dispatch(() =>
        {
            backToWinMoney();
            try
            {
                SceneManager.UnloadScene("Loader");
            }
            catch (ArgumentException ex) { }
            if (!string.IsNullOrEmpty(msg))
            {
                GameObject.Find("OupsTextMain").GetComponent<Text>().text = msg;
            }
            if (!string.IsNullOrEmpty(headertext))
            {
                GameObject.Find("headerText").GetComponent<Text>().text = headertext;
            }
            if (!string.IsNullOrEmpty(headertext2))
            {
                GameObject.Find("headerText2").GetComponent<Text>().text = headertext2;
            }
            ShowPopupError("popupOups");
        });
    }
    public void backToWinMoney()
    {
        ViewsEvents view = new ViewsEvents();
        view.WinMoneyClick();
        BottomMenuController bottomMenu = BottomMenuController.getInstance();
        bottomMenu.selectWinMoney();
        bottomMenu.unselectSettings();
        bottomMenu.unselectHome();
        bottomMenu.unselectHaveFun();
    }
    public void showProfil()
    {
        SceneManager.LoadScene("Profile", LoadSceneMode.Additive);
    }
    public void closeProfil()
    {
        HidePopupProfile();
        //SceneManager.UnloadScene ("Profile");
    }
    public void StepAnimation(string animatorName)
    {
        var animator = GameObject.Find(animatorName).GetComponent<Animator>();
        animator.SetBool("step", true);
    }
    public void closeCongratWithdraw()
    {
        HidePopup("popupCongratWithdraw", true);
    }
    public void closeInfoPersonnelWithdraw()
    {
        SceneManager.UnloadScene("WithdrawalInfo");
    }
    public void TryAgainPopupOps()
    {
        GameObject.Find("popupOups").transform.localScale = Vector3.zero;
        ViewsEvents e = new ViewsEvents();
        e.SettingsClick();
    }
    public void ButtonPopupCongrat()
    {
        HidePopup("popupCongrat", true);
    }
    public void CloseOupsPopup()
    {
        GameObject.Find("popupOups").transform.localScale = Vector3.zero;
    }
    //Open TCS in google drive
    public void TCs()
    {
        Application.OpenURL("http://www.seemba.com/downloads/T&Cs_Seemba.pdf");
    }
    //Open Privacy Policy in google drive
    public void PrivacyPolicy()
    {
        Application.OpenURL("http://www.seemba.com/downloads/Privacy_Policy.pdf");
    }
    public void closePopupPayment()
    {
        HidePopup("popup_payment", true);
    }
    public void backFromTournamentDetails()
    {
        SceneManager.UnloadScene("TournamentDetails");
    }
    public void backFromBracket()
    {
        SceneManager.LoadScene("Home");
    }
    public void ShowMore1vs1Pro()
    {
        Pro1vs1More.gameObject.SetActive(true);
        Pro1vs1Less.gameObject.SetActive(false);
    }
    public void ShowLess1vs1Pro()
    {
        Pro1vs1More.gameObject.SetActive(false);
        Pro1vs1Less.gameObject.SetActive(true);
    }
    public void ShowMoreBracketPro()
    {
        Pro1BracketMore.gameObject.SetActive(true);
        Pro1BracketLess.gameObject.SetActive(false);
    }
    public void ShowLessBracketPro()
    {
        Pro1BracketMore.gameObject.SetActive(false);
        Pro1BracketLess.gameObject.SetActive(true);
    }
    public void openCurrentProfile()
    {
        SceneManager.LoadScene("Profile", LoadSceneMode.Additive);
        ProfileViewPresenter.PlayerId = um.getCurrentUserId();
        ShowPopupProfile();
    }
    public void closeCurrentProfile()
    {
        var animator = GameObject.Find("PopupProfile").GetComponent<Animator>();
        animator.SetBool("ShowProfile", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(500);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadScene("CurrentProfile");
            });
        });
    }
    public void playAgain()
    {
        Debug.Log("CurrentChallenge:" + ChallengeManager.CurrentChallenge.gain);
        Debug.Log("CurrentChallenge:" + ChallengeManager.CurrentChallenge.gain_type);
        
        
        OpponentFound.adversaireName = null; //usr.username
        OpponentFound.Avatar = null;
        OpponentFound.AdvCountryCode = null; //.country_code;
        advFound = false;
        string gain = null;
        string gainType = null;
        string entrefee = GameObject.Find("Frais d'entréeValeur").GetComponent<Text>().text;
        Text entry_fee, Gain;
        switch (ChallengeManager.CurrentChallenge.gain_type)
        {
            case ChallengeManager.CHALLENGE_WIN_TYPE_CASH:
                if (float.Parse(ChallengeManager.CurrentChallenge.gain) == ChallengeManager.WIN_1V1_CASH_CONFIDENT)
                {
                    gain = ChallengeManager.WIN_1V1_CASH_CONFIDENT.ToString();
                    gainType = ChallengeManager.CHALLENGE_WIN_TYPE_CASH;
                    ChallengeName = "NoviceArgent";
                    entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
                    entry_fee.text = "Entry fee: " + ChallengeManager.FEE_1V1_CASH_CONFIDENT.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                    Gain = GameObject.Find("gain").GetComponent<Text>();
                    Gain.text = ChallengeManager.WIN_1V1_CASH_CONFIDENT.ToString("N2") + " " + CurrencyManager.CURRENT_CURRENCY;
                }
                else if (float.Parse(ChallengeManager.CurrentChallenge.gain) == ChallengeManager.WIN_1V1_CASH_CHAMPION)
                {
                    gain = ChallengeManager.WIN_1V1_CASH_CHAMPION.ToString();
                    gainType = ChallengeManager.CHALLENGE_WIN_TYPE_CASH;
                    ChallengeName = "AmateurArgent";
                    entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
                    entry_fee.text = "Entry fee: " + ChallengeManager.FEE_1V1_CASH_CHAMPION.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                    Gain = GameObject.Find("gain").GetComponent<Text>();
                    Gain.text = ChallengeManager.WIN_1V1_CASH_CHAMPION.ToString("N2") + " " + CurrencyManager.CURRENT_CURRENCY;
                }
                else if (float.Parse(ChallengeManager.CurrentChallenge.gain) == ChallengeManager.WIN_1V1_CASH_LEGEND)
                {
                    gain = ChallengeManager.WIN_1V1_CASH_LEGEND.ToString();
                    gainType = ChallengeManager.CHALLENGE_WIN_TYPE_CASH;
                    ChallengeName = "AmateurArgent";
                    entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
                    entry_fee.text = "Entry fee: " + ChallengeManager.FEE_1V1_BUBBLES_LEGEND.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                    Gain = GameObject.Find("gain").GetComponent<Text>();
                    Gain.text = ChallengeManager.WIN_1V1_CASH_LEGEND.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                }
                break;
            case ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES:
                if (float.Parse(ChallengeManager.CurrentChallenge.gain) == ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT)
                {
                    gain = ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT.ToString();
                    gainType = ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES;
                    GameObject.Find("Entry fee").GetComponent<Text>().text = "Entry fee: " + ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT;
                    GameObject.Find("gain").GetComponent<Text>().text = ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT.ToString();
                    GameObject.Find("gainBubbleText").GetComponent<Text>().transform.localScale = Vector3.one;
                    GameObject.Find("gainBubbleText").GetComponent<Text>().text = "Bubble";
                    GameObject.Find("_bubble").GetComponent<Image>().transform.localScale = Vector3.one;
                    ChallengeName = "NoviceGoutte";
                }
                else if (float.Parse(ChallengeManager.CurrentChallenge.gain) == ChallengeManager.WIN_1V1_BUBBLES_CHAMPION)
                {
                    gain = ChallengeManager.WIN_1V1_BUBBLES_CHAMPION.ToString();
                    gainType = ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES;
                    GameObject.Find("Entry fee").GetComponent<Text>().text = "Entry fee: " + ChallengeManager.FEE_1V1_BUBBLES_CHAMPION;
                    GameObject.Find("gainBubbleText").GetComponent<Text>().transform.localScale = Vector3.one;
                    GameObject.Find("gain").GetComponent<Text>().text = ChallengeManager.WIN_1V1_BUBBLES_CHAMPION.ToString();
                    GameObject.Find("_bubble").GetComponent<Image>().transform.localScale = Vector3.one;
                    ChallengeName = "AmateurGoutte";
                }
                else if (float.Parse(ChallengeManager.CurrentChallenge.gain) == ChallengeManager.WIN_1V1_BUBBLES_LEGEND)
                {
                    gain = ChallengeManager.WIN_1V1_BUBBLES_LEGEND.ToString();
                    gainType = ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES;
                    GameObject.Find("Entry fee").GetComponent<Text>().text = "Entry fee: " + ChallengeManager.FEE_1V1_BUBBLES_LEGEND;
                    GameObject.Find("gain").GetComponent<Text>().text = ChallengeManager.WIN_1V1_BUBBLES_LEGEND.ToString();
                    GameObject.Find("gainBubbleText").GetComponent<Text>().transform.localScale = Vector3.one;
                    GameObject.Find("_bubble").GetComponent<Image>().transform.localScale = Vector3.one;
                    ChallengeName = "ConfirmedGoutte";
                }
                break;
        }
        ShowPopup("popup");
    }
    public void QuitToFirstScene()
    {
        BackgroundController.CurrentBackground = null;
        EventsController.ChallengeType = null;
        SceneManager.LoadScene(0);
    }
    public void SeeHowMuchYouWon()
    {
        GameObject.Find("popup").transform.localScale = Vector3.one;
    }
    public void GreatButton()
    {
        GameObject.Find("popup").transform.localScale = Vector3.zero;
    }
    public void Play()
    {
        
        BackgroundController.CurrentBackground = null;
        EventsController.advFound = false;
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        StartCoroutine(LoadGameScene(GamesManager.GAME_SCENE_NAME));
    }
    IEnumerator LoadGameScene(String GameSceneName)
    {
        AsyncOperation AO = SceneManager.LoadSceneAsync(GameSceneName);
        AO.allowSceneActivation = false;
        while (AO.progress < 0.9f)
        {

            yield return new WaitForSeconds(5);

        }
        AO.allowSceneActivation = true;
        
    }
    public void CreatePassword()
    {
        InputField NewPassword = GameObject.Find("NewPasswordCreate").GetComponent<InputField>();
        InputField ConfirmPassword = GameObject.Find("ConfirmPasswordCreate").GetComponent<InputField>();
        string userId = um.getCurrentUserId();
        string userToken = um.getCurrentSessionToken();
        if (NewPassword.text == ConfirmPassword.text)
        {
            UnityThreading.ActionThread thread;
            GameObject.Find("WaitingCreatePassword").GetComponent<Image>().transform.localScale = Vector3.one;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                User user = um.getUser(userId, userToken);
                string[] attrib = { "password" };
                string[] value = { NewPassword.text };
                um.UpdateUserByField(userId, userToken, attrib, value);
                um.logingIn(user.email, NewPassword.text.ToString());
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    GameObject.Find("WaitingCreatePassword").GetComponent<Image>().transform.localScale = Vector3.zero;
                    HidePopup("popupUpdatePassword", false);
                    UnityThreadHelper.CreateThread(() =>
                    {
                        Thread.Sleep(500);
                        UnityThreadHelper.Dispatcher.Dispatch(() =>
                        {
                            ShowPopup("popupPasswordUpdated");
                        });
                    });
                });
            });
        }
    }
    public void OpenCreatePassword()
    {
        GameObject.Find("popupCreatePassword").transform.localScale = Vector3.one;
        GameObject.Find("popup").transform.localScale = Vector3.zero;
    }
    public void HidePopupChange()
    {
        GameObject.Find("popupChangeAccount").transform.localScale = Vector3.zero;
    }
    public void HidePopupCreatePassword()
    {
        try
        {
            HidePopup("popupCurrentPassword", false);
        }
        catch (NullReferenceException)
        {
        }
        try
        {
            HidePopup("popupUpdatePassword", false);
        }
        catch (NullReferenceException)
        {
        }
        try
        {
            HidePopup("popupPasswordUpdated", true);
        }
        catch (NullReferenceException)
        {
        }
        try
        {
            HidePopup("popupForgetPassword", true);
        }
        catch (NullReferenceException)
        {
        }
        try
        {
            HidePopup("popupEmailSent", true);
        }
        catch (NullReferenceException)
        {
        }
    }
    public void UpdatePassword()
    {
        string userId = um.getCurrentUserId();
        string userToken = um.getCurrentSessionToken();
        InputField OldPassword = GameObject.Find("OldPassword").GetComponent<InputField>();
        InputField NewPassword = GameObject.Find("NewPassword").GetComponent<InputField>();
        InputField ConfirmPassword = GameObject.Find("ConfirmPassword").GetComponent<InputField>();
        User user = um.getUser(userId, userToken);
        string res = um.logingIn(user.username, OldPassword.text.ToString());
        if (res == "success")
        {
            if (NewPassword.text == ConfirmPassword.text)
            {
                string[] attrib = { "password" };
                string[] value = { NewPassword.text };
                um.UpdateUserByField(userId, userToken, attrib, value);
                um.logingIn(user.username, NewPassword.text.ToString());
                GameObject.Find("popup").transform.localScale = Vector3.zero;
            }
            else
            {
            }
        }
    }
    void imageHandle(string message, byte[] data)
    {
        JSONArray jsa = (JSONArray)JSON.Parse(message);
        JSONNode jsn = jsa[0];
        int w = jsn["width"].AsInt;
        int h = jsn["height"].AsInt;
        Texture2D xx = new Texture2D(w, h, TextureFormat.BGRA32, false);
        xx.LoadImage(data);
        Sprite newSprite = Sprite.Create(xx as Texture2D, new Rect(0f, 0f, xx.width, xx.height), Vector2.zero);
        mImage.sprite = newSprite;
    }
    public void hidePopupAgeVerification()
    {
        HidePopup("popupAgeVerification", true);
    }
    public void hideUnkownDevice()
    {
        HidePopupError("UnknownDevice", true);
    }
    public void ContinueToShowAdsPopup()
    {
        OpponentFound.adversaireName = null;//usr.username
        OpponentFound.Avatar = null;
        OpponentFound.AdvCountryCode = null;//.country_code;
        advFound = false;
        if (SceneManager.GetActiveScene().name == "Home")
        {
            try
            {
                SceneManager.UnloadSceneAsync("ResultWIN");
            }
            catch (ArgumentException ex)
            {
            }
            try
            {
                SceneManager.UnloadSceneAsync("ResultLose");
            }
            catch (ArgumentException ex)
            {
            }
            try
            {
                SceneManager.UnloadSceneAsync("ResultEquality");
            }
            catch (ArgumentException ex)
            {
            }
            try
            {
                SceneManager.UnloadSceneAsync("ResultWaiting");
            }
            catch (ArgumentException ex)
            {
            }
        }
        else
        {
            SceneManager.LoadSceneAsync("Loader", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Home");
        }
        int GamePlayed = PlayerPrefs.GetInt("GamePlayed", 0);

        if (GamePlayed == 0)
        {
            //SceneManager.LoadSceneAsync ("Popup", LoadSceneMode.Additive);
        }
        else
        {
            if (PlayerPrefs.GetInt("GamePlayed") % 10 == 0) { }
            //SceneManager.LoadSceneAsync ("Popup", LoadSceneMode.Additive);
        }
        PlayerPrefs.SetInt("GamePlayed", ++GamePlayed);
    }
    public void CancelAdsPopup()
    {
        HidePopup("Popup", true);
        UnityThreading.ActionThread thread1;
        thread1 = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(600);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadScene("Popup");
            });
        });
    }
    public bool checkUserBirthday(User user)
    {
        string Years;
        DateTime birthdate;
        try
        {
            birthdate = DateTime.Parse(user.birthday);
        }
        catch (Exception e)
        {
            //Debug.Log ("this user didn't have a date of birth yet");
            Years = "0";
            return true;
        }
        if (!string.IsNullOrEmpty(user.birthday))
        {
            if (DateTime.UtcNow.Year - birthdate.Year >= 18)
                return false;
            else
                return true;
        }
        else
            return true;
    }
    public void AddxEuro(float MoneyToAdd)
    {
        string UserId = um.getCurrentUserId();
        string UserToken = um.getCurrentSessionToken();
        WalletScript.LastCredit = MoneyToAdd;
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            User user = um.getUser(UserId, UserToken);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                WalletScript.LastCredit = MoneyToAdd;
                Text PlayerMoney = GameObject.Find("solde_euro").GetComponent<Text>();
                if (checkUserBirthday(user))
                {
                    ShowPopup("popupAgeVerification");
                }
                else
                {
                    ShowPopup("popup_payment");
                }
                SceneManager.UnloadScene("Loader");
            });
        });
    }
    public void Add5Euro()
    {
        string UserId = um.getCurrentUserId();
        string UserToken = um.getCurrentSessionToken();
        WalletScript.LastCredit = 5;
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        UnityThreadHelper.CreateThread(() =>
        {
            User user = um.getUser(UserId, UserToken);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                WalletScript.LastCredit = 5;
                if (checkUserBirthday(user))
                {
                    ShowPopup("popupAgeVerification");
                }
                else
                {
                    ShowPopup("popup_payment");
                }
                SceneManager.UnloadScene("Loader");
            });
        });
    }
    public void UpdateAge()
    {
        string UserId = um.getCurrentUserId();
        string UserToken = um.getCurrentSessionToken();
        User user = um.getUser(UserId, UserToken);
        UnityThreading.ActionThread thread;
        //Text Years=GameObject.Find("Years").GetComponent<Text>();
        //    Text Days=GameObject.Find("Days").GetComponent<Text>();
        //    Text Months=GameObject.Find("Months").GetComponent<Text>();
        Text textDatePredefined = GameObject.Find("textDatePredefined").GetComponent<Text>();
        //textDatePredefined.text.CopyTo (0, Years, 0, 4);
        //Debug.Log (SelectedDateString2);
        //Debug.Log ("1032");
        string[] date = textDatePredefined.text.Split(new char[] { '-' }, 3);
        //Debug.Log ("1034");
        string Years = date[0];
        string Days = date[2];
        string Months = date[1];
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        thread = UnityThreadHelper.CreateThread(() =>
        {
            string[] attrib = { "birthdate" };
            string[] value = { Years + "-" + Months + "-" + Days };
            um.UpdateUserByField(UserId, UserToken, attrib, value);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                //Debug.Log(Years);
                if (DateTime.UtcNow.Year - int.Parse(Years) >= 18)
                {
                    //um.UpdateUserArgent(user["objectId"].Value,(float.Parse(user["data"]["money_credit"].Value)+MoneyToAdd*2).ToString("N2").Replace(",","."));
                    AddxEuro(WalletScript.LastCredit);
                    HidePopup("popupAgeVerification", false);
                    UnityThreading.ActionThread thread1;
                    UnityThreadHelper.CreateThread(() =>
                    {
                        Thread.Sleep(600);
                        UnityThreadHelper.Dispatcher.Dispatch(() =>
                        {
                            ShowPopup("popup_payment");
                            GameObject.Find("popupAgeVerification").GetComponent<GameObject>().transform.localScale = Vector3.zero;
                        });
                    });
                }
                else
                {
                    try
                    {
                        HidePopup("popupAgeVerification", false);
                        UnityThreading.ActionThread thread1;
                        thread1 = UnityThreadHelper.CreateThread(() =>
                        {
                            Thread.Sleep(600);
                            UnityThreadHelper.Dispatcher.Dispatch(() =>
                            {
                                ShowPopupError("popupAgeTooYoung");
                                GameObject.Find("popupAgeVerification").GetComponent<GameObject>().transform.localScale = Vector3.zero;
                            });
                        });
                    }
                    catch (NullReferenceException ex)
                    {
                    }
                }
                SceneManager.UnloadScene("Loader");
            });
        });
    }
    public void Add10Euro()
    {
        string UserToken = um.getCurrentSessionToken();
        string UserId = um.getCurrentUserId();
        //string email = um.getUserEmail (UserId);
        WalletScript.LastCredit = 10;
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            User user = um.getUser(UserId, UserToken);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                WalletScript.LastCredit = 10;
                if (checkUserBirthday(user))
                {
                    ShowPopup("popupAgeVerification");
                }
                else
                {
                    ShowPopup("popup_payment");
                }
                SceneManager.UnloadScene("Loader");
            });
        });
    }
    public void Add15Euro()
    {
        string UserToken = um.getCurrentSessionToken();
        string UserId = um.getCurrentUserId();
        //string email = um.getUserEmail (UserId);
        WalletScript.LastCredit = 15;
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            User user = um.getUser(UserId, UserToken);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                WalletScript.LastCredit = 15;
                //SceneManager.LoadScene ("completerInfo",LoadSceneMode.Additive);
                //um.UpdateUserArgent(user["objectId"].Value,(float.Parse(user["data"]["money_credit"].Value)+15).ToString("N2").Replace(",","."));
                //UserManager.CurrentMoney = (float.Parse(user["data"]["money_credit"].Value)+15).ToString("N2").Replace(",",".");
                if (checkUserBirthday(user))
                {
                    ShowPopup("popupAgeVerification");
                }
                else
                {
                    ShowPopup("popup_payment");
                }
                SceneManager.UnloadScene("Loader");
            });
        });
    }
    public void Add20Euro()
    {
        string UserId = um.getCurrentUserId();
        string UserToken = um.getCurrentSessionToken();
        //string email = um.getUserEmail (UserId);
        WalletScript.LastCredit = 20;
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            User user = um.getUser(UserId, UserToken);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                WalletScript.LastCredit = 20;
                //um.UpdateUserArgent(user["objectId"].Value,(float.Parse(user["data"]["money_credit"].Value)+20).ToString("N2").Replace(",","."));
                //UserManager.CurrentMoney = (float.Parse(user["data"]["money_credit"].Value)+20).ToString("N2").Replace(",",".");
                if (checkUserBirthday(user))
                {
                    ShowPopup("popupAgeVerification");
                }
                else
                {
                    ShowPopup("popup_payment");
                }
                SceneManager.UnloadScene("Loader");
            });
        });
    }
    public void closePopup_logout()
    {
        GameObject.Find("popup_logout").transform.localScale = Vector3.zero;
    }
    public void logoutInpopup()
    {
        InputField username = GameObject.Find("Username").GetComponent<InputField>();
        InputField password = GameObject.Find("Password").GetComponent<InputField>();
        string userId = um.getCurrentUserId();
        string userToken = um.getCurrentSessionToken();
        //um.UpdateUserMsgCoach (um.getCurrentUserId (), userToken, um.readStringFromFile ("Msg.dat"));
        string[] attrib = { "email", "password" };
        string[] value = { username.text, password.text };
        um.UpdateUserByField(userId, userToken, attrib, value);
        um.logingIn(username.text, password.text.ToString());
        um.logingOut();
        GameObject.Find("popup").transform.localScale = Vector3.zero;
        SceneManager.LoadScene("FirstScene");
    }
    public void OpenInfoPersonel()
    {
        SceneManager.LoadScene("PersonalInfo", LoadSceneMode.Additive);
        //GameObject.Find ("Canvas").GetComponent<Animator> ().SetBool ("showView", true);
    }
    public void UnloadInfoPersonel()
    {
        GameObject.Find("Container_Personalinfo").GetComponent<Animator>().SetBool("showView", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(500);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadSceneAsync("PersonalInfo");
            });
        });
    }
    public void history()
    {
        SceneManager.LoadScene("History", LoadSceneMode.Additive);
    }
    public void HideHistory()
    {
        GameObject.Find("Container_History").GetComponent<Animator>().SetBool("showView", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(500);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadScene("History");
            });
        });
    }
    public void cameraClick()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            #region [ Intent intent = new Intent(); ]
            //instantiate the class Intent
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            //instantiate the object Intent
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            #endregion [ Intent intent = new Intent(); ]
            #region [ intent.setAction(Intent.ACTION_GET_CONTENT); ]
            //call setAction setting ACTION_SEND as parameter
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_GET_CONTENT"));
            #endregion [ intent.setAction(Intent.ACTION_GET_CONTENT); ]
            #region [ intent.setData(Uri.parse("content://media/internal/images/media")); ]
            //instantiate the class Uri
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            //instantiate the object Uri with the parse of the url's file
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "content://media/internal/images/media");
            //call putExtra with the uri object of the file
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            #endregion [ intent.setData(Uri.parse("content://media/internal/images/media")); ]
            //set the type of file
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            #region [ startActivityForResult(intent , 1); ]
            //instantiate the class UnityPlayer
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //instantiate the object currentActivity
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            //call the activity with our Intent
            currentActivity.Call("startActivity", intentObject);
            #endregion [ startActivityForResult(intent , 1); ]
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
        }
    }
    public IEnumerator checkInternetConnection(Action<bool?> action)
    {
        AsyncOperation asc = SceneManager.LoadSceneAsync("Loader", LoadSceneMode.Additive);
        WWW www = new WWW("https://www.google.fr");
        float timer = 0;
        bool failed = false;
        while (!www.isDone)
        {
            try
            {
                GameObject.Find("checkConnection").transform.localScale = Vector3.one;
            }
            catch (NullReferenceException ex)
            {
            }
            if (timer > 5) { failed = true; break; }
            timer += Time.deltaTime;
            yield return null;
        }
        if (failed)
        {
            try
            {
                GameObject.Find("checkConnection").transform.localScale = Vector3.one;
                SceneManager.UnloadScene("ConnectionFailed");
            }
            catch (NullReferenceException ex)
            {
            }
            catch (ArgumentException ex)
            {
            }
            www.Dispose();
            action(false);
        }
        else
        {
            if (www.error == null)
            {
                action(true);
            }
            else
            {
                if (!asc.isDone)
                {
                    yield return new WaitForSeconds(1f);
                }
                SceneManager.UnloadSceneAsync("Loader");
                action(false);
            }
        }
    }
    public IEnumerator checkConnectionHome(Action<bool> action)
    {
        WWW www = new WWW("https://www.google.fr");
        float timer = 0;
        bool failed = false;
        while (!www.isDone)
        {
            if (timer > 5) { failed = true; break; }
            timer += Time.deltaTime;
            Debug.Log("timer :" + timer);
            yield return null;
        }
        if (failed)
        {
            www.Dispose();
            action(false);
        }
        else
        {
            if (www.error == null)
            {
                action(true);
            }
            else
            {
                action(false);
            }
        }
    }
    public void hideConnectionFailedPopup()
    {
        ConnectivityController.CURRENT_ACTION = "";
        try { SceneManager.UnloadSceneAsync("Loader"); } catch (ArgumentException ex) { }
        TryAgainLastAction();
    }
    public void TryAgainLastAction()
    {
        HidePopupError("popupConnectionFailed", true);
        UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(400);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                //SceneManager.UnloadSceneAsync("ConnectionFailed");
                Debug.Log(ConnectivityController.CURRENT_ACTION);
                switch (ConnectivityController.CURRENT_ACTION)
                {
                    case ConnectivityController.CHALLENGE_ACTION:
                        BeginPlaying(ChallengeManager.CurrentChallengeGain, ChallengeManager.CurrentChallengeGainType);
                        break;
                    case ConnectivityController.HOME_ACTION:
                        //Optimize This Fucking code
                        SceneManager.UnloadSceneAsync("Home");
                        SceneManager.LoadScene("Home");
                        break;
                    case ConnectivityController.ENTER_ESPORT_TOURNAMENT_ACTION:
                        Seemba seemba = new Seemba();
                        seemba.Enter();
                        break;
                    case ConnectivityController.CREDIT_ACTION:
                        break;
                    case ConnectivityController.LOGIN_ACTION:
                        UserService service = new UserService();
                        service.Login();
                        break;
                    case ConnectivityController.PERSONNEL_INFO_ACTION:
                        GameObject.Find("CanvasM").GetComponent<PersonelInfoController>().enabled = false;
                        GameObject.Find("CanvasM").GetComponent<PersonelInfoController>().enabled = true;
                        break;
                    case ConnectivityController.HISTORY_ACTION:
                        GameObject.Find("CanvasM").GetComponent<HistoryListController>().enabled = false;
                        GameObject.Find("CanvasM").GetComponent<HistoryListController>().enabled = true;
                        break;
                    case ConnectivityController.PERSONNEL_INFO_WITHDRAW_ACTION:
                        GameObject.Find("CanvasWithdrawalInfo").GetComponent<InfoPersonnelWithdraw>().enabled = false;
                        GameObject.Find("CanvasWithdrawalInfo").GetComponent<InfoPersonnelWithdraw>().enabled = true;
                        break;
                    default: SceneManager.UnloadSceneAsync("ConnectionFailed"); break;
                }
            });
        });
    }
    public void yesSoldeInsuffisant(string last_view)
    {
        HidePopupError("popupSoldeInsuffisant", true);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(500);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {   
                ViewsEvents v = new ViewsEvents();
                BottomMenuController bottomMenu = BottomMenuController.getInstance();
                bottomMenu.unselectWinMoney();
                bottomMenu.selectSettings();
                bottomMenu.unselectHome();
                bottomMenu.unselectHaveFun();
                v.WalletClick(last_view);
            });
        });
        
    }

    public void WalletBack()
    {
        ViewsEvents v = new ViewsEvents();
        BottomMenuController bottomMenu = BottomMenuController.getInstance();
        BottomMenuController.Show();
        switch (last_view)
        {
            case "Home":
                bottomMenu.unselectSettings();
                bottomMenu.unselectHaveFun();
                bottomMenu.unselectWinMoney();
                bottomMenu.selectHome();
                v.HomeClick();
                break;
            case "HaveFun":
                bottomMenu.unselectSettings();
                bottomMenu.unselectHome();
                bottomMenu.unselectWinMoney();
                bottomMenu.selectHaveFun();
                v.HaveFunClick();
                break;
            case "WinMoney":
                bottomMenu.unselectHome();
                bottomMenu.unselectHaveFun();
                bottomMenu.unselectSettings();
                bottomMenu.selectWinMoney();

                v.WinMoneyClick();
                break;
            case "Setting":
                bottomMenu.unselectHome();
                bottomMenu.unselectHaveFun();
                bottomMenu.unselectWinMoney();
                bottomMenu.selectSettings();
                v.SettingsClick();
                break;
        }
    }
    public void noSoldeInsuffisant()
    {
        HidePopupError("popupSoldeInsuffisant", true);
    }
    public void hideVpnPopup()
    {
        HidePopupError("popupVPN", true);
    }
    public void hideDevModePopup()
    {
        HidePopupError("popupDevMode", true);
    }
    public void hidepopupProhibitedLocation()
    {
        GameObject.Find("popupProhibitedLocation").transform.localScale = Vector3.zero;
    }
    public void hidepopupProhibitedLocationWithdraWINfo()
    {
        var animator = GameObject.Find("popupProhibitedLocation").GetComponent<Animator>();
        animator.SetBool("Show Error", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(400);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                GameObject.Find("popupProhibitedLocation").transform.localScale = Vector3.zero;
                try
                {
                    SceneManager.UnloadSceneAsync("WithdrawalInfo");
                }
                catch (ArgumentException ex) { }
            });
        });
    }
    public void hidepopupProhibitedLocationWithdraw()
    {
        var animator = GameObject.Find("popupProhibitedLocationWithdraw").GetComponent<Animator>();
        animator.SetBool("Show Error", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(400);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                GameObject.Find("CalqueWithdraw").transform.localScale = Vector3.zero;
            });
        });
    }
    public void hidepopupProhibitedLocationWallet()
    {
        var animator = GameObject.Find("popupProhibitedLocationWallet").GetComponent<Animator>();
        animator.SetBool("Show Error", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(400);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                GameObject.Find("CalqueWallet").transform.localScale = Vector3.zero;
            });
        });
    }
    public void hidepopupProhibitedLocationWINMoney()
    {
        var animator = GameObject.Find("popupProhibitedLocation").GetComponent<Animator>();
        animator.SetBool("Show Error", false);
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(400);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                GameObject.Find("Calque").transform.localScale = Vector3.zero;
            });
        });
    }
    public bool checkDeveleperMode(string gainType)
    {

        return false;
    #if UNITY_ANDROID
        if (gainType == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
        {
            using (var actClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                var context = actClass.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaClass systemGlobal = new AndroidJavaClass("android.provider.Settings$Global");
                var DevMode = 1;
                try
                {
                    DevMode = systemGlobal.CallStatic<int>("getInt", context.Call<AndroidJavaObject>("getContentResolver"), "development_settings_enabled");
                }
                catch (NullReferenceException ex) { }
                if (DevMode == 1)
                {
                    HidePopup("popup", false);
                    ShowPopupError("popupDevMode");
                }
                return (DevMode == 1) ? true : false;
            }
        }
        else
        {
            return false;
        }
#else
		return false;
#endif
    }
    public bool checkVPNBeforePlaying(string gainType)
    {
        UnityThreading.ActionThread thread;
        bool isVpnConnected = false;
        if (gainType == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
        {
            VPNManager vpn = new VPNManager();
            isVpnConnected = vpn.isVpnConnected();
            //Debug.Log ("isVpnConnected: " + isVpnConnected);
            if (isVpnConnected == true)
            {
                HidePopup("popup", false);
                UnityThreading.ActionThread thread1;
                thread1 = UnityThreadHelper.CreateThread(() =>
                {
                    Thread.Sleep(500);
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        ShowPopupError("popupVPN");
                    });
                });
                return isVpnConnected;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return isVpnConnected;
        }
    }
    void configBeforePlaying(string FeeGame, string GameMontant, string gain_type, string attrib, int value)
    {
        
    }
    public bool checkCountryBeforePlaying(string code, string gainType)
    {
        if (gainType == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
        {
            return CountryController.checkCountry(code);
        }
        else
        {
            return true;
        }
    }
    public void StartFirstChallenge(string token)
    {
        ChallengeManager.CurrentChallengeGain = "2";
        ChallengeManager.CurrentChallengeGainType = ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES;

        SearchingForPlayerPresenter.nbPlayer = "duel";
        ChallengeType = ChallengeManager.CHALLENGE_TYPE_1V1;

        UnityThreadHelper.CreateThread(() =>
        {   
            ChallengeManager.CurrentChallenge = cm.AddChallenge(token, "headTohead", ChallengeManager.CurrentChallengeGain, ChallengeManager.CurrentChallengeGainType, 0);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                configBeforePlaying(ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT.ToString(), "2", ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, "novice_bubble", 0);
                SceneManager.UnloadScene("Loader");
                SceneManager.LoadSceneAsync("SearchingPlayer", LoadSceneMode.Additive);
            });
        });
    }
    public void BeginPlaying(string gain, string gainType)
    {
        ConnectivityController.CURRENT_ACTION = ConnectivityController.CHALLENGE_ACTION;
        ChallengeManager.CurrentChallengeGain = gain;
        ChallengeManager.CurrentChallengeGainType = gainType;
        //CheckCountries
        string code = null;
        try
        {
            code = UserManager.CurrentCountryCode;
        }
        catch (NullReferenceException ex)
        {
        }
        if (checkCountryBeforePlaying(code, gainType) == true)
        {
            //Check Mode developer 
            if (checkDeveleperMode(gainType) == false)
            {
                //Check Internet Connexion
                StartCoroutine(checkInternetConnection((isConnected) =>
                {
                    if (isConnected == true)
                    {
                        // Check if user is using an VPN
                        if (checkVPNBeforePlaying(gainType) == false)
                        {
                            string userId = um.getCurrentUserId();
                            string token = um.getCurrentSessionToken();
                            SearchingForPlayerPresenter.nbPlayer = "duel";
                            ChallengeType = ChallengeManager.CHALLENGE_TYPE_1V1;
                            WithdrawManager wm = new WithdrawManager();
                            ChallengeManager.CurrentChallengeGain = gain;
                            ChallengeManager.CurrentChallengeGainType = gainType;
                            float advLevel = 0;
                            float userLevel = 0;
                            UnityThreadHelper.CreateThread(() =>
                            {
                                User user = um.getUser(userId, token);
                                ChallengeManager.CurrentChallenge = cm.AddChallenge(token, "headTohead", ChallengeManager.CurrentChallengeGain, ChallengeManager.CurrentChallengeGainType, 0);
                                UnityThreadHelper.Dispatcher.Dispatch(() =>
                                {
                                    HidePopup("popup", true);
                                    try
                                    {
                                        SceneManager.UnloadSceneAsync("Loader");
                                    }
                                    catch (ArgumentException ex) { }
                                    if (user != null)
                                    {
                                        if (ChallengeManager.CurrentChallenge != null)
                                        {
                                            Challenge challenge = ChallengeManager.CurrentChallenge;
                                            if (gainType == ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES)
                                            {
                                                if (gain == ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT.ToString())
                                                {
                                                    configBeforePlaying(ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT.ToString(), ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT.ToString(), ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, "novice_bubble", user.novice_bubble);
                                                }
                                                if (gain == ChallengeManager.WIN_1V1_BUBBLES_CHAMPION.ToString())
                                                {
                                                    configBeforePlaying(ChallengeManager.FEE_1V1_BUBBLES_CHAMPION.ToString(), ChallengeManager.WIN_1V1_BUBBLES_CHAMPION.ToString(), ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, "amateur_bubble", user.amateur_bubble);
                                                }
                                                if (gain == ChallengeManager.WIN_1V1_BUBBLES_LEGEND.ToString())
                                                {
                                                    configBeforePlaying(ChallengeManager.FEE_1V1_BUBBLES_LEGEND.ToString(), ChallengeManager.WIN_1V1_BUBBLES_LEGEND.ToString(), ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, "confirmed_bubble", user.confirmed_bubble);
                                                }
                                            }
                                            else if (gainType == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
                                            {
                                                if (gain == ChallengeManager.WIN_1V1_CASH_CONFIDENT.ToString())
                                                {
                                                    configBeforePlaying(ChallengeManager.FEE_1V1_CASH_CONFIDENT.ToString(), ChallengeManager.WIN_1V1_CASH_CONFIDENT.ToString(), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, "novice_money", user.novice_money);
                                                }
                                                if (gain == ChallengeManager.WIN_1V1_CASH_CHAMPION.ToString())
                                                {
                                                    configBeforePlaying(ChallengeManager.FEE_1V1_CASH_CHAMPION.ToString(), ChallengeManager.WIN_1V1_CASH_CHAMPION.ToString(), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, "amateur_money", user.amateur_money);
                                                }
                                                if (gain == ChallengeManager.WIN_1V1_CASH_LEGEND.ToString())
                                                {
                                                    configBeforePlaying(ChallengeManager.FEE_1V1_CASH_LEGEND.ToString(), ChallengeManager.WIN_1V1_CASH_LEGEND.ToString(), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, "confirmed_money", user.confirmed_money);
                                                }
                                            }
                                            SceneManager.LoadSceneAsync("SearchingPlayer", LoadSceneMode.Additive);
                                            UnityThreadHelper.CreateThread(() =>
                                            {
                                                //TODO
                                                if (ChallengeManager.CurrentChallengeStatus == "on going")
                                                {
                                                    //Debug.Log ("--> Adv Found");
                                                    Thread.Sleep(1000);
                                                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                                                    {
                                                        OpponentFound.adversaireName = challenge.matched_user_1.username;
                                                        OpponentFound.Avatar = challenge.matched_user_1.avatar;
                                                        OpponentFound.AdvCountryCode = challenge.matched_user_1.country_code;
                                                        advFound = true;
                                                    });
                                                }
                                            });
                                        }
                                        else
                                        {
                                            UnityThreadHelper.CreateThread(() =>
                                            {
                                                Thread.Sleep(300);
                                                UnityThreadHelper.Dispatcher.Dispatch(() =>
                                                {
                                                    SceneManager.LoadSceneAsync("ConnectionFailed", LoadSceneMode.Additive);
                                                });
                                            });
                                        }
                                    }
                                    else
                                    {
                                        UnityThreadHelper.CreateThread(() =>
                                        {
                                            Thread.Sleep(300);
                                            UnityThreadHelper.Dispatcher.Dispatch(() =>
                                            {
                                                SceneManager.LoadSceneAsync("ConnectionFailed", LoadSceneMode.Additive);
                                            });
                                        });
                                    }
                                });
                            });
                        }
                    }
                    else
                    {
                        HidePopup("popup", true);
                        SceneManager.UnloadSceneAsync("Loader");
                        UnityThreadHelper.CreateThread(() =>
                        {
                            Thread.Sleep(300);
                            UnityThreadHelper.Dispatcher.Dispatch(() =>
                            {
                                try
                                {
                                    SceneManager.UnloadSceneAsync("ConnectionFailed");
                                }
                                catch (ArgumentException ex) { }
                                SceneManager.LoadSceneAsync("ConnectionFailed", LoadSceneMode.Additive);
                            });
                        });
                    }
                }));
            }
        }
        else
        {
            UnityThreading.ActionThread thread, thr;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                Thread.Sleep(300);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    ShowPopupError("popupProhibitedLocation");
                });
            });
        }
    }
    public void ShowNoviceArgentpopUp()
    {
        ChallengeName = "NoviceArgent";
        Text entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
        entry_fee.text = "Entry fee: " + ChallengeManager.FEE_1V1_CASH_CONFIDENT.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        Text Gain = GameObject.Find("gain").GetComponent<Text>();
        Gain.text = ChallengeManager.WIN_1V1_CASH_CONFIDENT.ToString("N2") + " " + CurrencyManager.CURRENT_CURRENCY;
        ShowPopup("popup");
    }
    public void ShowAmateurArgentpopUp()
    {
        ChallengeName = "AmateurArgent";
        Text entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
        entry_fee.text = "Entry fee: " + ChallengeManager.FEE_1V1_CASH_CHAMPION.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        Text Gain = GameObject.Find("gain").GetComponent<Text>();
        Gain.text = ChallengeManager.WIN_1V1_CASH_CHAMPION.ToString("N2") + " " + CurrencyManager.CURRENT_CURRENCY;
        ShowPopup("popup");
    }
    public void ShowConfirmedArgentpopUp()
    {
        ChallengeName = "ConfirmedArgent";
        Text entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
        entry_fee.text = "Entry fee: " + ChallengeManager.FEE_1V1_CASH_LEGEND.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        Text Gain = GameObject.Find("gain").GetComponent<Text>();
        Gain.text = ChallengeManager.WIN_1V1_CASH_LEGEND.ToString("N2") + " " + CurrencyManager.CURRENT_CURRENCY;
        ShowPopup("popup");
    }
    public void ShowNoviceGouttepopUp()
    {
        ChallengeName = "NoviceGoutte";
        Text entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
        entry_fee.text = "Entry fee: " + ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT;
        Text Gain = GameObject.Find("gain").GetComponent<Text>();
        Gain.text = ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT.ToString();
        ShowPopup("popup");
    }
    public void ShowAmateurGouttepopUp()
    {
        ChallengeName = "AmateurGoutte";
        Text entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
        entry_fee.text = "Entry fee: " + ChallengeManager.FEE_1V1_BUBBLES_CHAMPION;
        Text Gain = GameObject.Find("gain").GetComponent<Text>();
        Gain.text = ChallengeManager.WIN_1V1_BUBBLES_CHAMPION.ToString();
        ShowPopup("popup");
    }
    public void ShowConfirmedGouttepopUp()
    {
        ChallengeName = "ConfirmedGoutte";
        Text entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
        entry_fee.text = "Entry fee: " + ChallengeManager.FEE_1V1_BUBBLES_LEGEND;
        Text Gain = GameObject.Find("gain").GetComponent<Text>();
        Gain.text = ChallengeManager.WIN_1V1_BUBBLES_LEGEND.ToString();
        ShowPopup("popup");
    }
  
    public void ShowTournamentBubbleConfidentpopUp()
    {
        ChallengeName = "TournamentBubbleConfident";
        Text entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
        entry_fee.text = "Entry fee: " + TournamentManager.FEE_BRACKET_BUBBLE_CONFIDENT;
        Text Gain = GameObject.Find("gain").GetComponent<Text>();
        Gain.text = TournamentManager.WIN_BRACKET_BUBBLE_CONFIDENT.ToString();
        ShowPopup("popup");
    }
    public void ShowTournamentBubbleChampionpopUp()
    {
        ChallengeName = "TournamentBubbleChampion";
        Text entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
        entry_fee.text = "Entry fee: " + TournamentManager.FEE_BRACKET_BUBBLE_CHAMPION;
        Text Gain = GameObject.Find("gain").GetComponent<Text>();
        Gain.text = TournamentManager.WIN_BRACKET_BUBBLE_CHAMPION.ToString();
        ShowPopup("popup");
    }
    public void ShowTournamentBubbleLegendpopUp()
    {
        ChallengeName = "TournamentBubbleLegend";
        Text entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
        entry_fee.text = "Entry fee: " + TournamentManager.FEE_BRACKET_BUBBLE_LEGEND;
        Text Gain = GameObject.Find("gain").GetComponent<Text>();
        Gain.text = TournamentManager.WIN_BRACKET_BUBBLE_LEGEND.ToString();
        ShowPopup("popup");
    }
    public void ShowTournamentCashConfidentpopUp()
    {
        ChallengeName = "TournamentCashConfident";
        Text entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
        entry_fee.text = "Entry fee: " + TournamentManager.FEE_BRACKET_CASH_CONFIDENT.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        Text Gain = GameObject.Find("gain").GetComponent<Text>();
        Gain.text = TournamentManager.WIN_BRACKET_CASH_CONFIDENT.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        ShowPopup("popup");
    }
    public void ShowTournamentCashChampionpopUp()
    {
        ChallengeName = "TournamentCashChampion";
        Text entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
        entry_fee.text = "Entry fee: " + TournamentManager.FEE_BRACKET_CASH_CHAMPION.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        Text Gain = GameObject.Find("gain").GetComponent<Text>();
        Gain.text = TournamentManager.WIN_BRACKET_CASH_CHAMPION.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        ShowPopup("popup");
    }
    public void ShowTournamentCashLegendpopUp()
    {
        ChallengeName = "TournamentCashLegend";
        Text entry_fee = GameObject.Find("Entry fee").GetComponent<Text>();
        entry_fee.text = "Entry fee: " + TournamentManager.FEE_BRACKET_CASH_LEGEND.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        Text Gain = GameObject.Find("gain").GetComponent<Text>();
        Gain.text = TournamentManager.WIN_BRACKET_CASH_LEGEND.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
        ShowPopup("popup");
    }
    public void CancelButtonpopupChallenge()
    {
        HidePopup("popup", true);
    }
    public void ButtonpopupInsufficientBubbles()
    {
        HidePopupError("insufficientBubbles", true);
    }
    
   
    public void noviceGoutteButton()
    {
        
    }
    public void AmateurGoutteButton()
    {
        
    }
    public void ConfirmedGoutteButton()
    {
        
    }
    public bool IsValidEmailAddress(string s)
    {
        var regex = new Regex(@"[a-z0-9!#$%&amp;'*+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&amp;'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
        return regex.IsMatch(s);
    }
    public void showDatePicker(UnityEngine.Object button)
    {
        string UserId = um.getCurrentUserId();
        string UserToken = um.getCurrentSessionToken();
        try
        {
            _textDatePredefined = GameObject.Find("textDatePredefined").GetComponent<Text>();
        }
        catch (NullReferenceException ex) { }
        NativePicker.Instance.ShowDatePicker(GetScreenRect(button as GameObject), NativePicker.DateTimeForDate(2012, 12, 23), (long val) =>
        {
            SelectedDateString2 = NativePicker.ConvertToDateTime(val).ToString("yyyy-MM-dd");
            ////Debug.Log("SelectedDateString2: "+SelectedDateString2);
        }, () =>
        {
            //SelectedDateString2 = "canceled";
            SelectedDateString2 = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                //_textDatePredefined.text=SelectedDateString2;
            }
            catch (NullReferenceException ex) { }
            try
            {
                GameObject.Find("WithdrawalInfo").GetComponent<GameObject>();
            }
            catch (NullReferenceException ex)
            {
                UpdateAge();
            }
        });
    }
    private Rect GetScreenRect(GameObject gameObject)
    {
        RectTransform transform = gameObject.GetComponent<RectTransform>();
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
        rect.x -= (transform.pivot.x * size.x);
        rect.y -= ((1.0f - transform.pivot.y) * size.y);
        return rect;
    }
    public void CancelDatePicker()
    {
        GameObject.Find("DatePickerPanel").transform.localScale = new Vector3(0, 0, 0);
    }
    public void OKDatePicker()
    {
        InputField Year = GameObject.Find("year").GetComponent<InputField>();
        Text Day = GameObject.Find("day").GetComponent<Text>();
        Text Month = GameObject.Find("month").GetComponent<Text>();
        Text AgeDays = GameObject.Find("Age/Days").GetComponent<Text>();
        Text AgeMonths = GameObject.Find("Age/Months").GetComponent<Text>();
        Text AgeYears = GameObject.Find("Age/Years").GetComponent<Text>();
        if (int.Parse(Year.text) >= 1920 && int.Parse(Year.text) <= DateTime.UtcNow.Year)
        {
            GameObject.Find("DatePickerPanel").transform.localScale = new Vector3(0, 0, 0);
            AgeDays.text = Day.text;
            AgeMonths.text = Month.text;
            AgeYears.text = Year.text;
        }
        else
        {
        }
    }
    public void AllStarClick()
    {
        SceneManager.LoadScene("TournamentDetails");
    }
    
    public void backBankingInformations()
    {
        SceneManager.UnloadScene("BankingInformation");
    }
    public void continuePayment()
    {
        HidePopup("popup_payment", true);
        UnityThreading.ActionThread myThread;
        myThread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(700);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                GameObject.Find("Calque").transform.localScale = Vector3.zero;
                SceneManager.LoadScene("BankingInformation", LoadSceneMode.Additive);
            });
        });
    }
    public void ExistingUser()
    {
        SceneManager.LoadSceneAsync("Login");
    }
    public void backToConnexion()
    {
        SceneManager.LoadScene("Signup");
    }
    public void returnToHome()
    {
        UnityThreading.ActionThread thread;
        SceneManager.LoadScene("Home");
        thread = UnityThreadHelper.CreateThread(() =>
        {
            Thread.Sleep(2000);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                GameObject.Find("popup").transform.localScale = new Vector3(1, 1, 1);
                GameObject.Find("TextMain").GetComponent<Text>().text = GameObject.Find("coach").GetComponent<Text>().text;
            });
        });
    }
    public void otherAmountClick()
    {
        GameObject.Find("AutreButton").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("Code Promo Button").transform.localScale = Vector3.one;
        GameObject.Find("enterAmount").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("validateAmount").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("enterPromoCode").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("validatePromoCode").transform.localScale = new Vector3(0, 0, 0);
    }
    public void promoCodeClick()
    {
        GameObject.Find("AutreButton").transform.localScale = Vector3.one;
        GameObject.Find("Code Promo Button").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("enterPromoCode").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("validatePromoCode").transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("enterAmount").transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("validateAmount").transform.localScale = new Vector3(0, 0, 0);
    }
    public void validateAmount()
    {
        string UserToken = um.getCurrentSessionToken();
        MoneyToAdd = int.Parse(GameObject.Find("enterAmount").GetComponent<InputField>().text);
        AddxEuro(MoneyToAdd);
    }
    public void BackIntro2()
    {
        SceneManager.LoadScene("Intro");
    }
}
