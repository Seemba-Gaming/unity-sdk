using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        public TextMeshProUGUI gain;
        public Text nb_players;
        public Text counter;
        public Image user_flag, user_avatar;
        public Text looking_for_opponent, start_now, same_game;
        public Button Continue;
        public static string nbPlayer, GameMontant;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            nb_players.text = nbPlayer;
            username.text = UserManager.Get.CurrentUser.username;
            var mTexture = new Texture2D(1, 1);
            var flagBytes = Convert.FromBase64String(PlayerPrefs.GetString("CurrentFlagBytesString"));
            mTexture.LoadImage(flagBytes);
            user_flag.sprite = Sprite.Create(mTexture, new Rect(0f, 0f, mTexture.width, mTexture.height), Vector2.zero);
            user_avatar.sprite = UserManager.Get.CurrentAvatarBytesString;

            TranslationManager.scene = "Home";
            if(ChallengeManager.CurrentChallenge.gain_type.Equals(ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES))
            {
                gain.text = ChallengeManager.CurrentChallenge.gain + " <sprite=0>";
            }
            else
            {
                gain.text = (float.Parse(ChallengeManager.CurrentChallenge.gain) * 100).ToString() + "<sprite=1>";
            }
            StartCoroutine(CheckOpponentCoroutine());

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
                                 try
                                 {
                                     looking_for_opponent.transform.localScale = Vector3.one;
                                     start_now.transform.localScale = Vector3.zero;
                                     same_game.transform.localScale = Vector3.zero;
                                 }
                                 catch (NullReferenceException)
                                 {
                                 }
                             }
                             counter.text = count.ToString();
                         });
                         count--;
                     }
                 });
            }
            catch (NullReferenceException)
            {
            }
        }

        #endregion

        #region Implementation

        public IEnumerator CheckOpponentCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            while(!EventsController.advFound)
            {
                yield return new WaitForSeconds(3f);
                CheckOpponent();
            }
        }
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
            }
        }
        #endregion
    }
}
