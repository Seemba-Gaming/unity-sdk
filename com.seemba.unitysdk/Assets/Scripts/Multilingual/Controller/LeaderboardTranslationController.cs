using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class LeaderboardTranslationController : MonoBehaviour
    {
        public Text Leaderboard;
        public Text Daily;
        public Text Weekly;
        public Text Monthly;
        void Start()
        {
            TranslationManager.scene = "Leaderboard";
            Leaderboard.text = TranslationManager.Get("leaderboard");
            Daily.text = TranslationManager.Get("daily");
            Weekly.text = TranslationManager.Get("weekly");
            Monthly.text = TranslationManager.Get("monthly");
        }
    }
}
