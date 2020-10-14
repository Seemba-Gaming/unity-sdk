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

    const string SHOW_LESS = "Less";
    const string SHOW_MORE = "More";

    void Start()
    {
        //Show Available challenges and tournaments
        ShowAvailableChallenges(SHOW_LESS);
        ShowAvailableTournaments(SHOW_LESS);

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
           
            object[] _params = { ChallengeManager.FEE_1V1_CASH_CONFIDENT.ToString("N2"), ChallengeManager.WIN_1V1_CASH_CONFIDENT.ToString("N2"), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        _1v1_cash_champion.onClick.AddListener(() =>
        {
           

            object[] _params = { ChallengeManager.FEE_1V1_CASH_CHAMPION.ToString("N2"), ChallengeManager.WIN_1V1_CASH_CHAMPION.ToString("N2"), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DUELS, _params);
        });
        _1v1_cash_legend.onClick.AddListener(() =>
        {
            

            object[] _params = { ChallengeManager.FEE_1V1_CASH_LEGEND.ToString("N2"), ChallengeManager.WIN_1V1_CASH_LEGEND.ToString("N2"), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
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
        
    }
    void MoreDuels()
    {
        ShowAvailableChallenges(SHOW_MORE);
        less_duels.gameObject.SetActive(true);
        more_duels.gameObject.SetActive(false);
    }
    void LessDuels()
    {
        ShowAvailableChallenges(SHOW_LESS);
        less_duels.gameObject.SetActive(false);
        more_duels.gameObject.SetActive(true);
    }
    void MoreTournaments()
    {
        ShowAvailableTournaments(SHOW_MORE);
        less_tournaments.gameObject.SetActive(true);
        more_tournaments.gameObject.SetActive(false);

    }
    void LessTournaments()
    {
        ShowAvailableTournaments(SHOW_LESS);
        less_tournaments.gameObject.SetActive(false);
        more_tournaments.gameObject.SetActive(true);
    }
    void ShowAvailableChallenges(string show_state)
    {
        if (ChallengeManager.AVALAIBLE_CHALLENGE.Count.Equals(1))
        {
            less_duels.gameObject.SetActive(false);
            more_duels.gameObject.SetActive(false);
        }
        _1v1_cash_confident.gameObject.SetActive(false);
        _1v1_cash_champion.gameObject.SetActive(false);
        _1v1_cash_legend.gameObject.SetActive(false);

        foreach (string challenge_type in ChallengeManager.AVALAIBLE_CHALLENGE)
        {
            switch (challenge_type)
            {
                case ChallengeManager.CHALLENGE_TYPE_NOVICE:
                    _1v1_cash_confident.gameObject.SetActive(true);
                    break;
                case ChallengeManager.CHALLENGE_TYPE_AMATEUR:
                    _1v1_cash_champion.gameObject.SetActive(true);
                    break;
                case ChallengeManager.CHALLENGE_TYPE_CONFIRMED:
                    _1v1_cash_legend.gameObject.SetActive(true);
                    break;
            }
            if (show_state.Equals(SHOW_LESS))
            {
                return;
            }
        }
    }
    void ShowAvailableTournaments(string show_state)
    {
        if (TournamentManager.AVALAIBLE_TOURNAMENTS.Count.Equals(1))
        {
            less_tournaments.gameObject.SetActive(false);
            more_tournaments.gameObject.SetActive(false);
        }
        _bracket_cash_confident.gameObject.SetActive(false);
        _bracket_cash_champion.gameObject.SetActive(false);
        _bracket_cash_legend.gameObject.SetActive(false);

        foreach (string tournament_type in TournamentManager.AVALAIBLE_TOURNAMENTS)
        {
            switch (tournament_type)
            {
                case TournamentManager.BRACKET_TYPE_CONFIDENT:
                    _bracket_cash_confident.gameObject.SetActive(true);
                    break;
                case TournamentManager.BRACKET_TYPE_CHAMPION:
                    _bracket_cash_champion.gameObject.SetActive(true);
                    break;
                case TournamentManager.BRACKET_TYPE_LEGEND:
                    _bracket_cash_legend.gameObject.SetActive(true);
                    break;
            }
            if (show_state.Equals(SHOW_LESS))
            {
                return;
            }
        }
    }

}
