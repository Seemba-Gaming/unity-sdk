using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using Seemba.Facebook.Unity;
using System.Collections.Generic;
using SimpleJSON;
using System.Net.Http;

[CLSCompliant(false)]
public class SignupPresenter : MonoBehaviour
{
    #region Script Parameters
    public InputField username, email, password, confirmPassword;
    public Button Signup;
    public Button FBLogin;
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
        //Init FB SDK
        Seemba.Facebook.Unity.FB.Init(this.OnFBInitComplete, this.OnFBHideUnity);

        Signin.onClick.AddListener(delegate
        {
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Login.gameObject);
        });

        changeCharacter.onClick.AddListener(() =>
        {
            TranslationManager.scene = "Signup";
            object[] _params = { TranslationManager.Get("choose_your_character") };
            PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_CHOOSE_CHARACTER, _params);
        });

        System.Random rnd = new System.Random();
        RandomValue = rnd.Next(0, 20);
        Avatar.sprite = spriteArray[RandomValue];

        username.onValueChanged.AddListener(async delegate
        {
            LoaderUsername.enabled = true;
            AcceptedUsername.enabled = false;
            DeclinedUsername.enabled = false;
            bool valide = await UserManager.Get.checkUsernameAsync(username.text.ToUpper());
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

        email.onValueChanged.AddListener(async delegate
        {
            LoaderEmail.enabled = true;
            AcceptedEmail.enabled = false;
            DeclinedEmail.enabled = false;
            bool valide = await UserManager.Get.checkMailAsync(email.text);
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
        FBLogin.onClick.AddListener(delegate
        {
            LoaderManager.Get.LoaderController.ShowLoader(null);
            Seemba.Facebook.Unity.FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email" }, this.HandleFBLoginResult);
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
    private void OnFBInitComplete()
    {
        if (Seemba.Facebook.Unity.AccessToken.CurrentAccessToken != null)
        {
            Debug.Log("FB CurrentAccessToken: " + Seemba.Facebook.Unity.AccessToken.CurrentAccessToken.ToString());
        }
    }
    private void OnFBHideUnity(bool isGameShown)
    {
        Debug.Log("FB Is game shown: " + isGameShown);
    }
    protected void HandleFBLoginResult(Seemba.Facebook.Unity.IResult result)
    {
        if (!string.IsNullOrEmpty(result.RawResult))
        {
            Debug.Log(result.RawResult);
            Debug.Log(result.ToString());
            /*  foreach (string perm in AccessToken.CurrentAccessToken.Permissions)
              {
                  // log each granted permission
                  Debug.Log(perm);
              }*/
            Seemba.Facebook.Unity.FB.API("/me/picture", Seemba.Facebook.Unity.HttpMethod.GET, this.HandleFBProfilePhotoResult);
            Seemba.Facebook.Unity.FB.API("/me?fields=id,name,email", Seemba.Facebook.Unity.HttpMethod.GET, this.HandleFBInfoResult);

        }
        else
        {
            LoaderManager.Get.LoaderController.HideLoader();
            if (result == null)
            {
                return;
            }
            // Some platforms return the empty string instead of null.
            if (!string.IsNullOrEmpty(result.Error))
            {
                Debug.Log(result.Error);

            }
            else if (result.Cancelled)
            {
                Debug.Log(result.Error);
            }
            else if (!string.IsNullOrEmpty(result.RawResult))
            {

            }
            else
            {
                //Empty Response
            }
        }
    }
    protected void HandleFBProfilePhotoResult(Seemba.Facebook.Unity.IGraphResult result)
    {
        Texture2D RoundTxt = ImagesManager.RoundCrop(result.Texture);
        Avatar.sprite = Sprite.Create(RoundTxt, new Rect(0, 0, RoundTxt.width, RoundTxt.height), new Vector2(0, 0));
        LoaderManager.Get.LoaderController.HideLoader();
        FBLogin.interactable = false;

    }
    protected void HandleFBInfoResult(Seemba.Facebook.Unity.IResult result)
    {
        if (result.Error == null)
        {
            Debug.Log(result.RawResult.ToString());

            var N = JSON.Parse(result.RawResult);
            if(!string.IsNullOrEmpty(N["name"].Value)) username.text = N["name"].Value;
            if(!string.IsNullOrEmpty(N["email"].Value)) email.text = N["email"].Value;

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
