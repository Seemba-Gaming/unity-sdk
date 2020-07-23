using SimpleJSON;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ResultManager : MonoBehaviour
{
    ChallengeManager challengeManager;
    Text Gain;
    public static bool AddGain, resultWaiting = false;
    public GameObject bubbles, triangle;
    private bool startCheck = false;
    // Use this for initialization
    void Start()
    {

        if (AddGain)
        {
        }
        challengeManager = new ChallengeManager();
        UserManager manager = new UserManager();
        Texture2D CurrentUserTxt = new Texture2D(1, 1);
        Texture2D AdvTxt = new Texture2D(1, 1);
        Texture2D txtDrapeauAdv = new Texture2D(1, 1);
        Texture2D txtDrapeauCurrent = new Texture2D(1, 1);
        Sprite newSprite;
        string UserId = manager.getCurrentUserId();
        string token = manager.getCurrentSessionToken();
        HistoryListController hc = new HistoryListController();
        UnityThreading.ActionThread thread;
        String CurrentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync("Loader", LoadSceneMode.Additive);
        try
        {
            GameObject.Find("ResultWaiting").GetComponent<Image>();
            resultWaiting = true;
        }
        catch (NullReferenceException ex)
        {
            resultWaiting = false;
        }
        UnityThreadHelper.CreateThread(() =>
        {
            Challenge challengeResult = challengeManager.getChallenge(ChallengeManager.CurrentChallengeId, token);
            User adversaire = null;
            Byte[] lnByte = null;
            if (!resultWaiting)
            {
                adversaire = manager.getUser(
                    challengeResult.matched_user_1._id == UserId ?
                    challengeResult.matched_user_2._id :
                    challengeResult.matched_user_1._id, token
                );
                lnByte = manager.getAvatar(adversaire.avatar);
            }
            else
            {
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    InvokeRepeating("checkGameFinished", 0f, 1f);
                });
            }
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                Image CurrentUserImage = GameObject.Find("ImageCurrentUser").GetComponent<Image>();
                CurrentUserImage.sprite = UserManager.CurrentAvatarBytesString;
                Text NomGagnant = GameObject.Find("NomCurrentUser").GetComponent<Text>();
                Text ScoreGagnant = GameObject.Find("ScoreCurrentUser").GetComponent<Text>();
                Text NomPerdant = GameObject.Find("NomAdv").GetComponent<Text>();
                Text ScorePerdant = GameObject.Find("ScoreAdv").GetComponent<Text>();
                Text Date = GameObject.Find("DateValeur").GetComponent<Text>();
                Text ChallengeId = GameObject.Find("ID").GetComponent<Text>();
                Text Fee = GameObject.Find("Frais d'entréeValeur").GetComponent<Text>();
                try
                {
                    Gain = GameObject.Find("MontantGagnant").GetComponent<Text>();
                    if (resultWaiting != true)
                    {
                        Image AdvImage = GameObject.Find("ImageAdv").GetComponent<Image>();
                        AdvImage.sprite = ImagesManager.getSpriteFromBytes(lnByte);
                    };
                }
                catch (Exception ex)
                {
                }
                try
                {
                    Image advDrapeau = GameObject.Find("DrapeauPerdant").GetComponent<Image>();
                    Byte[] img1 = Convert.FromBase64String(manager.GetFlagByte(adversaire.country_code));
                    txtDrapeauAdv.LoadImage(img1);
                    Sprite newSpriteDrapeau = Sprite.Create(txtDrapeauAdv as Texture2D, new Rect(0f, 0f, txtDrapeauAdv.width, txtDrapeauAdv.height), Vector2.zero);
                    advDrapeau.sprite = newSpriteDrapeau;
                }
                catch (Exception ex)
                {
                }
                try
                {
                    Image CurrentDrapeau = GameObject.Find("DrapeauGagnant").GetComponent<Image>();
                    Byte[] img1 = Convert.FromBase64String(UserManager.CurrentFlagBytesString);
                    txtDrapeauCurrent.LoadImage(img1);
                    Sprite newSpriteDrapeau = Sprite.Create(txtDrapeauCurrent as Texture2D, new Rect(0f, 0f, txtDrapeauCurrent.width, txtDrapeauCurrent.height), Vector2.zero);
                    CurrentDrapeau.sprite = newSpriteDrapeau;
                }
                catch (Exception ex)
                {
                }
                float? score1 = null;
                float? score2 = null;
                if (challengeResult.user_1_score != null)
                {
                    score1 = challengeResult.user_1_score;
                }
                if (challengeResult.user_2_score != null)
                {
                    score2 = challengeResult.user_2_score;
                }
                string gagnantId = null;
                string scoreG = null;
                string scoreP = null;
                string challengeId = challengeResult._id;
                try
                {
                    if (score1 > score2)
                    {
                        gagnantId = challengeResult.matched_user_1._id;
                        scoreG = score1.ToString();
                        scoreP = score2.ToString();
                    }
                    else
                    {
                        gagnantId = challengeResult.matched_user_2._id;
                        scoreG = score2.ToString();
                        scoreP = score1.ToString();
                    }
                }
                catch (FormatException e) { }//Debug.Log(e);}
                if (gagnantId == manager.getCurrentUserId())
                {
                    // user est gagnant
                    ScoreGagnant.text = scoreG;
                    if (String.IsNullOrEmpty(scoreP))
                    {
                        ScorePerdant.text = "-";
                    }
                    else
                    {
                        ScorePerdant.text = scoreP;
                    }
                    if (!string.IsNullOrEmpty(UserManager.CurrentUsername))
                    {
                        NomGagnant.text = UserManager.CurrentUsername;
                    }
                    else
                    {
                        NomGagnant.text = PlayerPrefs.GetString("CurrentUsername");
                    }
                    try
                    {
                        NomPerdant.text = adversaire.username;
                    }
                    catch (NullReferenceException ex)
                    {
                    }
                    if (challengeResult.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
                    {
                        try
                        {
                            Gain.text = challengeResult.gain + ",00 " + CurrencyManager.CURRENT_CURRENCY;
                        }
                        catch (Exception ex)
                        {
                        }
                        try
                        {
                            triangle.gameObject.SetActive(true);
                        }
                        catch (Exception ex)
                        {
                        }
                        switch (challengeResult.gain.ToString())
                        {
                            case "2":
                                if (AddGain)
                                {
                                    if (challengeResult.matched_user_1._id != "")
                                    {
                                        UserManager.CurrentWater = (int.Parse(UserManager.CurrentWater) + 4).ToString();
                                    }
                                }
                                Fee.text = "entry fee 1,20 " + CurrencyManager.CURRENT_CURRENCY;
                                break;
                            case "5":
                                if (AddGain)
                                {
                                    if (challengeResult.matched_user_1._id != "")
                                    {
                                        UserManager.CurrentWater = (int.Parse(UserManager.CurrentWater) + 6).ToString();
                                    }
                                }
                                Fee.text = "entry fee 3,00 " + CurrencyManager.CURRENT_CURRENCY;
                                break;
                            case "10":
                                if (AddGain)
                                {
                                    if (challengeResult.matched_user_1._id != "")
                                    {
                                        UserManager.CurrentMoney = (float.Parse(UserManager.CurrentMoney, CultureInfo.InvariantCulture.NumberFormat) + 10).ToString("N2").Replace(",", ".");
                                    }
                                }
                                Fee.text = "entry fee 6,00 " + CurrencyManager.CURRENT_CURRENCY;
                                break;
                        }
                    }
                    else if (challengeResult.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES)
                    {
                        try
                        {
                            Gain.text = challengeResult.gain + " bubbles";
                        }
                        catch (Exception ex)
                        {
                        }
                        try
                        {
                            bubbles.gameObject.SetActive(true);
                        }
                        catch (Exception ex)
                        {
                        }
                        switch (challengeResult.gain)
                        {
                            case "2":
                                if (AddGain)
                                {
                                    if (challengeResult.matched_user_2._id != "")
                                    {
                                        UserManager.CurrentWater = (int.Parse(UserManager.CurrentWater) + 2).ToString();
                                    }
                                }
                                Fee.text = "entry fee 1 bubble";
                                break;
                            case "6":
                                if (AddGain)
                                {
                                    if (challengeResult.matched_user_2._id != "")
                                    {
                                        UserManager.CurrentWater = (int.Parse(UserManager.CurrentWater) + 6).ToString();
                                    }
                                }
                                Fee.text = "entry fee 3 bubbles";
                                break;
                            case "10":
                                if (AddGain)
                                {
                                    if (challengeResult.matched_user_2._id != "")
                                    {
                                        UserManager.CurrentWater = (int.Parse(UserManager.CurrentWater) + 10).ToString();
                                    }
                                }
                                Fee.text = "entry fee 5 bubbles";
                                break;
                        }
                    }
                }
                else
                {
                    // user lost
                    ScoreGagnant.text = scoreP;
                    if (scoreP == "-1")
                    {
                        ScorePerdant.text = "-";
                    }
                    else
                    {
                        ScorePerdant.text = scoreG;
                    }
                    NomGagnant.text = UserManager.CurrentUsername;
                    //ca******
                    try
                    {
                        NomPerdant.text = adversaire.username;
                    }
                    catch (NullReferenceException ex)
                    {
                    }
                  ;
                    if (challengeResult.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
                    {
                        if (challengeResult.matched_user_2._id != "")
                        {
                            switch (challengeResult.gain.ToString())
                            {
                                case "2":
                                    Fee.text = "entry fee 1,20 " + CurrencyManager.CURRENT_CURRENCY;
                                    break;
                                case "5":
                                    Fee.text = "entry fee 3,00 " + CurrencyManager.CURRENT_CURRENCY;
                                    break;
                                case "10":
                                    Fee.text = "entry fee 6,00 " + CurrencyManager.CURRENT_CURRENCY;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (challengeResult.matched_user_2._id != "")
                        {
                            switch (challengeResult.gain.ToString())
                            {
                                case "2":
                                    Fee.text = "entry fee 1 bubble";
                                    break;
                                case "6":
                                    Fee.text = "entry fee 3 bubbles";
                                    break;
                                case "10":
                                    Fee.text = "entry fee 5 bubbles";
                                    break;
                            }
                        }
                    }
                }
                if (scoreP == "-1")
                {
                    ScoreGagnant.text = scoreG;
                    ScorePerdant.text = "-";
                }
                Date.text = (challengeResult.CreatedAt.ToString()).Remove(challengeResult.CreatedAt.ToString().LastIndexOf("-") + 3);
                ChallengeId.text = challengeResult._id.ToString();
                try
                {
                    SceneManager.UnloadScene("Loader");
                }
                catch (ArgumentException ex)
                {
                }
                startCheck = true;
            });
        });
    }
    // Update is called once per frame
    void Update()
    {
        if (startCheck)
        {
            try
            {
                SceneManager.UnloadSceneAsync("Loader");
            }
            catch (ArgumentException ex)
            {
            }
        }
    }
    void OnApplicationQuit()
    {
    }
    void checkGameFinished()
    {
        UserManager um = new UserManager();
        ChallengeManager cm = new ChallengeManager();
        string token = um.getCurrentSessionToken();
        string userId = um.getCurrentUserId();
        float? scoreUser1 = null;
        float? scoreUser2 = null;
        JSONNode challengeResult = null;
        UnityThreadHelper.CreateThread(() =>
        {
            challengeResult = cm.getChallengebyId(ChallengeManager.CurrentChallengeId, token);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                try
                {
                    if (String.IsNullOrEmpty(challengeResult["data"]["user_1_score"].Value))
                    {
                        scoreUser1 = null;
                    }
                    else
                        scoreUser1 = challengeResult["data"]["user_1_score"].AsFloat;
                }
                catch (NullReferenceException ex)
                {
                    scoreUser1 = null;
                }
                try
                {
                    if (String.IsNullOrEmpty(challengeResult["data"]["user_2_score"].Value))
                    {
                        scoreUser2 = null;
                    }
                    else
                        scoreUser2 = challengeResult["data"]["user_2_score"].AsFloat;
                }
                catch (NullReferenceException ex)
                {
                    scoreUser2 = null;
                }
                if (scoreUser1 != null && scoreUser2 != null)
                {
                    CancelInvoke();
                    //matched_user_1 is the Current user
                    if (challengeResult["data"]["matched_user_1"]["_id"].Value == userId && scoreUser1 > scoreUser2)
                    {
                        Win();
                        SceneManager.LoadScene("ResultWin");
                    }
                    else if (challengeResult["data"]["matched_user_1"]["_id"].Value == userId && scoreUser1 < scoreUser2)
                    {
                        Loss();
                        SceneManager.LoadScene("ResultLose");
                    }
                    //matched_user_2 is the Current user
                    else if (challengeResult["data"]["matched_user_2"]["_id"].Value == userId && scoreUser1 < scoreUser2)
                    {
                        Win();
                        SceneManager.LoadScene("ResultWIN");
                    }
                    else if (challengeResult["data"]["matched_user_2"]["_id"].Value == userId && scoreUser1 > scoreUser2)
                    {
                        Loss();
                        SceneManager.LoadScene("ResultLose");
                    }
                    //equality Result
                    else if (scoreUser1 == scoreUser2)
                    {
                        SceneManager.LoadScene("ResultEquality");
                    }
                }
            });
        });
    }
    private void Win()
    {
        UserManager userManager = new UserManager();
        string[] attrib = { "last_result" };
        string[] values = { "Win" };
        userManager.UpdateUserByField(userManager.getCurrentUserId(), userManager.getCurrentSessionToken(), attrib, values);
    }
    private void Loss()
    {
        UserManager userManager = new UserManager();
        string[] attrib = { "last_result" };
        string[] values = { "loss" };
        userManager.UpdateUserByField(userManager.getCurrentUserId(), userManager.getCurrentSessionToken(), attrib, values);
    }
}