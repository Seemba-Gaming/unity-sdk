using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[CLSCompliant(false)]
public class UserService : MonoBehaviour
{
    #region Static
    public static string Seemba_Email = "noreply@seemba.com,djo@seemba.com,slim@seemba.com,geoffrey@seemba.com,jean-philippe@seemba.com,mohamed@seemba.com";
    private static int sec, min;
    private static DateTime dt = new DateTime();
    #endregion

    #region Script Parameters
    public InputField digits;
    public Button SendEmail, Done, submitCode;
    public Button ShowPassword;
    public InputField Email, newPassword, confirmPassword;
    public InputField Username;
    public InputField Password;
    public Animator LoginAnimation;
    public Text chrono;
    #endregion

    #region Fields
    private int code;
    private Timer aTimer;
    private bool timeout;
    #endregion

    #region Unity Methods
    private void Start()
    {
        Username.ActivateInputField();
        Username.onValueChanged.AddListener(delegate
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

            if (string.IsNullOrEmpty(Password.text))
            {
                ShowPassword.transform.localScale = Vector3.zero;
            }
            else
            {
                ShowPassword.transform.localScale = Vector3.one;
            }
        });

        SendEmail.onClick.AddListener(async () =>
        {
            await requestForResetPasswordAsync();
        });

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
                {
                    submitCode.interactable = true;
                }
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

    private void Update()
    {
        try
        {
            if (int.Parse(digits.text) == code && timeout == false)
            {
                submitCode.interactable = true;
            }
            else
            {
                submitCode.interactable = false;
            }
        }
        catch (NullReferenceException)
        {
        }
        catch (FormatException)
        {
            if (string.IsNullOrEmpty(digits.text))
            {
                submitCode.interactable = false;
            }
        }
    }
    #endregion

    #region Methods
    public async void Logout()
    {
        var userId = UserManager.Get.getCurrentUserId();
        var deviceToken = PlayerPrefs.GetString("DeviceToken");
        LoaderManager.Get.LoaderController.ShowLoader(null);
        await UserManager.Get.removeUserDeviceTokenAsync(userId, GamesManager.GAME_ID, deviceToken);
        LoaderManager.Get.LoaderController.HideLoader();
        UserManager.Get.logingOut();
        SceneManager.LoadSceneAsync("SeembaEsports");
    }
    #endregion

    #region Implementation
    private async System.Threading.Tasks.Task requestForResetPasswordAsync()
    {
        digits.text = "";
        LoaderManager.Get.LoaderController.ShowLoader(null);
        int res = await UserManager.Get.send_emailAsync(Email.text);
        LoaderManager.Get.LoaderController.HideLoader();
        if (res == 0)
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_EMAIL_NOT_FOUND, PopupsText.Get.EmailNotFound());
        }
        else
        {
            if (res == -1)
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());
            }
            else
            {
                digits.ActivateInputField();
                code = res;
                dt.AddMinutes(2).ToString("mm:ss");
                chrono.text = "02:00";
                sec = 59;
                min = 1;
                timeout = false;
                if (aTimer != null)
                {
                    aTimer.Stop();
                }

                aTimer = null;
                aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                aTimer.Interval = 1000;
                aTimer.Enabled = true;
            }
        }
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
                    {
                        chrono.text = "0" + min + ":" + sec;
                    }
                    else
                    {
                        chrono.text = "0" + min + ":0" + sec;
                    }

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
            catch (MissingReferenceException)
            {
            }
        });
    }
    private async void resetPassword()
    {
        LoaderManager.Get.LoaderController.ShowLoader(null);
        bool res = await UserManager.Get.updatePassword(Email.text, newPassword.text);
        LoaderManager.Get.LoaderController.HideLoader();

        if (res == false)
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());
        }
        else
        {
            //what ?
        }
    }
    private bool IsValidEmail(string email)
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
    #endregion

}
