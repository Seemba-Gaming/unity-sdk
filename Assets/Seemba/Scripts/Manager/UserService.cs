using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UserService : MonoBehaviour
{
    public static string Seemba_Email = "noreply@seemba.com,djo@seemba.com,slim@seemba.com,geoffrey@seemba.com,jean-philippe@seemba.com,mohamed@seemba.com";
    public static InputField username, password, mail;
    public static string _Username, _Password, _Email, _Avatar;
    private int code;
    AndroidJavaObject currentActivity;
    public static int? statusCode;
    string toastString;
    public Button Signin, signup, SendEmail, Done, submitCode;
    public InputField digits, Email, newPassword, confirmPassword;
    System.Timers.Timer aTimer;
    static int sec, min;
    static DateTime dt = new DateTime();
    TimeSpan ts;
    public Text chrono;
    bool timeout;
    // Use this for initialization
    void Start()
    {
        try
        {
            InputField username = GameObject.Find("username").GetComponent<InputField>();
            InputField Password = GameObject.Find("password").GetComponent<InputField>();
            Animator LoginAnimation = GameObject.Find("SigninContent").GetComponent<Animator>();
            username.ActivateInputField();
            username.onValueChanged.AddListener(delegate
            {
                if (LoginAnimation.GetBool("loginFailed") == true)
                {
                    LoginAnimation.SetBool("loginFailed", false);
                }
            });
            Password.onValueChanged.AddListener(delegate
            {
                if (LoginAnimation.GetBool("loginFailed") == true)
                {
                    LoginAnimation.SetBool("loginFailed", false);
                }
                Button showPassword = GameObject.Find("showPassword").GetComponent<Button>();
                if (string.IsNullOrEmpty(Password.text))
                {
                    showPassword.transform.localScale = Vector3.zero;
                }
                else
                {
                    showPassword.transform.localScale = Vector3.one;
                }
            });
        }
        catch (NullReferenceException ex)
        {
        }
        try
        {
            SendEmail.onClick.AddListener(() =>
            {
                requestForResetPassword();
            });
        }
        catch (NullReferenceException ex) { }
        try
        {
            Signin.onClick.AddListener(() =>
            {
                Login();
            });
        }
        catch (NullReferenceException ex) { }
        try
        {
            signup.onClick.AddListener(() =>
            {
                Signup();
            });
        }
        catch (NullReferenceException ex) { }
        try
        {
            Email.onValueChanged.AddListener(delegate
            {
                if (IsValidEmail(Email.text))
                {
                    SendEmail.interactable = true;
                }
                else
                {
                    SendEmail.interactable = false;
                }
            });
            digits.onValueChanged.AddListener(delegate
            {
                if (digits.text.Length == 4)
                {
                    if (int.Parse(digits.text) == code)
                        submitCode.interactable = true;
                    else
                    {
                        submitCode.interactable = false;
                    }
                }
            });
            confirmPassword.onValueChanged.AddListener(delegate
            {
                if (confirmPassword.text.Equals(newPassword.text))
                {
                    Done.interactable = true;
                }
            });
            Done.onClick.AddListener(() =>
            {
                resetPassword();
            });
        }
        catch (NullReferenceException ex)
        {
        }
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            if (int.Parse(digits.text) == code && timeout == false)
                submitCode.interactable = true;
            else
            {
                submitCode.interactable = false;
            }
        }
        catch (NullReferenceException ex)
        {
        }
        catch (FormatException ex)
        {
            if (string.IsNullOrEmpty(digits.text))
            {
                submitCode.interactable = false;
            }
        }
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
    public void Logout()
    {
        UserManager um = new UserManager();
        var userId = um.getCurrentUserId();
        var deviceToken = PlayerPrefs.GetString("DeviceToken");
        var platform = "";
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        UnityThreadHelper.CreateThread(() =>
        {
            //remove Device Token
            um.removeUserDeviceToken(userId, GamesManager.GAME_ID, deviceToken);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadSceneAsync("Loader");
                um.logingOut();
                SceneManager.LoadSceneAsync("Signup");
            });
        });
    }
    void showLoginFailedAnimation()
    {
        GameObject.Find("SigninContent").GetComponent<Animator>().SetBool("loginFailed", true);
    }
    void showEmailNotfound()
    {
        EventsController nbs = new EventsController();
        nbs.ShowPopupError("popupEmailNotFound");
    }
    void showconnectionFailed()
    {
        EventsController nbs = new EventsController();
        nbs.ShowPopupError("popupConnectionFailed");
    }
    private void requestForResetPassword()
    {
        digits.text = "";
        UserManager um = new UserManager();
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        UnityThreadHelper.CreateThread(() =>
        {
            int res = um.send_email(Email.text);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadSceneAsync("Loader");
                if (res == 0)
                {
                    showEmailNotfound();
                }
                else
                {
                    if (res == -1)
                    {
                        showconnectionFailed();
                    }
                    else
                    {
                        digits.ActivateInputField();
                        code = res;
                        //Debug.Log(code);
                        dt.AddMinutes(2).ToString("mm:ss");
                        chrono.text = "02:00";
                        sec = 59;
                        min = 1;
                        timeout = false;
                        if (aTimer != null) aTimer.Stop();
                        aTimer = null;
                        aTimer = new System.Timers.Timer();
                        aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                        aTimer.Interval = 1000;
                        aTimer.Enabled = true;
                    }
                }
            });
        });
    }
    public void gotItEmailNotFound()
    {
        EventsController nbs = new EventsController();
        nbs.HidePopupError("popupEmailNotFound", true);
    }
    private void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        UnityThreadHelper.Dispatcher.Dispatch(() =>
        {
            try
            {
                if (sec >= 0 && min >= 0)
                {
                    if (sec >= 10)
                        chrono.text = "0" + min + ":" + sec;
                    else
                        chrono.text = "0" + min + ":0" + sec;
                    sec--;
                    if (sec < 0)
                    {
                        sec = 59;
                        min--;
                    }
                    if (sec < 0 && min < 0)
                    {
                        aTimer.Stop();
                        chrono.text = "00:00";
                        timeout = true;
                    }
                }
                else
                {
                    timeout = true;
                }
            }
            catch (MissingReferenceException ex)
            {
            }
        });
    }
    private void resetPassword()
    {
        UserManager um = new UserManager();
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        UnityThreadHelper.CreateThread(() =>
        {
            bool? res = um.updatePassword(Email.text, newPassword.text);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                SceneManager.UnloadSceneAsync("Loader");
                if (res == false)
                {
                    showconnectionFailed();
                }
                else
                {
                    if (res == null)
                    {
                        showconnectionFailed();
                    }
                    else
                    {
                    }
                }
            });
        });
    }
    public IEnumerator checkInternetConnection(Action<bool?> action)
    {
        AsyncOperation asc = SceneManager.LoadSceneAsync("Loader", LoadSceneMode.Additive);
        WWW www = new WWW("https://www.google.fr");
        float timer = 0;
        bool failed = false;
        while (!www.isDone)
        {
            try
            {
                GameObject.Find("checkConnection").transform.localScale = Vector3.one;
            }
            catch (NullReferenceException ex)
            {
            }
            if (timer > 5) { failed = true; break; }
            timer += Time.deltaTime;
            yield return null;
        }
        Debug.Log("failed" + failed);
        if (failed)
        {
            try
            {
                GameObject.Find("checkConnection").transform.localScale = Vector3.one;
                SceneManager.UnloadSceneAsync("Loader");
            }
            catch (NullReferenceException ex)
            {
            }
            catch (ArgumentException ex)
            {
            }
            Debug.Log("failed");
            www.Dispose();
            action(false);
        }
        else
        {
            Debug.Log("success");
            if (www.error == null)
            {
                SceneManager.UnloadSceneAsync("Loader");
                action(true);
            }
            else
            {
                Debug.Log("error not null");
                if (!asc.isDone)
                {
                    yield return new WaitForSeconds(1f);
                }
                SceneManager.UnloadSceneAsync("Loader");
                action(false);
            }
        }
    }
    public void Signup()
    {
        EventsController nbs = new EventsController();
        UserManager um = new UserManager();
        StartCoroutine(checkInternetConnection((isConnected) =>
        {
            if (isConnected == true)
            {
                InputField Username = GameObject.Find("UsernameField").GetComponent<InputField>();
                InputField Email = GameObject.Find("EmailField").GetComponent<InputField>();
                InputField Password = GameObject.Find("PasswordField").GetComponent<InputField>();
                Image Avatar = GameObject.Find("Avatar").GetComponent<Image>();
                Texture2D mytexture = Avatar.sprite.texture;
                byte[] bytes;
                bytes = mytexture.EncodeToPNG();
                GameObject.Find("Loader").transform.localScale = Vector3.one;
                string userID = null;
                statusCode = null;
                _Username = Username.text.ToUpper();
                _Email = Email.text;
                _Password = Password.text;
                //_Avatar = bytes;
                UnityThreadHelper.CreateThread(() =>
                {
                    if (string.IsNullOrEmpty(ImagesManager.AvatarURL))
                    {
                        UnityThreadHelper.Dispatcher.Dispatch(() =>
                        {
                            StartCoroutine(ImagesManager.FixImage(bytes));
                        });
                        while (string.IsNullOrEmpty(ImagesManager.AvatarURL)) {/*Waiting Callback*/}
                    }
                    if (ImagesManager.AvatarURL != "error")
                    {
                        _Avatar = ImagesManager.AvatarURL;
                        UnityThreadHelper.Dispatcher.Dispatch(() =>
                        {
                            StartCoroutine(um.signingUp(Username.text.ToUpper(), Email.text, Password.text, ImagesManager.AvatarURL));
                        });
                        while (statusCode == null) { }
                        Debug.Log(statusCode);
                        UnityThreadHelper.Dispatcher.Dispatch(() =>
                        {
                            GameObject.Find("Loader").transform.localScale = Vector3.zero;
                            if (statusCode == 0)
                            {
                                var deviceToken = PlayerPrefs.GetString("DeviceToken");
                                Debug.Log(deviceToken);
                                var userid = um.getCurrentUserId();
                                var platform = "";
                                if (Application.platform == RuntimePlatform.Android)
                                    platform = "android";
                                else platform = "ios";
                                UnityThreadHelper.CreateThread(() =>
                                {
                                    //Add Device Token To Receive notification on current device
                                    um.addUserDeviceToken(userid, GamesManager.GAME_ID, deviceToken, platform);
                                });
                                userID = um.getCurrentUserId();
                                //um.UpdateUserAvatar (userID,Convert.ToBase64String(bytes));
                                UserManager.CurrentUsername = Username.text;
                                UserManager.CurrentAvatarBytesString = ImagesManager.getSpriteFromBytes(bytes);
                                UserManager.CurrentFlagBytesString = um.GetFlagByte(um.GetGeoLoc());
                                UserManager.CurrentMoney = "0.00";
                                UserManager.CurrentWater = "25";
                                //Save Flag Byte
                                PlayerPrefs.SetString("CurrentFlagBytesString", UserManager.CurrentFlagBytesString);
                                //SceneManager.LoadScene ("Home");
                                ChallengeManager.CurrentChallengeGain = "2";
                                ChallengeManager.CurrentChallengeGainType = ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES;
                                nbs.startFirstChallenge();
                            }
                            else if (statusCode == 400)
                            {
                                //Text ErrorMessage = GameObject.Find ("ErrorMessage").GetComponent<Text> ();
                                //SceneManager.LoadScene ("ConnectionFailed", LoadSceneMode.Additive);
                                ConnectivityController.CURRENT_ACTION = "";
                                SceneManager.LoadScene("ConnectionFailed", LoadSceneMode.Additive);
                                GameObject.Find("Loader").transform.localScale = Vector3.zero;
                            }
                        });
                    }
                    else
                    {
                        UnityThreadHelper.Dispatcher.Dispatch(() =>
                        {
                            ConnectivityController.CURRENT_ACTION = "";
                            GameObject.Find("Loader").transform.localScale = Vector3.zero;
                            SceneManager.LoadScene("ConnectionFailed", LoadSceneMode.Additive);
                        });
                    }
                });
            }
            else
            {
                ConnectivityController.CURRENT_ACTION = "";
                GameObject.Find("Loader").transform.localScale = Vector3.zero;
                SceneManager.LoadScene("ConnectionFailed", LoadSceneMode.Additive);
            }
        }));
    }
    public static void updateCredit(string money_credit, string bubblesCredit)
    {
        if (!string.IsNullOrEmpty(money_credit))
            UserManager.CurrentMoney = money_credit;
        if (!string.IsNullOrEmpty(bubblesCredit))
            UserManager.CurrentWater = bubblesCredit;
    }
    public void Login()
    {
        UserManager um = new UserManager();
        mail = GameObject.Find("username").GetComponent<InputField>();
        username = GameObject.Find("username").GetComponent<InputField>();
        password = GameObject.Find("password").GetComponent<InputField>();
        try
        {
            SceneManager.UnloadSceneAsync("ConnectionFailed");
        }
        catch (ArgumentException ex) { }
        UnityThreading.ActionThread thread, thread1;

        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);

        var deviceToken = PlayerPrefs.GetString("DeviceToken");
        Debug.Log(deviceToken);
        thread = UnityThreadHelper.CreateThread(() =>
        {
            string res = um.logingIn(username.text, password.text);
            if (res == "success")
            {
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    string userId = um.getCurrentUserId();
                    string userToken = um.getCurrentSessionToken();
                    var platform = "";
                    UnityThreadHelper.CreateThread(() =>
                    {
                        if (Application.platform == RuntimePlatform.Android)
                            platform = "android";
                        else platform = "ios";
                        //Add Device Token To Receive notification on current device
                        um.addUserDeviceToken(userId, GamesManager.GAME_ID, deviceToken, platform);
                        UnityThreadHelper.Dispatcher.Dispatch(() =>
                        {
                            SceneManager.LoadSceneAsync("Home");
                        });
                    });
                });
            }
            else if (res == "failed")
            {
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    // track Signin Failed
                    print("auth failed");
                    //					mail.GetComponent<Image>().color=new Color(1f,122/255f,122/255f);
                    //					password.GetComponent<Image>().color=new Color(1f,122/255f,122/255f);
                    //	GameAnalytics.NewErrorEvent (GAErrorSeverity.Error, "SignINFailed:InvalidUsername/EmailOrPassword");
                    GameObject.Find("Loader").transform.localScale = Vector3.zero;
                    showLoginFailedAnimation();
                });
                //}
            }
            else
            {
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    ConnectivityController.CURRENT_ACTION = ConnectivityController.LOGIN_ACTION;
                    GameObject.Find("Loader").transform.localScale = Vector3.zero;
                    SceneManager.LoadScene("ConnectionFailed", LoadSceneMode.Additive);
                });
            }
        });
    }
}
