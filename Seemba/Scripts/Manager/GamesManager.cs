using UnityEngine;
using System.Collections;
using System.Net;
using SimpleJSON;
using System;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine.Networking;
using System.Threading.Tasks;

[CLSCompliant(false)]
public class GamesManager : MonoBehaviour
{
    #region Static
    public static GamesManager Get { get { return sInstance; } }

    private static GamesManager sInstance;
    #endregion

    //Set The Game Name*
    public static string GAME_NAME = "Desert Dash";
    //
    public static string GAME_ANDROID_URL = "https://play.google.com/store/apps/details?id=com.seemba.desertdashmexico";
    public static string GAME_IOS_URL = "Desert Dash";
    //Set Scene name of game , Entry point to the game
    public static string GAME_SCENE_NAME = "main";
    public static bool GAME_SCENE_SEPERATED = false;
    public static string EDITOR_ID;
    //The game level number used for your matchmaking. Otherwise, keep it null.
    public static int? GAME_LEVEL = null;

    //Set The Game Id shown in your Dashboard ,you can't start without setting the correct id
    //internal static string GAME_ID = "5aa62f71e7c48800057cab19"; //prod 
    internal static string GAME_ID = "5a80e1f3230fac86d8f6f2c7"; //staging


    public static string ICON_URL;
    public static string BACKGROUND_IMAGE_URL;


    public static bool? backgroundSaved = null;
    public static bool? iconSaved = null;
    public static Sprite CurrentIcon;
    public const string FREE_BUBBLES_PUSH = "free_bubbles_push";
    public const string ADS_WATCHED = "ads_watched";
    public bool AutoPlayActivated;
    private void Awake()
    {
        sInstance = this;
    }
    public static async Task<bool> SaveIcon(string url)
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("IconURL")) || !PlayerPrefs.GetString("IconURL").Equals(url))
        {
            var www = UnityWebRequestTexture.GetTexture(url);
            await www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                return false;
            }
            var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            byte[] bytes;
            bytes = texture.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + '/' + "icon.png", bytes);
            PlayerPrefs.SetString("IconURL", url);
            return true;
        }
        else
        {
            return true;
        }
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
    //public ArrayList getPromotions(string id)
    //{
    //    //UserManager um = new UserManager();
    //    string url = Endpoint.classesURL + "/promotions/promote/" + id;
    //    ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
    //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    //    request.Method = "GET";
    //    try
    //    {
    //        HttpWebResponse response;
    //        using (response = (HttpWebResponse)request.GetResponse())
    //        {
    //            System.IO.Stream s = response.GetResponseStream();
    //            using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
    //            {
    //                var jsonResponse = sr.ReadToEnd();
    //                Debug.Log(jsonResponse);
    //                var json = JSON.Parse(jsonResponse);
    //                var array = json["data"].AsArray;
    //                ArrayList listGames = new ArrayList();
    //                foreach (JSONNode N in array)
    //                {
    //                    Game game = new Game(N["_id"].Value, N["name"].Value, N["editorId"].Value, N["bundle_id"].Value, N["appstore_id"].Value, N["icon"].Value);
    //                    listGames.Add(game);
    //                }
    //                return listGames;
    //            }
    //        }
    //    }
    //    catch (WebException ex)
    //    {
    //        Debug.Log(ex);
    //        return null;
    //    }
    //}

    public async Task<Game> getGamebyId(string gameId)
    {
        var url = Endpoint.classesURL + "/games/" + gameId;
        var req = await SeembaWebRequest.Get.HttpsGet(url);

        if (req != null)
        {
            var N = JSON.Parse(req);

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

            GameData res = JsonUtility.FromJson<GameData>(req);
            gameId = res.data._id;
            GAME_NAME = res.data.name;
            EDITOR_ID = res.data.editorId;
            return res.data;
        }
        else
        {
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
