using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class LeaderboardPresenter : MonoBehaviour
    {
        public Toggle Daily;
        public Toggle Weekly;
        public Toggle Monthly;
        void OnEnable()
        {
            OnClickDailyAsync();
        }
        public async void OnClickDailyAsync()
        {
            if(Daily.isOn)
            {
                ChangeAlpha(Daily.GetComponent<Text>(), 1);
                ChangeAlpha(Weekly.GetComponent<Text>(), 0.6f);
                ChangeAlpha(Monthly.GetComponent<Text>(), 0.6f);
                await GetComponent<LeaderboardController>().GetLeaderBoardAsync("daily");
            }
        }
        public async void OnClickWeeklyAsync()
        {
            if (Weekly.isOn)
            {
                ChangeAlpha(Daily.GetComponent<Text>(), 0.6f);
                ChangeAlpha(Weekly.GetComponent<Text>(), 1);
                ChangeAlpha(Monthly.GetComponent<Text>(), 0.6f);
                await GetComponent<LeaderboardController>().GetLeaderBoardAsync("weekly");
            }
        }
        public async void OnClickMonthlyAsync()
        {
            if(Monthly.isOn)
            {
                ChangeAlpha(Daily.GetComponent<Text>(), 0.6f);
                ChangeAlpha(Weekly.GetComponent<Text>(), 0.6f);
                ChangeAlpha(Monthly.GetComponent<Text>(), 1);
                await GetComponent<LeaderboardController>().GetLeaderBoardAsync("monthly");
            }
        }
        public void ChangeAlpha(Text text, float alpha)
        {
            text.color = new Color(text.color.r,
                                    text.color.b,
                                    text.color.g,
                                    alpha);
        }
    }
}
