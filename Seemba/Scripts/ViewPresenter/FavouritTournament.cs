using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FavouritTournament : MonoBehaviour
{
    #region Static
    public static bool FavInPopUpProfile;
    const string OpenHtmlColorBracket = "<color=#535CB3>";
    const string CloseHtmlColorBracket = "</color>";
    #endregion

    #region Script Parameters
    public GameObject               Fav1V1;
    public GameObject               FavTournament;
    public Text                     Fav1V1Text;
    public Text                     FavTournamentText;
    #endregion

    #region Feilds
    string mCurrentGain;
    string mCurrentGainType;
    string mCurrentChallengeType;
    #endregion

    #region Unity Methods
    private async void OnEnable()
    {
        string userId;
        string token = UserManager.Get.getCurrentSessionToken();
        if (FavInPopUpProfile)
        {
            FavInPopUpProfile = false;
            userId = ViewsEvents.Get.Profile.PlayerId;
        }
        else
        {
            userId = UserManager.Get.CurrentUser._id;
        }

        await GetFavoriteTournament(userId, token);
    }
    #endregion

    #region Methods
    public async Task<bool> GetFavoriteTournament(string userId, string token)
    {
        string url = Endpoint.classesURL + "/users/" + userId + "/favorites";
        return await SeembaWebRequest.Get.HttpsGetJSON<bool>(url);

        //UnityWebRequest www = UnityWebRequest.Get(url);

        //if (token != null)
        //{
        //    www.SetRequestHeader("x-access-token", token);
        //    await www.SendWebRequest();
        //    if (www.isNetworkError || www.isHttpError)
        //    {
        //        return false;
        //    }
        //    var res = JSON.Parse(www.downloadHandler.text);
        //    ShowFavTournament(res["data"]["gain"].Value , res["data"]["gain_type"].Value, res["data"]["type"].Value);
        //}
        //return true;
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
        if (challengeType.Equals("1vs1"))
        {
            Fav1V1.SetActive(true);
            FavTournament.SetActive(false);
            if (gainType.Contains("cash"))
            {

                Fav1V1Text.text = OpenHtmlColorBracket + "Win " + CloseHtmlColorBracket + gain + " €";
            }
            else
            {
                Fav1V1Text.text = OpenHtmlColorBracket + "Win " + CloseHtmlColorBracket + gain + " Bubbles";
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
                FavTournamentText.text = OpenHtmlColorBracket + "Win " + CloseHtmlColorBracket + gain + " Bubbles";
            }
        }
    }
    #endregion
}
