using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace SeembaSDK
{
    public class GiftItem
    {
        public string status;
        public string _id;
        public string user;
        public GiftDetails product;
        public string transaction_id;
        public double transaction_amount;
        public string createdAt;
        public string updatedAt;
        public string __v;
        public GiftOrder response;
    }

    public class GiftDetails
    {
        public string active;
        public string _id;
        public string product;
        public double price;
        public string cover;
        public string name;
        public string description;
        public string altText;
        public int __v;
    }
    public class GiftOrder
    {
        public string error;
        public string code;
    }
    public class GiftHistoryController : MonoBehaviour
    {
        #region Script Parameters
        public GameObject Content;
        public GameObject GiftItemPrefab;
        #endregion

        #region Fields
        private GiftItem[] mOrders;
        #endregion

        #region Unity Methods
        private async void OnEnable()
        {
            mOrders = await GetOrders(1, 10);
            await FillOrdersAsync(mOrders);
        }

        private void OnDisable()
        {
            foreach (Transform child in Content.transform)
            {
                Destroy(child.gameObject);
            }
        }
        #endregion

        #region Methods
        public async Task<GiftItem[]> GetOrders(int page, int pagesize)
        {
            string url = Endpoint.classesURL + "/products/user/"+ UserManager.Get.CurrentUser._id + "/orders?page=" + page + "&pagesize=" + pagesize;
            var response = await SeembaWebRequest.Get.HttpsGetJSON<GiftItem[]>(url);
            return response;
        }

        public async Task FillOrdersAsync(GiftItem[] orders)
        {
            for(int i=0; i<orders.Length; i++)
            {
                var newOrder = Instantiate(GiftItemPrefab, Content.transform);
                await newOrder.GetComponent<GiftHistoryItemController>().InitAsync(orders[i]);
            }
        }
        #endregion
    }
}
