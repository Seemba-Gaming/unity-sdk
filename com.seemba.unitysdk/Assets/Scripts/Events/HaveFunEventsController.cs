using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class HaveFunEventsController : MonoBehaviour
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
        public Button _1v1_bubbles_amateur;
        public Button _1v1_bubbles_novice;
        public Button _1v1_bubbles_confirmed;
        public Button _bracket_bubbles_confident;
        public Button _bracket_bubbles_champion;
        public Button _bracket_bubbles_legend;
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
            _1v1_bubbles_amateur.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendDuelInfoEvent("Duel Coins", ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT, ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES);
                object[] _params = { ChallengeManager.FEE_1V1_BUBBLES_CONFIDENT, ChallengeManager.WIN_1V1_BUBBLES_CONFIDENT, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_1V1 };
                PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
            });
            _1v1_bubbles_novice.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendDuelInfoEvent("Duel Coins", ChallengeManager.FEE_1V1_BUBBLES_CHAMPION, ChallengeManager.WIN_1V1_BUBBLES_CHAMPION, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES);
                object[] _params = { ChallengeManager.FEE_1V1_BUBBLES_CHAMPION, ChallengeManager.WIN_1V1_BUBBLES_CHAMPION, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_1V1 };
                PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);

            });
            _1v1_bubbles_confirmed.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendDuelInfoEvent("Duel Coins", ChallengeManager.FEE_1V1_BUBBLES_LEGEND, ChallengeManager.WIN_1V1_BUBBLES_LEGEND, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES);
                object[] _params = { ChallengeManager.FEE_1V1_BUBBLES_LEGEND, ChallengeManager.WIN_1V1_BUBBLES_LEGEND, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_1V1 };
                PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);

            });
            _bracket_bubbles_confident.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendTournamentInfoEvent("Tournament Coins", TournamentManager.FEE_BRACKET_BUBBLE_AMATEUR, TournamentManager.WIN_BRACKET_BUBBLE_AMATEUR, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES);
                object[] _params = { TournamentManager.FEE_BRACKET_BUBBLE_AMATEUR, TournamentManager.WIN_BRACKET_BUBBLE_AMATEUR, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_BRACKET };
                PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);

            });
            _bracket_bubbles_champion.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendTournamentInfoEvent("Tournament Coins", TournamentManager.FEE_BRACKET_BUBBLE_AMATEUR, TournamentManager.WIN_BRACKET_BUBBLE_AMATEUR, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES);
                object[] _params = { TournamentManager.FEE_BRACKET_BUBBLE_NOVICE, TournamentManager.WIN_BRACKET_BUBBLE_NOVICE, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_BRACKET };
                PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);

            });
            _bracket_bubbles_legend.onClick.AddListener(() =>
            {
                SeembaAnalyticsManager.Get.SendTournamentInfoEvent("Tournament Coins", TournamentManager.FEE_BRACKET_BUBBLE_CONFIRMED, TournamentManager.WIN_BRACKET_BUBBLE_CONFIRMED, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES);
                object[] _params = { TournamentManager.FEE_BRACKET_BUBBLE_CONFIRMED, TournamentManager.WIN_BRACKET_BUBBLE_CONFIRMED, ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES, ChallengeManager.CHALLENGE_TYPE_BRACKET };
                PopupManager.Get.PopupController.ShowPopup(PopupType.DUELS, _params);
            });
        }

        private void MoreDuels()
        {
            SeembaAnalyticsManager.Get.SendUserEvent("More Coins Duels");
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
            SeembaAnalyticsManager.Get.SendUserEvent("More Coins Tournaments");
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
            _1v1_bubbles_novice.gameObject.SetActive(false);
            _1v1_bubbles_amateur.gameObject.SetActive(false);
            _1v1_bubbles_confirmed.gameObject.SetActive(false);

            foreach (string challenge_type in ChallengeManager.AVALAIBLE_CHALLENGE)
            {
                switch (challenge_type)
                {
                    case ChallengeManager.CHALLENGE_TYPE_NOVICE:
                        _1v1_bubbles_novice.gameObject.SetActive(true);
                        break;
                    case ChallengeManager.CHALLENGE_TYPE_AMATEUR:
                        _1v1_bubbles_amateur.gameObject.SetActive(true);
                        break;
                    case ChallengeManager.CHALLENGE_TYPE_CONFIRMED:
                        _1v1_bubbles_confirmed.gameObject.SetActive(true);
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
            _bracket_bubbles_confident.gameObject.SetActive(false);
            _bracket_bubbles_champion.gameObject.SetActive(false);
            _bracket_bubbles_legend.gameObject.SetActive(false);

            foreach (string tournament_type in TournamentManager.AVALAIBLE_TOURNAMENTS)
            {
                switch (tournament_type)
                {
                    case TournamentManager.BRACKET_TYPE_CONFIDENT:
                        _bracket_bubbles_confident.gameObject.SetActive(true);
                        break;
                    case TournamentManager.BRACKET_TYPE_CHAMPION:
                        _bracket_bubbles_champion.gameObject.SetActive(true);
                        break;
                    case TournamentManager.BRACKET_TYPE_LEGEND:
                        _bracket_bubbles_legend.gameObject.SetActive(true);
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
