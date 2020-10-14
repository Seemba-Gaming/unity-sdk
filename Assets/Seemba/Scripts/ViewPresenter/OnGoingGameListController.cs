using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using SimpleJSON;
public class OnGoingGameListController : MonoBehaviour
{
    public GameObject ContentPanel, ContentOngoing;
    public GameObject ListItemPrefab;
    public GameObject tournamentPrefab;
    UserManager um = new UserManager();
    TournamentManager tm = new TournamentManager();
    string userId;
    string token;
    ChallengeManager challengeManager = new ChallengeManager();
    public Sprite GainArgent, GainGoutte;
    int count = 0;
    public Sprite[] spriteArray;
    ArrayList Items1vs1Pending = new ArrayList();
    ArrayList Items1vs1SeeResults = new ArrayList();
    JSONArray ItemsTournamentsPending;
    int onGoingItemsCount;
    public VerticalLayoutGroup verticalLayoutGroupForOngoing, ContentLastResult;
    private bool initialized = false;
    // Use this for initialization

    void OnEnable()
    {
        if (initialized == false)
        {
            initialized = true;
            userId = um.getCurrentUserId();
            token = um.getCurrentSessionToken();

            HomeController.NoOngoing = false;

            InvokeRepeating("removeDuplication", 0f, 0.2f);

            UnityThreadHelper.CreateThread(() =>
            {
                Items1vs1SeeResults = challengeManager.getSeeResultsChallenges(token);
                Items1vs1Pending = challengeManager.getPendingChallenges(token);
                ItemsTournamentsPending = tm.getUserPendingTournaments(token);
                onGoingItemsCount = Items1vs1SeeResults.Count + Items1vs1Pending.Count;
                
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    SeeResultChallenges();
                    PendingTournament();
                    PendingChallenges();

                    if (ContentPanel.transform.childCount <= 0)
                    {
                        HomeController.NoOngoing = true;
                        ContentPanel.SetActive(false);
                    }
                    PullToRefresh.ongoingfinished = true;
                });
                
            });
        }
    }
    void OnDisable()
    {
        initialized = false;
        foreach (Transform child in ContentPanel.transform)
        {
            if (child.name != "PARTIES EN COURS")
            {
                Destroy(child.gameObject);
            }
        }
    }
    void SetControllerTitle(Challenge item, OnGoingGameListItemController controller)
    {
        if (item.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
        {
            if (item.gain == ChallengeManager.WIN_1V1_CASH_CONFIDENT.ToString())
            {
                //controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_PRO_CONFIDENT + CurrencyManager.CURRENT_CURRENCY;
                controller.titre.text = "WIN" + " " + ChallengeManager.WIN_1V1_CASH_CONFIDENT + CurrencyManager.CURRENT_CURRENCY;
            }
            else if (item.gain == ChallengeManager.WIN_1V1_CASH_CHAMPION.ToString())
            {
                //controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_PRO_CHAMPION + CurrencyManager.CURRENT_CURRENCY;
                controller.titre.text = "WIN" + " " + ChallengeManager.WIN_1V1_CASH_CHAMPION + CurrencyManager.CURRENT_CURRENCY;
            }
            else if (item.gain == ChallengeManager.WIN_1V1_CASH_LEGEND.ToString())
            {
                //controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_PRO_LEGEND + CurrencyManager.CURRENT_CURRENCY;
                controller.titre.text = "WIN" + " " + ChallengeManager.WIN_1V1_CASH_LEGEND + CurrencyManager.CURRENT_CURRENCY;
            }
        }
        else
        {
            if (item.gain == ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT.ToString())
            {
                //controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT + " " + HomeTranslationController.BUBBLES;
                controller.titre.text = "WIN" + " " + ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT + " " + "BUBBLES";
            }
            else if (item.gain == ChallengeManager.WIN_1V1_BUBBLES_CHAMPION.ToString())
            {
                //controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_BUBBLES_CHAMPION + " " + HomeTranslationController.BUBBLES;
                controller.titre.text = "WIN" + " " + ChallengeManager.WIN_1V1_BUBBLES_CHAMPION + " " + "BUBBLES";
            }
            else if (item.gain == ChallengeManager.WIN_1V1_BUBBLES_LEGEND.ToString())
            {
                //controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_BUBBLES_LEGEND + " " + HomeTranslationController.BUBBLES;
                controller.titre.text = "WIN" + " " + ChallengeManager.WIN_1V1_BUBBLES_LEGEND + " " + "BUBBLES";
            }
        }
    }
    void SetControllerTournamentTitle(JSONNode item, OnGoingTournamentListItemController controller)
    {
        if (item["gain_type"].Value == TournamentManager.GAIN_TYPE_CASH)
        {
            switch (item["gain"].AsFloat)
            {
                case TournamentManager.WIN_BRACKET_CASH_CONFIDENT:
                    controller.titre.text = HomeTranslationController.WIN + " " + TournamentManager.WIN_BRACKET_CASH_CONFIDENT + CurrencyManager.CURRENT_CURRENCY;
                    break;
                case TournamentManager.WIN_BRACKET_CASH_CHAMPION:
                    controller.titre.text = HomeTranslationController.WIN + " " + TournamentManager.WIN_BRACKET_CASH_CHAMPION + CurrencyManager.CURRENT_CURRENCY;
                    break;
                case TournamentManager.WIN_BRACKET_CASH_LEGEND:
                    controller.titre.text = HomeTranslationController.WIN + " " + TournamentManager.WIN_BRACKET_CASH_LEGEND + CurrencyManager.CURRENT_CURRENCY;
                    break;
            }
        }
        else
        {
            switch (item["gain"].AsFloat)
            {
                case TournamentManager.WIN_BRACKET_BUBBLE_CONFIDENT:
                    controller.titre.text = HomeTranslationController.WIN + " " + TournamentManager.WIN_BRACKET_BUBBLE_CONFIDENT + " " + HomeTranslationController.BUBBLES;
                    break;
                case TournamentManager.WIN_BRACKET_BUBBLE_CHAMPION:
                    controller.titre.text = HomeTranslationController.WIN + " " + TournamentManager.WIN_BRACKET_BUBBLE_CHAMPION + " " + HomeTranslationController.BUBBLES;
                    break;
                case TournamentManager.WIN_BRACKET_BUBBLE_LEGEND:
                    controller.titre.text = HomeTranslationController.WIN + " " + TournamentManager.WIN_BRACKET_BUBBLE_LEGEND + " " + HomeTranslationController.BUBBLES;
                    break;
            }
        }
    }
    void PendingChallenges()
    {
        if (Items1vs1Pending.Count != 0)
        {

            ContentOngoing.SetActive(true);
            foreach (Challenge item in Items1vs1Pending)
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
                string matched_user_2_id = null;
                try
                {
                    matched_user_2_id = item.matched_user_2._id;
                }
                catch (NullReferenceException ex)
                {
                    matched_user_2_id = null;
                }
                if ((item.matched_user_1._id == userId && score1 == null) || (matched_user_2_id == userId && score2 == null))
                {
                    ChallengeManager.CurrentChallengeId = item._id;
                    ReplayChallengePresenter.ChallengeToReplay = item;
                    SceneManager.LoadScene("ReplayChallenge", LoadSceneMode.Additive);
                    return;
                }
                else
                {

                    GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
                    OnGoingGameListItemController controller = newItem.GetComponent<OnGoingGameListItemController>();

                    controller.challengeResultId.text = item._id;
                    controller.challengeId.text = item._id;
                    SetControllerTitle(item, controller);
                    controller.Result.gameObject.SetActive(true);

                    var seperator_index = item.CreatedAt.ToString().Contains("T") ? item.CreatedAt.ToString().IndexOf("T") : item.CreatedAt.ToString().IndexOf(" ");
                    string date = item.CreatedAt.ToString().Substring(0, seperator_index).Replace("/", "-");
                    string hour = item.CreatedAt.ToString().Substring(seperator_index + 1, 5).Replace(":", "H") + "MIN";
                    
                    
                    controller.status.text = date + " " + HomeTranslationController.AT + " " + hour;
                    controller.status.text = date + " " + "AT" + " " + hour;
                    controller.pending_text.text = TranslationManager.Get("pending") != string.Empty ? TranslationManager.Get("pending") : "pending"; ;
                    controller.SeeResult.transform.localScale = Vector3.zero;
                    controller.Result.onClick.AddListener(() =>
                    {
                        ChallengeManager.CurrentChallengeId = newItem.transform.GetChild(8).gameObject.GetComponent<Text>().text;
                        SceneManager.LoadScene("ResultWaiting", LoadSceneMode.Additive);
                    });
                    newItem.transform.parent = ContentPanel.transform;
                    newItem.GetComponent<RectTransform>().transform.localScale = Vector3.one;
                    newItem.GetComponent<RectTransform>().transform.position = Vector3.one;
                }
            }
        }
    }
    void PendingTournament()
    {
        if (ItemsTournamentsPending.Count != 0)
        {
            ContentOngoing.SetActive(true);
            foreach (JSONNode item in ItemsTournamentsPending)
            {
                GameObject newItem = Instantiate(tournamentPrefab) as GameObject;
                OnGoingTournamentListItemController controller = newItem.GetComponent<OnGoingTournamentListItemController>();
                switch (item["nb_players"].AsInt)
                {
                    case TournamentManager.TOURNAMENT_8: break;
                    case TournamentManager.TOURNAMENT_16: break;
                    case TournamentManager.TOURNAMENT_32: break;
                }
                var created_at = string.IsNullOrEmpty(item["created_at"]) ? item["createdAt"].Value : item["created_at"].Value;
                
                var seperator_index = created_at.Contains("T") ? created_at.ToString().IndexOf("T") : created_at.ToString().IndexOf(" ");
                
                string date = created_at.ToString().Substring(0, seperator_index).Replace("/", "-");
                string hour = created_at.ToString().Substring(seperator_index + 1, 5).Replace(":", "H") + "MIN";
                
                controller.status.text = date + " " + HomeTranslationController.AT + " " + hour;
                controller.tournamentId.text = item["_id"].Value;
                SetControllerTournamentTitle(item, controller);
                controller.GoToBracket.onClick.AddListener(() =>
                {

                    TournamentController.setCurrentTournamentID(item["_id"].Value);
                    SceneManager.LoadScene("Bracket");
                });
                newItem.transform.parent = ContentPanel.transform;
                RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
                myLayoutElement.transform.localScale = Vector3.one;
            }
        }
    }
    void SeeResultChallenges()
    {
        if (Items1vs1SeeResults.Count != 0)
        {
            ContentOngoing.SetActive(true);
            foreach (Challenge item in Items1vs1SeeResults)
            {
                GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
                OnGoingGameListItemController controller = newItem.GetComponent<OnGoingGameListItemController>();
                controller.challengeResultId.text = item._id;
                controller.challengeId.text = item._id;
                SetControllerTitle(item, controller);
                controller.Result.transform.localScale = Vector3.zero;
                controller.SeeResult.gameObject.SetActive(true);
                controller.status.text = HomeTranslationController.GAME_FINISHED;
                controller.SeeResult.onClick.AddListener(() =>
                {
                    ResultManager.AddGain = true;
                    ChallengeManager.CurrentChallengeId = newItem.transform.GetChild(8).gameObject.GetComponent<Text>().text;
                    SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
                    UnityThreadHelper.CreateThread(() =>
                    {
                        Challenge challenge=challengeManager.UpdateChallengeStatusToFinished(token, ChallengeManager.CurrentChallengeId);
                        Challenge selectedChallenge = challengeManager.getChallenge(ChallengeManager.CurrentChallengeId, token);
                        UnityThreadHelper.Dispatcher.Dispatch(() =>
                        {
                            float? score1 = selectedChallenge.user_1_score;
                            float? score2 = selectedChallenge.user_2_score;
                            if (selectedChallenge.user_1_score == selectedChallenge.user_2_score)
                            {
                                SceneManager.LoadScene("ResultEquality");
                            }
                            else if (selectedChallenge.winner_user.Equals(userId))
                            {
                                if (challenge.matched_user_1._id.Equals(userId))
                                {
                                    UserManager.UpdateUserCredit(challenge.matched_user_1.money_credit.ToString(),challenge.matched_user_1.bubble_credit.ToString());
                                }
                                else if (challenge.matched_user_2._id.Equals(userId))
                                {
                                    UserManager.UpdateUserCredit(challenge.matched_user_2.money_credit.ToString(), challenge.matched_user_2.bubble_credit.ToString());
                                }
                                SceneManager.LoadScene("ResultWin");
                            }
                            else
                            {
                                SceneManager.LoadScene("ResultLose");
                            }
                            SceneManager.UnloadScene("Loader");
                        });
                    });
                });
                ContentOngoing.SetActive(true);
                newItem.transform.parent = ContentPanel.transform;
                RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
                myLayoutElement.transform.localScale = Vector3.one;
            }
        }

    }
    void removeDuplication()
    {
        try
        {
            if (ContentPanel.transform.childCount > Items1vs1SeeResults.Count + Items1vs1Pending.Count + ItemsTournamentsPending.Count)
            {
                Debug.Log("-Duplication Detected-");
                int count = 0;
                foreach (Transform child in ContentPanel.transform)
                {
                    if (count >= Items1vs1SeeResults.Count + Items1vs1Pending.Count + Items1vs1Pending.Count + ItemsTournamentsPending.Count)
                    {
                        DestroyObject(child.gameObject);
                    }
                    count++;
                }
            }
        }
        catch (NullReferenceException ex)
        {
        }
    }
    private void Win()
    {
        string[] attrib = { "last_result" };
        string[] values = { "win" };
        um.UpdateUserByField(userId, token, attrib, values);
    }
    private void Loss()
    {
        string[] attrib = { "last_result" };
        string[] values = { "loss" };
        um.UpdateUserByField(userId, token, attrib, values);
    }

}