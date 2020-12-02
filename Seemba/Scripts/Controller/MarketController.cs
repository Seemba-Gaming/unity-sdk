using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]

    public class GiftCard
    {
        public string _id;
        public string product;
        public float price;
        public string cover;
        public string name;
        public string description;
        public string altText;
        public string __v;
    }
    public class OrderResponse
    {
        public string result;
        public float buyPrice;
        public string orderId;
        public string code;
        public string error;
    }

    [CLSCompliant(false)]
    public class MarketController : MonoBehaviour
    {
        #region Script Parameters
        public GameObject GiftPrefab;
        public Transform GiftContainer;
        public Button GoUp;
        public Button GoDown;
        #endregion

        #region Fields
        private GiftCard mCurrentGiftCard;
        #endregion

        #region Unity Methods
        // Start is called before the first frame update
        async void OnEnable()
        {
            var GiftCards = await GetGiftCards();
            FillGifts(GiftCards);
        }
        #endregion

        #region Methods
        public GiftCard GetCurrentGiftCard()
        {
            return mCurrentGiftCard;
        }

        public void SetCurrentGiftCard(GiftCard card)
        {
            mCurrentGiftCard = card;
        }
        public async Task<bool> BuyGiftAsync()
        {
            string url = Endpoint.classesURL + "/products/order";
            WWWForm form = new WWWForm();
            form.AddField("product", mCurrentGiftCard.product);
            var response = await SeembaWebRequest.Get.HttpsPost(url, form);
            SeembaResponse<OrderResponse> responseData = JsonConvert.DeserializeObject<SeembaResponse<OrderResponse>>(response);
            Debug.LogWarning(responseData.data.code);
            if (responseData.data.code.Equals("su"))
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_GIFT_CARD_SUCCESS, PopupsText.Get.GiftCardSuccess());
            }
            else
            {
                if (responseData.data.code.Equals("if"))
                {
                    PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_GIFT_CARD_SUCCESS, PopupsText.Get.GiftCardSuccess());
                }
            }
            return true;
        }
        #endregion

        #region Implementations
        private void FillGifts(GiftCard[] giftCards)
        {
            foreach (GiftCard giftCard in giftCards)
            {
                Debug.LogWarning(giftCard.name);
                GameObject newGiftCard = Instantiate(GiftPrefab, GiftContainer);
                newGiftCard.GetComponent<RectTransform>().localScale = Vector3.one;
                newGiftCard.GetComponent<GiftCardController>().Init(giftCard);
            }
        }

        private async Task<GiftCard[]> GetGiftCards()
        {
            string url = Endpoint.classesURL + "/products";
            GiftCard[] giftCards = await SeembaWebRequest.Get.HttpsGetJSON<GiftCard[]>(url);
            return giftCards;
        }
        #endregion
    }
}
