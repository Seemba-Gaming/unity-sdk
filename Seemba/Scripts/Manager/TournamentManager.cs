using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class TournamentManager : MonoBehaviour
{
    #region Static
    public static TournamentManager Get { get { return sInstance; } }
    private static TournamentManager sInstance;

    public static List<String> AVALAIBLE_TOURNAMENTS = new List<String>();

    public const int TOURNAMENT_8 = 8;
    public const int TOURNAMENT_16 = 16;
    public const int TOURNAMENT_32 = 32;
    public const float FEE_BRACKET_CASH_CONFIDENT = 2.00f;
    public const float FEE_BRACKET_CASH_CHAMPION = 4.00f;
    public const float FEE_BRACKET_CASH_LEGEND = 8.00f;
    public const float WIN_BRACKET_CASH_CONFIDENT = 8.00f;
    public const float WIN_BRACKET_CASH_CHAMPION = 16.00f;
    public const float WIN_BRACKET_CASH_LEGEND = 32.00f;
    public const float FEE_BRACKET_BUBBLE_CONFIDENT = 5.00f;
    public const float FEE_BRACKET_BUBBLE_CHAMPION = 10.00f;
    public const float FEE_BRACKET_BUBBLE_LEGEND = 20.00f;
    public const float WIN_BRACKET_BUBBLE_CONFIDENT = 25f;
    public const float WIN_BRACKET_BUBBLE_CHAMPION = 50f;
    public const float WIN_BRACKET_BUBBLE_LEGEND = 100f;
    public const string GAIN_TYPE_CASH = "cash";
    public const string GAIN_TYPE_BUBBLE = "bubble";
    public const string BRACKET_TYPE_CONFIDENT = "Confident";
    public const string BRACKET_TYPE_CHAMPION = "Legend";
    public const string BRACKET_TYPE_LEGEND = "Champion";
    #endregion

    #region Unity Methods
    private void Awake()
    {
        sInstance = this;
    }
    #endregion

    #region Methods
    public async Task<JSONArray> getUserPendingTournaments(string token)
    {
        string url = Endpoint.classesURL + "/tournaments/pending/" + GamesManager.GAME_ID;
        var json = JSON.Parse(await SeembaWebRequest.Get.HttpsGet(url));
        return json["data"].AsArray;

        //UnityWebRequest www = UnityWebRequest.Get(url);
        //if (token != null)
        //{
        //    www.SetRequestHeader("x-access-token", token);
        //    await www.SendWebRequest();

        //    if (www.isNetworkError || www.isHttpError)
        //    {
        //        return null;
        //    }
        //    var jsonResponse = www.downloadHandler.text;
        //    var json = JSON.Parse(jsonResponse);
        //    return json["data"].AsArray;
        //}
        //return null;
    }
    public async Task<JSONArray> getUserFinishedTournaments()
    {

        string url = Endpoint.classesURL + "/tournaments/finished/" + GamesManager.GAME_ID;
        var req = await SeembaWebRequest.Get.HttpsGet(url);
        var json = JSON.Parse(req);
        return json["data"].AsArray;
        //var www = UnityWebRequest.Get(url);
        //var token = UserManager.Get.getCurrentSessionToken();

        //if (token != null)
        //{
        //    www.SetRequestHeader("x-access-token", token);
        //    await www.SendWebRequest();

        //    if (www.isNetworkError || www.isHttpError)
        //    {
        //        return null;
        //    }


        //    var jsonResponse = www.downloadHandler.text;
        //    var json = JSON.Parse(jsonResponse);
        //    return json["data"].AsArray;
        //}
        //return null;
    }
    public async Task<JSONNode> getTournament(string id, string token)
    {
        string url = Endpoint.classesURL + "/tournaments/" + id;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        var json = JSON.Parse(await SeembaWebRequest.Get.HttpsGet(url));
        return json;

        //UnityWebRequest www = UnityWebRequest.Get(url);
        //www.SetRequestHeader("x-access-token", token);
        //await www.SendWebRequest();
        //Debug.Log(www.downloadHandler.text);
        //if (www.isNetworkError || www.isHttpError)
        //{
        //    return null;
        //}
        //var json = JSON.Parse(www.downloadHandler.text);
        //return json;
    }
    public async Task<bool> addScore(string tournamentsID, float score)
    {

        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

        string json = "score=" + score + "&user_id=" + UserManager.Get.getCurrentUserId();
        byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);

        string url = Endpoint.classesURL + "/tournaments/" + tournamentsID;
        Debug.LogWarning(url);
        var www = UnityWebRequest.Put(url, jsonAsBytes);
        www.SetRequestHeader("x-access-token", UserManager.Get.getCurrentSessionToken());
        www.uploadHandler.contentType = "application/x-www-form-urlencoded";

        Debug.LogWarning(UserManager.Get.getCurrentSessionToken());
        Debug.LogWarning(UserManager.Get.getCurrentUserId());
        await www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.isNetworkError);
            return false;
        }
        return true;
    }
    public async Task<string> JoinOrCreateTournament(int nb_player, float gain, string gain_type, string userId, string token)
    {
        string url = Endpoint.classesURL + "/tournaments";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        WWWForm form = new WWWForm();
        form.AddField("nb_player", nb_player);
        form.AddField("gain", gain.ToString());
        form.AddField("gain_type", gain_type);
        form.AddField("game_id", GamesManager.GAME_ID);
        form.AddField("user_id", userId);
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        www.SetRequestHeader("x-access-token", UserManager.Get.getCurrentSessionToken());
        await www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            return null;
        }
        var tournementdata = JSON.Parse(www.downloadHandler.text);
        UserManager.Get.UpdateUserCredit((tournementdata["user"]["money_credit"].AsFloat).ToString(), tournementdata["user"]["bubble_credit"].Value);
        TournamentController.setCurrentTournamentID(tournementdata["tournament"]["_id"].Value);
        return tournementdata["tournament"]["_id"].Value;
    }
    public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain,
        // look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    continue;
                }
                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                bool chainIsValid = chain.Build((X509Certificate2)certificate);
                if (!chainIsValid)
                {
                    isOk = false;
                    break;
                }
            }
        }
        return isOk;
    }
    #endregion
}
