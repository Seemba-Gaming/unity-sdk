using SimpleJSON;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class SignupController : MonoBehaviour
    {
        // Start is called before the first frame update
        public static SignupController Get
        {
            get
            {
                if (sInstance == null)
                {
                    sInstance = new SignupController();
                }
                return sInstance;
            }
        }

        private static SignupController sInstance;

        void Start()
        {
        }
        public void Signup(string username, string email, string password, Image avatar)
        {
            LoaderManager.Get.LoaderController.ShowLoader(LoaderManager.CHECKING_CONNECTION);
            StartCoroutine(
                EventsController.Get.checkInternetConnection(async (isConnected) =>
                {
                    if (isConnected == true)
                    {

                        Texture2D mytexture = avatar.sprite.texture;
                        byte[] bytes;
                        bytes = mytexture.EncodeToPNG();

                        LoaderManager.Get.LoaderController.ShowLoader(LoaderManager.ACCOUNT_CREATING);

                        var avatarUrl = await ImagesManager.FixImage(bytes);

                        JSONNode Res = null;
                        if (avatarUrl != "error")
                        {
                            Res = await UserManager.Get.signingUp(username.ToUpper(), email, password, avatarUrl);
                        }
                        LoaderManager.Get.LoaderController.HideLoader();

                        if (Res["success"].AsBool)
                        {
                            SeembaAnalyticsManager.Get.SendGameEvent("Signed up");
                            var deviceToken = PlayerPrefs.GetString("DeviceToken");
                            var userid = UserManager.Get.getCurrentUserId();
                            var platform = "";
                            if (Application.platform == RuntimePlatform.Android)
                                platform = "android";
                            else platform = "ios";
                        //Add Device Token To Receive notification on current device
                        try
                            {
                                await UserManager.Get.addUserDeviceToken(userid, GamesManager.GAME_ID, deviceToken, platform);

                            }
                            catch (Exception)
                            {
                            }
                            UserManager.Get.CurrentUser.username = username;
                            LoaderManager.Get.LoaderController.ShowLoader(LoaderManager.SAVING);
                            UserManager.Get.CurrentAvatarBytesString = await UserManager.Get.getAvatar(avatarUrl);
                            var mTexture = await UserManager.Get.GetFlagBytes(await UserManager.Get.GetGeoLoc());
                            UserManager.Get.CurrentFlagBytesString = Convert.ToBase64String(mTexture.EncodeToPNG());
                            PlayerPrefs.SetString("CurrentFlagBytesString", UserManager.Get.CurrentFlagBytesString);
                            ChallengeManager.CurrentChallengeGain = "2";
                            ChallengeManager.CurrentChallengeGainType = ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES;
                            LoaderManager.Get.LoaderController.HideLoader();
                            EventsController.Get.startFirstChallenge(Res["token"].Value);
                            SeembaAnalyticsManager.Get.SendGameEvent("FirstChallenge");
                        }
                        else
                        {
                            ConnectivityController.CURRENT_ACTION = "";
                            LoaderManager.Get.LoaderController.HideLoader();
                            PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());
                            SeembaAnalyticsManager.Get.SendGameEvent("weak network");

                        }
                    }
                }));
        }
    }
}
