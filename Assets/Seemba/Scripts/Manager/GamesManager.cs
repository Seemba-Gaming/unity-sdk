using UnityEngine;
using System.Collections;
using System.Net;
using SimpleJSON;
using System;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine.UI;
using System.Collections.Generic;

public class GamesManager : MonoBehaviour
{

    #region INTEGRATION_PARAMETERS
    //Set The Game Name
    public static string GAME_NAME;			//"Desert Dash";
    //Stores URLS
    public static string GAME_ANDROID_URL = "https://play.google.com/store/apps/details?id=com.seemba.desertdashmexico";
    public static string GAME_IOS_URL = "Desert Dash";
    //Set Scene name of game , Entry point to the game
    public static string GAME_SCENE_NAME;		//"main";
    public static string EDITOR_ID;
    //The game level number used for your matchmaking. Otherwise, keep it null.
    public static int? GAME_LEVEL = null;
    //Set The Game Id shown in your Dashboard ,you can't start without setting the correct id
    public static string GAME_ID;           //"5aa62f71e7c48800057cab19";
    #endregion

    public static string ICON_URL;
    public static string BACKGROUND_IMAGE_URL;


    public static bool? backgroundSaved = null;
    public static bool? iconSaved = null;
    public static Sprite CurrentIcon;
    public const string FREE_BUBBLES_PUSH = "free_bubbles_push";
    public const string ADS_WATCHED = "ads_watched";
    public static IEnumerator SaveIcon(string url)
    {
        yield return waitBackground();
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("IconURL")) || !PlayerPrefs.GetString("IconURL").Equals(url))
        {
            if (GameObject.Find("downloading").transform.localScale != Vector3.one)
            {
                GameObject.Find("downloading").transform.localScale = Vector3.one;
            }
            PlayerPrefs.SetString("IconURL", url);
            var www = new WWW(url);
            yield return www;
            var texture = www.texture;
            byte[] bytes;
            bytes = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "icon.png", bytes);
            iconSaved = true;
        }
        else
        {
            iconSaved = true;
            yield return null;
        }
    }
    static IEnumerator waitBackground()
    {
        yield return new WaitWhile(() => backgroundSaved != null);
    }
    public static void LoadIcon()
    {
        byte[] bytes;
        bytes = System.IO.File.ReadAllBytes(Application.persistentDataPath + '/' + "icon.png");
        Texture2D txt = new Texture2D(1, 1);
        txt.LoadImage(bytes);
        txt.Apply();
        CurrentIcon = Sprite.Create(txt, new Rect(0, 0, txt.width, txt.height), new Vector2(0, 0));
    }
   
    public ArrayList getPromotions(string id)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/promotions/promote/" + id;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
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
                    ArrayList listGames = new ArrayList();
                    foreach (JSONNode N in array)
                    {
                        Game game = new Game(N["_id"].Value, N["name"].Value, N["editorId"].Value, N["bundle_id"].Value, N["appstore_id"].Value, N["icon"].Value, N["background_image"].Value);
                        listGames.Add(game);
                    }
                    return listGames;
                }
            }
        }
        catch (WebException ex)
        {
            Debug.Log(ex);
            return null;
        }
    }
    public string getGamebyId(string gameId)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/games/" + gameId;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    if (string.IsNullOrEmpty(jsonResponse)) { return null; }
                    print(jsonResponse);
                    var N = JSON.Parse(jsonResponse);
                    if (N["success"].AsBool == true)
                    {

                        gameId = N["data"]["_id"].Value;
                        GAME_NAME = N["data"]["name"].Value;
                        EDITOR_ID = N["data"]["editor"].Value;

                        foreach (JSONNode bracket_type in N["data"]["brackets"].AsArray)
                        {
                            TournamentManager.AVALAIBLE_TOURNAMENTS.Add(bracket_type.Value);
                        }
                        foreach (JSONNode tournament_type in N["data"]["tournaments"].AsArray)
                        {
                            ChallengeManager.AVALAIBLE_CHALLENGE.Add(tournament_type.Value);
                        }

                        if (!string.IsNullOrEmpty(N["data"]["background_image"].Value))
                        {
                            BACKGROUND_IMAGE_URL = N["data"]["background_image"].Value;
                        }
                        else
                        {
                            Debug.LogError("Please add background-image for your game in the dashboard");
                        }
                        if (!string.IsNullOrEmpty(N["data"]["icon"].Value))
                        {
                            ICON_URL = N["data"]["icon"].Value;
                        }
                        else
                        {
                            Debug.LogError("Please add icon for your game in the dashboard");
                        }
                        return gameId;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        catch (WebException ex)
        {
            Debug.Log(ex);
            return null;
        }
    }
    public string GetGameIdByName(string GameName)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/games";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        //request.Timeout = 5000;
        request.ContentType = "application/x-www-form-urlencoded";
        try
        {
            using (var stream = request.GetRequestStream())
            {
                string json = "game_name=" + GameName;
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
                    var N = JSON.Parse(jsonResponse);
                    if (N["success"].AsBool == true)
                    {
                        Debug.Log(jsonResponse);
                        GAME_ID = N["data"]["_id"].Value;
                        GameName = N["data"]["name"].Value;
                        EDITOR_ID = N["data"]["editor"].Value;
                        if (!string.IsNullOrEmpty(N["data"]["background_image"].Value))
                        {
                            BACKGROUND_IMAGE_URL = N["data"]["background_image"].Value;
                        }
                        else
                        {
                            Debug.LogError("Please add background-image for your game in the dashboard");
                        }
                        if (!string.IsNullOrEmpty(N["data"]["icon"].Value))
                        {
                            ICON_URL = N["data"]["icon"].Value;
                        }
                        else
                        {
                            Debug.LogError("Please add icon for your game in the dashboard");
                        }
                        return GAME_ID;
                    }
                    else
                    {
                        //Debug.LogError("verify the game's name ...");
                        return null;
                    }
                }
            }
        }
        catch (WebException ex)
        {
            Debug.Log(ex);
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
    public bool MyRemoteCertificateValidationCallback(System.Object sender,
        X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
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
}
