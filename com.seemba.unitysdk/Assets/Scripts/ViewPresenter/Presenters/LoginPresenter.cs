using System;
using UnityEngine;
using UnityEngine.UI;


namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class LoginPresenter : MonoBehaviour
    {
        [Header("Login")]
        public Button Signup;
        public Button Login;
        public Button MagicCode;
        public InputField Username;
        public InputField Password;
        public Animator LoginAnimator;
        public Animator SigninContent;
        //Go PressedListenerHandler for ShowPassword behavior
        public Button ShowPassword;

        [Header("Reset Password")]
        public Button ResetPassword;
        public Button SubmitResetPassword;
        public InputField EmailResetPassword;
        public InputField CodeResetPassword;
        public Text TimerResetPassword;



        // Start is called before the first frame update
        void Start()
        {
            Signup.onClick.AddListener(delegate
            {
                UserService.Get.ResetScreens();
                ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Signup.gameObject);
            });
            Login.onClick.AddListener(delegate
            {
                login();
            });

            Username.ActivateInputField();
            Username.onValueChanged.AddListener(delegate
            {
                if (LoginAnimator.GetBool("loginFailed") == true)
                {
                    LoginAnimator.SetBool("loginFailed", false);
                }
            });

            Password.onValueChanged.AddListener(delegate
            {
                if (LoginAnimator.GetBool("loginFailed") == true)
                {
                    LoginAnimator.SetBool("loginFailed", false);
                }
                if (string.IsNullOrEmpty(Password.text))
                {
                    ShowPassword.gameObject.SetActive(false);
                }
                else
                {
                    ShowPassword.gameObject.SetActive(true);
                }
            });

            EmailResetPassword.onValueChanged.AddListener(delegate
            {
                if (LoginController.Get.IsValidEmail(EmailResetPassword.text))
                {
                    ResetPassword.interactable = true;
                }
            });
        }

        // Update is called once per frame
        void Update()
        {
            if (string.IsNullOrEmpty(Username.text) || string.IsNullOrEmpty(Password.text))
            {
                Login.interactable = false;
            }
            else
            {
                Login.interactable = true;
            }
        }
        public void OnClickShowOrHidePassword()
        {
            if (Password.contentType == InputField.ContentType.Password)
            {
                Password.contentType = InputField.ContentType.Standard;
            }
            else
            {
                Password.contentType = InputField.ContentType.Password;
            }
            Password.ForceLabelUpdate();
        }
        async void login()
        {
            var res = await LoginController.Get.Login(Username.text, Password.text);
            if (!res)
            {
                showLoginFailedAnimation();
            }
        }
        void showLoginFailedAnimation()
        {
            SigninContent.SetBool("loginFailed", true);
        }
    }
}
