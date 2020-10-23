using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
public class SignupPresenter : MonoBehaviour
{
    #region Script Parameters
    public InputField username, email, password, confirmPassword;
    public Button Signup;
    public Button Signin;
    public Sprite[] spriteArray;
    public Button changeCharacter;
    public Button BackButton;
    public Image LoaderUsername;
    public Image AcceptedUsername;
    public Image DeclinedUsername;
    public Image LoaderEmail;
    public Image AcceptedEmail;
    public Image DeclinedEmail;
    public Image AcceptedConfirmPassword;
    public Image DeclinedConfirmPassword;
    public Image LoaderConfirmPassword;
    public Image Avatar;
    #endregion

    #region Fields
    private string[] files;
    private string pathPreFix;
    private int RandomValue;
    private bool isEmailValid, isUsernameValid, isPasswordValid, isPasswordConfirmed;
    #endregion

    #region Unity Methods
    void Start()
    {
        Signin.onClick.AddListener(delegate {
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Login.gameObject);
        });

        changeCharacter.onClick.AddListener(() =>
        {
            object[] _params = { "choose yout character" };
            PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_CHOOSE_CHARACTER, _params);
        });

        System.Random rnd = new System.Random();
        RandomValue = rnd.Next(0, 20);
        Avatar.sprite = spriteArray[RandomValue];

        username.onValueChanged.AddListener(delegate
        {
            LoaderUsername.enabled = true;
            AcceptedUsername.enabled = false;
            DeclinedUsername.enabled = false;

            UnityThreading.ActionThread thread;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                bool valide = UserManager.Get.checkUsername(username.text.ToUpper());
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    if (valide && username.text.Length >= 3)
                    {
                        LoaderUsername.enabled = false;
                        AcceptedUsername.enabled = true;
                        DeclinedUsername.enabled = false;
                        isUsernameValid = true;
                    }
                    else
                    {
                        LoaderUsername.enabled = false;
                        AcceptedUsername.enabled = false;
                        DeclinedUsername.enabled = true;
                        isUsernameValid = false;
                    }
                });
            });
        });

        email.onValueChanged.AddListener(delegate
        {
            LoaderEmail.enabled = true;
            AcceptedEmail.enabled = false;
            DeclinedEmail.enabled = false;
            UnityThreading.ActionThread thread;
            thread = UnityThreadHelper.CreateThread(() =>
            {
                bool valide = UserManager.Get.checkMail(email.text);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    if (valide && IsValidEmail(email.text))
                    {
                        LoaderEmail.enabled = false;
                        AcceptedEmail.enabled = true;
                        DeclinedEmail.enabled = false;
                        isEmailValid = true;

                    }
                    else
                    {
                        LoaderEmail.enabled = false;
                        AcceptedEmail.enabled = false;
                        DeclinedEmail.enabled = true;
                        isEmailValid = false;
                    }
                });
            });
        });

        password.onValueChanged.AddListener(delegate
        {
            isPasswordConfirmed = false;
            isPasswordValid = true;
        });

        confirmPassword.onValueChanged.AddListener(delegate
        {
            if (confirmPassword.text == password.text)
            {
                AcceptedConfirmPassword.enabled = true;
                DeclinedConfirmPassword.enabled = false;
                LoaderConfirmPassword.enabled = false;
                isPasswordConfirmed = true;
            }
            else
            {
                AcceptedConfirmPassword.enabled = false;
                DeclinedConfirmPassword.enabled = true;
                LoaderConfirmPassword.enabled = false; 
                isPasswordConfirmed = false;
            }

            if (string.IsNullOrEmpty(confirmPassword.text))
            {
                AcceptedConfirmPassword.enabled = false;
                DeclinedConfirmPassword.enabled = false;
                LoaderConfirmPassword.enabled = false;
                isPasswordConfirmed = false;
            }
        });

        Signup.onClick.AddListener(delegate
        {
            GetComponent<SignupController>().Signup(username.text, email.text, password.text, Avatar);
        });
    }
    void Update()
    {
        if (isEmailValid && isUsernameValid && isPasswordValid && isPasswordConfirmed)
        {
            Signup.interactable = true;
        }
        else
        {
            Signup.interactable = false;
        }
    }
    #endregion

    #region Methods
    public void OnToggleSelected(bool value, Image image)
    {
        if (value)
        {
            Avatar.sprite = image.sprite;
        }
    }
    #endregion

    #region Implementations
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
    #endregion
}
