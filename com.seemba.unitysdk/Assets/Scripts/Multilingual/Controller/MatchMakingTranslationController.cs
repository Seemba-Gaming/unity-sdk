using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class MatchMakingTranslationController : MonoBehaviour
    {
        public Text YouCanWin;
        public Text LookingForYourOpponent;
        public Text PlayNow;

        private string LookingFor, YourOpponent;
        // Start is called before the first frame update
        void Start()
        {
            TranslationManager.scene = "Matchmaking";
            YouCanWin.text = TranslationManager.Get("you_can_win") != string.Empty ? TranslationManager.Get("you_can_win") : YouCanWin.text;
            LookingFor = TranslationManager.Get("looking_for") != string.Empty ? TranslationManager.Get("looking_for") : LookingFor;
            YourOpponent = TranslationManager.Get("your_opponent") != string.Empty ? TranslationManager.Get("your_opponent") : YourOpponent;
            LookingForYourOpponent.text = LookingFor + " <color=#535CB3FF> " + YourOpponent + " </color>";
            PlayNow.text = TranslationManager.Get("play_now") != string.Empty ? TranslationManager.Get("play_now") : PlayNow.text;
        }
    }
}
