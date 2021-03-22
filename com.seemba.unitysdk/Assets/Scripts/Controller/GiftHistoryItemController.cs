using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class GiftHistoryItemController : MonoBehaviour
    {
        public Dictionary<int, Sprite> Images = new Dictionary<int, Sprite>();

        public Image Icon;
        public TextMeshProUGUI Description;
        public TextMeshProUGUI Date;
        public TextMeshProUGUI Status;

        public async Task InitAsync(GiftItem giftItem)
        {
            var hashCode = giftItem.product.cover.GetHashCode();
            Sprite sprite = null;
            if (!Images.TryGetValue(hashCode, out sprite))
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(giftItem.product.cover);
                await www.SendWebRequest();
                var avatarTexture = DownloadHandlerTexture.GetContent(www);
                sprite = Sprite.Create(avatarTexture, new Rect(0f, 0f, avatarTexture.width, avatarTexture.height), Vector2.zero);
                if (!Images.ContainsKey(hashCode))
                {
                    Images.Add(hashCode, sprite);
                }
                Icon.sprite = sprite;
            }
            else
            {
                Icon.sprite = sprite;
            }

            Description.text = giftItem.product.name;
            Date.text = DateTime.Parse(giftItem.createdAt).ToShortDateString();
            Status.text = giftItem.status;
        }
    }
}
