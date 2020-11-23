using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[CLSCompliant(false)]
public class GiftCardController : MonoBehaviour
{
    public Image GiftImage;
    public Text Description;
    public Text Price;

    public void Init(GiftCard card)
    {
        Description.text = card.name;
        Price.text = card.price + CurrencyManager.CURRENT_CURRENCY;
        StartCoroutine(GetGiftImage(card.cover, GiftImage));
    }
    public IEnumerator GetGiftImage(string url, Image giftImage)
    {
        Debug.LogWarning(url);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        www.timeout = 5000;
        yield return www.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(www);
        giftImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
    }
    public void OnClickGift()
    {
        Debug.LogWarning("OnClickGift");
    }

}