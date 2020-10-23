using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using System;
using UnityEngine.SceneManagement;
using System.Globalization;
public class ResultManager : MonoBehaviour
{
    //Text Gain;
    //public static bool AddGain, resultWaiting = false;
    //public GameObject bubbles, triangle;
    //private bool startCheck = false;
    //async void OnEnable()
    //{
    //    Texture2D CurrentUserTxt = new Texture2D(1, 1);
    //    Texture2D AdvTxt = new Texture2D(1, 1);
    //    Texture2D txtDrapeauAdv = new Texture2D(1, 1);
    //    Texture2D txtDrapeauCurrent = new Texture2D(1, 1);
    //    Sprite newSprite;
    //    string UserId = UserManager.Get.getCurrentUserId();
    //    string token = UserManager.Get.getCurrentSessionToken();
    //    HistoryListController hc = new HistoryListController();
    //    UnityThreading.ActionThread thread;
    //    string CurrentScene = SceneManager.GetActiveScene().name;
    //    LoaderManager.Get.LoaderController.ShowLoader(null);
    //    try
    //    {
    //        GameObject.Find("ResultWaiting").GetComponent<Image>();
    //        resultWaiting = true;
    //    }
    //    catch (NullReferenceException ex)
    //    {
    //        resultWaiting = false;
    //    }
    //    Challenge challenge = await ChallengeManager.Get.getChallenge(ChallengeManager.CurrentChallengeId);
    //    User adversaire = null;
    //    if (!resultWaiting)
    //    {
    //        adversaire = challenge.matched_user_1._id == UserId ? challenge.matched_user_2 : challenge.matched_user_1;
              
    //    }
    //    else
    //    {
    //        UnityThreadHelper.Dispatcher.Dispatch(() =>
    //        {
    //            InvokeRepeating("checkGameFinished", 0f, 1f);
    //        });
    //    }
            
