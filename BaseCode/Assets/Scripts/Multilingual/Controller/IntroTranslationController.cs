using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
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
            TranslationManager.scene = "Intro";
            CHANGE_THE_WAY_YOU_PLAY = TranslationManager.Get("change_the_way_you_play") != string.Empty ? TranslationManager.Get("change_the_way_you_play") : "CHANGE THE WAY YOU PLAY";
            DISCOVER_CASH_TOURNAMENT_POWERED_BY_SEEMBA = TranslationManager.Get("discover_cash_tournament_powered_by_seemba") != string.Empty ? TranslationManager.Get("discover_cash_tournament_powered_by_seemba") : "DISCOVER CASH TOURNAMENTS POWERED BY SEEMBA";
            challenges.text = TranslationManager.Get("challenges") != string.Empty ? TranslationManager.Get("challenges") : challenges.text;
            _continue.text = TranslationManager.Get("continue") != string.Empty ? TranslationManager.Get("continue") : _continue.text;
            Debug.Log("CHANGE_THE_WAY_YOU_PLAY :" + CHANGE_THE_WAY_YOU_PLAY);
            Debug.Log("DISCOVER_CASH_TOURNAMENT_POWERED_BY_SEEMBA :" + DISCOVER_CASH_TOURNAMENT_POWERED_BY_SEEMBA);
        }
    }
}
