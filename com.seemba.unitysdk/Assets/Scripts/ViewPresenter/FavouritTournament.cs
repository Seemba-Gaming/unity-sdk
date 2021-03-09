using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class FavouritTournament : MonoBehaviour
    {
        #region Static
        public static bool FavInPopUpProfile;
        const string OpenHtmlColorBracket = "<color=#535CB3>";
        const string CloseHtmlColorBracket = "</color>";
        #endregion

        #region Script Parameters
        public GameObject Fav1V1;
        public GameObject FavTournament;
        public Text Fav1V1Text;
        public Text FavTournamentText;
        #endregion

        #region Fields
        string mCurrentGain;
        string mCurrentGainType;
        string mCurrentChallengeType;
        #endregion

        #region Unity Methods
        private async void OnEnable()
        {
            string userId;
            userId = ViewsEvents.Get.Profile.PlayerId;
            await GetFavoriteTournament(userId);
        }
        #endregion

        #region Methods
        public async Task<bool> GetFavoriteTournament(string userId)
        {
            string url = Endpoint.classesURL + "/users/" + userId + "/favorites";
            var res = await SeembaWebRequest.Get.HttpsGet(url);
            if (!string.IsNullOrEmpty(res))
            {
                var json = JSON.Parse(res);
                ShowFavTournament(json["data"]["gain"].Value, json["data"]["gain_type"].Value, json["data"]["type"].Value);
            }
            return true;
        }

        public void OnClickFavouriteType()
        {
            ViewsEvents.Get.Profile.Animator.ResetTrigger("ShowProfile");
            EventsController.Get.closeProfil();
            var fee = ChallengeManager.Get.GetChallengeFee(float.Parse(mCurrentGain), mCurrentGainType);
            object[] _params = { fee, mCurrentGain, mCurrentGainType, mCurrentChallengeType };
            PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
        }
        #endregion

        #region Implementation

        private void ShowFavTournament(string gain, string gainType, string challengeType)
        {
            mCurrentGain = gain;
            mCurrentGainType = gainType;
            mCurrentChallengeType = challengeType;
            TranslationManager.scene = "Home";
            if (challengeType.Equals("1vs1"))
            {
                Fav1V1.SetActive(true);
                FavTournament.SetActive(false);
                if (gainType.Contains("cash"))
                {
                    Fav1V1Text.text = OpenHtmlColorBracket + TranslationManager.Get("win") + " " + CloseHtmlColorBracket + gain + " €";
                }
                else
                {
                    Fav1V1Text.text = OpenHtmlColorBracket + TranslationManager.Get("win") + " " + CloseHtmlColorBracket + gain + " " + TranslationManager.Get("bubbles");
                }
            }
            else
            {
                Fav1V1.SetActive(false);
                FavTournament.SetActive(true);
                if (gainType.Contains("cash"))
                {
                    FavTournamentText.text = OpenHtmlColorBracket + "Win " + CloseHtmlColorBracket + gain + " €";

                }
                else
                {
                    FavTournamentText.text = OpenHtmlColorBracket + "Win " + CloseHtmlColorBracket + gain + TranslationManager.Get("bubbles");
                }
            }
        }
        #endregion
    }
}
