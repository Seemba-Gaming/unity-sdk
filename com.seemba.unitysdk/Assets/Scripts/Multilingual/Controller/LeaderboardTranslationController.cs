using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class LeaderboardTranslationController : MonoBehaviour
    {
        public Text Leaderboard;
        public Text Daily;
        public Text Weekly;
        public Text Monthly;
        void Start()
        {
            TranslationManager._instance.scene = "Leaderboard";
            Leaderboard.text = TranslationManager._instance.Get("leaderboard");
            Daily.text = TranslationManager._instance.Get("daily");
            Weekly.text = TranslationManager._instance.Get("weekly");
            Monthly.text = TranslationManager._instance.Get("monthly");
        }
    }
}
