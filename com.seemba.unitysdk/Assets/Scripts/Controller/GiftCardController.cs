using System;
using System.Collections;
using UnityEngine; 
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class GiftCardController : MonoBehaviour
    {
        #region Script Parameters
        public string CardId;
        public string ProductId;
        public Image GiftImage;
        public Sprite default_image;
        public Text Description;
        public Text Price;
        #endregion

        #region Fields
        private GiftCard mCurrentCard;
        #endregion

        #region Methods
        public void Init(GiftCard card)
        {
            mCurrentCard = card;
            mCurrentCard.price = card.price * 100;  // a supprimer
            Description.text = card.name;
            Price.text = card.price.ToString();
            StartCoroutine(GetGiftImage(card.cover, GiftImage));
            CardId = card._id;
            ProductId = card.product;
        }
        public IEnumerator GetGiftImage(string url, Image giftImage)
        {
            var hashCode = url.GetHashCode();
            Sprite sprite = null;

            if (!UserManager.Get.Images.TryGetValue(hashCode, out sprite))
            {
                if (!string.IsNullOrEmpty(url))
                {
                    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
                    www.timeout = 5000;
                    yield return www.SendWebRequest();
                    try
                    {
                        var texture = DownloadHandlerTexture.GetContent(www);
                        giftImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                        if (texture == null)
                            giftImage.sprite = default_image;
                        if (!UserManager.Get.Images.ContainsKey(hashCode))
                        {
                            UserManager.Get.Images.Add(hashCode, giftImage.sprite);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        Debug.LogWarning(ex.Message);
                        giftImage.sprite = default_image;
                    }
                }
                else
                {
                    giftImage.sprite = default_image;
                }
            }
            else
            {
                giftImage.sprite = sprite;
            }
        }
        public void OnClickGift()
        {
            ViewsEvents.Get.Menu.Market.GetComponent<MarketController>().SetCurrentGiftCard(mCurrentCard);
            TranslationManager.scene = "Home";
            object[] _params = { TranslationManager.Get("purchase"), mCurrentCard.name, TranslationManager.Get("you_are_about_to_pay"), mCurrentCard.price, TranslationManager.Get("spend_bubbles") };
            PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_GIFT_CARD_INFO, _params, null, GiftImage);
        }
        #endregion
    }
}