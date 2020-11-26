using System;
using UnityEngine;
using UnityEngine.UI;

[CLSCompliant(false)]
public class PullToRefresh : MonoBehaviour
{
    public static bool isActivated;
    public static bool lastResultfinished, ongoingfinished, walletFinished;

    public ScrollRect scrollRect;
    public GameObject Content;
    public HomeController HomeController;
    public float ContentYini;
    public float ContentYCurrent;
    public float imgY;
    public Image Loader;
    public GameObject ContentHome;
    public GameObject PanelLastResults;
    public GameObject PanelOngoing;

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    private Vector3 pos;
    // Use this for initialization
    public void Start()
    {
        try
        {
            ContentYini = Content.transform.position.y;
            ContentYCurrent = ContentYini;
        }
        catch (NullReferenceException)
        {
        }
        pos = Content.transform.position;
        imgY = pos.y;
    }
    // Update is called once per frame

    private void refresh()
    {
        Loader.gameObject.SetActive(false);
        isActivated = true;
        lastResultfinished = false;
        ongoingfinished = false;
        walletFinished = false;
        foreach (Transform child in PanelLastResults.transform)
        {
            DestroyImmediate(child.gameObject, true);
        }
        foreach (Transform child in PanelOngoing.transform)
        {
            DestroyImmediate(child.gameObject, true);
        }
        ContentHome.SetActive(false);
        ContentHome.SetActive(true);
        RefreshHeader();
    }
    private async void RefreshHeader()
    {
        User user = await UserManager.Get.getUser();
        if (user != null)
        {
            ViewsEvents.Get.Menu.Header.GetComponent<HeaderController>().UpdateHeaderInfo(user);
        }
        else
        {
            lastResultfinished = true;
            ongoingfinished = true;
            walletFinished = true;
            HomeController.enabled = false;
            HomeController.enabled = true;
        }
    }

    public void BeginDrag()
    {
        firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void Drag()
    {
        secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
        if (currentSwipe.y < 0 && currentSwipe.x > -50f && currentSwipe.x < 50f)
        {
            Loader.gameObject.SetActive(true);
        }
    }

    public void EndDrag()
    {
        secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
        if (currentSwipe.y < 0 && Math.Abs(currentSwipe.x) <= 50f)
        {
            refresh();
        }
        Loader.gameObject.SetActive(false);
    }
}
