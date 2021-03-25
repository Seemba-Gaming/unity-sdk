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
    [CLSCompliant(false)]
    public class GiftsOrder
    {
        public string status;
        public string _id;
        public string user;
        public string product;
        public string transaction_id;
        public string transaction_amount;
        public string createdAt;
        public string updatedAt;
        public string __v;
    }

    [CLSCompliant(false)]
    public class MarketController : MonoBehaviour
    {
        #region Script Parameters
        public GameObject GiftPrefab;
        public Transform GiftContainer;
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

        void OnDisable()
        {
            ClearGifts();
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
            SeembaResponse<GiftsOrder> responseData = JsonConvert.DeserializeObject<SeembaResponse<GiftsOrder>>(response);
            if (responseData.message.Equals("order created !"))
            {
                User user = await UserManager.Get.getUser();
                if (user != null)
                {
                    ViewsEvents.Get.Menu.Header.GetComponent<HeaderController>().UpdateHeaderInfo(user);
                }
                PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_GIFT_CARD_SUCCESS, PopupsText.Get.GiftCardSuccess());
            }
            else
            {
                if (responseData.message.Equals("insufficient balance"))
                {
                    PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_GIFT_CARD_SUCCESS, PopupsText.Get.GiftCardIf());
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
                GameObject newGiftCard = Instantiate(GiftPrefab, GiftContainer);
                newGiftCard.GetComponent<RectTransform>().localScale = Vector3.one;
                newGiftCard.GetComponent<GiftCardController>().Init(giftCard);
            }
        }

        private async Task<GiftCard[]> GetGiftCards()
        {
            string url = Endpoint.classesURL + "/products?page=1&pagesize=30";
            GiftCard[] giftCards = await SeembaWebRequest.Get.HttpsGetJSON<GiftCard[]>(url);
            return giftCards;
        }

        private void ClearGifts()
        {
            foreach (Transform transform in GiftContainer)
            {
                Destroy(transform.gameObject);
            }
        }
        #endregion
    }
}
