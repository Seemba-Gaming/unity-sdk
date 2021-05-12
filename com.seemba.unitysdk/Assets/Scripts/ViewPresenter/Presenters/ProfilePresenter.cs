using UnityEngine;
using UnityEngine.UI;
using System;

namespace SeembaSDK
{
    public class ProfilePresenter : MonoBehaviour
    {
        #region Script Parameters
        public string PlayerId;
        public Animator Animator;
        public Image avatar;
        public Image pro;
        public Image drapeau;
        public Text username;
        public Text nbGameWon;
        public Text nbGameWonInARow;
        public Button changeAvatar;
        public GameObject Loading;
        public GameObject verified;
        public GameObject pending;
        public GameObject unverified;
        #endregion

        #region Methods
        public async void InitProfile(User user)
        {
            PlayerId = user._id;
            var sprite = await UserManager.Get.getAvatar(user.avatar);
            if (sprite != null)
            {
                avatar.sprite = sprite;

            }
            username.text = user.username;
            string userId = UserManager.Get.getCurrentUserId();

            string Vectoires = user.victories_count.ToString();
            string SerieVectoires = user.current_victories_count.ToString();

            Loading.SetActive(false);
            if (PlayerId != userId)
            {
                changeAvatar.interactable = false;
            }
            username.text = user.username;
            try
            {
                if (user.money_credit > 0)
                {
                    pro.gameObject.SetActive(true);
                }
                else
                {
                    pro.gameObject.SetActive(false);
                }
            }
            catch (FormatException)
            {
                if (user.money_credit > 0)
                {
                    pro.gameObject.SetActive(true);
                }
                else
                {
                    pro.gameObject.SetActive(false);
                }
            }
            var mTexture = await UserManager.Get.GetFlagBytes(user.country_code);
            drapeau.sprite = Sprite.Create(mTexture, new Rect(0f, 0f, mTexture.width, mTexture.height), Vector2.zero);
            nbGameWon.text = Vectoires;
            nbGameWonInARow.text = SerieVectoires;
        }
        #endregion

    }
}
