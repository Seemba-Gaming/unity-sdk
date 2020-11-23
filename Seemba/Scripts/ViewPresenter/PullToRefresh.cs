using System;
using UnityEngine;
using UnityEngine.UI;

[CLSCompliant(false)]
public class PullToRefresh : MonoBehaviour
{
    public static bool isActivated;
    public static bool lastResultfinished, ongoingfinished, walletFinished;

    public float minSwipeDistY;
    public float minSwipeDistX;
    public ScrollRect scrollRect;
    public GameObject Content;
    public HomeController HomeController;
    public float ContentYini;
    public float ContentYCurrent;
    public float imgY;
    public Image image;
    public Image anim;
    public GameObject ContentHome;
    public GameObject PanelLastResults;
    public GameObject PanelOngoing;

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    private Vector2 startPos;
    private UnityEngine.Color __alpha;
    private Vector3 pos;
    // Use this for initialization
    public void Start()
    {
        try
        {
            ContentYini = Content.transform.position.y;
            ContentYCurrent = ContentYini;
            __alpha = image.color;
        }
        catch (NullReferenceException)
        {
        }
        pos = Content.transform.position;
        imgY = pos.y;
    }
    // Update is called once per frame
    public void Anim()
    {
        if (!anim.IsActive())
        {
            Animator a = anim.GetComponent<Animator>();
            anim.gameObject.SetActive(true);
            a.gameObject.SetActive(true);
            refresh();
        }
    }
    private void refresh()
    {
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
            InvokeRepeating("hideAnimation", 0f, 0.5f);
        }
        else
        {
            lastResultfinished = true;
            ongoingfinished = true;
            walletFinished = true;
            InvokeRepeating("hideAnimation", 0f, 2f);
            HomeController.enabled = false;
            HomeController.enabled = true;
        }
    }

    private void hideAnimation()
    {
        UnityThreadHelper.CreateThread(() =>
        {
            if (lastResultfinished != false || ongoingfinished != false || walletFinished != false)
            {
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    CancelInvoke();
                    Animator a = anim.GetComponent<Animator>();
                    a.gameObject.SetActive(false);
                    anim.gameObject.SetActive(false);
                    __alpha.a = 0f;
                    image.color = __alpha;
                });
            }
        });
    }

    private void Update()
    {
        if (__alpha.a >= 1f)
        {
            Anim();
        }
        else
        {
            if (__alpha.a < 1f)
            {
                __alpha.a = (ContentYini - Content.transform.position.y);
                image.color = __alpha;
            }
            ////Debug.Log("verticalNormalizedPosition: ");
            if (Input.GetMouseButtonDown(0))
            {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            if (Input.GetMouseButtonUp(0))
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                //create vector from the two points
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
                //normalize the 2d vector
                currentSwipe.Normalize();
                //swipe upwards
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                }
                //swipe down
                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                }
                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                }
                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                }
            }
            //#if UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startPos = touch.position;
                        break;
                    case TouchPhase.Ended:
                        float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
                        if (swipeDistVertical > minSwipeDistY)
                        {
                            float swipeValue = Mathf.Sign(touch.position.y - startPos.y);
                            if (swipeValue > 0)//up swipe
                            { }
                            else if (swipeValue < 0)//down swipe
                            { }
                        }
                        float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
                        if (swipeDistHorizontal > minSwipeDistX)
                        {
                            float swipeValue = Mathf.Sign(touch.position.x - startPos.x);
                            if (swipeValue > 0)//right swipe
                                               //MoveRight ();
                            { }
                            else if (swipeValue < 0)//left swipe
                                                    //MoveLeft ();
                            { }
                        }
                        break;
                }
            }
        }
    }
}
