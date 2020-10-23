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
        UnityWebRequest www = UnityWebRequest.Get(url);

        if (token != null)
        {
            www.SetRequestHeader("x-access-token", token);
            await www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                return false;
            }
            Debug.LogWarning(www.downloadHandler.text);
            var res = JSON.Parse(www.downloadHandler.text);
            Debug.LogWarning(res["data"]["gain"].Value);
            ShowFavTournament(res["data"]["gain"].Value , res["data"]["gain_type"].Value, res["data"]["type"].Value);
        }
        return true;
    }
    #endregion
    #region Implementation

    private void ShowFavTournament(string gain, string type, string challengeType)
    {
        if (challengeType.Equals("challenge"))
        {
            Fav1V1.SetActive(true);
            FavTournament.SetActive(false);
            if (type.Contains("cash"))
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

            if (type.Contains("cash"))
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
