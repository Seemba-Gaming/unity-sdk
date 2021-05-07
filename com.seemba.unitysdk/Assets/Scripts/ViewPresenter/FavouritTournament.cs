using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace SeembaSDK
{
    public class FavouriteChallenge
    {
        public string gain;
        public string gain_type;
        public string type;
    }
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
        public TextMeshProUGUI Fav1V1Text;
        public TextMeshProUGUI FavTournamentText;
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
            var responseText = await SeembaWebRequest.Get.HttpsGet(url);
            if (!string.IsNullOrEmpty(responseText))
            {
                SeembaResponse<FavouriteChallenge> response = JsonConvert.DeserializeObject<SeembaResponse<FavouriteChallenge>>(responseText);
                ShowFavTournament(response.data.gain, response.data.gain_type, response.data.type);
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
            TranslationManager._instance.scene = "Home";
            if (challengeType.Equals("1vs1"))
            {
                Fav1V1.SetActive(true);
                FavTournament.SetActive(false);
                if (gainType.Contains("cash"))
                {
                    Fav1V1Text.text = OpenHtmlColorBracket + TranslationManager._instance.Get("win") + " " + CloseHtmlColorBracket + (float.Parse(gain) * 100) + " <sprite=1>";
                }
                else
                {
                    Fav1V1Text.text = OpenHtmlColorBracket + TranslationManager._instance.Get("win") + " " + CloseHtmlColorBracket + float.Parse(gain) + " <sprite=0>";
                }
            }
            else
            {
                Fav1V1.SetActive(false);
                FavTournament.SetActive(true);
                if (gainType.Contains("cash"))
                {
                    FavTournamentText.text = OpenHtmlColorBracket + "Win " + CloseHtmlColorBracket + (float.Parse(gain) * 100) + " <sprite=1>";

                }
                else
                {
                    FavTournamentText.text = OpenHtmlColorBracket + "Win " + CloseHtmlColorBracket + (float.Parse(gain) * 100) + " <sprite=0>";
                }
            }
        }
        #endregion
    }
}
