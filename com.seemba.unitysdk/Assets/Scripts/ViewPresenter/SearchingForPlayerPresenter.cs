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

            TranslationManager._instance.scene = "Home";
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
                Continue.interactable = false;
                int count = 5;
                counter.gameObject.SetActive(true);
                counter.text = count.ToString();
                UnityThreadHelper.CreateThread(() =>
                 {
                     while (count > 0)
                     {
                         Thread.Sleep(1000);
                         UnityThreadHelper.Dispatcher.Dispatch(() =>
                         {
                             if (count == 0 || EventsController.advFound)
                             {
                                Continue.interactable = true;
                                counter.gameObject.SetActive(false);
                                count = 0;
                             }
                             counter.text = count.ToString();
                         });
                         count--;
                     }
                 });
            }
            catch (NullReferenceException ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }

        #endregion

        #region Implementation

        public IEnumerator CheckOpponentCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            while (!EventsController.advFound)
            {
                yield return new WaitForSeconds(1f);
                CheckOpponent();
            }
        }
        private async void CheckOpponent()
        {
            string userID = UserManager.Get.getCurrentUserId();
            var response = await ChallengeManager.Get.getChallengebyIdAsync(ChallengeManager.CurrentChallengeId);
            if (response.matched_user_1 != null && !string.IsNullOrEmpty(response.matched_user_1._id) && response.matched_user_2 != null  && !string.IsNullOrEmpty(response.matched_user_2._id))
            {
                if (response.matched_user_1._id.Equals(userID))
                {
                    OpponentFound.adversaireName = response.matched_user_2.username;
                    OpponentFound.Avatar = response.matched_user_2.avatar;
                    OpponentFound.AdvCountryCode = response.matched_user_2.country_code;
                }
                else
                {
                    OpponentFound.adversaireName = response.matched_user_1.username;
                    OpponentFound.Avatar = response.matched_user_1.avatar;
                    OpponentFound.AdvCountryCode = response.matched_user_1.country_code;
                }
                EventsController.advFound = true;
            }
        }
        #endregion
    }
}
