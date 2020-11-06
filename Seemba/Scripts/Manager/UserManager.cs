using UnityEngine;
using System.Collections;
using System.Net;
using System;
using System.IO;
using System.Text;
using SimpleJSON;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine.Networking;
using System.Threading.Tasks;

#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
public class UserManager : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
{

    #region Static
    public static UserManager Get { get { return sInstance; } }

    private static UserManager sInstance;
    #endregion

    #region Script Parameters
    public string CurrentFlagBytesString;
    public Sprite CurrentAvatarBytesString;
    public User CurrentUser;
    #endregion

    #region Fields
    private string longLat;
    #endregion

    // Use this for initialization
    void OnEnable()
    {
    }

    private void Awake()
    {
        //Set The FPS target in the Awake function to avoid all changes from outsides.
        Application.targetFrameRate = 60;
        sInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public async void bubblesTrack(string type)
    {
        string url = Endpoint.classesURL + "/users/track/" + getCurrentUserId();
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        WWWForm form = new WWWForm();
        form.AddField("type", type);
        form.AddField("game", GamesManager.GAME_ID);
        await SeembaWebRequest.Get.HttpsPost(url, form);
    }
    public async Task<bool> WinFreeBubble(string token)
    {
        var url = Endpoint.classesURL + "/users/bubbles/free";
        await SeembaWebRequest.Get.HttpsPost(url, new WWWForm());
        return true;
    }

    public async void UpdateUserByField(string[] fieldName, string[] value)
    {
        string json = fieldName[0] + "=" + value[0];
        if (fieldName.Length > 1)
        {
            for (int i = 1; i < fieldName.Length; i++)
            {
                json = json + "&" + fieldName[i] + "=" + value[i];
            }
        }
        byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);

        string url = Endpoint.classesURL + "/users/" + getCurrentUserId();
        await SeembaWebRequest.Get.HttpsPut(url, jsonAsBytes);
    }

    public async Task<Texture2D> GetFlagBytes(string country_code)
    {
        string url = "https://seemba-api.herokuapp.com/flags/" + country_code + ".png";
        //var url = "https://seemba-users.s3.eu-west-2.amazonaws.com/1524121466763.jpeg";
        var www = UnityWebRequestTexture.GetTexture(url);
        await www.SendWebRequest();
        while (!www.isDone) { }

        var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        return texture;
    }
    public async Task<string> GetGeoLoc()
    {
        string url = Endpoint.locationURL;
        var req = await SeembaWebRequest.Get.HttpsGet(url);
        var N = JSON.Parse(req);
        return N["country"].Value.ToLower();
    }

