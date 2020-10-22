using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using SimpleJSON;
using System.Threading;
public class SearchingForPlayerPresenter : MonoBehaviour
{

    public Text fee, username, gain, nb_players;
    public Text counter;
    public Image user_flag, user_avatar;
    public Text looking_for_opponent,start_now,same_game;
    public Button Continue;
    public static string nbPlayer, GameMontant;

    public static bool isTrainingGame = false;
    void Start()
    {
        InvokeRepeating("CheckOpponent", 0.5f, 3f);

        UserManager manager = new UserManager();
        string UserId = manager.getCurrentUserId();
        string token = manager.getCurrentSessionToken();
        Texture2D txt = new Texture2D(1, 1);
        Sprite newSprite;

        nb_players.text = nbPlayer;
        username.text = UserManager.CurrentUsername;
        Byte[] img1 = Convert.FromBase64String(UserManager.CurrentFlagBytesString);
        txt.LoadImage(img1);
        Texture2D roundTxt1 = ImagesManager.RoundCrop(txt);
        newSprite = Sprite.Create(txt as Texture2D, new Rect(0f, 0f, txt.width, txt.height), Vector2.zero);
        user_flag.sprite = newSprite;
        user_avatar.sprite = UserManager.CurrentAvatarBytesString;
        gain.text = ChallengeManager.CurrentChallenge.gain.ToString();
        gain.text += (ChallengeManager.CurrentChallenge.gain_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES)) ? " bubbles" : " €";
        try
        {

            int count = 10;
            UnityThreadHelper.CreateThread(() =>
             {
                 while (count > 0)
                 {
                     Thread.Sleep(1000);
                     UnityThreadHelper.Dispatcher.Dispatch(() =>
                     {
                         if ((10 - count) == 3)
                         {
                             try
                             {
                                 Continue.transform.localScale = Vector3.one;
                             }
                             catch (NullReferenceException ex)
                             {
                             }
                         }
                         if ((10 - count) == 9)
                         {
                             try
                             {
                                 looking_for_opponent.transform.localScale = Vector3.one;
                                 start_now.transform.localScale = Vector3.zero;
                                 same_game.transform.localScale = Vector3.zero;
                             }
                             catch (NullReferenceException ex)
                             {
                             }
                         }
                         counter.text = count.ToString();
                     });
                     count--;
                 }
             });
        }
        catch (NullReferenceException ex)
        {
        }
    }
   
    void CheckOpponent()
    {
        UserManager um = new UserManager();
        string token = um.getCurrentSessionToken();
        string userID = um.getCurrentUserId();
        ChallengeManager cm = new ChallengeManager();
        UnityThreadHelper.CreateThread(() =>
        {
            var N = cm.getChallengebyId(ChallengeManager.CurrentChallengeId, token);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                if (!string.IsNullOrEmpty(N["data"]["matched_user_1"]["_id"].Value) && !string.IsNullOrEmpty(N["data"]["matched_user_2"]["_id"].Value))
                {
                    if (N["data"]["matched_user_1"]["_id"].Value.Equals(userID))
                    {
                        OpponentFound.adversaireName = N["data"]["matched_user_2"]["username"];
                        OpponentFound.Avatar = N["data"]["matched_user_2"]["avatar"];
                        OpponentFound.AdvCountryCode = N["data"]["matched_user_2"]["country_code"];
                    }
                    else
                    {
                        OpponentFound.adversaireName = N["data"]["matched_user_1"]["username"];
                        OpponentFound.Avatar = N["data"]["matched_user_1"]["avatar"];
                        OpponentFound.AdvCountryCode = N["data"]["matched_user_1"]["country_code"];
                    }
                    EventsController.advFound = true;
                    CancelInvoke("CheckOpponent");
                }
            });
        });
    }
}
