using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System;
using System.Text;
using SimpleJSON;
using System.Threading;
using System.Timers;
//using UnityEditor.VersionControl;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ChallengeManager : MonoBehaviour
{
    #region Static
    public static ChallengeManager Get { get { return sInstance; } }

    private static ChallengeManager sInstance;
    #endregion

    //UserManager UserManager.Get = new UserManager ();
    public string AdversaryId;
    public static string CurrentChallengeGain, CurrentChallengeGainType;
    public static string CurrentChallengeId;
    public static Challenge CurrentChallenge;
    public static string CurrentChallengeRequestId;
    public static string CurrentChallengeStatus;
    public static System.Timers.Timer aTimer = new System.Timers.Timer();
    public static System.Timers.Timer t;
    public static string date;
    //Set All Challenges Details
    public const float FEE_1V1_CASH_CONFIDENT = 1.20f;
    public const float FEE_1V1_CASH_CHAMPION = 3.00f;
    public const float FEE_1V1_CASH_LEGEND = 6.00f;
    public const float WIN_1V1_CASH_CONFIDENT = 2.00f;
    public const float WIN_1V1_CASH_CHAMPION = 5.00f;
    public const float WIN_1V1_CASH_LEGEND = 10.00f;
    public const float FEE_1V1_BUBBLES_CONFIDENT = 1.00f;
    public const float FEE_1V1_BUBBLES_CHAMPION = 3.00f;
    public const float FEE_1V1_BUBBLES_LEGEND = 5.00f;
    public const float WIN_1V1_BUBBLES_CONFIDENT = 2f;
    public const float WIN_1V1_BUBBLES_CHAMPION = 6f;
    public const float WIN_1V1_BUBBLES_LEGEND = 10f;
    public const string CHALLENGE_TYPE_BRACKET = "Bracket";
    public const string CHALLENGE_TYPE_1V1 = "1vs1";
    public const string CHALLENGE_WIN_TYPE_BUBBLES = "bubble";
    public const string CHALLENGE_WIN_TYPE_CASH = "cash";
    public const string CHALLENGE_STATUS_RESULT_PENDING = "results pending";
    public const string CHALLENGE_STATUS_SEE_RESULT_FOR_USER1 = "see results for user 1";
    public const string CHALLENGE_STATUS_SEE_RESULT_FOR_USER2 = "see results for user 2";
    public const string CHALLENGE_STATUS_FINISHED = "finished";

    public const string CHALLENGE_TYPE_AMATEUR = "Amateur";
    public const string CHALLENGE_TYPE_NOVICE = "Novice";
    public const string CHALLENGE_TYPE_CONFIRMED = "Confirmed";

    public static List<string> AVALAIBLE_CHALLENGE = new List<string>();

    private void Awake()
    {
        sInstance = this;
    }
    // Use this for initialization
    void Start()
    {
        //UserManager.Get = new UserManager ();
    }
    // Update is called once per frame
    void Update()
    {
    }

    public float GetChallengeFee(float gain, string gainType)
    {
        if(gainType.Equals(CHALLENGE_WIN_TYPE_CASH))
        {
            if(gain.Equals(WIN_1V1_CASH_CONFIDENT))
            {
                return FEE_1V1_CASH_CONFIDENT;
            }
            else if(gain.Equals(WIN_1V1_CASH_CHAMPION))
            {
                return FEE_1V1_CASH_CHAMPION;
            }
            else
            {
                return FEE_1V1_CASH_LEGEND;
            }
        }
        else
        {
            if (gain.Equals(WIN_1V1_BUBBLES_CONFIDENT))
            {
                return FEE_1V1_BUBBLES_CONFIDENT;
            }
            else if (gain.Equals(WIN_1V1_BUBBLES_CHAMPION))
            {
                return FEE_1V1_BUBBLES_CHAMPION;
            }
            else
            {
                return FEE_1V1_BUBBLES_LEGEND;
            }
        }
    }

        private async void Win()
    {
        string[] attrib = { "last_result" };
        string[] values = { "WIN" };
        UserManager.Get.UpdateUserByField(attrib, values);
    }
    private async void Loss()
    {
        string[] attrib = { "last_result" };
        string[] values = { "loss" };
        UserManager.Get.UpdateUserByField(attrib, values);
    }
    public async void ShowResult()
    {
        EventsController.Get.AudioListener.enabled = true;
        ViewsEvents.Get.GoToMenu(ViewsEvents.Get.ResultPresenter.gameObject);
    }
    public JSONNode getChallengebyId(string challengeId, string token)
    {
        string url = Endpoint.classesURL + "/challenges/" + challengeId;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    var N = JSON.Parse(jsonResponse);
                    if (N != null)
                    {
                        Challenge challenge = new Challenge(); //(N ["data"]["matched_user_1"].Value, N ["data"]["matched_user_2"].Value, N ["data"]["user_1_score"].AsFloat, N ["data"]["user_2_score"].AsFloat, N ["data"]["challenge_type"].Value, N ["data"]["game"].Value, N ["data"]["status"].Value, N ["data"]["gain"].Value, N ["data"]["gain_type"].Value, N ["data"]["level"].AsInt);
                        return N;
                    }
                    else
                        return null;
                }
            }
        }
        catch (WebException ex)
        {
            return null;
        }
    }
    public async Task<ArrayList> getPendingChallenges(string token)
    {
        string url = Endpoint.classesURL + "/challenges/pending?game_id=" + GamesManager.GAME_ID;
        Challenge[] challengesList = await SeembaWebRequest.Get.HttpsGetJSON<Challenge[]>(url);
        ArrayList pendingChallenges = new ArrayList();
        foreach (Challenge challenge in challengesList)
        {
            pendingChallenges.Add(challenge);
        }
        return pendingChallenges;
        //UnityWebRequest www = UnityWebRequest.Get(url);
        //if (token != null)
        //{
        //    www.SetRequestHeader("x-access-token", token);
        //    await www.SendWebRequest();
        //    if (www.isNetworkError || www.isHttpError)
        //    {
        //        return null;
        //    }
        //    ChallengeListData challengesList = JsonConvert.DeserializeObject<ChallengeListData>(www.downloadHandler.text);
        //    ArrayList pendingChallenges = new ArrayList();
        //    foreach (Challenge challenge in challengesList.data)
        //    {
        //        pendingChallenges.Add(challenge);
        //    }
        //    return pendingChallenges;
        //}
        //return null;
    }
    public async Task<ArrayList> getSeeResultsChallenges(string token)
    {
        string url = Endpoint.classesURL + "/challenges/see_results?game_id=" + GamesManager.GAME_ID;
        Challenge[] challengesList = await SeembaWebRequest.Get.HttpsGetJSON<Challenge[]>(url);
        ArrayList seeResultChallenges = new ArrayList();
        foreach (Challenge challenge in challengesList)
        {
            seeResultChallenges.Add(challenge);
        }
        return seeResultChallenges;
        //UnityWebRequest www = UnityWebRequest.Get(url);
        //if (token != null)
        //{
        //    www.SetRequestHeader("x-access-token", token);
        //    await www.SendWebRequest();
        //    if (www.isNetworkError || www.isHttpError)
        //    {
        //        return null;
        //    }
        //    ChallengeListData challengesList = JsonConvert.DeserializeObject<ChallengeListData>(www.downloadHandler.text);
        //    ArrayList seeResultChallenges = new ArrayList();
        //    foreach (Challenge challenge in challengesList.data)
        //    {
        //        seeResultChallenges.Add(challenge);
        //    }
        //    return seeResultChallenges;
        //}
        //return null;
    }
    public async Task<ArrayList> getFinishedChallenges(string token)
    {
        string url = Endpoint.classesURL + "/challenges/finished?game_id=" + GamesManager.GAME_ID;
        var challengeListData = await SeembaWebRequest.Get.HttpsGetJSON<Challenge[]>(url);
        ArrayList finishedChallenges = new ArrayList();
        foreach (Challenge challenge in challengeListData)
        {
            finishedChallenges.Add(challenge);
        }
        return finishedChallenges;
        //UnityWebRequest www = UnityWebRequest.Get(url);
        //if (token != null)
        //{
        //    www.SetRequestHeader("x-access-token", token);
        //    await www.SendWebRequest();

        //    if (www.isNetworkError || www.isHttpError)
        //    {
        //        return null;
        //    }

        //    var challengeListData = JsonConvert.DeserializeObject<ChallengeListData>(www.downloadHandler.text);
        //    ArrayList finishedChallenges = new ArrayList();
        //    foreach (Challenge challenge in challengeListData.data)
        //    {
        //        finishedChallenges.Add(challenge);
        //    }
        //    return finishedChallenges;
        //}
        //return null;
    }
    public async Task<ArrayList> listChallenges()
    {
        string url = Endpoint.classesURL + "/challenges?game_id=" + GamesManager.GAME_ID;
        Challenge[] Challenges = await SeembaWebRequest.Get.HttpsGetJSON<Challenge[]>(url);
        ArrayList challenges = new ArrayList();
        foreach (Challenge challenge in Challenges)
        {
             challenges.Add(challenge);
        }
        return challenges;
    }
    public async Task<Challenge> getChallenge(string challenge_id)
    {
        string url = Endpoint.classesURL + "/challenges/" + challenge_id;
        Debug.LogWarning("here");
        return await SeembaWebRequest.Get.HttpsGetJSON<Challenge>(url);
    }

    public async Task<bool> addScore(string challengeId, float score)
    {
        string url = Endpoint.classesURL + "/challenges";

        string json = "challenge_id=" + challengeId + "&score=" + score;
        byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);

        var www = UnityWebRequest.Put(url, jsonAsBytes);
        www.SetRequestHeader("x-access-token", UserManager.Get.getCurrentSessionToken());
        www.uploadHandler.contentType = "application/x-www-form-urlencoded";
        await www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            return false;
        }
        var challengeData = JsonConvert.DeserializeObject<ChallengeData>(www.downloadHandler.text);
        var challenge = challengeData.data;
        CurrentChallenge = challengeData.data;

        if(challenge != null)
        {
            if (challenge.matched_user_1._id == UserManager.Get.getCurrentUserId())
            {
                UserManager.Get.UpdateUserMoneyCredit(challenge.matched_user_1.money_credit.ToString());
                UserManager.Get.UpdateUserBubblesCredit(challenge.matched_user_1.bubble_credit.ToString());
            }
            else if (!string.IsNullOrEmpty(challenge.matched_user_2._id))
            {
                if (challenge.matched_user_2._id == UserManager.Get.getCurrentUserId())
                {
                    UserManager.Get.UpdateUserMoneyCredit(challenge.matched_user_2.money_credit.ToString());
                    UserManager.Get.UpdateUserBubblesCredit(challenge.matched_user_2.bubble_credit.ToString());
                }
            }

            if (ReplayChallengePresenter.IsReplayChallenge())
            {
                ReplayChallengePresenter.ReplayCompleted();
                GamesManager.GAME_LEVEL = ReplayChallengePresenter.GetOldGameLevel();
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    public void updateChallenge(string challengeId, float score)
    {
        //UserManager um = new UserManager ();
        string url = Endpoint.classesURL + "/challenges";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PUT";
        request.Headers["x-access-token"] = UserManager.Get.getCurrentSessionToken();
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
        {
            string json = "challenge_id=" + challengeId + "&score=" + score + "&is_finished=" + "false";
            byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);
            stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
        }
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    //Debug.Log(jsonResponse);
                }
            }
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        //Debug.Log(error);
                    }
                }
            }
        }
    }
    public async Task<Challenge> AddChallenge(string challenge_type, string gain, string gain_type, int level, string token = null)
    {
        string url = Endpoint.classesURL + "/challenges";
        WWWForm form = new WWWForm();
        form.AddField("game", GamesManager.GAME_ID);
        form.AddField("gain", gain);
        form.AddField("gain_type", gain_type);
        form.AddField("challenge_type", challenge_type);
        form.AddField("level", level);
        form.AddField("game_level", GamesManager.GAME_LEVEL.ToString());
        var www = UnityWebRequest.Post(url, form);
        www.uploadHandler.contentType = "application/x-www-form-urlencoded";
        if(token == null)
        {
            www.SetRequestHeader("x-access-token", UserManager.Get.getCurrentSessionToken());
        }
        else
        {
            www.SetRequestHeader("x-access-token", token);
        }

        await www.SendWebRequest();
        if (www.isNetworkError ||  www.isHttpError) return null;
        ChallengeData challengeData = JsonConvert.DeserializeObject<ChallengeData>(www.downloadHandler.text);
        if (challengeData.success)
        {
            CurrentChallengeId = challengeData.data._id;
            CurrentChallengeStatus = challengeData.data.status;
            return challengeData.data;
        }
        else return null;

    }
    public async Task<ArrayList> getUserOngoingChallenges(string token)
    {
        ArrayList challenges = new ArrayList();
        JSONArray challengesIDs = new JSONArray();
        challenges = await getSeeResultsChallenges(token);
        challenges.AddRange(await getPendingChallenges(token));
        return challenges;
    }
    public Challenge UpdateChallengeStatusToFinished(string token, string challengeId)
    {
        //UserManager um = new UserManager ();
        string url = Endpoint.classesURL + "/challenges";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PUT";
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
        {
            string json = "challenge_id=" + challengeId + "&is_finished=" + true;
            byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);
            stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
        }
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd(); Debug.Log(jsonResponse);
                    var N = JSON.Parse(jsonResponse);
                    User matched_user_1 = null, matched_user_2 = null;
                    if (N["data"]["status"].Value != "pending")
                    {
                        matched_user_2 = new User(N["data"]["matched_user_2"]["_id"].Value, N["data"]["matched_user_2"]["username"].Value, N["data"]["matched_user_2"]["avatar"].Value, N["data"]["matched_user_2"]["username_changed"].AsBool, N["data"]["matched_user_2"]["personal_id_number"].Value,
                            "", "", N["data"]["matched_user_2"]["money_credit"].AsFloat,
                            N["data"]["matched_user_2"]["bubble_credit"].AsFloat, N["data"]["matched_user_2"]["email"].Value, N["data"]["matched_user_2"]["password"].Value,
                            N["data"]["matched_user_2"]["amateur_bubble"].AsInt, N["data"]["matched_user_2"]["novice_bubble"].AsInt, N["data"]["matched_user_2"]["legend_bubble"].AsInt,
                            N["data"]["matched_user_2"]["confident_bubble"].AsInt, N["data"]["matched_user_2"]["confirmed_bubble"].AsInt, N["data"]["matched_user_2"]["champion_bubble"].AsInt,
                            N["data"]["matched_user_2"]["amateur_money"].AsInt, N["data"]["matched_user_2"]["novice_money"].AsInt, N["data"]["matched_user_2"]["legend_money"].AsInt,
                            N["data"]["matched_user_2"]["confident_money"].AsInt, N["data"]["matched_user_2"]["confirmed_money"].AsInt, N["data"]["matched_user_2"]["champion_money"].AsInt,
                            N["data"]["matched_user_2"]["losses_streak"].AsInt, N["data"]["matched_user_2"]["victories_streak"].AsInt, N["data"]["matched_user_2"]["long_lat"].Value,
                            "", N["data"]["matched_user_2"]["email_verified"].AsBool, N["data"]["matched_user_2"]["iban_uploaded"].AsBool,
                            N["data"]["matched_user_2"]["level"].AsInt, "", N["data"]["matched_user_2"]["id_proof_1_uploaded"].AsBool,
                            N["data"]["matched_user_2"]["id_proof_2_uploaded"].AsBool, "", N["data"]["matched_user_2"]["country_code"].Value,
                            "", 0, "",
                            N["data"]["matched_user_2"]["passport_uploaded"].AsBool, N["data"]["matched_user_2"]["last_result"].Value, "",
                            "", "", N["data"]["matched_user_2"]["residency_proof_uploaded"].AsBool,
                            N["data"]["matched_user_2"]["victories_count"].AsInt, N["data"]["matched_user_2"]["phone"].Value);
                    }
                    matched_user_1 = new User(N["data"]["matched_user_1"]["_id"].Value, N["data"]["matched_user_1"]["username"].Value, N["data"]["matched_user_1"]["avatar"].Value, N["data"]["matched_user_1"]["username_changed"].AsBool, N["data"]["matched_user_1"]["personal_id_number"].Value,
                        "", "", N["data"]["matched_user_2"]["money_credit"].AsFloat,
                        N["data"]["matched_user_1"]["bubble_credit"].AsFloat, N["data"]["matched_user_1"]["email"].Value, N["data"]["matched_user_1"]["password"].Value,
                        N["data"]["matched_user_1"]["amateur_bubble"].AsInt, N["data"]["matched_user_1"]["novice_bubble"].AsInt, N["data"]["matched_user_1"]["legend_bubble"].AsInt,
                        N["data"]["matched_user_1"]["confident_bubble"].AsInt, N["data"]["matched_user_1"]["confirmed_bubble"].AsInt, N["data"]["matched_user_1"]["champion_bubble"].AsInt,
                        N["data"]["matched_user_1"]["amateur_money"].AsInt, N["data"]["matched_user_1"]["novice_money"].AsInt, N["data"]["matched_user_1"]["legend_money"].AsInt,
                        N["data"]["matched_user_1"]["confident_money"].AsInt, N["data"]["matched_user_1"]["confirmed_money"].AsInt, N["data"]["matched_user_1"]["champion_money"].AsInt,
                        N["data"]["matched_user_1"]["losses_streak"].AsInt, N["data"]["matched_user_1"]["victories_streak"].AsInt, N["data"]["matched_user_1"]["long_lat"].Value,
                        "", N["data"]["matched_user_1"]["email_verified"].AsBool, N["data"]["matched_user_1"]["iban_uploaded"].AsBool,
                        N["data"]["matched_user_1"]["level"].AsInt, "", N["data"]["matched_user_1"]["id_proof_1_uploaded"].AsBool,
                        N["data"]["matched_user_1"]["id_proof_2_uploaded"].AsBool, "", N["data"]["matched_user_1"]["country_code"].Value,
                        "", 0, "",
                        N["data"]["matched_user_1"]["passport_uploaded"].AsBool, N["data"]["matched_user_1"]["last_result"].Value, "",
                        "", "", N["data"]["matched_user_1"]["residency_proof_uploaded"].AsBool,
                        N["data"]["matched_user_1"]["victories_count"].AsInt, N["data"]["matched_user_1"]["phone"].Value);
                    float? user_1_score, user_2_score;
                    int? game_level;
                    try
                    {
                        user_2_score = N["data"]["user_2_score"].AsFloat;
                    }
                    catch (NullReferenceException ex)
                    {
                        user_2_score = null;
                    }
                    try
                    {
                        user_1_score = N["data"]["user_1_score"].AsFloat;
                    }
                    catch (NullReferenceException ex)
                    {
                        user_1_score = null;
                    }
                    try
                    {
                        game_level = N["data"]["game_level"].AsInt;
                    }
                    catch (NullReferenceException ex)
                    {
                        game_level = null;
                    }
                    Game game = new Game(N["data"]["game"]["_id"].Value, N["data"]["game"]["name"].Value);
                    var created_at = string.IsNullOrEmpty(N["data"]["created_at"]) ? N["data"]["createdAt"].Value : N["data"]["created_at"].Value;
                    Challenge Challenge = new Challenge(N["data"]["_id"].Value, matched_user_1, matched_user_2, user_1_score, user_2_score, N["data"]["challenge_type"].Value, game, N["data"]["status"].Value, N["data"]["gain"].Value, N["data"]["gain_type"].Value, N["data"]["level"].AsInt, TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Parse(created_at)).ToString(), N["data"]["winner_user"].Value, game_level);

                    return Challenge;

                }
            }
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        //Debug.Log(error);
                    }
                }
            }
            return null;
        }
    }
    public async Task<ArrayList> getChallengesUserResults(string token)
    {
        ArrayList challenges = new ArrayList();
        JSONArray challengesIDs = new JSONArray();
        challenges = await getSeeResultsChallenges(token);
        challenges.AddRange(await getPendingChallenges(token));
        challenges.AddRange(await getFinishedChallenges(token));
        return challenges;
    }
}