    //signup with the new api
    public async Task<JSONNode> signingUp(string username, string email, string password, string avatar)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("country_code", await GetGeoLoc());
        form.AddField("long_lat", Input.location.lastData.latitude + "," + Input.location.lastData.longitude);
        form.AddField("game_id", GamesManager.GAME_ID);
        form.AddField("avatar", avatar);
        var url = Endpoint.classesURL + "/users";
        var response = await SeembaWebRequest.Get.HttpsPost(url, form);
        var N = JSON.Parse(response);
        //Save The current Session ID
        saveUserId(N["data"]["_id"].Value);
        //Save Session Token
        saveSessionToken(N["token"].Value);
        return N;
    }
    public async Task<Sprite> getAvatar(string url)
    {
        Texture2D texture = new Texture2D(100, 100);
        string prefsURL = PlayerPrefs.GetString(url);
        var www = UnityWebRequestTexture.GetTexture(url);
        await www.SendWebRequest();
        if(www.isNetworkError || www.isHttpError)
        {
            Debug.LogWarning(www.error);
            return null;
        }
        var avatarTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        texture = ImagesManager.RoundCrop(avatarTexture);
        PlayerPrefs.SetString(url, System.Convert.ToBase64String(texture.EncodeToPNG()));
        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
    }
    public void saveUserId(string user)
    {
        UnityThreadHelper.Dispatcher.Dispatch(() =>
        {
            writeStringToFile(user, "User.dat");
        });
    }
    public string getCurrentUserId()
    {
        return readStringFromFile("User.dat");
    }
    public void saveSessionToken(string token)
    {
        UnityThreadHelper.Dispatcher.Dispatch(() =>
        {
            writeStringToFile(token, "Token.dat");
        });
    }
    public string getCurrentSessionToken()
    {
        return readStringFromFile("Token.dat");
    }
    bool IsValidEmail(string email)
    {
        string expresion;
        expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        if (Regex.IsMatch(email, expresion))
        {
            if (Regex.Replace(email, expresion, string.Empty).Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public async Task<string> logingIn(string email_username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("game_id", GamesManager.GAME_ID);
        form.AddField("password", password);
        if (IsValidEmail(email_username))
        {
            form.AddField("email", email_username);
        }
        else
        {
            form.AddField("username", email_username.ToUpper());
        }
        string url = Endpoint.classesURL + "/authenticate";
        var response = await SeembaWebRequest.Get.HttpsPost(url, form);
        var N = JSON.Parse(response);
        var userData = JsonUtility.FromJson<UserData>(response);
        CurrentUser = userData.data;
        CurrentUser.token = N["token"].Value;
        LoaderManager.Get.LoaderController.ShowLoader(LoaderManager.LOADING);

        CurrentAvatarBytesString = await getAvatar(CurrentUser.avatar);
        var mTexture = await GetFlagBytes(await GetGeoLoc());
        CurrentFlagBytesString = Convert.ToBase64String(mTexture.EncodeToPNG());
        PlayerPrefs.SetString("CurrentFlagBytesString", CurrentFlagBytesString);
        LoaderManager.Get.LoaderController.HideLoader();

        if (N["success"].AsBool == true)
        {
            saveSessionToken(N["token"].Value);
            saveUserId(N["data"]["_id"].Value);
            CurrentUser._id = N["data"]["_id"].Value;
            CurrentUser.token = N["token"].Value;
            return "success";
        }
        else
        {
            return "failed";
        }
    }
    public void logingOut()
    {
        FreeBubblesController.nextRewardTime = DateTime.MinValue;
        deleteFile("Token.dat");
        deleteFile("User.dat");
        deleteFile("Msg.dat");
        PlayerPrefs.DeleteAll();
    }
    public async Task<User> getUser()
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        string url = Endpoint.classesURL + "/users/" + getCurrentUserId();
        var seembaResponse = await SeembaWebRequest.Get.HttpsGet(url);
        UserData userData = JsonUtility.FromJson<UserData>(seembaResponse);
        var token = getCurrentSessionToken();
        if (!userData.success)
        {
            if (userData.message == "Failed to authenticate token.")
            {
                ShowExpiredSession();
            }
            else if (userData.message == "Could not find user")
            {
                ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Signup.gameObject);
            }
            return null;
        }
        CurrentUser = userData.data;
        return CurrentUser;
    }
    void ShowExpiredSession()
    {
        LoaderManager.Get.LoaderController.HideLoader();
        PopupManager.Get.PopupController.ShowPopup(PopupType.SESSION_EXPIRED, PopupsText.Get.SessionExpired());
    }
    public async Task<int> send_emailAsync(string mail)
    {
        string url = Endpoint.classesURL + "/users/reset/password";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        WWWForm form = new WWWForm();
        form.AddField("email", mail);
        var response = await SeembaWebRequest.Get.HttpsPost(url, form);
        var N = JSON.Parse(response);
        Debug.Log(response);
        if (N["success"].AsBool == true)
        {
            return N["code"].AsInt;
        }
        else
        {
            return 0;
        }
    }
    public async Task<bool> addUserDeviceToken(string user_id, string game_id, string device_id, string platform)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", user_id);
        form.AddField("game_id", game_id);
        form.AddField("device_id", device_id);
        form.AddField("platform", platform);

        string url = Endpoint.classesURL + "/users/devices";
        await SeembaWebRequest.Get.HttpsPost(url, form);
        return true;
    }
    public async Task removeUserDeviceTokenAsync(string user_id, string gameId, string deviceToken)
    {
        string url = Endpoint.classesURL + "/users/devices";
        string json = "user_id=" + user_id + "&game_id=" + gameId + "&device_id=" + deviceToken;
        byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);
        var response = await SeembaWebRequest.Get.HttpsPut(url, jsonAsBytes);
        Debug.Log(response);
    }
    public async Task<bool> updatePassword(string email, string password)
    {
        string url = Endpoint.classesURL + "/users/update/password";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        WWWForm form = new WWWForm();

        form.AddField("email", email);
        form.AddField("password", password);
        var response = await SeembaWebRequest.Get.HttpsPost(url, form);
        var N = JSON.Parse(response);
        return N["success"].AsBool;
    }
    public async Task<bool> checkMailAsync(string mail)
    {
        string url = Endpoint.classesURL + "/users/check/email";
        WWWForm form = new WWWForm();
        form.AddField("email", mail);
        var response = await SeembaWebRequest.Get.HttpsPost(url, form);
        var N = JSON.Parse(response);
        return N["success"].AsBool;
    }
    public async Task<bool> checkUsernameAsync(string username)
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        string url = Endpoint.classesURL + "/users/check/username";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        var response = await SeembaWebRequest.Get.HttpsPost(url, form);
        var N = JSON.Parse(response);
        return N["success"].AsBool;
    }
    string getDataPath()
    {
        string path = "";
#if UNITY_ANDROID && !UNITY_EDITOR
		try {
			IntPtr obj_context = AndroidJNI.FindClass("android/content/ContextWrapper");
			IntPtr method_getFilesDir = AndroidJNIHelper.GetMethodID(obj_context, "getFilesDir", "()Ljava/io/File;");
			using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
					IntPtr file = AndroidJNI.CallObjectMethod(obj_Activity.GetRawObject(), method_getFilesDir, new jvalue[0]);
					IntPtr obj_file = AndroidJNI.FindClass("java/io/File");
					IntPtr method_getAbsolutePath = AndroidJNIHelper.GetMethodID(obj_file, "getAbsolutePath", "()Ljava/lang/String;");
					path = AndroidJNI.CallStringMethod(file, method_getAbsolutePath, new jvalue[0]);
					if(path != null) {
						////Debug.Log("Got internal path: " + path);
					}
					else {
						////Debug.Log("Using fallback path");
						path = "/data/data/com.DefaultCompany.sdkGame/files";
					}
				}
			}
			return path;
		}
		catch(Exception e) {
		////Debug.Log(e.ToString());
		return "";
		}
