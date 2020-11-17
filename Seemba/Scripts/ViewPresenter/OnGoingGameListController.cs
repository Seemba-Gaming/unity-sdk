using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections.Generic;

[CLSCompliant(false)]
public class OnGoingGameListController : MonoBehaviour
{
    public GameObject   ContentPanel, ContentOngoing;
    public GameObject   ListItemPrefab;
    public GameObject   tournamentPrefab;
    public Button       SeeMoreResult;
    public int          nbElement = 0;
    string token;
    public Sprite[] spriteArray;
    ArrayList OnGoingChallenges = new ArrayList();
    ArrayList LastResults = new ArrayList();
    JSONArray ItemsTournamentsPending;
    GenericChallenge[] mChallengesList;
    private bool initialized = false;
    int page = 1;
    int pageSize = 4;
    async void OnEnable()
    {
        if (initialized == false)
        {
            nbElement = 0;
            page = 1; 
            initialized = true;
            HomeController.NoOngoing = false;
            InvokeRepeating("removeDuplication", 0f, 0.2f);
            token = UserManager.Get.getCurrentSessionToken();
            if (token != null)
            {
                mChallengesList = await ChallengeManager.Get.GetOnGoingChallenges(1, 4);
                if(mChallengesList.Length > 0)
                {
                    ContentPanel.SetActive(true);
                    ContentOngoing.SetActive(true);
                    DisplayOnGoingChallenges(mChallengesList);
                }
                else
                {
                    ContentOngoing.SetActive(false);
                    ContentPanel.SetActive(false);
                }
                SeeMoreResult.onClick.RemoveAllListeners();
                SeeMoreResult.onClick.AddListener(async () =>
                {
                    page++;
                    mChallengesList = await ChallengeManager.Get.GetOnGoingChallenges(page, 4);
                    if (mChallengesList.Length > 0)
                    {
                        DisplayOnGoingChallenges(mChallengesList);
                    }
                    else
                    {
                        SeeMoreResult.gameObject.SetActive(false);
                    }
                });
                PullToRefresh.ongoingfinished = true;
            }
            else
            {
                Debug.LogWarning(initialized);
            }
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
        nbElement = 0;
        CancelInvoke("removeDuplication");
    }
    void DisplayOnGoingChallenges(GenericChallenge[] list)
    {
        nbElement += mChallengesList.Length;

        if (mChallengesList.Length == 4)
        {
            SeeMoreResult.gameObject.SetActive(true);
        }
        else
        {
            SeeMoreResult.gameObject.SetActive(false);
        }
        foreach (GenericChallenge challenge in list)
        {
            if (challenge.tournament_id != null)
            {
                InitOnGoingTournament(challenge);
            }
            else
            {
                InitOnGoingChallenge(challenge);
            }
        }
    }
    private void InitOnGoingTournament(GenericChallenge challenge)
    {
        GameObject newItem = Instantiate(tournamentPrefab) as GameObject;
        OnGoingTournamentListItemController controller = newItem.GetComponent<OnGoingTournamentListItemController>();
        string date = challenge.createdAt.Substring(0, challenge.createdAt.IndexOf("T"));
        string hour = challenge.createdAt.Substring(challenge.createdAt.IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
        controller.Date.text = date + " " + HomeTranslationController.AT + " " + hour;
        TranslationManager.scene = "Home";
        controller.status.text = TranslationManager.Get("pending");
        controller.tournamentId.text = challenge.tournament_id;
        controller.gainType = challenge.tournament.gain_type;
        controller.CreatedAt = challenge.tournament.createdAt;
        controller.gain = challenge.tournament.gain;
        SetControllerTournamentTitle(challenge, controller);
        controller.GoToBracket.onClick.AddListener(() =>
        {
            TournamentController.setCurrentTournamentID(challenge.tournament_id);
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Brackets.gameObject);
        });
        newItem.transform.SetParent(ContentPanel.transform);
        RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
        myLayoutElement.transform.localScale = Vector3.one;
    }
    private void InitOnGoingChallenge(GenericChallenge challenge)
    {
        GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
        OnGoingGameListItemController controller = newItem.GetComponent<OnGoingGameListItemController>();
        controller.challengeResultId.text = challenge._id;
        controller.challengeId.text = challenge._id;
        SetControllerTitle(challenge, controller);
        if(challenge.status.Equals("results pending") || challenge.status.Equals("pending"))
        {
            TranslationManager.scene = "Home";
            controller.pending_text.text = TranslationManager.Get("pending");
            controller.SeeResult.transform.localScale = Vector3.zero;
            controller.Result.gameObject.SetActive(true);
            controller.Result.onClick.AddListener(async () =>
            {
                LoaderManager.Get.LoaderController.ShowLoader(null);
                ChallengeManager.CurrentChallengeId = challenge._id;
                Challenge mCurrentChallenge = await ChallengeManager.Get.getChallenge(ChallengeManager.CurrentChallengeId);
                ChallengeManager.CurrentChallenge = mCurrentChallenge;
                ChallengeManager.Get.ShowResult();
                LoaderManager.Get.LoaderController.HideLoader();

            });
            controller.SeeResult.gameObject.SetActive(false);
        }
        else if (challenge.status.Equals("see results for user 1"))
        {
            controller.pending_text.text = HomeTranslationController.GAME_FINISHED;
            controller.SeeResult.transform.localScale = Vector3.one;
            controller.SeeResult.gameObject.SetActive(true);
            controller.SeeResult.onClick.AddListener(async () =>
            {
                ChallengeManager.CurrentChallengeId = challenge._id;
                LoaderManager.Get.LoaderController.ShowLoader(null);

                Challenge mUpdatedChallenge = await ChallengeManager.Get.UpdateChallengeStatusToFinishedAsync(token, ChallengeManager.CurrentChallengeId);
                Challenge Selectedchallenge = await ChallengeManager.Get.getChallenge(ChallengeManager.CurrentChallengeId);

                ChallengeManager.CurrentChallenge = Selectedchallenge;
                ChallengeManager.Get.ShowResult();
                LoaderManager.Get.LoaderController.HideLoader();

            });
            controller.Result.gameObject.SetActive(false);
        }
        string date = challenge.createdAt.ToString().Substring(0, challenge.createdAt.ToString().IndexOf("T"));
        string hour = challenge.createdAt.ToString().Substring(challenge.createdAt.ToString().IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
        controller.status.text = date + " " + HomeTranslationController.AT + " " + hour;
        newItem.transform.SetParent(ContentPanel.transform);
        RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
        myLayoutElement.transform.localScale = Vector3.one;
    }
    void SetControllerTournamentTitle(GenericChallenge item, OnGoingTournamentListItemController controller)
    {
        if (item.gain_type == TournamentManager.GAIN_TYPE_CASH)
        {
            switch (float.Parse(item.gain))
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
            switch (float.Parse(item.gain))
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
    void SetControllerTitle(GenericChallenge item, OnGoingGameListItemController controller)
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
    void removeDuplication()
    {
        if (ContentPanel.transform.childCount > nbElement)
        {
            CancelInvoke();
            int count = 0;
            foreach (Transform child in ContentPanel.transform)
            {
                if (count >= nbElement)
                {
                    Destroy(child.gameObject);
                }
                count++;
            }
        }
    }

    #region oldCode
    //Items1vs1SeeResults = await ChallengeManager.Get.getSeeResultsChallenges(token);
    // Items1vs1Pending = await ChallengeManager.Get.getPendingChallenges(token);
    //ItemsTournamentsPending = new JSONArray();
    // ItemsTournamentsPending = await TournamentManager.Get.getUserPendingTournaments(token);

    //if (ItemsTournamentsPending.Count != 0)
    //{
    //    ContentOngoing.SetActive(true);
    //    foreach (JSONNode item in ItemsTournamentsPending)
    //    {
    //        GameObject newItem = Instantiate(tournamentPrefab) as GameObject;
    //        OnGoingTournamentListItemController controller = newItem.GetComponent<OnGoingTournamentListItemController>();
    //        switch (item["nb_players"].AsInt)
    //        {
    //            case TournamentManager.TOURNAMENT_8: break;
    //            case TournamentManager.TOURNAMENT_16: break;
    //            case TournamentManager.TOURNAMENT_32: break;
    //        }
    //        string date = item["createdAt"].Value.ToString().Substring(0, item["createdAt"].Value.ToString().IndexOf("T"));
    //        string hour = item["createdAt"].Value.ToString().Substring(item["createdAt"].Value.ToString().IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
    //        controller.status.text = date + " " + HomeTranslationController.AT + " " + hour;
    //        controller.tournamentId.text = item["_id"].Value;
    //        SetControllerTournamentTitle(item, controller);
    //        controller.GoToBracket.onClick.AddListener(() =>
    //        {
    //            TournamentController.setCurrentTournamentID(item["_id"].Value);
    //            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Brackets.gameObject);
    //        });
    //        newItem.transform.SetParent(ContentPanel.transform);
    //        RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
    //        myLayoutElement.transform.localScale = Vector3.one;
    //    }
    //}
    //if (LastResults.Count != 0)
    //{
    //    ContentOngoing.SetActive(true);
    //    foreach (Challenge item in LastResults)
    //    {
    //        GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
    //        OnGoingGameListItemController controller = newItem.GetComponent<OnGoingGameListItemController>();
    //        controller.challengeResultId.text = item._id;
    //        controller.challengeId.text = item._id;
    //        SetControllerTitle(item, controller);
    //        controller.Result.transform.localScale = Vector3.zero;
    //        controller.SeeResult.gameObject.SetActive(true);
    //        controller.status.text = HomeTranslationController.GAME_FINISHED;
    //        controller.SeeResult.onClick.AddListener(async () =>
    //        {
    //            ChallengeManager.CurrentChallengeId = newItem.transform.GetChild(8).gameObject.GetComponent<Text>().text;
    //            LoaderManager.Get.LoaderController.ShowLoader(null);

    //            Challenge challenge = await ChallengeManager.Get.UpdateChallengeStatusToFinishedAsync(token, ChallengeManager.CurrentChallengeId);
    //            Challenge Selectedchallenge = await ChallengeManager.Get.getChallenge(ChallengeManager.CurrentChallengeId);

    //            ChallengeManager.CurrentChallenge = Selectedchallenge;
    //            ChallengeManager.Get.ShowResult();
    //            LoaderManager.Get.LoaderController.HideLoader();

    //        });
    //        ContentOngoing.SetActive(true);
    //        newItem.transform.SetParent(ContentPanel.transform);
    //        RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
    //        myLayoutElement.transform.localScale = Vector3.one;
    //    }
    //}
    //if (OnGoingChallenges.Count != 0)
    //{
    //    ContentOngoing.SetActive(true);
    //    foreach (Challenge item in OnGoingChallenges)
    //    {
    //        float? score1 = null;
    //        try
    //        {
    //            score1 = item.user_1_score;
    //        }
    //        catch (NullReferenceException)
    //        {
    //            score1 = null;
    //        }
    //        float? score2 = null;
    //        try
    //        {
    //            score2 = item.user_2_score;
    //        }
    //        catch (NullReferenceException)
    //        {
    //            score2 = null;
    //        }
    //        string matched_user_2_id = null;
    //        try
    //        {
    //            matched_user_2_id = item.matched_user_2._id;
    //        }
    //        catch (NullReferenceException)
    //        {
    //            matched_user_2_id = null;
    //        }
    //        if ((item.matched_user_1._id == userId && score1 == null) || (matched_user_2_id == userId && score2 == null))
    //        {
    //            ChallengeManager.CurrentChallengeId = item._id;
    //            ReplayChallengePresenter.ChallengeToReplay = item;
    //            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.ReplayChallenge.gameObject);
    //            return;
    //        }
    //        else
    //        {
    //            GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
    //            OnGoingGameListItemController controller = newItem.GetComponent<OnGoingGameListItemController>();
    //            controller.challengeResultId.text = item._id;
    //            controller.challengeId.text = item._id;
    //            SetControllerTitle(item, controller);
    //            controller.Result.gameObject.SetActive(true);
    //            string date = item.CreatedAt.ToString().Substring(0, item.CreatedAt.ToString().IndexOf("T"));
    //            string hour = item.CreatedAt.ToString().Substring(item.CreatedAt.ToString().IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
    //            controller.status.text = date + " " + HomeTranslationController.AT + " " + hour;
    //            controller.pending_text.text = TranslationManager.Get("pending") != string.Empty ? TranslationManager.Get("pending") : "pending"; ;
    //            controller.SeeResult.transform.localScale = Vector3.zero;
    //            controller.Result.onClick.AddListener(async () =>
    //            {
    //                LoaderManager.Get.LoaderController.ShowLoader(null);
    //                ChallengeManager.CurrentChallengeId = newItem.transform.GetChild(8).gameObject.GetComponent<Text>().text;
    //                Challenge challenge = await ChallengeManager.Get.getChallenge(ChallengeManager.CurrentChallengeId);
    //                ChallengeManager.CurrentChallenge = challenge;
    //                ChallengeManager.Get.ShowResult();
    //                LoaderManager.Get.LoaderController.HideLoader();

    //            });
    //            newItem.transform.SetParent(ContentPanel.transform);
    //            RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
    //            myLayoutElement.transform.localScale = Vector3.one;
    //        }
    //    }
    //}
    #endregion


}