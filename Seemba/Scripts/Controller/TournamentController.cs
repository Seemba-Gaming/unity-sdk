using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;
using static PopupsViewPresenter;

[CLSCompliant(false)]
public class TournamentController : MonoBehaviour
{
    #region Static
    public static TournamentController Get { get { return sInstance; } }
    private static TournamentController sInstance;

    private static string CURRENT_TOURNAMENT_ID;
    public static int CURRENT_TOURNAMENT_NB_PLAYER;
    public static float CURRENT_TOURNAMENT_GAIN;
    public static string CURRENT_TOURNAMENT_GAIN_TYPE;
    #endregion

    public TournamentPresenter tp;
    public JSONNode tournamentJson;
    public Button _Play, _Great;
    public GameObject Bubbles, Others;

    string userId, token;

    private void Start()
    {
        sInstance = this;
    }

    public void Play(object[] _duel_params)
    {

        userId = UserManager.Get.getCurrentUserId();
        token = UserManager.Get.getCurrentSessionToken();
        float entry_fee = float.Parse(_duel_params[0].ToString());
        float gain = float.Parse(_duel_params[1].ToString());
        string gain_type = _duel_params[2].ToString();

        if (isCreditSuffisant(entry_fee, gain_type))
        {

            ChallengeController.ChallengeType = ChallengeManager.CHALLENGE_TYPE_BRACKET;
            EventsController.ChallengeType = ChallengeManager.CHALLENGE_TYPE_1V1;

            if (gain_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_CASH))
            {
                StartCashTournamentAsync(entry_fee, gain, gain_type);
            }
            else
            {
                StartBubblesTournament(entry_fee, gain, gain_type);
            }
        }
        else
        {
            if (gain_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_CASH))
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_INSUFFICIENT_BALANCE, PopupsText.Get.insufficient_balance());
            }
            else
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_INSUFFICIENT_BUBBLES, PopupsText.Get.insufficient_bubbles());
            }
        }
    }
    public void StartBubblesTournament(float entry_fee, float gain, string gain_type)
    {
        JoinTournament(entry_fee, gain, gain_type);
    }
    public async System.Threading.Tasks.Task StartCashTournamentAsync(float entry_fee, float gain, string gain_type)
    {
        if (isProhibitedLocation(UserManager.Get.CurrentUser.country_code))
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.PROHIBITED_LOCATION, PopupsText.Get.prohibited_location());
            return;
        }
        var mIsVpnEnabled = await isVPNEnabledAsync();
        if (mIsVpnEnabled)
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.VPN, PopupsText.Get.vpn());
            return;
        }

        JoinTournament(entry_fee, gain, gain_type);
    }
    private async void JoinTournament(float entry_fee, float gain, string gain_type)
    {
        LoaderManager.Get.LoaderController.ShowLoader(null);
        string tournamentId = await TournamentManager.Get.JoinOrCreateTournament(TournamentManager.TOURNAMENT_8, gain, gain_type, userId, token);
        LoaderManager.Get.LoaderController.HideLoader();
        if (tournamentId != null)
        {
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Brackets.gameObject);
        }
    }
    private bool isDeveloperModeEnabled()
    {

#if UNITY_EDITOR
        return false;
#elif UNITY_ANDROID

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
                //HidePopup("popup", false);
                //ShowPopupError("popupDevMode");
            }
            return (DevMode == 1) ? true : false;
        }

#else
		return false;
#endif
    }
    private bool isProhibitedLocation(string code)
    {
        return CountryController.checkCountry(code);
    }
    private async System.Threading.Tasks.Task<bool> isVPNEnabledAsync()
    {
        VPNManager vpn = new VPNManager();
        return await vpn.isVpnConnectedAsync();
    }

    private bool isCreditSuffisant(float entry_fee, string win_type)
    {
        if (win_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_CASH) && UserManager.Get.CurrentUser.money_credit >= entry_fee)
        {
            return true;
        }
        if (win_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES) && UserManager.Get.CurrentUser.bubble_credit >= entry_fee)
        {
            return true;
        }
        return false;
    }
    public static void setCurrentTournamentID(string id)
    {
        CURRENT_TOURNAMENT_ID = id;
    }
    public static string getCurrentTournamentID()
    {
        return CURRENT_TOURNAMENT_ID;
    }
}
