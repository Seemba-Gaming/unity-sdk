using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class OpponentFound : MonoBehaviour
    {
        public static string adversaireName;
        public Text opponent_username;
        public static string Avatar, AdvCountryCode;
        public Image opponent_avatar, opponent_flag;
        public GameObject loaderPending, PanelLookingForPlayer, PanelPlayerFound, OpponentName;
        public GameObject Versus_container;
        public Animator Versus_background;
        public void OnEnable()
        {
            StartCoroutine(InitCoroutine());
        }

        public IEnumerator InitCoroutine()
        {
            while(!EventsController.advFound)
            {
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(1f);
            init();
        }
        public async void init()
        {
            Texture2D txt = new Texture2D(1, 1);
            Sprite newSprite;
            PanelLookingForPlayer.SetActive(false);
            PanelPlayerFound.SetActive(true);
            opponent_username.text = adversaireName;
            var sprite = await UserManager.Get.getAvatar(Avatar);
            if (sprite != null)
            {
                opponent_avatar.sprite = sprite;

            }
            try
            {
                var mTexture = await UserManager.Get.GetFlagBytes(AdvCountryCode);
                newSprite = Sprite.Create(mTexture, new Rect(0f, 0f, mTexture.width, mTexture.height), Vector2.zero);
                opponent_flag.sprite = newSprite;
            }
            catch (NullReferenceException)
            {

            }
            Versus_background.SetBool("StopBG", true);
            Versus_container.SetActive(true);
        }

        public void ResetOpponent()
        {
            PanelLookingForPlayer.SetActive(true);
            PanelPlayerFound.SetActive(false);
            Versus_container.SetActive(false);
            opponent_username.text = string.Empty;
            opponent_flag.sprite = null;
            opponent_avatar.sprite = null;
        }
    }
}
