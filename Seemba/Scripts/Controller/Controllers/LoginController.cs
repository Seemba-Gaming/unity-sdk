using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class LoginController : MonoBehaviour
    {
        // Start is called before the first frame update
        private static LoginController sInstance;

        public static LoginController Get
        {
            get
            {
                return sInstance;
            }
        }

        private void Awake()
        {
            sInstance = this;
        }


        public async Task<bool> Login(string username, string password)
        {
            LoaderManager.Get.LoaderController.ShowLoader(null);
            ResetLoginFailedAnimation();
            var deviceToken = PlayerPrefs.GetString("DeviceToken");
            string res = await UserManager.Get.logingIn(username, password);
            LoaderManager.Get.LoaderController.HideLoader();
            if (res == "success")
            {
                var platform = "";
                if (Application.platform == RuntimePlatform.Android)
                    platform = "android";
                else platform = "ios";
                await UserManager.Get.addUserDeviceToken(UserManager.Get.CurrentUser._id, GamesManager.GAME_ID, deviceToken, platform);
                ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Menu.gameObject);
                ViewsEvents.Get.ShowScene(ViewsEvents.Get.Menu.Home);
                //ViewsEvents.Get.ShowScene(ViewsEvents.Get.Menu.HaveFun);

                return true;
            }
            else if (res == "failed")
            {
                print("auth failed");
                ShowLoginFailedAnimation();
                return false;
            }
            else
            {
                ConnectivityController.CURRENT_ACTION = ConnectivityController.LOGIN_ACTION;
                LoaderManager.Get.LoaderController.HideLoader();

                return false;
            }
        }
        void ShowLoginFailedAnimation()
        {
            ViewsEvents.Get.Login.LoginAnimator.SetBool("loginFailed", true);
        }
        void ResetLoginFailedAnimation()
        {
            ViewsEvents.Get.Login.LoginAnimator.SetBool("loginFailed", false);
        }
        public bool IsValidEmail(string email)
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
    }
}
