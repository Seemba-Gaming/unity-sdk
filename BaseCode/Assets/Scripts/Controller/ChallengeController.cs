using System;
using System.Globalization;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
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
        public async void Play(object[] _duel_params)
        {
            float entry_fee = float.Parse(_duel_params[0].ToString().Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
            float gain = float.Parse(_duel_params[1].ToString().Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
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
                    await StartCashChallengeAsync(entry_fee, gain, gain_type);
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
        public async System.Threading.Tasks.Task StartCashChallengeAsync(float entry_fee, float gain, string gain_type)
        {
            if (isProhibitedLocation(UserManager.Get.CurrentUser.country_code))
            {
                Debug.LogWarning("hyere");
                PopupManager.Get.PopupController.ShowPopup(PopupType.PROHIBITED_LOCATION, PopupsText.Get.prohibited_location());
                return;
            }
            var mIsVpnEnabled = await isVPNEnabledAsync();
            if (mIsVpnEnabled)
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
            catch (NullReferenceException) { }
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
            return !CountryController.checkCountry(code);
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
        #endregion
    }
}
