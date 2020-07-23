using UnityEngine;
using UnityEngine.UI;
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
                _selfText.text = _pretext + " " + ChallengeManager.WIN_1V1_PRO_CONFIDENT.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;   //1V1_PRO_CONFIDENT
                break;
            case ChallengesType._1V1_PRO_CHAMPION:
                _selfText.text = _pretext + " " + ChallengeManager.WIN_1V1_PRO_CHAMPION.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;    //1V1_PRO_CHAMPION
                break;
            case ChallengesType._1V1_PRO_LEGEND:
                _selfText.text = _pretext + " " + ChallengeManager.WIN_1V1_PRO_LEGEND.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;  //1V1_PRO_LEGEND
                break;
            case ChallengesType._BRACKET_BUBBLES_CONFIDENT:
                _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_BUBBLE_CONFIDENT + " " + HomeTranslationController.BUBBLES;   //BRACKET_BUBBLES_CONFIDENT  
                break;
            case ChallengesType._BRACKET_BUBBLES_CHAMPION:
                _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_BUBBLE_CHAMPION + " " + HomeTranslationController.BUBBLES;  //BRACKET_BUBBLES_CHAMPION
                break;
            case ChallengesType._BRACKET_BUBBLES_LEGEND:
                _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_BUBBLE_LEGEND + " " + HomeTranslationController.BUBBLES;    //BRACKET_BUBBLES_LEGEND
                break;
            case ChallengesType._BRACKET_CASH_CONFIDENT:
                _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_CASH_CONFIDENT.ToString("N2") + CurrencyManager.CURRENT_CURRENCY; //BRACKET_CASH_CONFIDENT
                break;
            case ChallengesType._BRACKET_CASH_CHAMPION:
                _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_CASH_CHAMPION.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;  //BRACKET_CASH_CHAMPION
                break;
            case ChallengesType._BRACKET_CASH_LEGEND:
                _selfText.text = _pretext + " " + TournamentManager.WIN_BRACKET_CASH_LEGEND.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;    //BRACKET_CASH_LEGEND
                break;
        }
    }
}
