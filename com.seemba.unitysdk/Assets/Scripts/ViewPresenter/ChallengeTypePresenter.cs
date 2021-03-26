using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        private TextMeshProUGUI _selfText { get { return GetComponent<TextMeshProUGUI>(); } }
        public ChallengesType challenges_type;
        private string _pretext = "";
        // Start is called before the first frame update
        void OnEnable()
        {
            TranslationManager.scene = "Home";
            _pretext = "<color=#535CB3>" + HomeTranslationController.WIN + "</color>";

            switch (challenges_type)
            {
                case ChallengesType._1V1_BUBBLES_CONFIDENT:
                    _selfText.text = _pretext + " " + ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT + " <sprite=0>";   //1V1_BUBBLES_CONFIDENT
                    break;
                case ChallengesType._1V1_BUBBLES_CHAMPION:
                    _selfText.text = _pretext + " " + ChallengeManager.WIN_1V1_BUBBLES_CHAMPION + " <sprite=0>";  //1V1_BUBBLES_CHAMPION
                    break;
                case ChallengesType._1V1_BUBBLES_LEGEND:
                    _selfText.text = _pretext + " " + ChallengeManager.WIN_1V1_BUBBLES_LEGEND + " <sprite=0>";    //1V1_BUBBLES_LEGEND
                    break;
                case ChallengesType._1V1_PRO_CONFIDENT:
                    _selfText.text = _pretext + " " + (ChallengeManager.WIN_1V1_CASH_CONFIDENT * 100).ToString() + " <sprite=1>";// + " " + HomeTranslationController.CROWNS; //1V1_PRO_CONFIDENT
                    break;
                case ChallengesType._1V1_PRO_CHAMPION:
                    _selfText.text = _pretext + " " + (ChallengeManager.WIN_1V1_CASH_CHAMPION * 100).ToString() + " <sprite=1>";// + " " + HomeTranslationController.CROWNS;    //1V1_PRO_CHAMPION
                    break;
                case ChallengesType._1V1_PRO_LEGEND:
                    _selfText.text = _pretext + " " + (ChallengeManager.WIN_1V1_CASH_LEGEND * 100).ToString() + " <sprite=1>";// + " " + HomeTranslationController.CROWNS;  //1V1_PRO_LEGEND
                    break;
                case ChallengesType._BRACKET_BUBBLES_CONFIDENT:
                    _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_BUBBLE_AMATEUR + " <sprite=0>";   //BRACKET_BUBBLES_CONFIDENT  
                    break;
                case ChallengesType._BRACKET_BUBBLES_CHAMPION:
                    _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_BUBBLE_NOVICE + " <sprite=0>";  //BRACKET_BUBBLES_CHAMPION
                    break;
                case ChallengesType._BRACKET_BUBBLES_LEGEND:
                    _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_BUBBLE_CONFIRMED + " <sprite=0>";    //BRACKET_BUBBLES_LEGEND
                    break;
                case ChallengesType._BRACKET_CASH_CONFIDENT:
                    _selfText.text = _pretext + " " + (TournamentManager.WIN_BRACKET_CASH_AMATEUR * 100).ToString() + " <sprite=1>";// + " " + HomeTranslationController.CROWNS; //BRACKET_CASH_CONFIDENT
                    break;
                case ChallengesType._BRACKET_CASH_CHAMPION:
                    _selfText.text = _pretext + " " + (TournamentManager.WIN_BRACKET_CASH_NOVICE * 100).ToString() + " <sprite=1>";// + " " + HomeTranslationController.CROWNS;  //BRACKET_CASH_CHAMPION
                    break;
                case ChallengesType._BRACKET_CASH_LEGEND:
                    _selfText.text = _pretext + " " + (TournamentManager.WIN_BRACKET_CASH_CONFIRMED * 100).ToString() + " <sprite=1>";// + " " + HomeTranslationController.CROWNS;    //BRACKET_CASH_LEGEND
                    break;
            }
        }
    }
}
