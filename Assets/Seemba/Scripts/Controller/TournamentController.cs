using SimpleJSON;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PopupsViewPresenter;

public class TournamentController : MonoBehaviour
{
    static TournamentController _Instance;

    private static string CURRENT_TOURNAMENT_ID;
    public static int CURRENT_TOURNAMENT_NB_PLAYER;
    public static float CURRENT_TOURNAMENT_GAIN;
    public static string CURRENT_TOURNAMENT_GAIN_TYPE;
    TournamentManager tm;
    public TournamentPresenter tp;
    public JSONNode tournamentJson;
    public Button _Play, _Great;
    public GameObject Bubbles, Others;

    private UserManager userManager = new UserManager();
    string userId, token;
    public static TournamentController getInstance()
    {
        if (!_Instance) _Instance = new TournamentController();
        return _Instance;
    }

    public void Play(object[] _duel_params)
    {

        userId = userManager.getCurrentUserId();
        token = userManager.getCurrentSessionToken();

        float entry_fee = float.Parse(_duel_params[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
        float gain = float.Parse(_duel_params[1].ToString(), CultureInfo.InvariantCulture.NumberFormat);
        string gain_type = _duel_params[2].ToString();

        if (isCreditSuffisant(entry_fee, gain_type))
        {

            ChallengeController.ChallengeType = ChallengeManager.CHALLENGE_TYPE_BRACKET;
            //Needs Change
            EventsController.ChallengeType = ChallengeManager.CHALLENGE_TYPE_1V1;


            if (gain_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_CASH))
            {
                StartCashTournament(entry_fee, gain, gain_type);
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
                ShowPopup(PopupsController.PopupType.INSUFFICIENT_BALANCE, PopupsText.getInstance().insufficient_balance());
            }
            else
            {
                ShowPopup(PopupsController.PopupType.INSUFFICIENT_BUBBLES, PopupsText.getInstance().insufficient_bubbles());
            }
        }
    }
    public void StartBubblesTournament(float entry_fee, float gain, string gain_type)
    {
        JoinTournament(entry_fee, gain, gain_type);
    }
    public void StartCashTournament(float entry_fee, float gain, string gain_type)
    {
        if (isDeveloperModeEnabled())
        {
            ShowPopup(PopupsController.PopupType.DEV_MODE, PopupsText.getInstance().dev_mode());
            return;
        }

        if (isProhibitedLocation(UserManager.CurrentCountryCode))
        {
            ShowPopup(PopupsController.PopupType.PROHIBITED_LOCATION, PopupsText.getInstance().prohibited_location());
            return;
        }

        if (isVPNEnabled())
        {
            ShowPopup(PopupsController.PopupType.VPN, PopupsText.getInstance().vpn());
            return;
        }

        JoinTournament(entry_fee, gain, gain_type);
    }
    private void JoinTournament(float entry_fee, float gain, string gain_type)
    {
        //Show The Loader
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);

        UnityThreadHelper.CreateThread(() =>
        {
            TournamentManager tm = new TournamentManager();
            string tournamentId = tm.JoinOrCreateTournament(TournamentManager.TOURNAMENT_8, gain, gain_type, userId, token);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadSceneAsync("Loader");

                if (tournamentId != null)
                {
                    //UserManager.CurrentWater = (int.Parse(UserManager.CurrentWater) - entry_fee).ToString();
                    setCurrentTournamentID(tournamentId);
                    SceneManager.LoadScene("Bracket", LoadSceneMode.Additive);
                }
            });
        });
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
    private bool isVPNEnabled()
    {
        VPNManager vpn = new VPNManager();
        return vpn.isVpnConnected();
    }
    void ShowPopup(PopupsController.PopupType popup, object[] popup_details)
    {
        PopupsController.getInstance().ShowPopup(popup, popup_details);
    }
    private bool isCreditSuffisant(float entry_fee, string win_type)
    {

        if (win_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_CASH) && (float.Parse(UserManager.CurrentMoney, CultureInfo.InvariantCulture.NumberFormat) >= entry_fee))
        {
            return true;
        }
        if (win_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES) && (float.Parse(UserManager.CurrentWater) >= entry_fee))
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
