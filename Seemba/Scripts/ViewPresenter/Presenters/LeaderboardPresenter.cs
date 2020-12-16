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
            Daily.GetComponent<Text>().color = new Color(Daily.GetComponent<Text>().color.r,
                                                        Daily.GetComponent<Text>().color.b,
                                                        Daily.GetComponent<Text>().color.g, 
                                                        1);
            Weekly.GetComponent<Text>().color = new Color(Weekly.GetComponent<Text>().color.r,
                                                        Weekly.GetComponent<Text>().color.b,
                                                        Weekly.GetComponent<Text>().color.g,
                                                        0.6f);
            Monthly.GetComponent<Text>().color = new Color(Monthly.GetComponent<Text>().color.r,
                                                        Monthly.GetComponent<Text>().color.b,
                                                        Monthly.GetComponent<Text>().color.g,
                                                        0.6f);
        }
        public void OnClickDaily()
        {
            if(Daily.isOn)
            {
                ChangeAlpha(Daily.GetComponent<Text>(), 1);
                ChangeAlpha(Weekly.GetComponent<Text>(), 0.6f);
                ChangeAlpha(Monthly.GetComponent<Text>(), 0.6f);
                GetComponent<LeaderboardController>().GetLeaderBoard("day");
            }
        }
        public void OnClickWeekly()
        {
            if (Weekly.isOn)
            {
                ChangeAlpha(Daily.GetComponent<Text>(), 0.6f);
                ChangeAlpha(Weekly.GetComponent<Text>(), 1);
                ChangeAlpha(Monthly.GetComponent<Text>(), 0.6f);
                GetComponent<LeaderboardController>().GetLeaderBoard("week");
            }
        }
        public void OnClickMonthly()
        {
            if(Monthly.isOn)
            {
                ChangeAlpha(Daily.GetComponent<Text>(), 0.6f);
                ChangeAlpha(Weekly.GetComponent<Text>(), 0.6f);
                ChangeAlpha(Monthly.GetComponent<Text>(), 1);
                GetComponent<LeaderboardController>().GetLeaderBoard("month");
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
