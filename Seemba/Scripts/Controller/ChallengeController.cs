using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ChallengeTypePresenter;
using static PopupsViewPresenter;

public class ChallengeController : MonoBehaviour
{
    static ChallengeController _Instance;
    public static string ChallengeType;

    UserManager userManager = new UserManager();
    ChallengeManager challengeManager = new ChallengeManager();
    string userId, token;


    public static ChallengeController getInstance()
    {
        if (!_Instance) _Instance = new ChallengeController();
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
            SearchingForPlayerPresenter.nbPlayer = "duel";
            ChallengeType = ChallengeManager.CHALLENGE_TYPE_1V1;

            //Needs Change
            EventsController.ChallengeType = ChallengeManager.CHALLENGE_TYPE_1V1;

            ChallengeManager.CurrentChallengeGain = gain.ToString();
            ChallengeManager.CurrentChallengeGainType = gain_type.ToString();

            
            

            if (gain_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_CASH))
            {
                StartCashChallenge(entry_fee, gain, gain_type);
            }
            else
            {
                StartBubblesChallenge(entry_fee, gain, gain_type);
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
    void ShowPopup(PopupsController.PopupType popup, object[] popup_details)
    {
        PopupsController.getInstance().ShowPopup(popup, popup_details);
    }
    public void StartBubblesChallenge(float entry_fee, float gain, string gain_type)
    {
        JoinChallenge(entry_fee, gain, gain_type);
    }
    public void StartCashChallenge(float entry_fee, float gain, string gain_type)
    {
        if (isDeveloperModeEnabled())
        {
            ShowPopup(PopupsController.PopupType.DEV_MODE, PopupsText.getInstance().dev_mode());
            return;
        }

        /*if (isProhibitedLocation(UserManager.CurrentCountryCode))
        {
            ShowPopup(PopupsController.PopupType.PROHIBITED_LOCATION, PopupsText.getInstance().prohibited_location());
            return;
        }

        if (isVPNEnabled())
        {
            ShowPopup(PopupsController.PopupType.VPN, PopupsText.getInstance().vpn());
            return;
        }*/

        JoinChallenge(entry_fee, gain, gain_type);
    }

    private void JoinChallenge(float entry_fee, float gain, string gain_type)
    {
        //Show The Loader
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);

        UnityThreadHelper.CreateThread(() =>
        {

            ChallengeManager.CurrentChallenge = challengeManager.AddChallenge(token, "headTohead", gain.ToString(), gain_type.ToString(),0);
            
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadSceneAsync("Loader");
                
                if (ChallengeManager.CurrentChallenge != null)
                {
                    //remove_fee_and_search( entry_fee,  gain,  gain_type);
                    search();
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

    void remove_fee_and_search(float entry_fee, float gain, string gain_type)
        {
           
        if (gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
        {   
           
            TopWalletPresenter.getInstance().remove_fees(entry_fee);
        }
       search();
    }

    void search(){
        Debug.Log("search..");
        SceneManager.LoadScene("SearchingPlayer", LoadSceneMode.Additive);
    }


    private bool isCreditSuffisant(float entry_fee, string win_type)
    {

        if (win_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_CASH) && (UserManager.CurrentUser.money_credit >= entry_fee))
        {
            return true;
        }
        if (win_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES) && (UserManager.CurrentUser.bubble_credit >= entry_fee))
        {
            return true;
        }

        return false;

    }
}
