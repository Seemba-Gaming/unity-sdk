using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Data;
using System;
using UnityEngine.SceneManagement;
using SimpleJSON;
using System.IO;
public class HistoryListController : MonoBehaviour
{
    public GameObject ContentPanelPro, ContentPro, ContentTraining;
    public GameObject ListItemPrefab;
    public Text nbGameWon, nbGameWonInARow;
    // Use this for initialization
    ArrayList Items, bubbleItems, proItems;
    int i;
    UserManager usermanager = new UserManager();
    ChallengeManager challengeManager = new ChallengeManager();
    Scene scene;
    void OnEnable()
    {
        try
        {
            SceneManager.UnloadSceneAsync("ConnectionFailed");
        }
        catch (ArgumentException ex) { }
        Items = new ArrayList();
        bubbleItems = new ArrayList();
        proItems = new ArrayList();
        DateTime dateTime = DateTime.UtcNow.Date;
        show();
    }
    void OnDisable()
    {
        foreach (Transform child in ContentPanelPro.transform)
        {
            Destroy(child.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //
    }
    IEnumerator CheckItems()
    {
        float timer = 0;
        bool failed = false;
        while (ContentPanelPro.transform.childCount == 0 && !failed)
        {
            if (timer > 5)
            {
                failed = true;
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        if (failed)
        {
            try
            {
                SceneManager.UnloadSceneAsync("ConnectionFailed");
            }
            catch (ArgumentException ex) { }
            ConnectivityController.CURRENT_ACTION = ConnectivityController.HISTORY_ACTION;
            SceneManager.LoadScene("ConnectionFailed", LoadSceneMode.Additive);
            try
            {
                SceneManager.UnloadSceneAsync("Loader");
            }
            catch (ArgumentException ex) { }
        }
    }
    void show()
    {
        SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
        GamesManager.LoadIcon();
        bool isFinished = false;
        Items = new ArrayList();
        string UserId = usermanager.getCurrentUserId();
        string token = usermanager.getCurrentSessionToken();
        UnityThreading.ActionThread thread;
        //StartCoroutine(CheckItems());
        thread = UnityThreadHelper.CreateThread(() =>
        {
            User user = usermanager.getUser(UserId, token);
            Items = challengeManager.getChallengesUserResults(token);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                nbGameWon.text = user.victories_count.ToString();
                nbGameWonInARow.text = user.victories_streak.ToString();
                if (Items != null)
                {
                    foreach (Challenge item in Items)
                    {
                        if ((item.status == "finished" || item.status == "see results for user 1" || item.status == "see results for user 2" || item.status == "results pending"))
                        {
                            proItems.Add(item);
                        }
                    }
                }
                foreach (Challenge item in proItems)
                {
                    //JSONNode Result = challengeManager.getChallengeResult (item.ChallengeId);
                    if ((item.user_1_score != null && item.user_2_score != null) || item.status == "results pending")
                    {
                        GameObject newItem = Instantiate(ListItemPrefab) as GameObject;
                        HistoryListItemController controller = newItem.GetComponent<HistoryListItemController>();
                        GamesManager game = new GamesManager();
                        thread = UnityThreadHelper.CreateThread(() =>
                        {
                            UnityThreadHelper.Dispatcher.Dispatch(() =>
                            {
                                float? adv2score, adv1score;
                                adv1score = item.user_1_score;
                                adv2score = item.user_2_score;
                                controller.GameName.text = item.game.name;
                                controller.Date.text = DateTime.Parse(item.CreatedAt).ToString("yyyy/MM/dd HH:mm:ss");
                                controller.ChallengeID.text = "ID: " + item._id;
                                controller.Icon.sprite = GamesManager.CurrentIcon;

                                if (item.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
                                {
                                    if (item.status == "results pending")
                                    {
                                        switch (item.gain)
                                        {
                                            case "2":
                                                controller.Gain.text = "-" + ChallengeManager.FEE_1V1_CASH_CONFIDENT.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                                break;
                                            case "5":
                                                controller.Gain.text = "-" + ChallengeManager.FEE_1V1_CASH_CHAMPION.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                                break;
                                            case "10":
                                                controller.Gain.text = "-" + ChallengeManager.FEE_1V1_CASH_LEGEND.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                                break;
                                        }
                                        controller.Gain.color = new Color(129 / 255f, 130 / 255f, 170 / 255f);
                                    }
                                    else if (item.winner_user == usermanager.getCurrentUserId())
                                    {
                                        controller.Gain.text = "+" + float.Parse(item.gain).ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                        GameObject newItem1 = Instantiate(ListItemPrefab) as GameObject;
                                        HistoryListItemController controller1 = newItem1.GetComponent<HistoryListItemController>();
                                        switch (item.gain)
                                        {
                                            case "2":
                                                controller1.Gain.text = "-" + ChallengeManager.FEE_1V1_CASH_CONFIDENT.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                                break;
                                            case "5":
                                                controller1.Gain.text = "-" + ChallengeManager.FEE_1V1_CASH_CHAMPION.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                                break;
                                            case "10":
                                                controller1.Gain.text = "-" + ChallengeManager.FEE_1V1_CASH_LEGEND.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                                break;
                                        }
                                        controller1.GameName.text = item.game.name;
                                        controller1.Icon.sprite = GamesManager.CurrentIcon;
                                        controller1.Date.text = DateTime.Parse(item.CreatedAt).ToString("yyyy/MM/dd HH:mm:ss");
                                        controller1.ChallengeID.text = "ID: " + item._id;
                                        controller1.Gain.color = new Color(129 / 255f, 130 / 255f, 170 / 255f);
                                        newItem1.transform.parent = ContentPanelPro.transform;
                                        RectTransform myLayoutElement1 = newItem1.GetComponent<RectTransform>();
                                        myLayoutElement1.sizeDelta = new Vector2(391, 60);
                                        myLayoutElement1.transform.localScale = Vector3.one;
                                    }
                                    else
                                    {
                                        switch (item.gain)
                                        {
                                            case "2":
                                                controller.Gain.text = "-" + ChallengeManager.FEE_1V1_CASH_CONFIDENT.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                                break;
                                            case "5":
                                                controller.Gain.text = "-" + ChallengeManager.FEE_1V1_CASH_CHAMPION.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                                break;
                                            case "10":
                                                controller.Gain.text = "-" + ChallengeManager.FEE_1V1_CASH_LEGEND.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                                break;
                                        }
                                        controller.Gain.color = new Color(129 / 255f, 130 / 255f, 170 / 255f);
                                    }
                                }
                                else
                                {
                                    if (item.status == "results pending")
                                    {
                                        switch (item.gain)
                                        {
                                            case "2":
                                                controller.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT + " Bubble";
                                                break;
                                            case "6":
                                                controller.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CHAMPION + " Bubbles";
                                                break;
                                            case "10":
                                                controller.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_LEGEND + " Bubbles";
                                                break;
                                        }
                                        controller.Gain.color = new Color(129 / 255f, 130 / 255f, 170 / 255f);
                                    }
                                    else
                                    {
                                        if (item.winner_user == usermanager.getCurrentUserId())
                                        {
                                            controller.Gain.text = "+" + item.gain + " Bubbles";
                                            GameObject newItem3 = Instantiate(ListItemPrefab) as GameObject;
                                            HistoryListItemController controller3 = newItem3.GetComponent<HistoryListItemController>();
                                            switch (item.gain)
                                            {
                                                case "2":
                                                    controller3.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT + " Bubble";
                                                    break;
                                                case "6":
                                                    controller3.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CHAMPION + " Bubbles";
                                                    break;
                                                case "10":
                                                    controller3.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_LEGEND + " Bubbles";
                                                    break;
                                            }
                                            //TODO
                                            //controller3.Icon.sprite = newSprite1;
                                            controller3.GameName.text = item.game.name;
                                            controller3.Icon.sprite = GamesManager.CurrentIcon;
                                            controller3.Date.text = DateTime.Parse(item.CreatedAt).ToString("yyyy/MM/dd HH:mm:ss");
                                            controller3.ChallengeID.text = "ID: " + item._id;
                                            controller3.Gain.color = new Color(129 / 255f, 130 / 255f, 170 / 255f);
                                            newItem3.transform.parent = ContentPanelPro.transform;
                                            RectTransform myLayoutElement3 = newItem3.GetComponent<RectTransform>();
                                            myLayoutElement3.sizeDelta = new Vector2(391, 60);
                                            myLayoutElement3.transform.localScale = Vector3.one;
                                        }
                                        else
                                        {
                                            switch (item.gain)
                                            {
                                                case "2":
                                                    controller.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT + " Bubble";
                                                    break;
                                                case "6":
                                                    controller.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CHAMPION + " Bubbles";
                                                    break;
                                                case "10":
                                                    controller.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_LEGEND + " Bubbles";
                                                    break;
                                            }
                                            controller.Gain.color = new Color(129 / 255f, 130 / 255f, 170 / 255f);
                                        }
                                    }
                                }
                            });
                        });
                        newItem.transform.parent = ContentPanelPro.transform;
                        RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
                        myLayoutElement.sizeDelta = new Vector2(391, 60);
                        myLayoutElement.transform.localScale = Vector3.one;
                    }
                }
                SceneManager.UnloadSceneAsync("Loader");
            });
        });
    }
}
