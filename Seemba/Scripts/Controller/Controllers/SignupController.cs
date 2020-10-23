﻿using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignupController : MonoBehaviour
{
    // Start is called before the first frame update
    public static SignupController Get {
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
    public async void Signup(string username, string email, string password, Image avatar)
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

                    LoaderManager.Get.LoaderController.ShowLoader(null);

                    var avatarUrl = await ImagesManager.FixImage(bytes);
                    JSONNode Res = null;
                    if (avatarUrl != "error")
                    {
                        Res = await UserManager.Get.signingUp(username.ToUpper(), email, password, avatarUrl);
                    }
                    LoaderManager.Get.LoaderController.HideLoader();

                    if (Res["success"].AsBool)
                    {
                        var deviceToken = PlayerPrefs.GetString("DeviceToken");
                        Debug.Log(deviceToken);
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
                        catch (Exception ex)
                        {
                            Debug.LogWarning(" deviceToken is null");
                        }
                        UserManager.Get.CurrentUser.username = username;
                        UserManager.Get.CurrentAvatarBytesString = await UserManager.Get.getAvatar(avatarUrl);
                        var mTexture = await UserManager.Get.GetFlagBytes(await UserManager.Get.GetGeoLoc());
                        UserManager.Get.CurrentFlagBytesString = Convert.ToBase64String(mTexture.EncodeToPNG());
                        UserManager.Get.CurrentUser.money_credit = 0.00f;//unhealthey to do hardcode anything
                        UserManager.Get.CurrentUser.bubble_credit = 25f;//unhealthey to do hardcode anything
                        //Save Flag Byte
                        PlayerPrefs.SetString("CurrentFlagBytesString", UserManager.Get.CurrentFlagBytesString);
                        ChallengeManager.CurrentChallengeGain = "2";
                        ChallengeManager.CurrentChallengeGainType = ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES;
                        EventsController.Get.startFirstChallenge(Res["token"].Value);
                    }
                    else
                    {
                        ConnectivityController.CURRENT_ACTION = "";
                        LoaderManager.Get.LoaderController.HideLoader();
                        PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());
                    }
                }
            }));
    }
}