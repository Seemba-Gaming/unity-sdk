using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class ChallengeTypePresenter : MonoBehaviour
    {
        public enum ChallengesType // your custom enumeration
        {
            _1V1_PRO_CONFIDENT,
            _1V1_PRO_CHAMPION,
            _1V1_PRO_LEGEND,
            _1V1_BUBBLES_CONFIDENT,
            _1V1_BUBBLES_CHAMPION,
            _1V1_BUBBLES_LEGEND,
            _BRACKET_CASH_CONFIDENT,
            _BRACKET_CASH_CHAMPION,
            _BRACKET_CASH_LEGEND,
            _BRACKET_BUBBLES_CONFIDENT,
            _BRACKET_BUBBLES_CHAMPION,
            _BRACKET_BUBBLES_LEGEND
        };
        private Text _selfText { get { return GetComponent<Text>(); } }
        public ChallengesType challenges_type;
        public Image CrownImage;
        private string _pretext = "";
        // Start is called before the first frame update
        void OnEnable()
        {
            TranslationManager.scene = "Home";
            _pretext = "<color=#535CB3>" + HomeTranslationController.WIN + "</color>";

            switch (challenges_type)
            {
                case ChallengesType._1V1_BUBBLES_CONFIDENT:
                    _selfText.text = _pretext + " " + ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT + " " + HomeTranslationController.BUBBLES;   //1V1_BUBBLES_CONFIDENT
                    break;
                case ChallengesType._1V1_BUBBLES_CHAMPION:
                    _selfText.text = _pretext + " " + ChallengeManager.WIN_1V1_BUBBLES_CHAMPION + " " + HomeTranslationController.BUBBLES;  //1V1_BUBBLES_CHAMPION
                    break;
                case ChallengesType._1V1_BUBBLES_LEGEND:
                    _selfText.text = _pretext + " " + ChallengeManager.WIN_1V1_BUBBLES_LEGEND + " " + HomeTranslationController.BUBBLES;    //1V1_BUBBLES_LEGEND
                    break;
                case ChallengesType._1V1_PRO_CONFIDENT:
                    _selfText.text = _pretext + " " + ChallengeManager.WIN_1V1_CASH_CONFIDENT.ToString();// + " " + HomeTranslationController.CROWNS; //1V1_PRO_CONFIDENT
                    CrownImage.gameObject.SetActive(true);
                    break;
                case ChallengesType._1V1_PRO_CHAMPION:
                    _selfText.text = _pretext + " " + ChallengeManager.WIN_1V1_CASH_CHAMPION.ToString();// + " " + HomeTranslationController.CROWNS;    //1V1_PRO_CHAMPION
                    CrownImage.gameObject.SetActive(true);
                    break;
                case ChallengesType._1V1_PRO_LEGEND:
                    _selfText.text = _pretext + " " + ChallengeManager.WIN_1V1_CASH_LEGEND.ToString();// + " " + HomeTranslationController.CROWNS;  //1V1_PRO_LEGEND
                    CrownImage.gameObject.SetActive(true);
                    break;
                case ChallengesType._BRACKET_BUBBLES_CONFIDENT:
                    _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_BUBBLE_AMATEUR + " " + HomeTranslationController.BUBBLES;   //BRACKET_BUBBLES_CONFIDENT  
                    break;
                case ChallengesType._BRACKET_BUBBLES_CHAMPION:
                    _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_BUBBLE_NOVICE + " " + HomeTranslationController.BUBBLES;  //BRACKET_BUBBLES_CHAMPION
                    break;
                case ChallengesType._BRACKET_BUBBLES_LEGEND:
                    _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_BUBBLE_CONFIRMED + " " + HomeTranslationController.BUBBLES;    //BRACKET_BUBBLES_LEGEND
                    break;
                case ChallengesType._BRACKET_CASH_CONFIDENT:
                    _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_CASH_AMATEUR.ToString();// + " " + HomeTranslationController.CROWNS; //BRACKET_CASH_CONFIDENT
                    CrownImage.gameObject.SetActive(true);
                    break;
                case ChallengesType._BRACKET_CASH_CHAMPION:
                    _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_CASH_NOVICE.ToString();// + " " + HomeTranslationController.CROWNS;  //BRACKET_CASH_CHAMPION
                    CrownImage.gameObject.SetActive(true);
                    break;
                case ChallengesType._BRACKET_CASH_LEGEND:
                    _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_CASH_CONFIRMED.ToString();// + " " + HomeTranslationController.CROWNS;    //BRACKET_CASH_LEGEND
                    CrownImage.gameObject.SetActive(true);
                    break;
            }
        }
    }
}
