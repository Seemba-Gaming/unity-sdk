using System;
using System.Collections;
using System.Collections.Generic;
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
    public class MarketController : MonoBehaviour
    {
        #region Script Parameters
        public GameObject GiftPrefab;
        public Transform GiftContainer;
        public Button GoUp;
        public Button GoDown;
        #endregion

        #region Unity Methods
        // Start is called before the first frame update
        async void OnEnable()
        {
            var GiftCards = await GetGiftCards();
            FillGifts(GiftCards);
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
