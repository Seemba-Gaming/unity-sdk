using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class FreeBubblesController : MonoBehaviour
    {
        private const int rewardIntervalHours = 2;
        public static DateTime nextRewardTime;
        public GameObject WaitingPanel;
        public Image WaitingProgress, Loader;
        public Text TimerGUI;
        public Button TimerButton;
        public Animator Shadow;
        public ParticleSystem FreeBubbleEffect;

        private Coroutine mSetTimerGui;

        private void Start()
        {
            TimerButton.onClick.AddListener(() =>
            {
                SetNextTime();
            });
        }
        private void OnEnable()
        {
            ConfigNextRewardTime();
            if(mSetTimerGui != null)
            {
                mSetTimerGui = null;
            }
            StartCoroutine(SetTimeCoroutine());
        }
        public async void SetNextTime()
        {
            var token = UserManager.Get.getCurrentSessionToken();
            nextRewardTime = DateTime.UtcNow.AddHours(rewardIntervalHours);
            UserManager.Get.CurrentUser.bubble_credit += 5;
            Loader.gameObject.SetActive(true);
            if (await UserManager.Get.WinFreeBubble(token))
            {
                SeembaAnalyticsManager.Get.SendUserEvent("Free Bubbles");
                string[] attrib = { "last_bubble_click" };
                string[] value = { DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") };
                UserManager.Get.UpdateUserByField(attrib, value);
                FreeBubbleEffect.Play();
                updateHeader();
                TimerButton.gameObject.SetActive(false);
                WaitingPanel.SetActive(true);
                mSetTimerGui = StartCoroutine(SetTimeCoroutine());
            }
            Loader.gameObject.SetActive(false);
        }
        public void ConfigNextRewardTime()
        {
            if (nextRewardTime == DateTime.MinValue)
            {
                Loader.gameObject.SetActive(true);

                Loader.gameObject.SetActive(false);
                if (UserManager.Get.CurrentUser != null)
                {
                    if (string.IsNullOrEmpty(UserManager.Get.CurrentUser.last_bubble_click))
                    {
                        nextRewardTime = DateTime.MinValue;
                    }
                    else
                    {
                        nextRewardTime = Convert.ToDateTime(UserManager.Get.CurrentUser.last_bubble_click).ToUniversalTime().AddHours(rewardIntervalHours);
                    }
                }
                else
                {
                    nextRewardTime = DateTime.MinValue;
                }
                checkTimer();
            }
            else
            {
                checkTimer();
            }
        }
        public IEnumerator SetTimeCoroutine()
        {
            while(nextRewardTime.Subtract(DateTime.UtcNow) >= TimeSpan.Zero)
            {
                yield return new WaitForSeconds(0.1f);
                setGUITimer();
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
                mSetTimerGui = null;
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
                if(mSetTimerGui == null)
                {
                    StartCoroutine(SetTimeCoroutine());
                }
            }
            else
            {
                Shadow.SetBool("showShadow", true);
            }
        }
        private void updateHeader()
        {
            ViewsEvents.Get.Menu.Header.GetComponent<HeaderController>().VirtualMoney.text = UserManager.Get.GetCurrentBubblesCredit();
        }
    }
}