#else
        return Application.persistentDataPath;
#endif
    }
    public void writeStringToFile(string str, string filename)
    {
#if !WEB_BUILD
        string path = pathForDocumentsFile(filename);
        ////Debug.Log(path);
        FileStream file = new FileStream(path, System.IO.FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(file);
        sw.WriteLine(str);
        sw.Close();
        file.Close();
#endif
    }
    public string readStringFromFile(string filename)//, int lineIndex )
    {
#if !WEB_BUILD
        string path = pathForDocumentsFile(filename);
        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, System.IO.FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);
            string str = null;
            str = sr.ReadLine();
            sr.Close();
            file.Close();
            return str;
        }
        else
        {
            return null;
        }
#else
			return null;
#endif
    }
    public void deleteFile(string filename)//, int lineIndex )
    {
#if !WEB_BUILD
        string path = pathForDocumentsFile(filename);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
#endif
    }
    public string pathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            if (!Directory.Exists(Path.Combine(path, "Documents")))
            {
                Directory.CreateDirectory(Path.Combine(path, "Documents"));
            }
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }
    public void UpdateUserCredit(string money_credit, string bubble_credit)
    {
        CurrentUser.money_credit = float.Parse(money_credit);
        CurrentUser.bubble_credit = float.Parse(bubble_credit);
    }
    public void UpdateUserMoneyCredit(string moneyCredit)
    {
        CurrentUser.money_credit = float.Parse(moneyCredit);
    }
    public void UpdateUserBubblesCredit(string bubblesCredit)
    {
        CurrentUser.bubble_credit = float.Parse(bubblesCredit);
    }
    public string GetCurrentBubblesCredit()
    {
        return CurrentUser.bubble_credit.ToString();
    }
    public string GetCurrentMoneyCredit()
    {
        return CurrentUser.money_credit.ToString("N2");
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

    #region DeadCode
    //IEnumerator checkUser()
    //{
    //    if (CurrentUser != null)
    //    {
    //        try
    //        {
    //            if (GameObject.Find("reconnect").transform.localScale == Vector3.zero)
    //                GameObject.Find("reconnect").transform.localScale = Vector3.one;
    //        }
    //        catch (NullReferenceException ex)
    //        {
    //            LoaderManager.Get.LoaderController.ShowLoader(null);
    //        }
    //        yield return null;
    //    }
    //    else
    //    {
    //        LoaderManager.Get.LoaderController.HideLoader();
    //        //SceneManager.UnloadScene("Loader");
    //        CancelInvoke();
    //        yield return null;
    //    }
    //}
    #endregion
}
