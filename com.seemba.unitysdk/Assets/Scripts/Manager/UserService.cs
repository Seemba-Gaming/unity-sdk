using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class UserService : MonoBehaviour
    {
        #region Static
        public static string Seemba_Email = "noreply@seemba.com,djo@seemba.com,slim@seemba.com,geoffrey@seemba.com,jean-philippe@seemba.com,mohamed@seemba.com";
        public static UserService Get { get { return sInstance; } }
        private static UserService sInstance;
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
        public GameObject SignIn;
        public GameObject ResetPasswordEmail;
        public GameObject ResetPasswordCode;
        public GameObject ResetPasswordNewPassword;
        #endregion

        #region Fields
        private int code;
        private bool timeout;
        private float timeRemaining = 120f;
        private string minutes;
        private string seconds;
        private int remainingSeconds;
        private int remainingMinutes;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            sInstance = this;
        }
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
            catch (NullReferenceException ex)
            {
                Debug.LogWarning(ex.Message);
            }
            catch (FormatException)
            {
                if (string.IsNullOrEmpty(digits.text))
                {
                    submitCode.interactable = false;
                }
            }
            if (ResetPasswordCode.activeSelf)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    remainingMinutes = Mathf.FloorToInt(timeRemaining % 60);

                    if (remainingMinutes < 10)
                    {
                        minutes = "0" + remainingMinutes;
                    }
                    else
                    {
                        minutes = remainingMinutes.ToString();
                    }
                    remainingSeconds = Mathf.FloorToInt(timeRemaining % 60);

                    if (remainingSeconds < 10)
                    {
                        seconds = "0" + remainingSeconds;
                    }
                    else
                    {
                        seconds = remainingSeconds.ToString();
                    }
                    chrono.text = minutes + ":" + seconds;
                }
                else
                {
                    Debug.Log("Time has run out!");
                    timeRemaining = 0f;
                    timeout = true;
                }
            }
            else
            {
                timeRemaining = 120f;
            }
        }
        #endregion

        #region Methods
        public void ResetScreens()
        {
            SignIn.SetActive(true);
            ResetPasswordCode.SetActive(false);
            ResetPasswordEmail.SetActive(false);
            ResetPasswordNewPassword.SetActive(false);
        }
        public async void Logout()
        {
            var userId = UserManager.Get.getCurrentUserId();
            var deviceToken = PlayerPrefs.GetString("DeviceToken");
            LoaderManager.Get.LoaderController.ShowLoader(null);
            await UserManager.Get.removeUserDeviceTokenAsync(userId, GamesManager.GAME_ID, deviceToken);
            LoaderManager.Get.LoaderController.HideLoader();
            UserManager.Get.logingOut();
            PopupManager.Get.InitPopup();
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
                    ResetPasswordEmail.SetActive(false);
                    ResetPasswordCode.SetActive(true);
                    digits.ActivateInputField();
                    code = res;
                }
            }
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
}