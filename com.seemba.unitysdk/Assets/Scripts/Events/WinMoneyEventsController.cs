using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class WinMoneyEventsController : MonoBehaviour
    {
        #region Static
        private const string SHOW_LESS = "Less";
        private const string SHOW_MORE = "More";
        #endregion

        #region Script Parameters
        public Button more_duels;
        public Button less_duels;
        public Button more_tournaments;
        public Button less_tournaments;
        public Button _1v1_cash_confident;
        public Button _1v1_cash_champion;
        public Button _1v1_cash_legend;
        public Button _bracket_cash_confident;
        public Button _bracket_cash_champion;
        public Button _bracket_cash_legend;
        #endregion

        private void Start()
        {
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
                SeembaAnalyticsManager.Get.SendDuelInfoEvent("Duel Money", ChallengeManager.FEE_1V1_CASH_CONFIDENT, ChallengeManager.WIN_1V1_CASH_CONFIDENT, ChallengeManager.CHALLENGE_WIN_TYPE_CASH);
                object[] _params = { (ChallengeManager.FEE_1V1_CASH_CONFIDENT).ToString(), (ChallengeManager.WIN_1V1_CASH_CONFIDENT).ToString(), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
                PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
            });
            _1v1_cash_champion.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendDuelInfoEvent("Duel Money", ChallengeManager.FEE_1V1_CASH_CHAMPION, ChallengeManager.WIN_1V1_CASH_CHAMPION, ChallengeManager.CHALLENGE_WIN_TYPE_CASH);
                object[] _params = { (ChallengeManager.FEE_1V1_CASH_CHAMPION ).ToString(), (ChallengeManager.WIN_1V1_CASH_CHAMPION ).ToString(), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
                PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
            });
            _1v1_cash_legend.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendDuelInfoEvent("Duel Money", ChallengeManager.FEE_1V1_CASH_LEGEND, ChallengeManager.WIN_1V1_CASH_LEGEND, ChallengeManager.CHALLENGE_WIN_TYPE_CASH);
                object[] _params = { (ChallengeManager.FEE_1V1_CASH_LEGEND).ToString(), (ChallengeManager.WIN_1V1_CASH_LEGEND ).ToString(), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_1V1 };
                PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
            });
            _bracket_cash_confident.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendTournamentInfoEvent("Tournament Money", TournamentManager.FEE_BRACKET_CASH_AMATEUR, TournamentManager.WIN_BRACKET_CASH_AMATEUR, ChallengeManager.CHALLENGE_WIN_TYPE_CASH);
                object[] _params = { (TournamentManager.FEE_BRACKET_CASH_AMATEUR ).ToString(), (TournamentManager.WIN_BRACKET_CASH_AMATEUR ).ToString(), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_BRACKET };
                PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
            });
            _bracket_cash_champion.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendTournamentInfoEvent("Tournament Money", TournamentManager.FEE_BRACKET_CASH_NOVICE, TournamentManager.WIN_BRACKET_CASH_NOVICE, ChallengeManager.CHALLENGE_WIN_TYPE_CASH);
                object[] _params = { (TournamentManager.FEE_BRACKET_CASH_NOVICE).ToString(), (TournamentManager.WIN_BRACKET_CASH_NOVICE ).ToString(), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_BRACKET };
                PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
            });
            _bracket_cash_legend.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendTournamentInfoEvent("Tournament Money", TournamentManager.FEE_BRACKET_CASH_CONFIRMED, TournamentManager.WIN_BRACKET_CASH_CONFIRMED, ChallengeManager.CHALLENGE_WIN_TYPE_CASH);
                object[] _params = { (TournamentManager.FEE_BRACKET_CASH_CONFIRMED ).ToString(), (TournamentManager.WIN_BRACKET_CASH_CONFIRMED).ToString(), ChallengeManager.CHALLENGE_WIN_TYPE_CASH, ChallengeManager.CHALLENGE_TYPE_BRACKET };
                PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
            });
        }

        private void MoreDuels()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("More Money Duels");
            ShowAvailableChallenges(SHOW_MORE);
            less_duels.gameObject.SetActive(true);
            more_duels.gameObject.SetActive(false);
        }

        private void LessDuels()
        {
            ShowAvailableChallenges(SHOW_LESS);
            less_duels.gameObject.SetActive(false);
            more_duels.gameObject.SetActive(true);
        }

        private void MoreTournaments()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("More Money Tournaments");
            ShowAvailableTournaments(SHOW_MORE);
            less_tournaments.gameObject.SetActive(true);
            more_tournaments.gameObject.SetActive(false);

        }

        private void LessTournaments()
        {
            ShowAvailableTournaments(SHOW_LESS);
            less_tournaments.gameObject.SetActive(false);
            more_tournaments.gameObject.SetActive(true);
        }

        private void ShowAvailableChallenges(string show_state)
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

        private void ShowAvailableTournaments(string show_state)
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
}