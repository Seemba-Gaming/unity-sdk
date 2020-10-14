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

public class ChallengeManager : MonoBehaviour
{
    UserManager userManager = new UserManager();
    public string AdversaryId;
    public static string CurrentChallengeGain, CurrentChallengeGainType;
    public static string CurrentChallengeId;
    public static Challenge CurrentChallenge;
    public static string CurrentChallengeRequestId;
    public static string CurrentChallengeStatus;
    public static System.Timers.Timer aTimer = new System.Timers.Timer();
    public static System.Timers.Timer t;
    public static string date;

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
    public static List<String> AVALAIBLE_CHALLENGE = new List<String>();

    // Use this for initialization
    void Start()
    {
        userManager = new UserManager();
    }
    // Update is called once per frame
    private void Win()
    {
        string[] attrib = { "last_result" };
        string[] values = { "WIN" };
        userManager.UpdateUserByField(userManager.getCurrentUserId(), userManager.getCurrentSessionToken(), attrib, values);
    }
    private void Loss()
    {
        string[] attrib = { "last_result" };
        string[] values = { "loss" };
        userManager.UpdateUserByField(userManager.getCurrentUserId(), userManager.getCurrentSessionToken(), attrib, values);
    }
    public void ShowResults()
    {

        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        UnityThreading.ActionThread thread = null;
        float? scoreUser1 = null;
        float? scoreUser2 = null;
        JSONNode challengeResult = null;
        string userId = userManager.getCurrentUserId();
        string token = userManager.getCurrentSessionToken();
        User user = userManager.getUser(userId, token);
        thread = UnityThreadHelper.CreateThread(() =>
        {
            challengeResult = getChallengebyId(CurrentChallengeId, token);
            try
            {
                if (String.IsNullOrEmpty(challengeResult["data"]["user_1_score"].Value))
                {
                    scoreUser1 = null;
                }
                else scoreUser1 = challengeResult["data"]["user_1_score"].AsFloat;
            }
            catch (NullReferenceException ex)
            {
                scoreUser1 = null;
            }
            try
            {
                if (String.IsNullOrEmpty(challengeResult["data"]["user_2_score"].Value))
                {
                    scoreUser2 = null;
                }
                else scoreUser2 = challengeResult["data"]["user_2_score"].AsFloat;
            }
            catch (NullReferenceException ex)
            {
                scoreUser2 = null;
            }
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadScene("Loader");
                //TODO
                if (scoreUser2 == null || scoreUser1 == null)
                {
                    SceneManager.LoadScene("ResultWaiting");
                }
                else
                {
                    //DRAW
                    if (scoreUser1 == scoreUser2)
                    {
                        SceneManager.LoadScene("ResultEquality");
                    }
                    else if (userId.Equals(challengeResult["data"]["winner_user"].Value))
                    {
                        Win();
                        SceneManager.LoadScene("ResultWin");
                    }
                    else
                    {
                        Loss();
                        SceneManager.LoadScene("ResultLose");
                    }
                }
            });
        });
    }
    public JSONNode getChallengebyId(string challengeId, string token)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/challenges/" + challengeId;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Headers["x-access-token"] = token;
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log("getChallengebyId: " + jsonResponse);
                    var N = JSON.Parse(jsonResponse);
                    if (N != null)
                    {

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
    public ArrayList getPendingChallenges(string token)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/challenges/pending?game_id=" + GamesManager.GAME_ID;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Timeout = 5000;
        request.Headers["x-access-token"] = token;
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();

                    var json = JSON.Parse(jsonResponse);
                    Debug.Log(jsonResponse);

                    var array = json["data"].AsArray;
                    ArrayList pendingChallenges = new ArrayList();
                    foreach (JSONNode N in array)
                    {
                        User matched_user_1 = new User(N["matched_user_1"]["_id"].Value, N["matched_user_1"]["username"].Value, N["matched_user_1"]["avatar"].Value, N["matched_user_1"]["username_changed"].AsBool, N["matched_user_1"]["personal_id_number"].Value,
                            "", "", N["matched_user_1"]["money_credit"].AsFloat,
                            N["matched_user_1"]["bubble_credit"].AsFloat, N["matched_user_1"]["email"].Value, N["matched_user_1"]["password"].Value,
                            N["matched_user_1"]["amateur_bubble"].AsInt, N["matched_user_1"]["novice_bubble"].AsInt, N["matched_user_1"]["legend_bubble"].AsInt,
                            N["matched_user_1"]["confident_bubble"].AsInt, N["matched_user_1"]["confirmed_bubble"].AsInt, N["matched_user_1"]["champion_bubble"].AsInt,
                            N["matched_user_1"]["amateur_money"].AsInt, N["matched_user_1"]["novice_money"].AsInt, N["matched_user_1"]["legend_money"].AsInt,
                            N["matched_user_1"]["confident_money"].AsInt, N["matched_user_1"]["confirmed_money"].AsInt, N["matched_user_1"]["champion_money"].AsInt,
                            N["matched_user_1"]["losses_streak"].AsInt, N["matched_user_1"]["victories_streak"].AsInt, N["matched_user_1"]["long_lat"].Value,
                            "", N["matched_user_1"]["email_verified"].AsBool, N["matched_user_1"]["iban_uploaded"].AsBool,
                            N["matched_user_1"]["level"].AsInt, "", N["matched_user_1"]["id_proof_1_uploaded"].AsBool,
                            N["matched_user_1"]["id_proof_2_uploaded"].AsBool, "", N["matched_user_1"]["country_code"].Value,
                            "", 0, "",
                            N["matched_user_1"]["passport_uploaded"].AsBool, N["matched_user_1"]["last_result"].Value, "",
                            "", "", N["matched_user_1"]["residency_proof_uploaded"].AsBool,
                            N["matched_user_1"]["victories_count"].AsInt, N["matched_user_1"]["phone"].Value);
                        Game game = new Game(N["game"]["_id"].Value, N["game"]["name"].Value);
                        User matched_user_2 = null;
                        if (N["matched_user_2"]["_id"].Value != null)
                        {
                            matched_user_2 = new User(N["matched_user_2"]["_id"].Value, N["matched_user_2"]["username"].Value, N["matched_user_2"]["avatar"].Value, N["matched_user_2"]["username_changed"].AsBool, N["matched_user_2"]["personal_id_number"].Value,
                                "", "", N["matched_user_2"]["money_credit"].AsFloat,
                                N["matched_user_2"]["bubble_credit"].AsFloat, N["matched_user_2"]["email"].Value, N["matched_user_2"]["password"].Value,
                                N["matched_user_2"]["amateur_bubble"].AsInt, N["matched_user_2"]["novice_bubble"].AsInt, N["matched_user_2"]["legend_bubble"].AsInt,
                                N["matched_user_2"]["confident_bubble"].AsInt, N["matched_user_2"]["confirmed_bubble"].AsInt, N["matched_user_2"]["champion_bubble"].AsInt,
                                N["matched_user_2"]["amateur_money"].AsInt, N["matched_user_2"]["novice_money"].AsInt, N["matched_user_2"]["legend_money"].AsInt,
                                N["matched_user_2"]["confident_money"].AsInt, N["matched_user_2"]["confirmed_money"].AsInt, N["matched_user_2"]["champion_money"].AsInt,
                                N["matched_user_2"]["losses_streak"].AsInt, N["matched_user_2"]["victories_streak"].AsInt, N["matched_user_2"]["long_lat"].Value,
                                "", N["matched_user_2"]["email_verified"].AsBool, N["matched_user_2"]["iban_uploaded"].AsBool,
                                N["matched_user_2"]["level"].AsInt, "", N["matched_user_2"]["id_proof_1_uploaded"].AsBool,
                                N["matched_user_2"]["id_proof_2_uploaded"].AsBool, "", N["matched_user_2"]["country_code"].Value,
                                "", 0, "",
                                N["matched_user_2"]["passport_uploaded"].AsBool, N["matched_user_2"]["last_result"].Value, "",
                                "", "", N["matched_user_2"]["residency_proof_uploaded"].AsBool,
                                N["matched_user_2"]["victories_count"].AsInt, N["matched_user_2"]["phone"].Value);
                        }
                        float? user_1_score, user_2_score;
                        int? game_level;
                        try
                        {
                            user_2_score = N["user_2_score"].AsFloat;
                        }
                        catch (NullReferenceException ex)
                        {
                            user_2_score = null;
                        }
                        try
                        {
                            user_1_score = N["user_1_score"].AsFloat;
                        }
                        catch (NullReferenceException ex)
                        {
                            user_1_score = null;
                        }
                        try
                        {
                            game_level = N["game_level"].AsInt;
                        }
                        catch (NullReferenceException ex)
                        {
                            game_level = null;
                        }
                        var created_at = string.IsNullOrEmpty(N["created_at"]) ? N["createdAt"].Value : N["created_at"].Value;
                        Challenge Challenge = new Challenge(N["_id"].Value, matched_user_1, matched_user_2, user_1_score, user_2_score, N["challenge_type"].Value, game, N["status"].Value, N["gain"].Value, N["gain_type"].Value, N["level"].AsInt, TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Parse(created_at)).ToString(), N["winner_user"].Value, game_level);
                        pendingChallenges.Add(Challenge);
                    }
                    return pendingChallenges;
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
                        Debug.Log(error);
                    }
                }
            }
            return null;
        }
    }
    public ArrayList getSeeResultsChallenges(string token)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/challenges/see_results?game_id=" + GamesManager.GAME_ID;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Timeout = 5000;
        request.Headers["x-access-token"] = token;
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log(jsonResponse);
                    var json = JSON.Parse(jsonResponse);
                    var array = json["data"].AsArray;
                    ArrayList SeeResultsChallenges = new ArrayList();
                    foreach (JSONNode N in array)
                    {
                        User matched_user_1 = new User(N["matched_user_1"]["_id"].Value, N["matched_user_1"]["username"].Value, N["matched_user_1"]["avatar"].Value, N["matched_user_1"]["username_changed"].AsBool, N["matched_user_1"]["personal_id_number"].Value,
                            "", "", N["matched_user_2"]["money_credit"].AsFloat,
                            N["matched_user_1"]["bubble_credit"].AsFloat, N["matched_user_1"]["email"].Value, N["matched_user_1"]["password"].Value,
                            N["matched_user_1"]["amateur_bubble"].AsInt, N["matched_user_1"]["novice_bubble"].AsInt, N["matched_user_1"]["legend_bubble"].AsInt,
                            N["matched_user_1"]["confident_bubble"].AsInt, N["matched_user_1"]["confirmed_bubble"].AsInt, N["matched_user_1"]["champion_bubble"].AsInt,
                            N["matched_user_1"]["amateur_money"].AsInt, N["matched_user_1"]["novice_money"].AsInt, N["matched_user_1"]["legend_money"].AsInt,
                            N["matched_user_1"]["confident_money"].AsInt, N["matched_user_1"]["confirmed_money"].AsInt, N["matched_user_1"]["champion_money"].AsInt,
                            N["matched_user_1"]["losses_streak"].AsInt, N["matched_user_1"]["victories_streak"].AsInt, N["matched_user_1"]["long_lat"].Value,
                            "", N["matched_user_1"]["email_verified"].AsBool, N["matched_user_1"]["iban_uploaded"].AsBool,
                            N["matched_user_1"]["level"].AsInt, "", N["matched_user_1"]["id_proof_1_uploaded"].AsBool,
                            N["matched_user_1"]["id_proof_2_uploaded"].AsBool, "", N["matched_user_1"]["country_code"].Value,
                            "", 0, "",
                            N["matched_user_1"]["passport_uploaded"].AsBool, N["matched_user_1"]["last_result"].Value, "",
                            "", "", N["matched_user_1"]["residency_proof_uploaded"].AsBool,
                            N["matched_user_1"]["victories_count"].AsInt, N["matched_user_1"]["phone"].Value);
                        User matched_user_2 = new User(N["matched_user_2"]["_id"].Value, N["matched_user_2"]["username"].Value, N["matched_user_2"]["avatar"].Value, N["matched_user_2"]["username_changed"].AsBool, N["matched_user_2"]["personal_id_number"].Value,
                            "", "", N["matched_user_2"]["money_credit"].AsFloat,
                            N["matched_user_2"]["bubble_credit"].AsFloat, N["matched_user_2"]["email"].Value, N["matched_user_2"]["password"].Value,
                            N["matched_user_2"]["amateur_bubble"].AsInt, N["matched_user_2"]["novice_bubble"].AsInt, N["matched_user_2"]["legend_bubble"].AsInt,
                            N["matched_user_2"]["confident_bubble"].AsInt, N["matched_user_2"]["confirmed_bubble"].AsInt, N["matched_user_2"]["champion_bubble"].AsInt,
                            N["matched_user_2"]["amateur_money"].AsInt, N["matched_user_2"]["novice_money"].AsInt, N["matched_user_2"]["legend_money"].AsInt,
                            N["matched_user_2"]["confident_money"].AsInt, N["matched_user_2"]["confirmed_money"].AsInt, N["matched_user_2"]["champion_money"].AsInt,
                            N["matched_user_2"]["losses_streak"].AsInt, N["matched_user_2"]["victories_streak"].AsInt, N["matched_user_2"]["long_lat"].Value,
                            "", N["matched_user_2"]["email_verified"].AsBool, N["matched_user_2"]["iban_uploaded"].AsBool,
                            N["matched_user_2"]["level"].AsInt, "", N["matched_user_2"]["id_proof_1_uploaded"].AsBool,
                            N["matched_user_2"]["id_proof_2_uploaded"].AsBool, "", N["matched_user_2"]["country_code"].Value,
                            "", 0, "",
                            N["matched_user_2"]["passport_uploaded"].AsBool, N["matched_user_2"]["last_result"].Value, "",
                            "", "", N["matched_user_2"]["residency_proof_uploaded"].AsBool,
                            N["matched_user_2"]["victories_count"].AsInt, N["matched_user_2"]["phone"].Value);
                        Game game = new Game(N["game"]["_id"].Value, N["game"]["name"].Value);
                        float? user_1_score, user_2_score;
                        int? game_level;
                        try
                        {
                            user_2_score = N["user_2_score"].AsFloat;
                        }
                        catch (NullReferenceException ex)
                        {
                            user_2_score = null;
                        }
                        try
                        {
                            user_1_score = N["user_1_score"].AsFloat;
                        }
                        catch (NullReferenceException ex)
                        {
                            user_1_score = null;
                        }
                        try
                        {
                            game_level = N["game_level"].AsInt;
                        }
                        catch (NullReferenceException ex)
                        {
                            game_level = null;
                        }
                        var created_at = string.IsNullOrEmpty(N["created_at"]) ? N["createdAt"].Value : N["created_at"].Value;
                        Challenge Challenge = new Challenge(N["_id"].Value, matched_user_1, matched_user_2, user_1_score, user_2_score, N["challenge_type"].Value, game, N["status"].Value, N["gain"].Value, N["gain_type"].Value, N["level"].AsInt, TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Parse(created_at)).ToString(), N["winner_user"].Value, game_level);
                        SeeResultsChallenges.Add(Challenge);
                    }
                    return SeeResultsChallenges;
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
    public ArrayList getFinishedChallenges(string token)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/challenges/finished?game_id=" + GamesManager.GAME_ID;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Timeout = 5000;
        request.Headers["x-access-token"] = token;
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log(jsonResponse);
                    var json = JSON.Parse(jsonResponse);
                    var array = json["data"].AsArray;
                    ArrayList finishedChallenges = new ArrayList();
                    foreach (JSONNode N in array)
                    {
                        User matched_user_1 = new User(N["matched_user_1"]["_id"].Value, N["matched_user_1"]["username"].Value, N["matched_user_1"]["avatar"].Value, N["matched_user_1"]["username_changed"].AsBool, N["matched_user_1"]["personal_id_number"].Value,
                            "", "", N["matched_user_2"]["money_credit"].AsFloat,
                            N["matched_user_1"]["bubble_credit"].AsFloat, N["matched_user_1"]["email"].Value, N["matched_user_1"]["password"].Value,
                            N["matched_user_1"]["amateur_bubble"].AsInt, N["matched_user_1"]["novice_bubble"].AsInt, N["matched_user_1"]["legend_bubble"].AsInt,
                            N["matched_user_1"]["confident_bubble"].AsInt, N["matched_user_1"]["confirmed_bubble"].AsInt, N["matched_user_1"]["champion_bubble"].AsInt,
                            N["matched_user_1"]["amateur_money"].AsInt, N["matched_user_1"]["novice_money"].AsInt, N["matched_user_1"]["legend_money"].AsInt,
                            N["matched_user_1"]["confident_money"].AsInt, N["matched_user_1"]["confirmed_money"].AsInt, N["matched_user_1"]["champion_money"].AsInt,
                            N["matched_user_1"]["losses_streak"].AsInt, N["matched_user_1"]["victories_streak"].AsInt, N["matched_user_1"]["long_lat"].Value,
                            "", N["matched_user_1"]["email_verified"].AsBool, N["matched_user_1"]["iban_uploaded"].AsBool,
                            N["matched_user_1"]["level"].AsInt, "", N["matched_user_1"]["id_proof_1_uploaded"].AsBool,
                            N["matched_user_1"]["id_proof_2_uploaded"].AsBool, "", N["matched_user_1"]["country_code"].Value,
                            "", 0, "",
                            N["matched_user_1"]["passport_uploaded"].AsBool, N["matched_user_1"]["last_result"].Value, "",
                            "", "", N["matched_user_1"]["residency_proof_uploaded"].AsBool,
                            N["matched_user_1"]["victories_count"].AsInt, N["matched_user_1"]["phone"].Value);
                        User matched_user_2 = new User(N["matched_user_2"]["_id"].Value, N["matched_user_2"]["username"].Value, N["matched_user_2"]["avatar"].Value, N["matched_user_2"]["username_changed"].AsBool, N["matched_user_2"]["personal_id_number"].Value,
                            "", "", N["matched_user_2"]["money_credit"].AsFloat,
                            N["matched_user_2"]["bubble_credit"].AsFloat, N["matched_user_2"]["email"].Value, N["matched_user_2"]["password"].Value,
                            N["matched_user_2"]["amateur_bubble"].AsInt, N["matched_user_2"]["novice_bubble"].AsInt, N["matched_user_2"]["legend_bubble"].AsInt,
                            N["matched_user_2"]["confident_bubble"].AsInt, N["matched_user_2"]["confirmed_bubble"].AsInt, N["matched_user_2"]["champion_bubble"].AsInt,
                            N["matched_user_2"]["amateur_money"].AsInt, N["matched_user_2"]["novice_money"].AsInt, N["matched_user_2"]["legend_money"].AsInt,
                            N["matched_user_2"]["confident_money"].AsInt, N["matched_user_2"]["confirmed_money"].AsInt, N["matched_user_2"]["champion_money"].AsInt,
                            N["matched_user_2"]["losses_streak"].AsInt, N["matched_user_2"]["victories_streak"].AsInt, N["matched_user_2"]["long_lat"].Value,
                            "", N["matched_user_2"]["email_verified"].AsBool, N["matched_user_2"]["iban_uploaded"].AsBool,
                            N["matched_user_2"]["level"].AsInt, "", N["matched_user_2"]["id_proof_1_uploaded"].AsBool,
                            N["matched_user_2"]["id_proof_2_uploaded"].AsBool, "", N["matched_user_2"]["country_code"].Value,
                            "", 0, "",
                            N["matched_user_2"]["passport_uploaded"].AsBool, N["matched_user_2"]["last_result"].Value, "",
                            "", "", N["matched_user_2"]["residency_proof_uploaded"].AsBool,
                            N["matched_user_2"]["victories_count"].AsInt, N["matched_user_2"]["phone"].Value);
                        float? user_1_score, user_2_score;
                        int? game_level;
                        try
                        {
                            user_2_score = N["user_2_score"].AsFloat;
                        }
                        catch (NullReferenceException ex)
                        {
                            user_2_score = null;
                        }
                        try
                        {
                            user_1_score = N["user_1_score"].AsFloat;
                        }
                        catch (NullReferenceException ex)
                        {
                            user_1_score = null;
                        }
                        try
                        {
                            game_level = N["game_level"].AsInt;
                        }
                        catch (NullReferenceException ex)
                        {
                            game_level = null;
                        }
                        Game game = new Game(N["game"]["_id"].Value, N["game"]["name"].Value);
                        var created_at = string.IsNullOrEmpty(N["created_at"]) ? N["createdAt"].Value : N["created_at"].Value;
                        Challenge Challenge = new Challenge(N["_id"].Value, matched_user_1, matched_user_2, user_1_score, user_2_score, N["challenge_type"].Value, game, N["status"].Value, N["gain"].Value, N["gain_type"].Value, N["level"].AsInt, TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Parse(created_at)).ToString(), N["winner_user"].Value, game_level);
                        finishedChallenges.Add(Challenge);
                    }
                    return finishedChallenges;
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
                    }
                }
            }
            return null;
        }
    }
    public ArrayList listChallenges(string token)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/challenges?game_id=" + GamesManager.GAME_ID;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Headers["x-access-token"] = token;
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log(jsonResponse);
                    var json = JSON.Parse(jsonResponse);
                    var array = json["data"].AsArray;
                    ArrayList challenges = new ArrayList();
                    foreach (JSONNode N in array)
                    {
                        User matched_user_1 = new User(N["matched_user_1"]["_id"].Value, N["matched_user_1"]["username"].Value, N["matched_user_1"]["avatar"].Value, N["matched_user_1"]["username_changed"].AsBool, N["matched_user_1"]["personal_id_number"].Value,
                            "", "", N["matched_user_2"]["money_credit"].AsFloat,
                            N["matched_user_1"]["bubble_credit"].AsFloat, N["matched_user_1"]["email"].Value, N["matched_user_1"]["password"].Value,
                            N["matched_user_1"]["amateur_bubble"].AsInt, N["matched_user_1"]["novice_bubble"].AsInt, N["matched_user_1"]["legend_bubble"].AsInt,
                            N["matched_user_1"]["confident_bubble"].AsInt, N["matched_user_1"]["confirmed_bubble"].AsInt, N["matched_user_1"]["champion_bubble"].AsInt,
                            N["matched_user_1"]["amateur_money"].AsInt, N["matched_user_1"]["novice_money"].AsInt, N["matched_user_1"]["legend_money"].AsInt,
                            N["matched_user_1"]["confident_money"].AsInt, N["matched_user_1"]["confirmed_money"].AsInt, N["matched_user_1"]["champion_money"].AsInt,
                            N["matched_user_1"]["losses_streak"].AsInt, N["matched_user_1"]["victories_streak"].AsInt, N["matched_user_1"]["long_lat"].Value,
                            "", N["matched_user_1"]["email_verified"].AsBool, N["matched_user_1"]["iban_uploaded"].AsBool,
                            N["matched_user_1"]["level"].AsInt, "", N["matched_user_1"]["id_proof_1_uploaded"].AsBool,
                            N["matched_user_1"]["id_proof_2_uploaded"].AsBool, "", N["matched_user_1"]["country_code"].Value,
                            "", 0, "",
                            N["matched_user_1"]["passport_uploaded"].AsBool, N["matched_user_1"]["last_result"].Value, "",
                            "", "", N["matched_user_1"]["residency_proof_uploaded"].AsBool,
                            N["matched_user_1"]["victories_count"].AsInt, N["matched_user_1"]["phone"].Value);
                        User matched_user_2 = new User(N["matched_user_2"]["_id"].Value, N["matched_user_2"]["username"].Value, N["matched_user_2"]["avatar"].Value, N["matched_user_2"]["username_changed"].AsBool, N["matched_user_2"]["personal_id_number"].Value,
                            "", "", N["matched_user_2"]["money_credit"].AsFloat,
                            N["matched_user_2"]["bubble_credit"].AsFloat, N["matched_user_2"]["email"].Value, N["matched_user_2"]["password"].Value,
                            N["matched_user_2"]["amateur_bubble"].AsInt, N["matched_user_2"]["novice_bubble"].AsInt, N["matched_user_2"]["legend_bubble"].AsInt,
                            N["matched_user_2"]["confident_bubble"].AsInt, N["matched_user_2"]["confirmed_bubble"].AsInt, N["matched_user_2"]["champion_bubble"].AsInt,
                            N["matched_user_2"]["amateur_money"].AsInt, N["matched_user_2"]["novice_money"].AsInt, N["matched_user_2"]["legend_money"].AsInt,
                            N["matched_user_2"]["confident_money"].AsInt, N["matched_user_2"]["confirmed_money"].AsInt, N["matched_user_2"]["champion_money"].AsInt,
                            N["matched_user_2"]["losses_streak"].AsInt, N["matched_user_2"]["victories_streak"].AsInt, N["matched_user_2"]["long_lat"].Value,
                            "", N["matched_user_2"]["email_verified"].AsBool, N["matched_user_2"]["iban_uploaded"].AsBool,
                            N["matched_user_2"]["level"].AsInt, "", N["matched_user_2"]["id_proof_1_uploaded"].AsBool,
                            N["matched_user_2"]["id_proof_2_uploaded"].AsBool, "", N["matched_user_2"]["country_code"].Value,
                            "", 0, "",
                            N["matched_user_2"]["passport_uploaded"].AsBool, N["matched_user_2"]["last_result"].Value, "",
                            "", "", N["matched_user_2"]["residency_proof_uploaded"].AsBool,
                            N["matched_user_2"]["victories_count"].AsInt, N["matched_user_2"]["phone"].Value);
                        float? user_1_score, user_2_score;
                        int? game_level;
                        try
                        {
                            user_2_score = N["user_2_score"].AsFloat;
                        }
                        catch (NullReferenceException ex)
                        {
                            user_2_score = null;
                        }
                        try
                        {
                            user_1_score = N["user_1_score"].AsFloat;
                        }
                        catch (NullReferenceException ex)
                        {
                            user_1_score = null;
                        }
                        try
                        {
                            game_level = N["game_level"].AsInt;
                        }
                        catch (NullReferenceException ex)
                        {
                            game_level = null;
                        }
                        Game game = new Game(N["game"]["_id"].Value, N["game"]["name"].Value);
                        var created_at = string.IsNullOrEmpty(N["created_at"]) ? N["createdAt"].Value : N["created_at"].Value;
                        Challenge Challenge = new Challenge(N["_id"].Value, matched_user_1, matched_user_2, user_1_score, user_2_score, N["challenge_type"].Value, game, N["status"].Value, N["gain"].Value, N["gain_type"].Value, N["level"].AsInt, TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Parse(created_at)).ToString(), N["winner_user"].Value, game_level);
                        challenges.Add(Challenge);
                    }
                    return challenges;
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
                        string error = reader.ReadToEnd();//Debug.Log(error);
                    }
                }
            }
            return null;
        }
    }
    public Challenge getChallenge(string challengeId, string token)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/challenges/" + challengeId;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Headers["x-access-token"] = token;
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log(jsonResponse);
                    var N = JSON.Parse(jsonResponse);
                    User matched_user_1 = null, matched_user_2 = null;
                    if (!jsonResponse.Contains("\"data\":null"))
                    {
                        if (N["data"]["status"].Value != "pending")
                        {
                            matched_user_2 = new User(N["data"]["matched_user_2"]["_id"].Value, N["data"]["matched_user_2"]["username"].Value, N["data"]["matched_user_2"]["avatar"].Value, N["data"]["matched_user_2"]["username_changed"].AsBool, N["matched_user_1"]["personal_id_number"].Value,
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
                            N["data"]["matched_user_2"]["victories_count"].AsInt, N["matched_user_2"]["phone"].Value);
                        }
                        matched_user_1 = new User(N["data"]["matched_user_1"]["_id"].Value, N["data"]["matched_user_1"]["username"].Value, N["data"]["matched_user_1"]["avatar"].Value, N["data"]["matched_user_1"]["username_changed"].AsBool, N["matched_user_2"]["personal_id_number"].Value,
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
                        N["data"]["matched_user_1"]["victories_count"].AsInt, N["matched_user_1"]["phone"].Value);
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
                        var created_at = String.IsNullOrEmpty(N["data"]["created_at"].Value) ? N["data"]["createdAt"].Value : N["data"]["created_at"].Value;
                        Debug.Log(created_at);
                        Challenge Challenge = new Challenge(N["data"]["_id"].Value, matched_user_1, matched_user_2, user_1_score, user_2_score, N["data"]["challenge_type"].Value, game, N["data"]["status"].Value, N["data"]["gain"].Value, N["data"]["gain_type"].Value, N["data"]["level"].AsInt, TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Parse(created_at)).ToString(), N["data"]["winner_user"].Value, game_level);
                        Debug.Log("cc bb");
                        return Challenge;
                    }
                    else return null;
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
                        Debug.Log(error);
                        return null;
                    }
                }
            }
            return null;
        }
    }
    public void addScore(string userId, string token, string challengeId, float score)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/challenges";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PUT";
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
        {
            string json = "challenge_id=" + challengeId + "&score=" + score;
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
                    Debug.Log("add score: " + jsonResponse);
                    var res = JSON.Parse(jsonResponse);
                    if (res["data"]["matched_user_1"]["_id"].Value == userId)
                    {
                        UserManager.UpdateUserCredit(res["data"]["matched_user_1"]["money_credit"].Value, res["data"]["matched_user_1"]["bubble_credit"].Value);
                    }
                    else if (!string.IsNullOrEmpty(res["data"]["matched_user_2"]["_id"].Value))
                    {
                        if (res["data"]["matched_user_2"]["_id"].Value == userId)
                        {
                            UserManager.UpdateUserCredit(res["data"]["matched_user_2"]["money_credit"].Value, res["data"]["matched_user_2"]["bubble_credit"].Value);
                        }
                    }
                    if (ReplayChallengePresenter.IsReplayChallenge())
                    {
                        ReplayChallengePresenter.ReplayCompleted();
                        GamesManager.GAME_LEVEL = ReplayChallengePresenter.GetOldGameLevel();
                    }
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
    public void updateChallenge(string challengeId, float score)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/challenges";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PUT";
        request.Headers["x-access-token"] = um.getCurrentSessionToken();
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
    public Challenge AddChallenge(string token, string challengeType, string gain, string gainType, int level)
    {
        string url = Endpoint.classesURL + "/challenges";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        try
        {
            using (var stream = request.GetRequestStream())
            {
                string json = "game=" + GamesManager.GAME_ID + "&gain=" + gain + "&gain_type=" + gainType + "&challenge_type=" + challengeType + "&level=" + level + "&game_level=" + GamesManager.GAME_LEVEL;
                Debug.Log("AddChallenge: " + json);
                byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);
                stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
            }
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log(jsonResponse);
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
                    CurrentChallengeId = N["data"]["_id"].Value;
                    CurrentChallengeStatus = N["data"]["status"].Value;
                    CurrentChallenge = Challenge;
                    return CurrentChallenge;
                }
            }
        }
        catch (WebException ex)
        {
            Debug.Log(ex);
            return null;
        }
    }
    public ArrayList getUserOngoingChallenges(string token)
    {
        ArrayList challenges = new ArrayList();
        JSONArray challengesIDs = new JSONArray();
        challenges = getSeeResultsChallenges(token);
        challenges.AddRange(getPendingChallenges(token));
        return challenges;
    }
    public Challenge UpdateChallengeStatusToFinished(string token, string challengeId)
    {
        UserManager um = new UserManager();
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
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log(jsonResponse);
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
                        
                    }
                }
            }
            return null;
        }
    }
    public ArrayList getChallengesUserResults(string token)
    {
        ArrayList challenges = new ArrayList();
        JSONArray challengesIDs = new JSONArray();
        challenges = getSeeResultsChallenges(token);
        challenges.AddRange(getPendingChallenges(token));
        challenges.AddRange(getFinishedChallenges(token));
        return challenges;
    }
}
