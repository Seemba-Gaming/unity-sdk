using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinMoneyEventsController : MonoBehaviour
{
    [SerializeField]
    private Button
         more_duels,
         less_duels,
         more_tournaments,
         less_tournaments;
    [SerializeField]
    private Button
        _1v1_cash_confident,
        _1v1_cash_champion,
        _1v1_cash_legend,
        _bracket_cash_confident,
        _bracket_cash_champion,
        _bracket_cash_legend;



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
        _1v1_cash_confident.onClick.AddListener(() =>
        {
           
            object[] _params = { ChallengeManager.FEE_1V1_PRO_CONFIDENT.ToString("N2"), ChallengeManager.WIN_1V1_PRO_CONFIDENT.ToString("N2"), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        _1v1_cash_champion.onClick.AddListener(() =>
        {
           

            object[] _params = { ChallengeManager.FEE_1V1_PRO_CHAMPION.ToString("N2"), ChallengeManager.WIN_1V1_PRO_CHAMPION.ToString("N2"), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        _1v1_cash_legend.onClick.AddListener(() =>
        {
            

            object[] _params = { ChallengeManager.FEE_1V1_PRO_LEGEND.ToString("N2"), ChallengeManager.WIN_1V1_PRO_LEGEND.ToString("N2"), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        _bracket_cash_confident.onClick.AddListener(() =>
        {
            object[] _params = { TournamentManager.FEE_BRACKET_CASH_CONFIDENT.ToString("N2"), TournamentManager.WIN_BRACKET_CASH_CONFIDENT.ToString("N2"), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_BRACKET };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        _bracket_cash_champion.onClick.AddListener(() =>
        {
            object[] _params = { TournamentManager.FEE_BRACKET_CASH_CHAMPION.ToString("N2"), TournamentManager.WIN_BRACKET_CASH_CHAMPION.ToString("N2"), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_BRACKET };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        _bracket_cash_legend.onClick.AddListener(() =>
        {
            object[] _params = { TournamentManager.FEE_BRACKET_CASH_LEGEND.ToString("N2"), TournamentManager.WIN_BRACKET_CASH_LEGEND.ToString("N2"), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_BRACKET };
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
        _1v1_cash_champion.gameObject.SetActive(true);
        _1v1_cash_legend.gameObject.SetActive(true);
        less_duels.gameObject.SetActive(true);
        more_duels.gameObject.SetActive(false);
    }
    void LessDuels()
    {
        _1v1_cash_champion.gameObject.SetActive(false);
        _1v1_cash_legend.gameObject.SetActive(false);
        less_duels.gameObject.SetActive(false);
        more_duels.gameObject.SetActive(true);
    }
    void MoreTournaments()
    {
        _bracket_cash_champion.gameObject.SetActive(true);
        _bracket_cash_legend.gameObject.SetActive(true);
        less_tournaments.gameObject.SetActive(true);
        more_tournaments.gameObject.SetActive(false);

    }
    void LessTournaments()
    {
        _bracket_cash_champion.gameObject.SetActive(false);
        _bracket_cash_legend.gameObject.SetActive(false);
        less_tournaments.gameObject.SetActive(false);
        more_tournaments.gameObject.SetActive(true);
    }

}
