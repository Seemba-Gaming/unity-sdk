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

    void OnDisable()
    {
        foreach (Transform child in ContentPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    void OnEnable()
    {
        HomeController.NoLastResult = false;
        gradient = new Gradient();
        gradient = GradientComponent.GetComponent<Gradient>();
        ContentLastResult.SetActive(false);
        Items = new ArrayList();
        lastResultItem = new ArrayList();
        ItemsTournament = new JSONArray();
        UserId = userManager.getCurrentUserId();
        token = userManager.getCurrentSessionToken();
        EventsController nbs = new EventsController();
        UnityThreadHelper.CreateThread(() => {
            Items = challengeManager.getFinishedChallenges(token);
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
                if (item.status.Equals("on going") && (((item.matched_user_1._id == UserId && score1 == null)) || (item.matched_user_2._id == UserId && score2 == null)))
                {
                    ChallengeManager.CurrentChallengeId = item._id;
                    ReplayChallengePresenter.ChallengeToReplay = item;
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
                UnityThreadHelper.Dispatcher.Dispatch(() =>{
                    int count = 0;
                    ContentLastResult.SetActive(true);
                    if (lastResultItem != null && ItemsTournament != null)
                    {
                        if (lastResultItem.Count == 0 && ItemsTournament.Count == 0)
                        {
                            ContentLastResult.SetActive(false);
                            HomeController.NoLastResult = true;
                            isFinished = true;
                        }
                        else if (lastResultItem.Count + ItemsTournament.Count <= 4)
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
                
                var created_at = string.IsNullOrEmpty(item["created_at"]) ? item["createdAt"].Value : item["created_at"].Value;
                
                var seperator_index = created_at.Contains("T") ? created_at.ToString().IndexOf("T") : created_at.ToString().IndexOf(" ");
                
                string date = created_at.ToString().Substring(0, seperator_index).Replace("/", "-");
                string hour = created_at.ToString().Substring(seperator_index + 1, 5).Replace(":", "H") + "MIN";
                
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
                if (item.user_1_score != null && item.matched_user_2 != null)
                {
                    count++;
                    if (count <= nbElement && count > nbElement - 4)
                    {
                        GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
                        LastResultListController controller = newItem.GetComponent<LastResultListController>();
                        if (UserId == item.matched_user_1._id)
                        {
                            SetOpponentDetails(controller, item.matched_user_2);
                        }
                        else
                        {
                            SetOpponentDetails(controller, item.matched_user_1);
                        }
                        SetChallengeDetails(controller, item);
                        controller.avatar.GetComponentInChildren<Button>().onClick.AddListener(() =>
                        {
                            ProfileViewPresenter.PlayerId = newItem.transform.GetChild(21).gameObject.GetComponent<Text>().text;
                            ProfileViewPresenter.Avatar = newItem.transform.GetChild(20).gameObject.GetComponent<Image>().sprite;
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

                                    Debug.Log("user_id    :" + UserId);
                                    Debug.Log("winner_user:" + challenge.winner_user);
                                    Debug.Log("user_1_id  :" + challenge.matched_user_1._id);
                                    Debug.Log("user_2_id  :" + challenge.matched_user_2._id);

                                    if (challenge.user_1_score == challenge.user_2_score)
                                    {
                                        SceneManager.LoadScene("ResultEquality", LoadSceneMode.Additive);
                                    }
                                    else if (challenge.winner_user.Equals(UserId))
                                    {
                                        SceneManager.LoadScene("ResultWin", LoadSceneMode.Additive);
                                    }
                                    else
                                    {
                                        SceneManager.LoadScene("ResultLose", LoadSceneMode.Additive);
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
        }
        isFinished = true;
        PullToRefresh.lastResultfinished = true;
        InvokeRepeating("removeDuplicationLR", 0f, 0.2f);
    }
    void SetOpponentDetails(LastResultListController controller, User user)
    {
        UnityThreadHelper.CreateThread(() =>
        {
            Byte[] lnByte = userManager.getAvatar(user.avatar);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                controller.AdvId.text = user._id;
                Byte[] bytes = Convert.FromBase64String(userManager.GetFlagByte(user.country_code));
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);
                Sprite sprite = Sprite.Create(texture as Texture2D, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
                controller.Drapeau.sprite = sprite;
                controller.Drapeau.transform.localScale = Vector3.one;
                controller.avatar.sprite = ImagesManager.getSpriteFromBytes(lnByte); ;
                controller.AdversaryName.text = user.username;
            });
        });
    }
    void SetChallengeDetails(LastResultListController controller, Challenge challenge)
    {
        Debug.Log(challenge.CreatedAt.ToString());
        var seperator_index = challenge.CreatedAt.Contains("T") ? challenge.CreatedAt.ToString().IndexOf("T") : challenge.CreatedAt.ToString().IndexOf(" ");
        string date = challenge.CreatedAt.ToString().Substring(0, seperator_index).Replace("/", "-");
        string hour = challenge.CreatedAt.ToString().Substring(seperator_index + 1, 5).Replace(":", "H") + "MIN";
        controller.GameDate.text = date + " AT " + hour;
        controller.matchId.text = challenge._id;

        if (challenge.user_1_score == challenge.user_2_score)
        {
            controller.equality.text = HomeTranslationController.EQUALITY;
            controller.equality.transform.localScale = Vector3.one;
            controller.result.text = HomeTranslationController.SCORE_DRAW;
        }
        else if (UserId.Equals(challenge.winner_user))
        {
            controller.victory.text = HomeTranslationController.VICTORY;
            controller.victory.transform.localScale = Vector3.one;
            controller.result.text = HomeTranslationController.YOU_WON + controller.result.text;
        }
        else
        {
            controller.defeat.text = HomeTranslationController.DEFEAT;
            controller.defeat.transform.localScale = Vector3.one;
            controller.result.text = HomeTranslationController.YOU_LOST;
        }

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