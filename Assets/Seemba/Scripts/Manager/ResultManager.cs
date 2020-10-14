using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using System;
using UnityEngine.SceneManagement;
using System.Globalization;
public class ResultManager : MonoBehaviour
{
    ChallengeManager cm;
    Text Gain;
    public static bool AddGain, resultWaiting = false;
    public GameObject bubbles, triangle;
    private bool startCheck = false;
    // Use this for initialization
    void Start()
    {

        cm = new ChallengeManager();
        UserManager um = new UserManager();
        Texture2D CurrentUserTxt = new Texture2D(1, 1);
        Texture2D AdvTxt = new Texture2D(1, 1);
        Texture2D txtDrapeauAdv = new Texture2D(1, 1);
        Texture2D txtDrapeauCurrent = new Texture2D(1, 1);
        string UserId = um.getCurrentUserId();
        string token = um.getCurrentSessionToken();
        HistoryListController hc = new HistoryListController();
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
            Challenge challengeResult = cm.getChallenge(ChallengeManager.CurrentChallengeId, token);
            User adversaire = null;
            Byte[] lnByte = null;
            if (!resultWaiting)
            {
                adversaire = um.getUser(
                    challengeResult.matched_user_1._id == UserId ?
                    challengeResult.matched_user_2._id :
                    challengeResult.matched_user_1._id, token
                );
                lnByte = um.getAvatar(adversaire.avatar);
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
                Text Username = GameObject.Find("NomCurrentUser").GetComponent<Text>();
                Text OpponentName = GameObject.Find("NomAdv").GetComponent<Text>();
                Text ScoreCurrentUser = GameObject.Find("ScoreCurrentUser").GetComponent<Text>();
                Text ScoreOpponent = GameObject.Find("ScoreAdv").GetComponent<Text>();
                Text Date = GameObject.Find("DateValeur").GetComponent<Text>();
                Text ChallengeId = GameObject.Find("ID").GetComponent<Text>();
                Text Fee = GameObject.Find("Fee").GetComponent<Text>();

                CurrentUserImage.sprite = UserManager.CurrentAvatarBytesString;
                if (resultWaiting != true)
                {
                    Image AdvImage = GameObject.Find("ImageAdv").GetComponent<Image>();
                    AdvImage.sprite = ImagesManager.getSpriteFromBytes(lnByte);
                }
                try
                {
                    Image advDrapeau = GameObject.Find("DrapeauPerdant").GetComponent<Image>();
                    Byte[] img1 = Convert.FromBase64String(um.GetFlagByte(adversaire.country_code));
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
                string challengeId = challengeResult._id;
                if (!string.IsNullOrEmpty(UserManager.CurrentUsername))
                {
                    Username.text = UserManager.CurrentUsername;
                }
                else
                {
                    Username.text = PlayerPrefs.GetString("CurrentUsername");
                }

                if (challengeResult.user_2_score != null)
                {
                    OpponentName.text = adversaire.username;

                    if (challengeResult.matched_user_1._id.Equals(um.getCurrentUserId()))
                    {
                        ScoreCurrentUser.text = challengeResult.user_1_score.ToString();
                        ScoreOpponent.text = challengeResult.user_2_score.ToString();
                    }
                    else if (challengeResult.matched_user_2._id.Equals(um.getCurrentUserId()))
                    {
                        ScoreCurrentUser.text = challengeResult.user_2_score.ToString();
                        ScoreOpponent.text = challengeResult.user_1_score.ToString();
                    }
                }
                else
                {
                    ScoreCurrentUser.text = challengeResult.user_1_score.ToString();
                }

                if (challengeResult.winner_user.Equals(um.getCurrentUserId()))
                {
                    Debug.Log("here");
                    Gain = GameObject.Find("Gain").GetComponent<Text>();
                    triangle.gameObject.SetActive(true);
                    if (challengeResult.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
                    {
                        Gain.text = float.Parse(challengeResult.gain).ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                    }
                    else if (challengeResult.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES)
                    {
                        Gain.text = challengeResult.gain + " bubbles";
                    }
                    Debug.Log("here");
                }
                if (challengeResult.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
                {
                    switch (float.Parse(challengeResult.gain))
                    {
                        case ChallengeManager.WIN_1V1_CASH_CONFIDENT:
                            Fee.text = "Fee: " + ChallengeManager.FEE_1V1_CASH_CONFIDENT + CurrencyManager.CURRENT_CURRENCY;
                            break;
                        case ChallengeManager.WIN_1V1_CASH_CHAMPION:
                            Fee.text = "Fee: " + ChallengeManager.FEE_1V1_CASH_CONFIDENT + CurrencyManager.CURRENT_CURRENCY;
                            break;
                        case ChallengeManager.WIN_1V1_CASH_LEGEND:
                            Fee.text = "Fee: " + ChallengeManager.FEE_1V1_CASH_CONFIDENT + CurrencyManager.CURRENT_CURRENCY;
                            break;
                    }
                }
                else if (challengeResult.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES)
                {
                    switch (float.Parse(challengeResult.gain))
                    {
                        case ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT:
                            Fee.text = "Fee: " + ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT + " Bubbles";
                            break;
                        case ChallengeManager.WIN_1V1_BUBBLES_CHAMPION:
                            Fee.text = "Fee: " + ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT + " Bubbles";
                            break;
                        case ChallengeManager.WIN_1V1_BUBBLES_LEGEND:
                            Fee.text = "Fee: " + ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT + " Bubbles";
                            break;
                    }
                }

                Debug.Log("here");

                var seperator_index = challengeResult.CreatedAt.ToString().Contains("T") ? challengeResult.CreatedAt.ToString().IndexOf("T") : challengeResult.CreatedAt.ToString().IndexOf(" ");
                string date = challengeResult.CreatedAt.ToString().Substring(0, seperator_index).Replace("/", "-");
                string hour = challengeResult.CreatedAt.ToString().Substring(seperator_index + 1, 5).Replace(":", "H") + "MIN";
                Date.text = date + " " + "AT" + " " + hour;

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