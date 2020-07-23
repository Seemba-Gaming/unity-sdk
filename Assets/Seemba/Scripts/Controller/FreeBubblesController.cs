using System;
using UnityEngine;
using UnityEngine.UI;
public class FreeBubblesController : MonoBehaviour
{
    private const int rewardIntervalHours = 2;
    public static DateTime nextRewardTime;
    public GameObject WaitingPanel;
    public Image WaitingProgress, Loader;
    public Text TimerGUI;
    public Button TimerButton;
    public Animator Shadow;
    private UserManager um = new UserManager();
    public ParticleSystem FreeBubbleEffect;
    private string userId, token;
    // Use this for initialization
    void Start()
    {
        TimerButton.onClick.AddListener(() =>
        {
            SetNextTime();
        });
    }
    void OnEnable()
    {
        ConfigNextRewardTime();
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void SetNextTime()
    {
        userId = um.getCurrentUserId();
        token = um.getCurrentSessionToken();
        nextRewardTime = DateTime.UtcNow.AddHours(rewardIntervalHours);
        UserManager.CurrentWater = ((int.Parse(UserManager.CurrentWater) + 5)).ToString();
        string[] attrib = { "bubble_credit", "last_bubble_click" };
        string[] value = { UserManager.CurrentWater, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") };
        Loader.gameObject.SetActive(true);
        UnityThreadHelper.CreateThread(() =>
        {
            um.UpdateUserByField(userId, token, attrib, value);
            um.bubblesTrack(userId, token, GamesManager.GAME_ID, GamesManager.FREE_BUBBLES_PUSH);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                FreeBubbleEffect.Play();
                Loader.gameObject.SetActive(false);
                updateHeader();
                TimerButton.gameObject.SetActive(false);
                WaitingPanel.SetActive(true);
                InvokeRepeating("setGUITimer", 0f, 0.1f);
            });
        });
    }
    public void ConfigNextRewardTime()
    {
        if (nextRewardTime == DateTime.MinValue)
        {
            userId = um.getCurrentUserId();
            token = um.getCurrentSessionToken();
            Loader.gameObject.SetActive(true);
            UnityThreadHelper.CreateThread(() =>
            {
                User user = um.getUser(userId, token);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    Loader.gameObject.SetActive(false);
                    if (user != null)
                    {
                        if (string.IsNullOrEmpty(user.last_bubble_click))
                        {
                            nextRewardTime = DateTime.MinValue;
                        }
                        else
                        {
                            nextRewardTime = Convert.ToDateTime(user.last_bubble_click).ToUniversalTime().AddHours(rewardIntervalHours);
                        }
                    }
                    else
                        nextRewardTime = DateTime.MinValue;
                    checkTimer();
                });
            });
        }
        else
        {
            checkTimer();
        }
    }
    private void setGUITimer()
    {
        if (nextRewardTime.Subtract(DateTime.UtcNow) >= TimeSpan.Zero)
        {
            Shadow.SetBool("showShadow", false);
            TimeSpan remainingTime = nextRewardTime.Subtract(DateTime.UtcNow);
            float hoursFloat = 7200 - float.Parse(remainingTime.TotalSeconds.ToString());
            WaitingProgress.fillAmount = hoursFloat * 0.000138889f;
            TimerGUI.text = remainingTime.ToString().Remove(8);
        }
        else
        {
            TimerButton.gameObject.SetActive(true);
            WaitingPanel.SetActive(false);
            Shadow.SetBool("showShadow", true);
        }
    }
    public void checkTimer()
    {
        if (nextRewardTime != DateTime.MinValue && nextRewardTime.Subtract(DateTime.UtcNow) > TimeSpan.Zero)
        {
            TimerButton.gameObject.SetActive(false);
            WaitingPanel.SetActive(true);
            InvokeRepeating("setGUITimer", 0f, 0.1f);
        }
        else
        {
            Shadow.SetBool("showShadow", true);
        }
    }
    private void updateHeader()
    {
        Text PlayerWater = GameObject.Find("virtual_money").GetComponent<Text>();
        PlayerWater.text = UserManager.CurrentWater;
    }
}