    //    Image CurrentUserImage = GameObject.Find("ImageCurrentUser").GetComponent<Image>();
    //    CurrentUserImage.sprite = UserManager.Get.CurrentAvatarBytesString;
    //    Text NomGagnant = GameObject.Find("NomCurrentUser").GetComponent<Text>();
    //    Text ScoreGagnant = GameObject.Find("ScoreCurrentUser").GetComponent<Text>();
    //    Text NomPerdant = GameObject.Find("NomAdv").GetComponent<Text>();
    //    Text ScorePerdant = GameObject.Find("ScoreAdv").GetComponent<Text>();
    //    Text Date = GameObject.Find("DateValeur").GetComponent<Text>();
    //    Text ChallengeId = GameObject.Find("ID").GetComponent<Text>();
    //    Text Fee = GameObject.Find("Frais d'entréeValeur").GetComponent<Text>();
    //    try
    //    {
    //        Gain = GameObject.Find("MontantGagnant").GetComponent<Text>();
    //        if (resultWaiting != true)
    //        {
    //            Image AdvImage = GameObject.Find("ImageAdv").GetComponent<Image>();
    //            AdvImage.sprite = await UserManager.Get.getAvatar(adversaire.avatar);
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //    try
    //    {
    //        Image advDrapeau = GameObject.Find("DrapeauPerdant").GetComponent<Image>();
    //        var texture = await UserManager.Get.GetFlagBytes(adversaire.country_code);
    //        Sprite newSpriteDrapeau = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
    //        advDrapeau.sprite = newSpriteDrapeau;
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //    try
    //    {
    //        Image CurrentDrapeau = GameObject.Find("DrapeauGagnant").GetComponent<Image>();
    //        Byte[] img1 = Convert.FromBase64String(UserManager.Get.CurrentFlagBytesString);
    //        txtDrapeauCurrent.LoadImage(img1);
    //        Sprite newSpriteDrapeau = Sprite.Create(txtDrapeauCurrent as Texture2D, new Rect(0f, 0f, txtDrapeauCurrent.width, txtDrapeauCurrent.height), Vector2.zero);
    //        CurrentDrapeau.sprite = newSpriteDrapeau;
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //    float? score1 = null;
    //    float? score2 = null;
    //    if (challenge.user_1_score != null)
    //    {
    //        score1 = challenge.user_1_score;
    //    }
    //    if (challenge.user_2_score != null)
    //    {
    //        score2 = challenge.user_2_score;
    //    }
    //    string gagnantId = null;
    //    string scoreG = null;
    //    string scoreP = null;
    //    string challengeId = challenge._id;
    //    try
    //    {
    //        if(score2 != null)
    //        {
    //            if (score1 > score2)
    //            {
    //                gagnantId = challenge.matched_user_1._id;
    //                scoreG = score1.ToString();
    //                scoreP = score2.ToString();
    //            }
    //            else
    //            {
    //                gagnantId = challenge.matched_user_2._id;
    //                scoreG = score2.ToString();
    //                scoreP = score1.ToString();
    //            }
    //        }
    //        else
    //        {
    //            gagnantId = challenge.matched_user_1._id;
    //            scoreG = score1.ToString();
    //            //scoreP = score2.ToString();
    //        }
    //    }
    //    catch (Exception e) {Debug.Log(e);}
    //    if (gagnantId == UserManager.Get.getCurrentUserId())
    //    {
    //        // user est gagnant
    //        ScoreGagnant.text = scoreG;
    //        if (String.IsNullOrEmpty(scoreP))
    //        {
    //            ScorePerdant.text = "-";
    //        }
    //        else
    //        {
    //            ScorePerdant.text = scoreP;
    //        }
    //        if (!string.IsNullOrEmpty(UserManager.Get.CurrentUser.username))
    //        {
    //            NomGagnant.text = UserManager.Get.CurrentUser.username;
    //        }
    //        else
    //        {
    //            NomGagnant.text = PlayerPrefs.GetString("CurrentUsername");
    //        }
    //        try
    //        {
    //            NomPerdant.text = adversaire.username;
    //        }
    //        catch (NullReferenceException ex)
    //        {
    //        }
    //        if (challenge.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
    //        {
    //            try
    //            {
    //                Gain.text = challenge.gain + ",00 " + CurrencyManager.CURRENT_CURRENCY;
    //            }
    //            catch (Exception ex)
    //            {
    //            }
    //            try
    //            {
    //                triangle.gameObject.SetActive(true);
    //            }
    //            catch (Exception ex)
    //            {
    //            }
    //            switch (challenge.gain.ToString())
    //            {
    //                case "2":
    //                    Fee.text = "entry fee 1,20 " + CurrencyManager.CURRENT_CURRENCY;
    //                    break;
    //                case "5":
    //                    Fee.text = "entry fee 3,00 " + CurrencyManager.CURRENT_CURRENCY;
    //                    break;
    //                case "10":
    //                    Fee.text = "entry fee 6,00 " + CurrencyManager.CURRENT_CURRENCY;
    //                    break;
    //            }
    //        }
    //        else if (challenge.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES)
    //        {
    //            try
    //            {
    //                Gain.text = challenge.gain + " bubbles";
    //            }
    //            catch (Exception ex)
    //            {
    //            }
    //            try
    //            {
    //                bubbles.gameObject.SetActive(true);
    //            }
    //            catch (Exception ex)
    //            {
    //            }
    //            switch (challenge.gain)
    //            {
    //                case "2":
    //                    Fee.text = "entry fee 1 bubble";
    //                    break;
    //                case "6":
    //                    Fee.text = "entry fee 3 bubbles";
    //                    break;
    //                case "10":
    //                    Fee.text = "entry fee 5 bubbles";
    //                    break;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        // user lost
    //        ScoreGagnant.text = scoreP;
    //        if (scoreP == "-1")
    //        {
    //            ScorePerdant.text = "-";
    //        }
    //        else
    //        {
    //            ScorePerdant.text = scoreG;
    //        }
    //        NomGagnant.text = UserManager.Get.CurrentUser.username;
    //        //ca******
    //        try
    //        {
    //            NomPerdant.text = adversaire.username;
    //        }
    //        catch (NullReferenceException ex)
    //        {
    //        }
    //        ;
    //        if (challenge.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
    //        {
    //            if (challenge.matched_user_2 != null && challenge.matched_user_2._id != "")
    //            {
    //                switch (challenge.gain.ToString())
    //                {
    //                    case "2":
    //                        Fee.text = "entry fee 1,20 " + CurrencyManager.CURRENT_CURRENCY;
    //                        break;
    //                    case "5":
    //                        Fee.text = "entry fee 3,00 " + CurrencyManager.CURRENT_CURRENCY;
    //                        break;
    //                    case "10":
    //                        Fee.text = "entry fee 6,00 " + CurrencyManager.CURRENT_CURRENCY;
    //                        break;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (challenge.matched_user_2 != null && challenge.matched_user_2._id != "")
    //            {
    //                switch (challenge.gain.ToString())
    //                {
    //                    case "2":
    //                        Fee.text = "entry fee 1 bubble";
    //                        break;
    //                    case "6":
    //                        Fee.text = "entry fee 3 bubbles";
    //                        break;
    //                    case "10":
    //                        Fee.text = "entry fee 5 bubbles";
    //                        break;
    //                }
    //            }
    //        }
    //    }
    //    if (scoreP == "-1")
    //    {
    //        ScoreGagnant.text = scoreG;
    //        ScorePerdant.text = "-";
    //    }
    //    Date.text = (challenge.CreatedAt.ToString()).Remove(challenge.CreatedAt.ToString().LastIndexOf("-") + 3);
    //    ChallengeId.text = challenge._id.ToString();
    //    try
    //    {
    //        LoaderManager.Get.LoaderController.HideLoader();
    //    }
    //    catch (ArgumentException ex)
    //    {
    //    }
    //    startCheck = true;
    //}
    //private async void Win()
    //{
    //    //UserManager UserManager.Get = new UserManager();
    //    string[] attrib = { "last_result" };
    //    string[] values = { "Win" };
    //    UserManager.Get.UpdateUserByField(attrib, values);
    //}
    //private async void Loss()
    //{
    //    //UserManager UserManager.Get = new UserManager();
    //    string[] attrib = { "last_result" };
    //    string[] values = { "loss" };
    //    UserManager.Get.UpdateUserByField(attrib, values);
    //}
}