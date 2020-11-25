using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using SimpleJSON;

[CLSCompliant(false)]
public class LastResultListItemController : MonoBehaviour
{
    public GameObject ContentPanel;
    public GameObject ProfilePopup;
    public GameObject ListItemPrefab;
    public GameObject ListTournamentItemPrefab;
    //public GameObject GradientComponent;
    ArrayList Items, lastResultItem;
    JSONArray ItemsTournament;
    //private Gradient gradient;
    public GameObject ContentLastResult, PanelObjects;
    public int nbElement = 0;
    public Button SeeMoreResult;
    public Sprite[] spriteArray;
    public string gaintext;
    public int Length = 0;
    private int nbChild = 0;
    private string UserId, token;
    GenericChallenge[] mChallengesList;
    int page = 1;
    int pageSize = 4;
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
    async void OnEnable()
    {
        nbElement = 0;
        page = 1;
        HomeController.NoLastResult = false;
        ContentLastResult.SetActive(false);
        Items = new ArrayList();
        lastResultItem = new ArrayList();
        ItemsTournament = new JSONArray();
        UserId = UserManager.Get.getCurrentUserId();
        token = UserManager.Get.getCurrentSessionToken();
        if(token != null)
        {
            mChallengesList = await ChallengeManager.Get.GetLastResulatChallenges(1,pageSize);

            if (mChallengesList.Length > 0)
            {
                ContentPanel.SetActive(true);
                ContentLastResult.SetActive(true);
                await DisplayLastResultsChallengesAsync(mChallengesList);
                SeeMoreResult.onClick.RemoveAllListeners();
                SeeMoreResult.onClick.AddListener(async () =>
                {
                    LoaderManager.Get.LoaderController.ShowLoader();
                    page++;
                    mChallengesList = await ChallengeManager.Get.GetLastResulatChallenges(page, pageSize);
                    if (mChallengesList.Length > 0)
                    {
                        await DisplayLastResultsChallengesAsync(mChallengesList);
                    }
                    else
                    {
                        SeeMoreResult.gameObject.SetActive(false);
                    }
                    LoaderManager.Get.LoaderController.HideLoader();
                });
            }
            else
            {
                ContentPanel.SetActive(false);
                ContentLastResult.SetActive(false);
            }
        }
    }
    void OnDisable()
    {
        foreach (Transform child in ContentPanel.transform)
        {
            Destroy(child.gameObject);
        }
        nbElement = 0;
    }

