using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class Round
    {
        public GenericChallenge[] challenges;
        public bool closed;
        public string _id;
        public string tournament;
        public int round_number;
        public string start_at;
        public string ends_at;
        public string createdAt;
        public string updatedAt;
        public string __v;
    }
    public class TournamentPresenter : MonoBehaviour
    {
        #region Script Parameters
        public Button Play;
        public GameObject BracketsPrefab;
        public Transform BracketContent;
        #endregion

        #region Fields
        private TournamentInfo tournamentJson;
        private Round[] rounds;
        private List<GenericChallenge> challenges = new List<GenericChallenge>();
        private User[] participants;
        private string userId;
        private string token;
        private GameObject mCurrentBracket;
        private ToursController mToursController;
        #endregion

        #region Unity Methods
        public async void OnEnable()
        {
            Destroy(mCurrentBracket);
            userId = UserManager.Get.getCurrentUserId();
            tournamentJson = await TournamentManager.Get.getTournament(TournamentController.getCurrentTournamentID());

            setTournamentData();
            initUI(challenges, participants);
            if (!isAvailable() || !isNextChallengeAvailable())
            {
                Play.interactable = false;
            }
            else
            {
                Play.interactable = true;
            }
        }
        private void OnDisable()
        {
            Destroy(mCurrentBracket);
        }

        #endregion

        #region Methods
        public void OnclickPlay()
        {
            EventsController.ChallengeType = "Bracket";
            if (mCurrentBracket != null)
            {
                Destroy(mCurrentBracket);
            }
            ViewsEvents.Get.GetCurrentMenu().SetActive(false);
            var fee = TournamentManager.Get.GetChallengeFee(TournamentController.CURRENT_TOURNAMENT_GAIN, TournamentController.CURRENT_TOURNAMENT_GAIN_TYPE);
            SeembaAnalyticsManager.Get.SendTournamentInfoEvent("Play Tournament", fee, TournamentController.CURRENT_TOURNAMENT_GAIN, TournamentController.CURRENT_TOURNAMENT_GAIN_TYPE);
            SceneManager.LoadScene(GamesManager.GAME_SCENE_NAME, LoadSceneMode.Additive);
        }
        public List<GenericChallenge> getChallenges(Round[] rounds)
        {
            List<GenericChallenge> challenges = new List<GenericChallenge>();
            foreach (Round round in rounds)
            {
                foreach (GenericChallenge challenge in round.challenges)
                {
                    challenges.Add(challenge);
                }
            }
            return challenges;
        }
        public void initUI(List<GenericChallenge> challenges, User[] participants)
        {
            if(mCurrentBracket != null)
            {
                Destroy(mCurrentBracket);
            }
            mCurrentBracket = Instantiate(BracketsPrefab, BracketContent);
            mToursController = mCurrentBracket.GetComponent<ToursController>();
            int pos = 0;
            int tourIndex = 0;
            for (int i = 0; i < challenges.Count; i++)
            {
                InitBracketsAsync(challenges[i], pos, participants, tourIndex);
                if (mToursController.Tours[tourIndex].ToursChallenges.Count - 1 > pos)
                {
                    pos++;
                }
                else
                {
                    tourIndex++;
                    pos = 0;
                }

                if (i == challenges.Count - 1)
                {
                    InitBracketsAsync(challenges[i], 0, participants, tourIndex);
                }
            }
        }
        public void InitBracketsAsync(GenericChallenge match, int pos, User[] participants, int tourIndex)
        {
            if (match != null)
            {
                float? user_1_score = null;
                float? user_2_score = null;
                float? user_1_old_score = null;
                float? user_2_old_score = null;

                if (match.user_1_score != null)
                {
                    user_1_score = match.user_1_score;
                }

                if (match.user_2_score != null)
                {
                    user_2_score = match.user_2_score;
                }

                if (match.users_old_scores.Length > 0)
                {
                    user_1_old_score = match.users_old_scores[match.users_old_scores.Length -1].user_1_score;
                }

                if (match.users_old_scores.Length > 0)
                {
                    user_2_old_score = match.users_old_scores[match.users_old_scores.Length - 1].user_2_score; ;
                }

                User mFirstMatchedUser = null;
                if(match.matched_user_1 != null)
                {
                    mFirstMatchedUser = getUserFromParticipants(match.matched_user_1._id, participants);
                }
                User mSecondMatchedUser = null;
                if (match.matched_user_2 != null)
                {
                    mSecondMatchedUser = getUserFromParticipants(match.matched_user_2._id, participants);
                }

                if (tourIndex < mToursController.Tours.Count - 1)
                {
                    if (user_1_score == null && user_2_score == null)
                    {
                        if (user_1_old_score != null && user_2_old_score != null)
                        {
                            mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.ShowDraw(user_1_old_score);
                            mToursController.Tours[tourIndex].ToursChallenges[pos].Player2.ShowDraw(user_2_old_score);
                        }
                    }
                    else
                    {
                        if (user_1_score == null && user_1_old_score != null)
                        {
                            mToursController.Tours[tourIndex].ToursChallenges[pos].Player2.ShowBothScores(user_2_old_score, user_2_score);
                        }
                        else if (user_2_score == null && user_2_old_score != null)
                        {
                            mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.ShowBothScores(user_1_old_score, user_1_score);
                        }
                        else
                        {
                            if (match.matched_user_1 != null)
                            {
                                if(match.user_1_score != null)
                                {
                                    mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.Score.text = ((float)Math.Round((float)match.user_1_score * 100f) / 100f).ToString();
                                }
                            }

                            if (match.matched_user_2 != null)
                            {
                                if (match.user_2_score != null)
                                {
                                    mToursController.Tours[tourIndex].ToursChallenges[pos].Player2.Score.text = ((float)Math.Round((float)match.user_2_score * 100f) / 100f).ToString();
                                }
                            }
                        }
                    }
                    initBrakcetPlayersInfo(mFirstMatchedUser, mSecondMatchedUser, pos, tourIndex);
                }
                else
                {
                    if (user_1_score != null && user_2_score != null)
                    {
                        if (user_1_score == user_2_score)
                        {
                            mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.ShowDraw(user_1_score);
                            mToursController.Tours[tourIndex].ToursChallenges[pos].Player2.ShowDraw(user_2_score);
                        }
                        else
                        {
                            if (match.matched_user_1._id == match.winner_user)
                            {
                                mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.Score.text = user_1_score.ToString();
                                User user = getUserFromParticipants(match.matched_user_1._id, participants);
                                mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.InitAsync(user);

                                if (match.matched_user_1._id == UserManager.Get.getCurrentUserId())
                                {
                                    PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_WIN, PopupsText.Get.Win(int.Parse(match.gain), match.gain_type));
                                }
                            }
                            else if (match.matched_user_2._id == match.winner_user)
                            {
                                mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.Score.text = user_2_score.ToString();
                                User user = getUserFromParticipants(match.matched_user_2._id, participants);
                                mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.InitAsync(user);

                                if (match.matched_user_2._id == UserManager.Get.getCurrentUserId())
                                {
                                    PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_WIN, PopupsText.Get.Win(int.Parse(match.gain), match.gain_type));
                                }
                            }
                        }
                    }
                }
            }
        }
        private void initBrakcetPlayersInfo(User player1, User player2, int pos, int tourIndex)
        {
            mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.InitAsync(player1);
            mToursController.Tours[tourIndex].ToursChallenges[pos].Player2.InitAsync(player2);
        }
        #endregion

        #region Implementation
        private bool isNextChallengeAvailable()
        {
            float? user_1_score = null;
            try
            {
                user_1_score = tournamentJson.current_challenge.user_1_score;
            }
            catch (NullReferenceException ) {};
            float? user_2_score = null;
            try
            {
                user_2_score = tournamentJson.current_challenge.user_2_score;
            }
            catch (NullReferenceException ) { };

            string matched_user_1 = null;
            try
            {
                matched_user_1 = tournamentJson.current_challenge.matched_user_1._id;
            }
            catch (NullReferenceException ) {};

            string matched_user_2 = null;
            try
            {
                matched_user_2 = tournamentJson.current_challenge.matched_user_2._id;
            }
            catch (NullReferenceException ) { };

            if (matched_user_1 == userId && user_1_score != null && user_2_score == null)
            {
                return false;
            }
            if (matched_user_2 == userId && user_2_score != null && user_1_score == null)
            {
                return false;
            }
            return true;
        }
        private bool isAvailable()
        {
            var lose = false;
            foreach (string loser in tournamentJson.tournament.losers)
            {
                if (loser == userId)
                {
                    lose = true;
                    break;
                }
            }

            if (lose == true)
            {
                return false;
            }
            else if (lose == false && tournamentJson.tournament.status == "finished")
            {
                return false;
            }
            return true;
        }
        private void setTournamentData()
        {
            rounds = tournamentJson.tournament.rounds;
            challenges = getChallenges(rounds);
            participants = tournamentJson.tournament.participants;
            TournamentController.CURRENT_TOURNAMENT_NB_PLAYER = tournamentJson.tournament.nb_players;
            TournamentController.CURRENT_TOURNAMENT_GAIN = int.Parse(tournamentJson.tournament.gain);
            TournamentController.CURRENT_TOURNAMENT_GAIN_TYPE = tournamentJson.tournament.gain_type;
        }
        private User getUserFromParticipants(string id, User[] participants)
        {
            foreach (User player in participants)
            {
                if (player._id == id)
                {
                    return player;
                }
            }
            return null;
        }

        #endregion
    }
}
