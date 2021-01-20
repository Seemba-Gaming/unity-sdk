using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class SearchingForPlayerPresenter : MonoBehaviour
    {
        #region Static
        public static bool isTrainingGame = false;
        #endregion

        #region Script Parameters
        public Text fee;
        public Text username;
        public Text gain;
        public Image user_flag, user_avatar;
        public Text looking_for_opponent;
        public Button Continue;
        public static string nbPlayer, GameMontant;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            username.text = UserManager.Get.CurrentUser.username;
            var mTexture = new Texture2D(1, 1);
            var flagBytes = Convert.FromBase64String(PlayerPrefs.GetString("CurrentFlagBytesString"));
            mTexture.LoadImage(flagBytes);
            user_flag.sprite = Sprite.Create(mTexture, new Rect(0f, 0f, mTexture.width, mTexture.height), Vector2.zero);
            user_avatar.sprite = UserManager.Get.CurrentAvatarBytesString;

            gain.text = ChallengeManager.CurrentChallenge.gain.ToString();
            gain.text += (ChallengeManager.CurrentChallenge.gain_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES)) ? " bubbles" : " €";
            InvokeRepeating("CheckOpponent", .5f, 3f);

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
                                 catch (NullReferenceException)
                                 {
                                 }
                             }
                             if ((10 - count) == 9)
                             {
                                looking_for_opponent.transform.localScale = Vector3.one;
                             }
                         });
                         count--;
                     }
                 });
            }
            catch (NullReferenceException)
            {
            }
        }
        private void OnDisable()
        {
            CancelInvoke("CheckOpponent");
        }
        #endregion

        #region Implementation
        private async void CheckOpponent()
        {
            string token = UserManager.Get.getCurrentSessionToken();
            string userID = UserManager.Get.getCurrentUserId();
            var N = await ChallengeManager.Get.getChallengebyIdAsync(ChallengeManager.CurrentChallengeId, token);
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
        }
        #endregion
    }
}
