using UnityEngine;
using UnityEngine.UI;

public class HaveFunEventsController : MonoBehaviour
{
    [SerializeField]
    private Button
        more_duels,
        less_duels,
        more_tournaments,
        less_tournaments;
    [SerializeField]
    private Button
        _1v1_bubbles_confident,
        _1v1_bubbles_champion,
        _1v1_bubbles_legend,
        _bracket_bubbles_confident,
        _bracket_bubbles_champion,
        _bracket_bubbles_legend;
    [SerializeField]
    private Button
        free_bubbles,
        extra_bubbles;



    void Start()
    {
        more_duels.onClick.AddListener(() =>
        {
            MoreDuels();
        });
        less_duels.onClick.AddListener(() =>
        {
            LessDuels();
        });
        more_tournaments.onClick.AddListener(() =>
        {
            MoreTournaments();
        });
        less_tournaments.onClick.AddListener(() =>
        {
            LessTournaments();
        });
        _1v1_bubbles_confident.onClick.AddListener(() =>
        {
            object[] _params = { ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT, ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        _1v1_bubbles_champion.onClick.AddListener(() =>
        {
            object[] _params = { ChallengeManager.FEE_1V1_BUBBLES_CHAMPION, ChallengeManager.WIN_1V1_BUBBLES_CHAMPION, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        _1v1_bubbles_legend.onClick.AddListener(() =>
        {
            object[] _params = { ChallengeManager.FEE_1V1_BUBBLES_LEGEND, ChallengeManager.WIN_1V1_BUBBLES_LEGEND, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        _bracket_bubbles_confident.onClick.AddListener(() =>
        {
            object[] _params = { TournamentManager.FEE_BRACKET_BUBBLE_CONFIDENT, TournamentManager.WIN_BRACKET_BUBBLE_CONFIDENT, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_BRACKET };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        _bracket_bubbles_champion.onClick.AddListener(() =>
        {
            object[] _params = { TournamentManager.FEE_BRACKET_BUBBLE_CHAMPION, TournamentManager.WIN_BRACKET_BUBBLE_CHAMPION, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_BRACKET };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        _bracket_bubbles_legend.onClick.AddListener(() =>
        {
            object[] _params = { TournamentManager.FEE_BRACKET_BUBBLE_LEGEND, TournamentManager.WIN_BRACKET_BUBBLE_LEGEND, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_BRACKET };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        /*free_bubbles.onClick.AddListener(() =>
        {

        });
        extra_bubbles.onClick.AddListener(() =>
        {

        });*/
    }
    void MoreDuels()
    {
        _1v1_bubbles_champion.gameObject.SetActive(true);
        _1v1_bubbles_legend.gameObject.SetActive(true);
        less_duels.gameObject.SetActive(true);
        more_duels.gameObject.SetActive(false);
    }
    void LessDuels()
    {
        _1v1_bubbles_champion.gameObject.SetActive(false);
        _1v1_bubbles_legend.gameObject.SetActive(false);
        less_duels.gameObject.SetActive(false);
        more_duels.gameObject.SetActive(true);
    }
    void MoreTournaments()
    {
        _bracket_bubbles_champion.gameObject.SetActive(true);
        _bracket_bubbles_legend.gameObject.SetActive(true);
        less_tournaments.gameObject.SetActive(true);
        more_tournaments.gameObject.SetActive(false);

    }
    void LessTournaments()
    {
        _bracket_bubbles_champion.gameObject.SetActive(false);
        _bracket_bubbles_legend.gameObject.SetActive(false);
        less_tournaments.gameObject.SetActive(false);
        more_tournaments.gameObject.SetActive(true);
    }

}
