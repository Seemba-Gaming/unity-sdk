using System;
using System.Globalization;
using UnityEngine;

public class ChallengeController : MonoBehaviour
{
    #region Static
    public static ChallengeController Get => sInstance;
    private static ChallengeController sInstance;
    #endregion
    public static string ChallengeType;

    #region Unity Methods
    private void Awake()
    {
        sInstance = this;
    }
    #endregion
    #region Methods
    public void Play(object[] _duel_params)
    {
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
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_INSUFFICIENT_BALANCE, PopupsText.Get.insufficient_balance());
            }
            else
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_INSUFFICIENT_BUBBLES, PopupsText.Get.insufficient_bubbles());
            }
        }
    }
    public void StartBubblesChallenge(float entry_fee, float gain, string gain_type)
    {
        JoinChallenge(entry_fee, gain, gain_type);
    }
    public void StartCashChallenge(float entry_fee, float gain, string gain_type)
    {
        if (isProhibitedLocation(UserManager.Get.CurrentUser.country_code))
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.PROHIBITED_LOCATION, PopupsText.Get.prohibited_location());
            return;
        }

        if (isVPNEnabled())
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.VPN, PopupsText.Get.vpn());
            return;
        }

        JoinChallenge(entry_fee, gain, gain_type);
    }
    #endregion

    #region Implementation
    private async void JoinChallenge(float entry_fee, float gain, string gain_type)
    {
        LoaderManager.Get.LoaderController.ShowLoader(null);
        ChallengeManager.CurrentChallenge = await ChallengeManager.Get.AddChallenge("headTohead", gain.ToString(), gain_type.ToString(), 0);
        LoaderManager.Get.LoaderController.HideLoader();

        if (ChallengeManager.CurrentChallenge != null)
        {
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Matchmaking.gameObject);
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
    private bool isVPNEnabled()
    {
        VPNManager vpn = new VPNManager();
        return vpn.isVpnConnected();
    }

    private void update(float entry_fee, float gain, string gain_type, string attrib, int value)
    {
        /*if (gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES)
        {
            UserManager.CurrentWater = (int.Parse(UserManager.CurrentWater) - int.Parse(entry_fee.ToString())).ToString();
        }
        else if (gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
        {
            Debug.Log(ChallengeManager.CHALLENGE_WIN_TYPE_CASH);
            SearchingForPlayerPresenter.GameMontant = GameMontant + CurrencyManager.CURRENT_CURRENCY;
            UserManager.CurrentMoney = (float.Parse(UserManager.CurrentMoney) - float.Parse(FeeGame)).ToString("N2");
        }
        SearchingForPlayerPresenter.isTrainingGame = false;
        UnityThreading.ActionThread thread;
        thread = UnityThreadHelper.CreateThread(() =>
        {
            string[] attribute = { attrib };
            string[] Att_value = { (value + 1).ToString() };
            userManager.UpdateUserByField(userId, token, attribute, Att_value);
        });*/
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
    #endregion
}
