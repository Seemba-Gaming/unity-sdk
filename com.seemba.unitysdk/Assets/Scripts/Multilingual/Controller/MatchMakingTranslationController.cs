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
            TranslationManager._instance.scene = "Matchmaking";
            YouCanWin.text = TranslationManager._instance.Get("you_can_win") != string.Empty ? TranslationManager._instance.Get("you_can_win") : YouCanWin.text;
            LookingFor = TranslationManager._instance.Get("looking_for") != string.Empty ? TranslationManager._instance.Get("looking_for") : LookingFor;
            YourOpponent = TranslationManager._instance.Get("your_opponent") != string.Empty ? TranslationManager._instance.Get("your_opponent") : YourOpponent;
            LookingForYourOpponent.text = LookingFor + " <color=#535CB3FF> " + YourOpponent + " </color>";
            PlayNow.text = TranslationManager._instance.Get("play_now") != string.Empty ? TranslationManager._instance.Get("play_now") : PlayNow.text;
        }
    }
}
