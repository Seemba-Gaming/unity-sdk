using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[CLSCompliant(false)]
public class HistoryListController : MonoBehaviour
{
    #region Script Parameters
    public GameObject                           ContentPanelPro;
    public GameObject                           ListItemPrefab;
    public Text                                 nbGameWon;
    public Text                                 nbGameWonInARow;
    #endregion

    #region Fields
    private ArrayList                           Items;
    private ArrayList                           proItems;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        Items = new ArrayList();
        proItems = new ArrayList();
        show();
    }

    private void OnDisable()
    {
        foreach (Transform child in ContentPanelPro.transform)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion

    #region Implementation
    private IEnumerator CheckItems()
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
            ConnectivityController.CURRENT_ACTION = ConnectivityController.HISTORY_ACTION;
            PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());

            try
            {
                LoaderManager.Get.LoaderController.HideLoader();
            }
            catch (ArgumentException) { }
        }
    }

    private async Task show()
    {
        LoaderManager.Get.LoaderController.ShowLoader();
        GamesManager.LoadIcon();
        Items = new ArrayList();
        string token = UserManager.Get.getCurrentSessionToken();
        StartCoroutine(CheckItems());
        User user = await UserManager.Get.getUser();
        Items = await ChallengeManager.Get.getChallengesUserResults(token);
        UnityThreadHelper.Dispatcher.Dispatch(() =>
        {
            nbGameWon.text = user.victories_count.ToString();
            nbGameWonInARow.text = user.current_victories_count.ToString();
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
            TranslationManager.scene = "Home";
            foreach (Challenge item in proItems)
            {
                //JSONNode Result = ChallengeManager.Get.getChallengeResult (item.ChallengeId);
                if ((item.user_1_score != null && item.user_2_score != null) || item.status == "results pending")
                {
                    GameObject newItem = Instantiate(ListItemPrefab);
                    HistoryListItemController controller = newItem.GetComponent<HistoryListItemController>();
                    UnityThreadHelper.CreateThread(() =>
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
                                else if (item.matched_user_1._id == UserManager.Get.getCurrentUserId() && adv1score > adv2score)
                                {
                                    controller.Gain.text = "+" + float.Parse(item.gain).ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                    GameObject newItem1 = Instantiate(ListItemPrefab);
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
                                    //TODO
                                    //controller1.Icon.sprite = newSprite1;
                                    controller1.GameName.text = item.game.name;
                                    controller1.Icon.sprite = GamesManager.CurrentIcon;
                                    controller1.Date.text = DateTime.Parse(item.CreatedAt).ToString("yyyy/MM/dd HH:mm:ss");
                                    controller1.ChallengeID.text = "ID: " + item._id;
                                    controller1.Gain.color = new Color(129 / 255f, 130 / 255f, 170 / 255f);
                                    newItem1.transform.SetParent(ContentPanelPro.transform);
                                    RectTransform myLayoutElement1 = newItem1.GetComponent<RectTransform>();
                                    myLayoutElement1.sizeDelta = new Vector2(391, 60);
                                    myLayoutElement1.transform.localScale = Vector3.one;
                                }
                                else if (item.matched_user_2 != null && item.matched_user_2._id == UserManager.Get.getCurrentUserId() && adv2score > adv1score)
                                {
                                    controller.Gain.text = "+" + float.Parse(item.gain).ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                    GameObject newItem2 = Instantiate(ListItemPrefab);
                                    HistoryListItemController controller2 = newItem2.GetComponent<HistoryListItemController>();
                                    switch (item.gain)
                                    {
                                        case "2":
                                            controller2.Gain.text = "-" + ChallengeManager.FEE_1V1_CASH_CONFIDENT.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                            break;
                                        case "5":
                                            controller2.Gain.text = "-" + ChallengeManager.FEE_1V1_CASH_CHAMPION.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                            break;
                                        case "10":
                                            controller2.Gain.text = "-" + ChallengeManager.FEE_1V1_CASH_LEGEND.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
                                            break;
                                    }
                                    //TODO
                                    //controller2.Icon.sprite = newSprite1;
                                    controller2.GameName.text = item.game.name;
                                    controller2.Icon.sprite = GamesManager.CurrentIcon;
                                    controller2.Date.text = DateTime.Parse(item.CreatedAt).ToString("yyyy/MM/dd HH:mm:ss");
                                    controller2.ChallengeID.text = "ID: " + item._id;
                                    controller2.Gain.color = new Color(129 / 255f, 130 / 255f, 170 / 255f);
                                    newItem2.transform.SetParent(ContentPanelPro.transform);
                                    RectTransform myLayoutElement2 = newItem2.GetComponent<RectTransform>();
                                    myLayoutElement2.sizeDelta = new Vector2(391, 60);
                                    myLayoutElement2.transform.localScale = Vector3.one;
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
                                            controller.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT + " " + TranslationManager.Get("bubbles");
                                            break;
                                        case "6":
                                            controller.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CHAMPION + " " + TranslationManager.Get("bubbles");
                                            break;
                                        case "10":
                                            controller.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_LEGEND + " " + TranslationManager.Get("bubbles");
                                            break;
                                    }
                                    controller.Gain.color = new Color(129 / 255f, 130 / 255f, 170 / 255f);
                                }
                                else
                                {
                                    if (item.matched_user_1._id == UserManager.Get.getCurrentUserId() && adv1score > adv2score)
                                    {
                                        controller.Gain.text = "+" + item.gain + " " + TranslationManager.Get("bubbles");
                                        GameObject newItem3 = Instantiate(ListItemPrefab);
                                        HistoryListItemController controller3 = newItem3.GetComponent<HistoryListItemController>();
                                        switch (item.gain)
                                        {
                                            case "2":
                                                controller3.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT + " " + TranslationManager.Get("bubbles");
                                                break;
                                            case "6":
                                                controller3.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CHAMPION + " " + TranslationManager.Get("bubbles");
                                                break;
                                            case "10":
                                                controller3.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_LEGEND + " " + TranslationManager.Get("bubbles");
                                                break;
                                        }
                                        //TODO
                                        //controller3.Icon.sprite = newSprite1;
                                        controller3.GameName.text = item.game.name;
                                        controller3.Icon.sprite = GamesManager.CurrentIcon;
                                        controller3.Date.text = DateTime.Parse(item.CreatedAt).ToString("yyyy/MM/dd HH:mm:ss");
                                        controller3.ChallengeID.text = "ID: " + item._id;
                                        controller3.Gain.color = new Color(129 / 255f, 130 / 255f, 170 / 255f);
                                        newItem3.transform.SetParent(ContentPanelPro.transform);
                                        RectTransform myLayoutElement3 = newItem3.GetComponent<RectTransform>();
                                        myLayoutElement3.sizeDelta = new Vector2(391, 60);
                                        myLayoutElement3.transform.localScale = Vector3.one;
                                    }
                                    else if (item.matched_user_2 != null && item.matched_user_2._id == UserManager.Get.getCurrentUserId() && adv2score > adv1score)
                                    {
                                        controller.Gain.text = "+" + item.gain + " " + TranslationManager.Get("bubbles");
                                        GameObject newItem4 = Instantiate(ListItemPrefab);
                                        HistoryListItemController controller4 = newItem4.GetComponent<HistoryListItemController>();
                                        switch (item.gain)
                                        {
                                            case "2":
                                                controller4.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT + " " + TranslationManager.Get("bubbles");
                                                break;
                                            case "6":
                                                controller4.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CHAMPION + " " + TranslationManager.Get("bubbles");
                                                break;
                                            case "10":
                                                controller4.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_LEGEND + " " + TranslationManager.Get("bubbles");
                                                break;
                                        }
                                        //TODO
                                        //controller4.Icon.sprite = newSprite1;
                                        controller4.GameName.text = item.game.name;
                                        controller4.Date.text = DateTime.Parse(item.CreatedAt).ToString("yyyy/MM/dd HH:mm:ss");
                                        controller4.ChallengeID.text = "ID: " + item._id;
                                        controller4.Icon.sprite = GamesManager.CurrentIcon;
                                        controller4.Gain.color = new Color(129 / 255f, 130 / 255f, 170 / 255f);
                                        newItem4.transform.SetParent(ContentPanelPro.transform);
                                        RectTransform myLayoutElement4 = newItem4.GetComponent<RectTransform>();
                                        myLayoutElement4.sizeDelta = new Vector2(391, 60);
                                        myLayoutElement4.transform.localScale = Vector3.one;
                                    }
                                    else
                                    {
                                        switch (item.gain)
                                        {
                                            case "2":
                                                controller.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT + " " + TranslationManager.Get("bubbles");
                                                break;
                                            case "6":
                                                controller.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_CHAMPION + " " + TranslationManager.Get("bubbles");
                                                break;
                                            case "10":
                                                controller.Gain.text = "-" + ChallengeManager.FEE_1V1_BUBBLES_LEGEND + " " + TranslationManager.Get("bubbles");
                                                break;
                                        }
                                        controller.Gain.color = new Color(129 / 255f, 130 / 255f, 170 / 255f);
                                    }
                                }
                            }
                        });
                    });
                    newItem.transform.SetParent(ContentPanelPro.transform);
                    RectTransform myLayoutElement = newItem.GetComponent<RectTransform>();
                    myLayoutElement.sizeDelta = new Vector2(391, 60);
                    myLayoutElement.transform.localScale = Vector3.one;
                }
            }
            LoaderManager.Get.LoaderController.HideLoader();
        });
    }
    #endregion
}
