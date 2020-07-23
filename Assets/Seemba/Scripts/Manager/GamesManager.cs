using SimpleJSON;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
public class GamesManager : MonoBehaviour
{

    //Set The Game Name*
    public static string GAME_NAME = "";
    //
    public static string GAME_ANDROID_URL = "";
    public static string GAME_IOS_URL = "";
    //Set Scene name of game , Entry point to the game
    public static string GAME_SCENE_NAME = "";
    public static string EDITOR_ID;
    //The game level number used for your matchmaking. Otherwise, keep it null.
    public static int? GAME_LEVEL = null;

    //Set The Game Id shown in your Dashboard ,you can't start without setting the correct id
    internal static string GAME_ID = "";




    public static string ICON_URL;


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
    private static HttpWebRequest CreateWebRequest(Uri uri)
    {
        //Webrequest creation does fail on MONO randomly when using WebRequest.Create
        //the issue occurs in the GetCreator method here: http://www.oschina.net/code/explore/mono-2.8.1/mcs/class/System/System.Net/WebRequest.cs
        var type = Type.GetType("System.Net.HttpRequestCreator, System, Version=4.0.0.0,Culture=neutral, PublicKeyToken=b77a5c561934e089");
        var creator = Activator.CreateInstance(type, nonPublic: true) as IWebRequestCreate;
        return creator.Create(uri) as HttpWebRequest;
    }
    public ArrayList getPromotions(string id)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/promotions/promote/" + id;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
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
                    Debug.Log(jsonResponse);
                    var json = JSON.Parse(jsonResponse);
                    var array = json["data"].AsArray;
                    ArrayList listGames = new ArrayList();
                    foreach (JSONNode N in array)
                    {
                        Game game = new Game(N["_id"].Value, N["name"].Value, N["editorId"].Value, N["bundle_id"].Value, N["appstore_id"].Value, N["icon"].Value);
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
                    if (string.IsNullOrEmpty(jsonResponse)) { return null; }

                    var N = JSON.Parse(jsonResponse);
                    if (N["success"].AsBool == true)
                    {

                        gameId = N["data"]["_id"].Value;
                        GAME_NAME = N["data"]["name"].Value;
                        EDITOR_ID = N["data"]["editor"].Value;
                        if (!string.IsNullOrEmpty(N["data"]["background_image"].Value))
                        {
                            BackgroundController.backgroundURL = N["data"]["background_image"].Value;
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
                        Debug.LogError("verify the game's id ...");
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
                            BackgroundController.backgroundURL = N["data"]["background_image"].Value;
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
