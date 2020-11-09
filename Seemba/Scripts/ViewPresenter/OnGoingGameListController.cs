using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using SimpleJSON;

[CLSCompliant(false)]
public class OnGoingGameListController : MonoBehaviour
{
    public GameObject ContentPanel, ContentOngoing;
    public GameObject ListItemPrefab;
    public GameObject tournamentPrefab;
    ArrayList ItemsBracket;
    string userId;
    string token;
    public Sprite[] spriteArray;
    ArrayList Items1vs1Pending = new ArrayList();
    ArrayList Items1vs1SeeResults = new ArrayList();
    JSONArray ItemsTournamentsPending;

    private bool initialized = false;
    public bool ItemsBracketFinished()
    {
        if (ItemsBracket == null)
        {
            return true;
        }
        return false;
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
                controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_CASH_CONFIDENT + CurrencyManager.CURRENT_CURRENCY;
            }
            else if (item.gain == ChallengeManager.WIN_1V1_CASH_CHAMPION.ToString())
            {
                controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_CASH_CHAMPION + CurrencyManager.CURRENT_CURRENCY;
            }
            else if (item.gain == ChallengeManager.WIN_1V1_CASH_LEGEND.ToString())
            {
                controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_CASH_LEGEND + CurrencyManager.CURRENT_CURRENCY;
            }
        }
        else
        {
            if (item.gain == ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT.ToString())
            {
                controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT + " " + HomeTranslationController.BUBBLES;
            }
            else if (item.gain == ChallengeManager.WIN_1V1_BUBBLES_CHAMPION.ToString())
            {
                controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_BUBBLES_CHAMPION + " " + HomeTranslationController.BUBBLES;
            }
            else if (item.gain == ChallengeManager.WIN_1V1_BUBBLES_LEGEND.ToString())
            {
                controller.titre.text = HomeTranslationController.WIN + " " + ChallengeManager.WIN_1V1_BUBBLES_LEGEND + " " + HomeTranslationController.BUBBLES;
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
    async void OnEnable()
    {
        if (initialized == false)
        {
            initialized = true;
            HomeController.NoOngoing = false;
            InvokeRepeating("removeDuplication", 0f, 0.2f);
            userId = UserManager.Get.getCurrentUserId();
            token = UserManager.Get.getCurrentSessionToken();
            if(token != null)
            {
                ArrayList ListItems = new ArrayList();
                ArrayList controllers = new ArrayList();
                UnityThreading.ActionThread thread;

                Items1vs1SeeResults = await ChallengeManager.Get.getSeeResultsChallenges(token);
                Items1vs1Pending = await ChallengeManager.Get.getPendingChallenges(token);
                ItemsTournamentsPending = new JSONArray();
                ItemsTournamentsPending = await TournamentManager.Get.getUserPendingTournaments(token);

                ContentOngoing.SetActive(true);
                ContentOngoing.SetActive(false);
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
                        string date = item["createdAt"].Value.ToString().Substring(0, item["createdAt"].Value.ToString().IndexOf("T"));
                        string hour = item["createdAt"].Value.ToString().Substring(item["createdAt"].Value.ToString().IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
                        controller.status.text = date + " " + HomeTranslationController.AT + " " + hour;
                        controller.tournamentId.text = item["_id"].Value;
                        SetControllerTournamentTitle(item, controller);
                        controller.GoToBracket.onClick.AddListener(() =>
                        {
                            TournamentController.setCurrentTournamentID(item["_id"].Value);
                            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Brackets.gameObject);
                        });
                        newItem.transform.SetParent(ContentPanel.transform);
                        RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
                        myLayoutElement.transform.localScale = Vector3.one;
                    }
                }

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
                        controller.SeeResult.onClick.AddListener(async () =>
                        {
                            ChallengeManager.CurrentChallengeId = newItem.transform.GetChild(8).gameObject.GetComponent<Text>().text;
                            LoaderManager.Get.LoaderController.ShowLoader(null);

                            Challenge challenge = await ChallengeManager.Get.UpdateChallengeStatusToFinishedAsync(token, ChallengeManager.CurrentChallengeId);
                            Challenge Selectedchallenge = await ChallengeManager.Get.getChallenge(ChallengeManager.CurrentChallengeId);

                            ChallengeManager.CurrentChallenge = Selectedchallenge;
                            ChallengeManager.Get.ShowResult();
                            LoaderManager.Get.LoaderController.HideLoader();

                        });
                        ContentOngoing.SetActive(true);
                        newItem.transform.SetParent(ContentPanel.transform);
                        RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
                        myLayoutElement.transform.localScale = Vector3.one;
                    }
                }
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
                            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.ReplayChallenge.gameObject);
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
                            string date = item.CreatedAt.ToString().Substring(0, item.CreatedAt.ToString().IndexOf("T"));
                            string hour = item.CreatedAt.ToString().Substring(item.CreatedAt.ToString().IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
                            controller.status.text = date + " " + HomeTranslationController.AT + " " + hour;
                            controller.pending_text.text = TranslationManager.Get("pending") != string.Empty ? TranslationManager.Get("pending") : "pending"; ;
                            controller.SeeResult.transform.localScale = Vector3.zero;
                            controller.Result.onClick.AddListener(async () =>
                            {
                                LoaderManager.Get.LoaderController.ShowLoader(null);
                                ChallengeManager.CurrentChallengeId = newItem.transform.GetChild(8).gameObject.GetComponent<Text>().text;
                                Challenge challenge = await ChallengeManager.Get.getChallenge(ChallengeManager.CurrentChallengeId);
                                ChallengeManager.CurrentChallenge = challenge;
                                ChallengeManager.Get.ShowResult();
                                LoaderManager.Get.LoaderController.HideLoader();

                            });
                            newItem.transform.SetParent(ContentPanel.transform);
                            RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
                            myLayoutElement.transform.localScale = Vector3.one;
                        }
                    }
                }
                if (ContentPanel.transform.childCount <= 0)
                {
                    HomeController.NoOngoing = true;
                    ContentPanel.SetActive(false);
                }
                PullToRefresh.ongoingfinished = true;
            }
            else
            {
                Debug.LogWarning(initialized);
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
                        Destroy(child.gameObject);
                    }
                    count++;
                }
            }
        }
        catch (NullReferenceException ex)
        {
        }
    }
    private async void Win()
    {
        string[] attrib = { "last_result" };
        string[] values = { "win" };
        UserManager.Get.UpdateUserByField(attrib, values);
    }
    private async void Loss()
    {
        string[] attrib = { "last_result" };
        string[] values = { "loss" };
        UserManager.Get.UpdateUserByField(attrib, values);
    }
    // Update is called once per frame
    void Update()
    {

    }
}