    async System.Threading.Tasks.Task DisplayLastResultsChallengesAsync(GenericChallenge[] list)
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
            if ((challenge.matched_user_1 == UserManager.Get.CurrentUser._id && challenge.user_1_score == null) || (challenge.matched_user_2 == UserManager.Get.CurrentUser._id && challenge.user_2_score == null))
            {
                ChallengeManager.CurrentChallengeId = challenge._id;
                ReplayChallengePresenter.ChallengeToReplay = challenge;
                ViewsEvents.Get.GoToMenu(ViewsEvents.Get.ReplayChallenge.gameObject);
                return;
            }
            else
            {
                if (challenge.tournament_id != null)
                {
                    InitTournamentLastResult(challenge);
                }
                else
                {
                    await InitChallengeLastResultAsync(challenge);
                }
            }
        }
    }
    private void InitTournamentLastResult(GenericChallenge challenge)
    {
        GameObject newItem = Instantiate(ListTournamentItemPrefab) as GameObject;
        LastResultTournamentListController controller = newItem.GetComponent<LastResultTournamentListController>();
        if (challenge.gain_type == TournamentManager.GAIN_TYPE_BUBBLE)
        {
            controller.title.text = HomeTranslationController.WIN + " " + challenge.gain + " " + HomeTranslationController.BUBBLES;
        }
        else
        {
            controller.title.text = HomeTranslationController.WIN + " " + challenge.gain + CurrencyManager.CURRENT_CURRENCY;
        }
        string date = challenge.createdAt.Substring(0, challenge.createdAt.IndexOf("T"));
        string hour = challenge.createdAt.Substring(challenge.createdAt.IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
        controller.date.text = date + " " + HomeTranslationController.AT + " " + hour;
        var lose = false;
        foreach (string loser in challenge.tournament.losers)
        {
            if (loser == UserId)
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
        else if (lose == false && challenge.tournament.status == "finished")
        {
            controller.victory.text = HomeTranslationController.VICTORY;
            controller.victory.transform.localScale = Vector3.one;
        }
        controller.showResult.onClick.AddListener(() =>
        {
            TournamentController.setCurrentTournamentID(challenge.tournament_id);
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Brackets.gameObject);
        });
        newItem.transform.SetParent(ContentPanel.transform);
        RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
        myLayoutElement.transform.localScale = Vector3.one;
        Debug.LogWarning("InitTournamentLastResult");
    }
    private async System.Threading.Tasks.Task InitChallengeLastResultAsync(GenericChallenge challenge)
    {
        User mOpponent;
        GameObject newItem = Instantiate(ListItemPrefab);
        LastResultListController controller = newItem.GetComponent<LastResultListController>();

        if (challenge.matched_user_1.Equals(UserManager.Get.CurrentUser._id))
        {
            mOpponent = await UserManager.Get.GetUserById(challenge.matched_user_2);
            await SetOpponentDetailsAsync(controller, mOpponent);
        }
        else
        {
            mOpponent = await UserManager.Get.GetUserById(challenge.matched_user_1);
            await SetOpponentDetailsAsync(controller, mOpponent);
        }

        SetChallengeDetails(controller, challenge);
        controller.avatar.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            ViewsEvents.Get.Profile.InitProfile(mOpponent);
            ViewsEvents.Get.ShowOverayMenu(ViewsEvents.Get.Profile.gameObject);
        });
        controller.showResult.onClick.AddListener(async () =>
        {
            ChallengeManager.CurrentChallengeId = challenge._id;
            LoaderManager.Get.LoaderController.ShowLoader(null);
            Challenge mCurrentChallenge = await ChallengeManager.Get.getChallenge(ChallengeManager.CurrentChallengeId);
            ChallengeManager.CurrentChallenge = mCurrentChallenge;
            EventsController.Get.AudioListener.enabled = true;
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.ResultPresenter.gameObject);
            LoaderManager.Get.LoaderController.HideLoader();

        });

        newItem.transform.SetParent(ContentPanel.transform);
        RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
        myLayoutElement.transform.localScale = Vector3.one;
    }
    async System.Threading.Tasks.Task SetOpponentDetailsAsync(LastResultListController controller, User user)
    {
        Sprite sprite = await UserManager.Get.getAvatar(user.avatar);
        var Flag = await UserManager.Get.GetFlagBytes(user.country_code);
        controller.Drapeau.sprite = Sprite.Create(Flag, new Rect(0f, 0f, Flag.width, Flag.height), Vector2.zero);
        controller.Drapeau.transform.localScale = Vector3.one;
        controller.avatar.sprite = sprite;
        controller.AdversaryName.text = user.username;
        controller.AdvId.text = user._id;
    }
    void SetChallengeDetails(LastResultListController controller, GenericChallenge challenge)
    {
        string date = challenge.createdAt.ToString().Substring(0, challenge.createdAt.ToString().IndexOf("T")).Replace("/", "-");
        string hour = challenge.createdAt.ToString().Substring(challenge.createdAt.ToString().IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
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
                    Destroy(child.gameObject);
                }
                count++;
            }
        }
    }

    //void SetChallengeDetails(LastResultListController controller, Challenge challenge)
    //{
    //    string date = challenge.CreatedAt.ToString().Substring(0, challenge.CreatedAt.ToString().IndexOf("T")).Replace("/", "-");
    //    string hour = challenge.CreatedAt.ToString().Substring(challenge.CreatedAt.ToString().IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
    //    controller.GameDate.text = date + " AT " + hour;
    //    controller.matchId.text = challenge._id;

    //    if (challenge.user_1_score == challenge.user_2_score)
    //    {
    //        controller.equality.text = HomeTranslationController.EQUALITY;
    //        controller.equality.transform.localScale = Vector3.one;
    //        controller.result.text = HomeTranslationController.SCORE_DRAW;
    //    }
    //    else if (UserId.Equals(challenge.winner_user))
    //    {
    //        controller.victory.text = HomeTranslationController.VICTORY;
    //        controller.victory.transform.localScale = Vector3.one;
    //        controller.result.text = HomeTranslationController.YOU_WON + controller.result.text;
    //    }
    //    else
    //    {
    //        controller.defeat.text = HomeTranslationController.DEFEAT;
    //        controller.defeat.transform.localScale = Vector3.one;
    //        controller.result.text = HomeTranslationController.YOU_LOST;
    //    }

    //}

    //async System.Threading.Tasks.Task showAsync(int nbElement)
    //{
    //    int count = 0;
    //    foreach (JSONNode item in ItemsTournament)
    //    {
    //        count++;
    //        if (count <= nbElement && count > nbElement - 4)
    //        {
    //            GameObject newItem = Instantiate(ListTournamentItemPrefab) as GameObject;
    //            LastResultTournamentListController controller = newItem.GetComponent<LastResultTournamentListController>();
    //            if (item["gain_type"].Value == TournamentManager.GAIN_TYPE_BUBBLE)
    //            {
    //                controller.title.text = HomeTranslationController.WIN + " " + item["gain"].Value + " " + HomeTranslationController.BUBBLES;
    //            }
    //            else
    //            {
    //                controller.title.text = HomeTranslationController.WIN + " " + item["gain"].Value + CurrencyManager.CURRENT_CURRENCY;
    //            }
    //            string date = item["createdAt"].Value.ToString().Substring(0, item["createdAt"].Value.ToString().IndexOf("T"));
    //            string hour = item["createdAt"].Value.ToString().Substring(item["createdAt"].Value.ToString().IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
    //            controller.date.text = date + " " + HomeTranslationController.AT + " " + hour;
    //            var lose = false;
    //            foreach (JSONNode loser in item["losers"].AsArray)
    //            {
    //                if (loser.Value == UserId)
    //                {
    //                    lose = true;
    //                    break;
    //                }
    //            }
    //            if (lose)
    //            {
    //                controller.defeat.text = HomeTranslationController.DEFEAT;
    //                controller.defeat.transform.localScale = Vector3.one;
    //            }
    //            else if (lose == false && item["status"].Value == "finished")
    //            {
    //                controller.victory.text = HomeTranslationController.VICTORY;
    //                controller.victory.transform.localScale = Vector3.one;
    //            }
    //            controller.showResult.onClick.AddListener(() =>
    //            {
    //                TournamentController.setCurrentTournamentID(item["_id"].Value);
    //                ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Brackets.gameObject);
    //            });
    //            newItem.transform.SetParent(ContentPanel.transform);
    //            RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
    //            myLayoutElement.transform.localScale = Vector3.one;
    //        }
    //    }
    //    foreach (Challenge item in lastResultItem)
    //    {
    //        if (item.status == "finished" || (item.status == "see results for user 1" && item.matched_user_2._id == UserId) || (item.status == "see results for user 2" && item.matched_user_1._id == UserId))
    //        {
    //            if (item.user_1_score != null && item.matched_user_2 != null)
    //            {
    //                count++;
    //                if (count <= nbElement && count > nbElement - 4)
    //                {
    //                    GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
    //                    LastResultListController controller = newItem.GetComponent<LastResultListController>();
    //                    if (UserId == item.matched_user_1._id)
    //                    {
    //                        await SetOpponentDetailsAsync(controller, item.matched_user_2);
    //                    }
    //                    else
    //                    {
    //                        await SetOpponentDetailsAsync(controller, item.matched_user_1);
    //                    }
    //                    SetChallengeDetails(controller, item);
    //                    controller.avatar.GetComponentInChildren<Button>().onClick.AddListener(() =>
    //                    {
    //                        if (UserId == item.matched_user_1._id)
    //                        {
    //                            ViewsEvents.Get.Profile.InitProfile(item.matched_user_2);
    //                        }
    //                        else
    //                        {
    //                            ViewsEvents.Get.Profile.InitProfile(item.matched_user_1);
    //                        }
    //                        ViewsEvents.Get.ShowOverayMenu(ViewsEvents.Get.Profile.gameObject);
    //                    });
    //                    controller.showResult.onClick.AddListener(async () =>
    //                    {
    //                        ChallengeManager.CurrentChallengeId = newItem.transform.GetChild(18).gameObject.GetComponent<Text>().text;
    //                        LoaderManager.Get.LoaderController.ShowLoader(null);
    //                        Challenge challenge = await ChallengeManager.Get.getChallenge(ChallengeManager.CurrentChallengeId);
    //                        ChallengeManager.CurrentChallenge = challenge;
    //                        EventsController.Get.AudioListener.enabled = true;
    //                        ViewsEvents.Get.GoToMenu(ViewsEvents.Get.ResultPresenter.gameObject);
    //                        LoaderManager.Get.LoaderController.HideLoader();

    //                    });

    //                    newItem.transform.SetParent(ContentPanel.transform);
    //                    RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
    //                    myLayoutElement.transform.localScale = Vector3.one;
    //                }
    //            }

    //        }
    //    }
    //    PullToRefresh.lastResultfinished = true;
    //    InvokeRepeating("removeDuplicationLR", 0f, 0.2f);
    //}
}