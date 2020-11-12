using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[CLSCompliant(false)]
[System.Serializable]
public class TourPlayer : MonoBehaviour
{
    public Text Username;
    public Image Avatar;
    public Text Score;
    public Text OldScore;
    public GameObject DrawIcon;
    public Image Flag;
    public Image Pro;
    public GameObject Loader;


    private void Awake()
    {
        TranslationManager.scene = "Bracket";
        Username.text = TranslationManager.Get("to_be_determined");
    }
    public async void InitAsync(JSONNode info)
    {
        if(info != null && info["avatar"] != null)
        {
            Username.text = info["username"].Value;
            StartCoroutine(initPlayerAvatar(info["avatar"].Value, Avatar));
            var mTexture = await UserManager.Get.GetFlagBytes(info["country_code"].Value);
            SetPlayerFlag(Flag, mTexture);
            if (info["money_credit"] != null && info["money_credit"].AsFloat > 0f)
            {
                Pro.enabled = true;
            }
            else
            {
                Pro.enabled = false;
            }
        }
        if(Loader != null)
        {
            Loader.SetActive(false);
        }
    }

    public void ShowDraw(float? score)
    {
        DrawIcon.SetActive(true);
        Score.gameObject.SetActive(false);
        OldScore.text = score.ToString();
    }

    public void ShowBothScores(float? oldScore, float? newScore)
    {
        DrawIcon.SetActive(true);
        DrawIcon.GetComponent<Image>().enabled = false;
        Score.text = newScore.ToString();
        OldScore.text = oldScore.ToString();
    }

    public IEnumerator initPlayerAvatar(string url, Image avatar)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(www);
        Texture2D RoundTxt = ImagesManager.RoundCrop(texture);
        avatar.sprite = Sprite.Create(RoundTxt, new Rect(0, 0, RoundTxt.width, RoundTxt.height), new Vector2(0, 0));
    }

    private void SetPlayerFlag(Image flag, Texture2D texture)
    {
        flag.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
    }
}
