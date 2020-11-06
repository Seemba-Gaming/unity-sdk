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
    public int nbElement = 1;
    public Button SeeMoreResult;
    public Sprite[] spriteArray;
    public string gaintext;
    public int Length = 0;
    private int nbChild = 0;
    private string UserId, token;

    void OnDisable()
    {
        foreach (Transform child in ContentPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    async void OnEnable()
    {
        HomeController.NoLastResult = false;
        //gradient = GradientComponent.GetComponent<Gradient>();
        ContentLastResult.SetActive(false);
        Items = new ArrayList();
        lastResultItem = new ArrayList();
        ItemsTournament = new JSONArray();
        UserId = UserManager.Get.getCurrentUserId();
        token = UserManager.Get.getCurrentSessionToken();
        if(token != null)
        {
            Items = await ChallengeManager.Get.getFinishedChallenges(token);
            ItemsTournament = await TournamentManager.Get.getUserFinishedTournaments();

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
                    ViewsEvents.Get.GoToMenu(ViewsEvents.Get.ReplayChallenge.gameObject);
                }
                if (item.status == "finished" || (item.status == "see results for user 1" && item.matched_user_2._id == UserId) || (item.status == "see results for user 2" && item.matched_user_1._id == UserId))
                {
                    lastResultItem.Add(item);
                }
            }
            if (lastResultItem == null && ItemsTournament == null)
            {
                ContentLastResult.SetActive(false);
            }
            else
            {
                int count = 0;
                ContentLastResult.SetActive(true);
                if (lastResultItem != null && ItemsTournament != null)
                {
                    if (lastResultItem.Count == 0 && ItemsTournament.Count == 0)
                    {
                        ContentLastResult.SetActive(false);
                        HomeController.NoLastResult = true;
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
            }
        }
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
                    ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Brackets.gameObject);
                });
                newItem.transform.SetParent(ContentPanel.transform);
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
                            SetOpponentDetailsAsync(controller, item.matched_user_2);
                        }
                        else
                        {
                            SetOpponentDetailsAsync(controller, item.matched_user_1);
                        }
                        SetChallengeDetails(controller, item);
                        controller.avatar.GetComponentInChildren<Button>().onClick.AddListener(() =>
                        {
                            if (UserId == item.matched_user_1._id)
                            {
                                ViewsEvents.Get.Profile.InitProfile(item.matched_user_2);
                            }
                            else
                            {
                                ViewsEvents.Get.Profile.InitProfile(item.matched_user_1);
                            }
                            ViewsEvents.Get.ShowOverayMenu(ViewsEvents.Get.Profile.gameObject);
                        });
                        controller.showResult.onClick.AddListener(async () =>
                        {
                            ChallengeManager.CurrentChallengeId = newItem.transform.GetChild(18).gameObject.GetComponent<Text>().text;
                            LoaderManager.Get.LoaderController.ShowLoader(null);
                            Challenge challenge = await ChallengeManager.Get.getChallenge(ChallengeManager.CurrentChallengeId);
                            ChallengeManager.CurrentChallenge = challenge;
                            EventsController.Get.AudioListener.enabled = true;
                            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.ResultPresenter.gameObject);
                            LoaderManager.Get.LoaderController.HideLoader();

                        });

                        newItem.transform.SetParent(ContentPanel.transform);
                        RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
                        myLayoutElement.transform.localScale = Vector3.one;
                    }
                }

            }
        }
        PullToRefresh.lastResultfinished = true;
        InvokeRepeating("removeDuplicationLR", 0f, 0.2f);
    }
    async System.Threading.Tasks.Task SetOpponentDetailsAsync(LastResultListController controller, User user)
    {
        Sprite sprite = await UserManager.Get.getAvatar(user.avatar);
        controller.Drapeau.sprite = sprite;
        controller.Drapeau.transform.localScale = Vector3.one;
        controller.avatar.sprite = sprite;
        controller.AdversaryName.text = user.username;
        controller.AdvId.text = user._id;
    }
    void SetChallengeDetails(LastResultListController controller, Challenge challenge)
    {
        string date = challenge.CreatedAt.ToString().Substring(0, challenge.CreatedAt.ToString().IndexOf("T")).Replace("/", "-");
        string hour = challenge.CreatedAt.ToString().Substring(challenge.CreatedAt.ToString().IndexOf("T") + 1, 5).Replace(":", "H") + "MIN";
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