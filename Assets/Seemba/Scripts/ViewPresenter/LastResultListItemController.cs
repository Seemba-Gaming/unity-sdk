using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net;
using System.Threading;
using System.IO;
using SimpleJSON;
using System.Globalization;
public class LastResultListItemController : MonoBehaviour
{
    public GameObject ContentPanel;
    public GameObject ProfilePopup;
    public GameObject ListItemPrefab;
    public GameObject ListTournamentItemPrefab;
    public GameObject GradientComponent;
    ArrayList Items, lastResultItem;
    JSONArray ItemsTournament;
    ChallengeManager challengeManager = new ChallengeManager();
    TournamentManager tournamentManager = new TournamentManager();
    UserManager userManager = new UserManager();
    public Sprite argentGain;
    public Sprite GoutteGain;
    public Sprite argentFee;
    public string UserId, token;
    public Sprite GoutteFee;
    private Gradient gradient;
    public GameObject ContentLastResult, PanelObjects;
    public int nbElement = 1;
    public Button SeeMoreResult;
    public GameObject Loading;
    public Sprite[] spriteArray;
    public string gaintext;
    public int Length = 0;
    private int nbChild = 0;
    private bool isFinished = false;
    // Use this for initialization
   
   
    void OnDisable()
    {
        foreach (Transform child in ContentPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    IEnumerator ShowLoader()
    {
        float timer = 0;
        bool failed = false;
        Debug.Log("ShowLoader");
        Debug.Log("isFinished" + isFinished);
        isFinished = false;
        while (!isFinished)
        {
            if (timer > 5)
            {
                try
                {
                    GameObject downloading = GameObject.Find("downloading");
                    if (downloading == null)
                    {
                        if (GameObject.Find("LoadingHomeBloc").transform.localScale != Vector3.one)
                        {
                            GameObject.Find("LoadingHomeBloc").transform.localScale = Vector3.one;
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    GameObject.Find("LoadingHomeBloc").transform.localScale = Vector3.one;
                }
            }
            timer += Time.deltaTime;
            yield return null;
        }
        if (isFinished)
        {
            GameObject.Find("LoadingHomeBloc").transform.localScale = Vector3.zero;
        }
    }
    void OnEnable()
    {
        HomeController.NoLastResult = false;
        gradient = new Gradient();
        gradient = GradientComponent.GetComponent<Gradient>();
        ContentLastResult.SetActive(false);
        try
        {
            Items = new ArrayList();
            lastResultItem = new ArrayList();
            ItemsTournament = new JSONArray();
            UserId = userManager.getCurrentUserId();
            token = userManager.getCurrentSessionToken();

            EventsController nbs = new EventsController();
            UnityThreadHelper.CreateThread(() =>
            {
                Items = challengeManager.listChallenges(token);
                ItemsTournament = tournamentManager.getUserFinishedTournaments(token);
                foreach (Challenge item in Items)
                {
                    float? score1 = null;
                    try
                    {
                        score1 = item.user_1_score;
                    }
                    catch (NullReferenceException ex)
                    {
                        score1 = null;
                    }
                    float? score2 = null;
                    try
                    {
                        score2 = item.user_2_score;
                    }
                    catch (NullReferenceException ex)
                    {
                        score2 = null;
                    }
                    //Debug.Log(item.status);
                    if (item.status.Equals("ongoing") && (((item.matched_user_1._id == UserId && score1 == null)) || (item.matched_user_2._id == UserId && score2 == null)))
                    {
                        ChallengeManager.CurrentChallengeId = item._id;
                        ReplayChallenge.ChallengeToReplay = item;
                        UnityThreadHelper.Dispatcher.Dispatch(() =>
                        {
                            SceneManager.LoadScene("ReplayChallenge", LoadSceneMode.Additive);
                        });
                    }
                    if (item.status == "finished" || (item.status == "see results for user 1" && item.matched_user_2._id == UserId) || (item.status == "see results for user 2" && item.matched_user_1._id == UserId))
                    {
                        lastResultItem.Add(item);
                    }
                }
                if (lastResultItem == null && ItemsTournament == null)
                {
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        ContentLastResult.SetActive(false);
                    });
                }
                else
                {
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        int count = 0;
                        ContentLastResult.SetActive(true);
                        if (lastResultItem != null && ItemsTournament != null)
                        {
                            //Items = Sort (Items);
                            if (lastResultItem.Count == 0 && ItemsTournament.Count == 0)
                            {
                                ContentLastResult.SetActive(false);
                                HomeController.NoLastResult = true;
                                isFinished = true;
                            }
                            else
                                if (lastResultItem.Count + ItemsTournament.Count <= 4)
                            {
                                SeeMoreResult.gameObject.SetActive(false);
                                count = lastResultItem.Count + ItemsTournament.Count;
                                nbElement = count;
                            }
                            else
                            {
                                SeeMoreResult.gameObject.SetActive(true);
                                nbElement = 4;
                            }
                            show(nbElement);
                            SeeMoreResult.onClick.AddListener(() =>
                            {
                                nbElement += 4;
                                show(nbElement);
                                if (nbElement >= count)
                                {
                                    SeeMoreResult.gameObject.SetActive(false);
                                }
                            });
                        }
                    });
                }
            });
        }
        catch (NullReferenceException ex)
        {
        }
        catch (WebException ex) { }
    }
    // Update is called once per frame
    void Update()
    {
        nbChild = 0;
        foreach (Transform child in ContentPanel.transform)
        {
            nbChild++;
        }
        if (nbChild > nbElement)
        {
            Debug.Log("-Duplication Detected-");
            int count = 1;
            foreach (Transform child in ContentPanel.transform)
            {
                if (count == (nbChild / 2))
                {
                    DestroyImmediate(child.gameObject, true);
                }
                count++;
            }
        }
    }
    void show(int nbElement)
    {
        int count = 0;
        foreach (JSONNode item in ItemsTournament)
        {
            count++;
            if (count <= nbElement && count > nbElement - 4)
            {
                GameObject newItem = Instantiate(ListTournamentItemPrefab) as GameObject;
                LastResultTournamentListController controller = newItem.GetComponent<LastResultTournamentListController>();
                if (item["gain_type"].Value == TournamentManager.GAIN_TYPE_BUBBLE)
                {
                    controller.title.text = HomeTranslationController.WIN + " " + item["gain"].Value + " " + HomeTranslationController.BUBBLES;
                }
                else
                {
                    controller.title.text = HomeTranslationController.WIN + " " + item["gain"].Value + CurrencyManager.CURRENT_CURRENCY;
                }
                string date = item["createdAt"].Value.ToString().Substring(0, item["createdAt"].Value.ToString().IndexOf("T"));
                string hour = item["createdAt"].Value.ToString().Substring(item["createdAt"].Value.ToString().IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
                controller.date.text = date + " " + HomeTranslationController.AT + " " + hour;
                var lose = false;
                foreach (JSONNode loser in item["losers"].AsArray)
                {
                    if (loser.Value == UserId)
                    {
                        lose = true;
                        break;
                    }
                }
                if (lose)
                {
                    controller.defeat.text = HomeTranslationController.DEFEAT;
                    controller.defeat.transform.localScale = Vector3.one;
                }
                else if (lose == false && item["status"].Value == "finished")
                {
                    controller.victory.text = HomeTranslationController.VICTORY;
                    controller.victory.transform.localScale = Vector3.one;
                }
                controller.showResult.onClick.AddListener(() =>
                {
                    TournamentController.setCurrentTournamentID(item["_id"].Value);
                    SceneManager.LoadScene("Bracket");
                });
                newItem.transform.parent = ContentPanel.transform;
                RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
                myLayoutElement.transform.localScale = Vector3.one;
            }
        }
        foreach (Challenge item in lastResultItem)
        {
            if (item.status == "finished" || (item.status == "see results for user 1" && item.matched_user_2._id == UserId) || (item.status == "see results for user 2" && item.matched_user_1._id == UserId))
            {
                try
                {
                    if (item.user_1_score != null && item.matched_user_2 != null)
                    {
                        count++;
                        if (count <= nbElement && count > nbElement - 4)
                        {
                            GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
                            LastResultListController controller = newItem.GetComponent<LastResultListController>();
                            controller.matchId.text = item._id;
                            //06/04/2018 16:51:38
                            string date = item.CreatedAt.ToString().Substring(0, item.CreatedAt.ToString().IndexOf(" ")).Replace("/", "-");
                            string hour = item.CreatedAt.ToString().Substring(item.CreatedAt.ToString().IndexOf(" ") + 1, 5).Replace(":", "H") + "MIN";
                            UnityThreading.ActionThread myThread;
                            //gradient.enabled = false;
                            if (UserId == item.matched_user_1._id)
                            {
                                string UserToken = userManager.getCurrentSessionToken();
                                myThread = UnityThreadHelper.CreateThread(() =>
                                {
                                    //User user = userManager.getUser (item.matched_user_2._id, UserToken);
                                    Byte[] lnByte = userManager.getAvatar(item.matched_user_2.avatar);
                                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                                    {
                                        controller.AdvId.text = item.matched_user_2._id;
                                        Byte[] img1 = Convert.FromBase64String(userManager.GetFlagByte(item.matched_user_2.country_code));
                                        Texture2D txt1 = new Texture2D(1, 1);
                                        txt1.LoadImage(img1);
                                        Sprite newSprite1 = Sprite.Create(txt1 as Texture2D, new Rect(0f, 0f, txt1.width, txt1.height), Vector2.zero);
                                        controller.Drapeau.sprite = newSprite1;
                                        controller.Drapeau.transform.localScale = Vector3.one;
                                        try
                                        {
                                            controller.avatar.sprite = ImagesManager.getSpriteFromBytes(lnByte); ;
                                        }
                                        catch (NullReferenceException ex)
                                        {
                                        }
                                        controller.AdversaryName.text = item.matched_user_2.username;
                                        if (item.user_1_score > item.user_2_score)
                                        {
                                            controller.victory.text = HomeTranslationController.VICTORY;
                                            controller.victory.transform.localScale = Vector3.one;
                                            controller.result.text = HomeTranslationController.YOU_WON + controller.result.text;
                                        }
                                        else if (item.user_1_score < item.user_2_score)
                                        {
                                            controller.gainPro.transform.localScale = Vector3.zero;
                                            controller.gainBubble.transform.localScale = Vector3.zero;
                                            controller.proWon.transform.localScale = Vector3.zero;
                                            controller.bubbleWon.transform.localScale = Vector3.zero;
                                            controller.defeat.text = HomeTranslationController.DEFEAT;
                                            controller.defeat.transform.localScale = Vector3.one;
                                            controller.result.text = HomeTranslationController.YOU_LOST;
                                        }
                                        else if (item.user_1_score == item.user_2_score)
                                        {
                                            controller.gainPro.transform.localScale = Vector3.zero;
                                            controller.gainBubble.transform.localScale = Vector3.zero;
                                            controller.proWon.transform.localScale = Vector3.zero;
                                            controller.bubbleWon.transform.localScale = Vector3.zero;
                                            controller.equality.text = HomeTranslationController.EQUALITY;
                                            controller.equality.transform.localScale = Vector3.one;
                                            controller.result.text = HomeTranslationController.SCORE_DRAW;
                                        }
                                    });
                                });
                            }
                            else
                            {
                                string UserToken = userManager.getCurrentSessionToken();
                                myThread = UnityThreadHelper.CreateThread(() =>
                                {
                                    //User user = userManager.getUser (item.matched_user_1._id, UserToken);
                                    Byte[] lnByte = userManager.getAvatar(item.matched_user_1.avatar);
                                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                                    {
                                        try
                                        {
                                            controller.AdvId.text = item.matched_user_1._id;
                                        }
                                        catch (MissingReferenceException ex) { }
                                        Byte[] img1 = Convert.FromBase64String(userManager.GetFlagByte(item.matched_user_1.country_code));
                                        Texture2D txt1 = new Texture2D(1, 1);
                                        txt1.LoadImage(img1);
                                        Sprite newSprite1 = Sprite.Create(txt1 as Texture2D, new Rect(0f, 0f, txt1.width, txt1.height), Vector2.zero);
                                        try
                                        {
                                            controller.Drapeau.sprite = newSprite1;
                                            controller.Drapeau.transform.localScale = Vector3.one;
                                        }
                                        catch (MissingReferenceException)
                                        {
                                        }
                                        try
                                        {
                                            controller.avatar.sprite = ImagesManager.getSpriteFromBytes(lnByte); ;
                                        }
                                        catch (NullReferenceException ex)
                                        {
                                        }
                                        controller.AdversaryName.text = item.matched_user_1.username;
                                        try
                                        {
                                            if (item.user_1_score > item.user_2_score)
                                            {
                                                //controller.result.text = "You Lose";
                                                controller.gainPro.transform.localScale = Vector3.zero;
                                                controller.gainBubble.transform.localScale = Vector3.zero;
                                                controller.defeat.text = HomeTranslationController.DEFEAT;
                                                controller.defeat.transform.localScale = Vector3.one;
                                                controller.result.text = HomeTranslationController.YOU_LOST;
                                                //newItem.transform.GetChild (4).gameObject.GetComponent<Image>().transform.localScale=Vector3.zero;
                                                //gradient.enabled = false;
                                                //	newItem.transform.GetChild (4).gameObject.GetComponent<Image>().transform.localScale=Vector3.zero;
                                            }
                                            else if (item.user_1_score < item.user_2_score)
                                            {
                                                //controller.result.text = "You Won";
                                                controller.victory.text = HomeTranslationController.VICTORY;
                                                controller.victory.transform.localScale = Vector3.one;
                                                controller.result.text = HomeTranslationController.YOU_WON + " " + controller.result.text;
                                                //gradient.enabled = true;
                                                //newItem.transform.GetChild (4).gameObject.GetComponent<Image>().transform.localScale=Vector3.one;
                                            }
                                            else if (item.user_1_score == item.user_2_score)
                                            {
                                                controller.gainPro.transform.localScale = Vector3.zero;
                                                controller.gainBubble.transform.localScale = Vector3.zero;
                                                controller.proWon.transform.localScale = Vector3.zero;
                                                controller.bubbleWon.transform.localScale = Vector3.zero;
                                                //controller.lost.transform.localScale = Vector3.one;
                                                controller.equality.text = HomeTranslationController.EQUALITY;
                                                controller.equality.transform.localScale = Vector3.one;
                                                controller.result.text = HomeTranslationController.SCORE_DRAW;
                                            }
                                        }
                                        catch (FormatException ex)
                                        {
                                        }
                                    });
                                });
                            }
                            controller.GameDate.text = HomeTranslationController.DUEL + "- " + date + " " + HomeTranslationController.AT + " " + hour;
                            controller.avatar.GetComponentInChildren<Button>().onClick.AddListener(() =>
                            {
                                Profile.PlayerId = newItem.transform.GetChild(21).gameObject.GetComponent<Text>().text;
                                Profile.Avatar = newItem.transform.GetChild(20).gameObject.GetComponent<Image>().sprite;
                                ProfilLastResultListController.profileSceneOpened = true;
                                SceneManager.LoadScene("Profile", LoadSceneMode.Additive);
                                //ProfilePopup.SetActive(true);
                            });
                            controller.showResult.onClick.AddListener(() =>
                            {
                                ResultManager.AddGain = false;
                                UnityThreading.ActionThread thread;
                                ChallengeManager.CurrentChallengeId = newItem.transform.GetChild(18).gameObject.GetComponent<Text>().text;
                                SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
                                thread = UnityThreadHelper.CreateThread(() =>
                                {
                                    Challenge challenge = challengeManager.getChallenge(ChallengeManager.CurrentChallengeId, token);
                                    ChallengeManager.CurrentChallenge = challenge;
                                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                                    {
                                        string UserId = userManager.getCurrentUserId();
                                        float? scoreUser11 = challenge.user_1_score;
                                        float? scoreUser22 = challenge.user_2_score;
                                        if (challenge.user_1_score > challenge.user_2_score && challenge.matched_user_1._id == UserId)
                                        {
                                            SceneManager.LoadScene("ResultWin", LoadSceneMode.Additive);
                                        }
                                        else if (challenge.user_1_score < challenge.user_2_score && challenge.matched_user_1._id == UserId)
                                        {
                                            SceneManager.LoadScene("ResultLose", LoadSceneMode.Additive);
                                        }
                                        else if (challenge.user_1_score > challenge.user_2_score && challenge.matched_user_2._id == UserId)
                                        {
                                            SceneManager.LoadScene("ResultLose", LoadSceneMode.Additive);
                                        }
                                        else if (challenge.user_1_score < challenge.user_2_score && challenge.matched_user_2._id == UserId)
                                        {
                                            SceneManager.LoadScene("ResultWin", LoadSceneMode.Additive);
                                        }
                                        else if (challenge.user_1_score == challenge.user_2_score)
                                        {
                                            SceneManager.LoadScene("ResultEquality", LoadSceneMode.Additive);
                                        }
                                        SceneManager.UnloadScene("Loader");
                                    });
                                });
                            });
                            newItem.transform.parent = ContentPanel.transform;
                            RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
                            myLayoutElement.transform.localScale = Vector3.one;
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                }
            }
        }
        isFinished = true;
        PullToRefresh.lastResultfinished = true;
        InvokeRepeating("removeDuplicationLR", 0f, 0.2f);
    }
    void removeDuplicationLR()
    {
        if (ContentPanel.transform.childCount > nbElement)
        {
            CancelInvoke();
            int count = 0;
            foreach (Transform child in ContentPanel.transform)
            {
                if (count >= nbElement)
                {
                    DestroyObject(child.gameObject);
                }
                count++;
            }
        }
    }
}