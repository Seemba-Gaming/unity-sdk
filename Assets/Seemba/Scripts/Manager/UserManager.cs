using RestSharp;
using SimpleJSON;
using System;
using System.Collections;
using System.IO;
//using System.Threading.Tasks;
//using System.Net.Http;
//using Parse.Common.Internal;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class UserManager : MonoBehaviour
{
    public static string CurrentUsername, UserId, userToken, UserEmail;
    string longLat;
    public static string CurrentMoney;
    public static string CurrentWater;
    public static User CurrentUser;
    public static string CurrentCountryCode;
    public static string CurrentCountryRegion;
    public static string currentBirthdate;
    public static Sprite CurrentAvatarBytesString;
    public static string CurrentAvatarURL;
    public static string CurrentFlagBytesString;
    public static string CurrentFlagPath;
    public static bool CurrentProLabel;
    public static float CurrentNiveau;
    public static string CurrentHipayOrderId;
    public static float niveau = 0f;
    // Use this for initialization
    void OnEnable()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator checkUser()
    {
        if (string.IsNullOrEmpty(CurrentMoney))
        {
            try
            {
                if (GameObject.Find("reconnect").transform.localScale == Vector3.zero)
                    GameObject.Find("reconnect").transform.localScale = Vector3.one;
            }
            catch (NullReferenceException ex)
            {
                SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
            }
            yield return null;
        }
        else
        {
            SceneManager.UnloadScene("Loader");
            CancelInvoke();
            yield return null;
        }
    }
    public void bubblesTrack(string userid, string token, string gameId, string type)
    {
        string url = Endpoint.classesURL + "/users/track/" + userid;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Timeout = 5000;
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        try
        {
            using (var stream = request.GetRequestStream())
            {
                string json = "type=" + type + "&game=" + gameId;
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
                }
            }
        }
        catch (WebException ex) { }
    }
    public void UpdateUserByField(string userId, string token, string[] fieldName, string[] value)
    {
        string url = Endpoint.classesURL + "/users/" + userId;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PUT";
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
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
                    //Debug.Log (jsonResponse);
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
                        //Debug.Log (error);
                    }
                }
            }
        }
    }
    public string GetFlagByte(string country_code)
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(country_code)))
        {
            return PlayerPrefs.GetString(country_code);
        }
        else
        {
            //string url = "https://96c214a1.ngrok.io/flags/" + country_code + ".png";
            string url = "https://seemba-api.herokuapp.com/flags/" + country_code + ".png";
            HttpWebRequest lxRequest = (HttpWebRequest)WebRequest.Create(url);
            // returned values are returned as a stream, then read into a string
            String lsResponse = string.Empty;
            using (HttpWebResponse lxResponse = (HttpWebResponse)lxRequest.GetResponse())
            {
                using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                {
                    Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    string result = Convert.ToBase64String(lnByte);
                    PlayerPrefs.SetString(country_code, result);
                    return result;
                }
            }
        }
    }
    public string GetGeoLoc()
    {
        string url = Endpoint.locationURL;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        try
        {
            using (System.IO.Stream s = request.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    var N = JSON.Parse(jsonResponse);
                    longLat = N["loc"].Value;
                    CurrentCountryRegion = N["region"].Value.ToLower();
                    return N["country"].Value.ToLower();
                }
            }
        }
        catch (WebException ex)
        {
            return null;
        }
    }
    public string GetGeoLocRegion()
    {
        string url = Endpoint.locationURL;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        try
        {
            using (System.IO.Stream s = request.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    var N = JSON.Parse(jsonResponse);
                    return N["region"].Value.ToLower();
                }
            }
        }
        catch (WebException ex)
        {
            return null;
        }
    }
    public void updateAvatar(string userId, string token, byte[] avatar)
    {
        var client = new RestClient(Endpoint.classesURL + "/users/" + userId);
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        var request = new RestRequest(Method.PUT);
        request.AddHeader("x-access-token", token);
        request.AddFile("avatar", avatar, "avatar");
        bool cmp = false;
        string res = null;
        client.ExecuteAsync(request, response =>
        {
            //Debug.Log(response.Content);
            res = response.Content;
            cmp = true;
        });
        if (!string.IsNullOrEmpty(res))
        {
            var N = JSON.Parse(res);
            if (N["success"].AsBool == true)
            {
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    UserManager.CurrentAvatarBytesString = ImagesManager.getSpriteFromBytes(avatar);
                });
            }
        }
    }
    public IEnumerator SaveDataAfterSignup()
    {
        //Debug.Log (UserService._Username);
        CoroutineWithData cd = new CoroutineWithData(this, signingUp(UserService._Username, UserService._Email, UserService._Password, UserService._Avatar));
        yield return cd.coroutine;
        if (int.Parse(cd.result.ToString()) == 0)
        {
            UserService.statusCode = 0;
            yield return 0;
        }
        else
        {
            UserService.statusCode = 400;
            yield return 400;
        }
        //Debug.Log (cd.result);
    }
    //signup with the new api
    public IEnumerator signingUp(string username, string email, string password, string avatar)
    {
        WithdrawManager wm = new WithdrawManager();
        Debug.Log("username: " + username);
        Debug.Log("email: " + email);
        Debug.Log("password: " + password);
        Debug.Log("country_code: " + GetGeoLoc());
        Debug.Log("longLat: " + longLat);
        Debug.Log("avatar: " + avatar);
        Debug.Log("game_id: " + GamesManager.GAME_ID);
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("country_code", GetGeoLoc());
        form.AddField("long_lat", longLat);
        form.AddField("game_id", GamesManager.GAME_ID);
        form.AddField("avatar", avatar);
        var download = UnityWebRequest.Post(Endpoint.classesURL + "/users", form);
        download.timeout = 4000;
        yield return download.Send();
        //Debug.Log (download.downloadHandler.text);
        if (download.isNetworkError)
        {
            print("Error downloading: " + download.error);
            UserService.statusCode = 400;
            try
            {
                SceneManager.UnloadSceneAsync("Loader");
            }
            catch (ArgumentException ex)
            {
            }
            yield return 400;
        }
        else
        {
            if (download.responseCode == 200)
            {
                Debug.Log(download.downloadHandler.text);
                var N = JSON.Parse(download.downloadHandler.text);
                //Attach Info to Stripe Account
                wm.attachBusinessProfileToAccount(N["token"].Value);
                wm.attachInfoToAccount(N["token"].Value, N["data"]["email"].Value, "email");
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    Debug.Log(download.downloadHandler.text);
                    //Save The current Session ID
                    saveUserId(N["data"]["_id"].Value);
                    //Save Session Token
                    saveSessionToken(N["token"].Value);
                    UserService.statusCode = 0;
                });
                yield return 0;
            }
            else
            {
                UserService.statusCode = 400;
                yield return 400;
            }
        }
    }
    public Byte[] getAvatar(string url)
    {
        Byte[] lnByte = null;
        string prefsURL = null;
        var Getingtask = UnityThreadHelper.Dispatcher.Dispatch(() =>
        {
            prefsURL = PlayerPrefs.GetString(url);
        });
        Getingtask.Wait();
        if (!string.IsNullOrEmpty(prefsURL))
        {

            lnByte = System.Convert.FromBase64String(prefsURL);
            return lnByte;
        }
        else
        {
            Debug.Log("No Avatar in prefs");
            HttpWebRequest lxRequest = (HttpWebRequest)WebRequest.Create(url);
            // returned values are returned as a stream, then read into a string
            String lsResponse = string.Empty;
            using (HttpWebResponse lxResponse = (HttpWebResponse)lxRequest.GetResponse())
            {
                using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                {
                    lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        PlayerPrefs.SetString(url, System.Convert.ToBase64String(lnByte));
                    });
                    //return ImagesManager.getSpriteFromBytes (lnByte);
                    return lnByte;
                }
            }
        }
    }
    public Byte[] OldGetAvatar(string url)
    {
        Byte[] lnByte = null;
        HttpWebRequest lxRequest = (HttpWebRequest)WebRequest.Create(url);
        // returned values are returned as a stream, then read into a string
        String lsResponse = string.Empty;
        using (HttpWebResponse lxResponse = (HttpWebResponse)lxRequest.GetResponse())
        {
            using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
            {
                lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                //return ImagesManager.getSpriteFromBytes (lnByte);
                return lnByte;
            }
        }
    }
    public string getData(string url, string username, string password)
    {
        try
        {
            var request = System.Net.WebRequest.Create(url);
            if (request != null)
            {
                request.Method = "GET";
                request.Timeout = 12000;
                request.ContentType = @"application/json";
                string authInfo = username + ":" + password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                //like this:
                request.Headers["Authorization"] = "Basic " + authInfo;
                request.Headers["X-Parse-Application-Id"] = "seembaapi";
                using (System.IO.Stream s = request.GetResponse().GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        //Console.WriteLine(String.Format("Response: {0}", jsonResponse));
                        //return String.Format("Response: {0}", jsonResponse);
                        return jsonResponse;
                    }
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return (ex.ToString());
        }
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
    public string logingIn(string email, string password)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/authenticate";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        JSONNode N;
        request.Method = "POST";
        request.Timeout = 5000;
        request.ContentType = "application/x-www-form-urlencoded";
        string json;
        try
        {
            using (var stream = request.GetRequestStream())
            {
                if (IsValidEmail(email))
                {
                    json = "email=" + email + "&password=" + password + "&game_id=" + GamesManager.GAME_ID;
                }
                else
                {
                    json = "username=" + email.ToUpper() + "&password=" + password + "&game_id=" + GamesManager.GAME_ID;
                }
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
                    N = JSON.Parse(jsonResponse);
                    if (N["success"].AsBool == true)
                    {
                        saveSessionToken(N["token"].Value);
                        saveUserId(N["user"]["_id"].Value);
                        UserManager.CurrentUsername = N["user"]["username"].Value;
                        UserService.updateCredit(N["user"]["money_credit"].Value, N["user"]["bubble_credit"].Value);
                        Byte[] lnByte = um.getAvatar(N["user"]["avatar"].Value);

                        UnityThreadHelper.Dispatcher.Dispatch(() =>
                        {
                            UserManager.CurrentFlagBytesString = um.GetFlagByte(N["user"]["country_code"].Value);
                            PlayerPrefs.SetString("CurrentFlagBytesString", UserManager.CurrentFlagBytesString);
                            UserManager.CurrentCountryCode = N["user"]["country_code"].Value;
                            UserManager.CurrentAvatarBytesString = ImagesManager.getSpriteFromBytes(lnByte);
                        });
                        //Debug.Log("login success");
                        return "success";
                    }
                    else
                    {
                        //Debug.Log("login failed");
                        return "failed";
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
                        N = JSON.Parse(error);
                        if (N["success"].AsBool == false)
                        {
                            return "failed";
                        }
                        else
                        {
                            return "error";
                        }
                        //Debug.Log (error);
                    }
                }
            }
            else
            {
                return "error";
            }
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
    public User getUser(string id, string token)
    {
        string lastname, firstname, bubble_click, account_id, city, country_code, state, birthday, adress, country, personal_id_number;
        float max_withdraw;
        string zipcode;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        string url = Endpoint.classesURL + "/users/" + id;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        try
        {
            using (System.IO.Stream s = request.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();

                    var N = JSON.Parse(jsonResponse);
                    if (N["success"].AsBool == true)
                    {
                        try
                        {
                            lastname = N["data"]["lastname"].Value;
                        }
                        catch (NullReferenceException nre)
                        {
                            lastname = null;
                        }
                        try
                        {
                            firstname = N["data"]["firstname"].Value;
                        }
                        catch (NullReferenceException nre)
                        {
                            firstname = null;
                        }
                        try
                        {
                            bubble_click = N["data"]["last_bubble_click"].Value;
                        }
                        catch (NullReferenceException nre)
                        {
                            bubble_click = null;
                        }
                        try
                        {
                            account_id = N["data"]["payment_account_id"].Value;
                        }
                        catch (NullReferenceException nre)
                        {
                            account_id = null;
                        }
                        try
                        {
                            city = N["data"]["city"].Value;
                        }
                        catch (NullReferenceException nre)
                        {
                            city = null;
                        }
                        try
                        {
                            country_code = N["data"]["country_code"].Value;
                        }
                        catch (NullReferenceException nre)
                        {
                            country_code = null;
                        }
                        try
                        {
                            state = N["data"]["state"].Value;
                        }
                        catch (NullReferenceException nre)
                        {
                            state = null;
                        }
                        try
                        {
                            max_withdraw = N["data"]["max_withdraw"].AsFloat;
                        }
                        catch (NullReferenceException nre)
                        {
                            max_withdraw = 0;
                        }
                        try
                        {
                            zipcode = N["data"]["zipcode"].Value;
                        }
                        catch (NullReferenceException nre)
                        {
                            zipcode = "";
                        }
                        try
                        {
                            birthday = N["data"]["birthdate"].Value;
                        }
                        catch (NullReferenceException nre)
                        {
                            birthday = null;
                        }
                        try
                        {
                            adress = N["data"]["address"].Value;
                        }
                        catch (NullReferenceException nre)
                        {
                            adress = null;
                        }
                        try
                        {
                            country = N["data"]["country"].Value;
                        }
                        catch (NullReferenceException nre)
                        {
                            country = null;
                        }
                        try
                        {
                            personal_id_number = N["data"]["personal_id_number"].Value;
                        }
                        catch (NullReferenceException nre)
                        {
                            personal_id_number = null;
                        }
                        User user = new User(N["data"]["_id"].Value, N["data"]["username"].Value, N["data"]["avatar"].Value,
                            N["data"]["username_changed"].AsBool, personal_id_number, lastname, firstname, N["data"]["money_credit"].AsFloat,
                                        N["data"]["bubble_credit"].AsFloat, N["data"]["email"].Value, N["data"]["password"].Value,
                                        N["data"]["amateur_bubble"].AsInt, N["data"]["novice_bubble"].AsInt, N["data"]["legend_bubble"].AsInt,
                                        N["data"]["confident_bubble"].AsInt, N["data"]["confirmed_bubble"].AsInt, N["data"]["champion_bubble"].AsInt,
                                        N["data"]["amateur_money"].AsInt, N["data"]["novice_money"].AsInt, N["data"]["legend_money"].AsInt,
                                        N["data"]["confident_money"].AsInt, N["data"]["confirmed_money"].AsInt, N["data"]["champion_money"].AsInt,
                                        N["data"]["losses_streak"].AsInt, N["data"]["victories_streak"].AsInt, N["data"]["long_lat"].Value,
                                        bubble_click, N["data"]["email_verified"].AsBool, N["data"]["iban_uploaded"].AsBool,
                                        N["data"]["level"].AsInt, account_id, N["data"]["id_proof_1_uploaded"].AsBool,
                                        N["data"]["id_proof_2_uploaded"].AsBool, city, country_code,
                                        state, max_withdraw, zipcode,
                                        N["data"]["passport_uploaded"].AsBool, N["data"]["last_result"].Value, birthday,
                                        adress, country, N["data"]["residency_proof_uploaded"].AsBool,
                                        N["data"]["victories_count"].AsInt, N["data"]["phone"].Value);
                        CurrentUser = user;
                        return user;
                    }
                    else
                    {
                        if (N["message"].Value == "Failed to authenticate token.")
                        {
                            ShowExpiredSession();
                            return new User();
                        }
                        else if (N["message"].Value == "Could not find user")
                        {
                            SceneManager.LoadSceneAsync("Signup");
                            return new User();
                        }
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
    void ShowExpiredSession()
    {
        UnityThreadHelper.Dispatcher.Dispatch(() =>
        {
            EventsController nbs = new EventsController();
            SceneManager.UnloadSceneAsync("Loader");
            SceneManager.LoadScene("SessionExpired", LoadSceneMode.Additive);
            UnityThreadHelper.CreateThread(() =>
            {
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    nbs.ShowPopupError("popupSessionExpired");
                });
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    InvokeRepeating("UnloadConnection", 0f, 0.1f);
                });
            });
        });
    }
    void UnloadConnection()
    {
        try
        {
            SceneManager.UnloadSceneAsync("ConnectionFailed");
        }
        catch (ArgumentException ex)
        {
        }
    }
    public int send_email(string mail)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/users/reset/password";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Timeout = 5000;
        request.ContentType = "application/x-www-form-urlencoded";
        try
        {
            using (var stream = request.GetRequestStream())
            {
                string json = "email=" + mail;
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
                    Debug.Log(jsonResponse);
                    if (N["success"].AsBool == true)
                    {
                        return N["code"].AsInt;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        catch (WebException ex)
        {
            //Debug.Log(ex);
            if (ex.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        //Debug.Log(error);
                        var N = JSON.Parse(error);
                        if (N["success"].AsBool == false)
                        {
                            return 0;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
            else
                return -1;
        }
    }
    public void addUserDeviceToken(string user_id, string gameId, string deviceToken, string platform)
    {
        Debug.Log("addUserDeviceToken BEGIN");
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/users/devices";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
        {
            string json = "user_id=" + user_id + "&game_id=" + gameId + "&device_id=" + deviceToken + "&platform=" + platform;
            Debug.Log("jsonDevice: " + json);
            Debug.Log("deviceToken: " + deviceToken);
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
        }
    }
    public void removeUserDeviceToken(string user_id, string gameId, string deviceToken)
    {
        Debug.Log("removeUserDeviceToken BEGIN");
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/users/devices";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PUT";
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
        {
            string json = "user_id=" + user_id + "&game_id=" + gameId + "&device_id=" + deviceToken;
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
        }
    }
    public bool? updatePassword(string mail, string password)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/users/update/password";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Timeout = 5000;
        request.ContentType = "application/x-www-form-urlencoded";
        try
        {
            using (var stream = request.GetRequestStream())
            {
                string json = "email=" + mail + "&password=" + password;
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
                    //Debug.Log(jsonResponse);
                    return N["success"].AsBool;
                }
            }
        }
        catch (WebException ex)
        {
            //Debug.Log(ex);
            if (ex.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        //Debug.Log(error);
                        var N = JSON.Parse(error);
                        if (N["success"].AsBool == false)
                        {
                            return false;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            else
                return null;
        }
    }
    public bool checkMail(string mail)
    {
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/users/check/email";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
        {
            string json = "email=" + mail;
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
                    var N = JSON.Parse(jsonResponse);
                    return N["success"].AsBool;
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
                        //Debug.Log (error);
                    }
                }
            }
            return false;
        }
    }
    public bool checkUsername(string username)
    {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/users/check/username";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
        {
            string json = "username=" + username;
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
                    var N = JSON.Parse(jsonResponse);
                    return N["success"].AsBool;
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
                        //Debug.Log (error);
                    }
                }
            }
            return false;
        }
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

    public static void updateUserCredit(String money_credit, String bubble_credit)
    {
        UserManager.CurrentMoney = money_credit;
        UserManager.CurrentWater = bubble_credit;
    }
}
