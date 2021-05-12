using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class IntroTranslationController : MonoBehaviour
    {
        public Text challenges;
        public Text _continue;
        public static string
                      CHANGE_THE_WAY_YOU_PLAY,
                      DISCOVER_CASH_TOURNAMENT_POWERED_BY_SEEMBA;
        // Start is called before the first frame update
        void Start()
        {
            TranslationManager._instance.scene = "Intro";
            CHANGE_THE_WAY_YOU_PLAY = TranslationManager._instance.Get("change_the_way_you_play") != string.Empty ? TranslationManager._instance.Get("change_the_way_you_play") : "CHANGE THE WAY YOU PLAY";
            DISCOVER_CASH_TOURNAMENT_POWERED_BY_SEEMBA = TranslationManager._instance.Get("discover_cash_tournament_powered_by_seemba") != string.Empty ? TranslationManager._instance.Get("discover_cash_tournament_powered_by_seemba") : "DISCOVER CASH TOURNAMENTS POWERED BY SEEMBA";
            challenges.text = TranslationManager._instance.Get("challenges") != string.Empty ? TranslationManager._instance.Get("challenges") : challenges.text;
            _continue.text = TranslationManager._instance.Get("continue") != string.Empty ? TranslationManager._instance.Get("continue") : _continue.text;
            Debug.Log("CHANGE_THE_WAY_YOU_PLAY :" + CHANGE_THE_WAY_YOU_PLAY);
            Debug.Log("DISCOVER_CASH_TOURNAMENT_POWERED_BY_SEEMBA :" + DISCOVER_CASH_TOURNAMENT_POWERED_BY_SEEMBA);
        }
    }
}
