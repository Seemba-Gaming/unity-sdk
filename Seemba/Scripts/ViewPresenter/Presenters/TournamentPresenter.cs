using SimpleJSON;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class TournamentPresenter : MonoBehaviour
    {
        #region Script Parameters
        public Button Play;
        public GameObject BracketsPrefab;
        public Transform BracketContent;
        #endregion

        #region Fields
        private JSONNode tournamentJson;
        private JSONArray rounds;
        private JSONArray challenges;
        private JSONArray participants;
        private string userId;
        private string token;
        private GameObject mCurrentBracket;
        private ToursController mToursController;
        #endregion

        #region Unity Methods
        public async void OnEnable()
        {
            userId = UserManager.Get.getCurrentUserId();
            token = UserManager.Get.getCurrentSessionToken();
            tournamentJson = await TournamentManager.Get.getTournament(TournamentController.getCurrentTournamentID(), token);
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
        public JSONArray getChallenges(JSONArray rounds)
        {
            JSONArray challenges = new JSONArray();
            foreach (JSONNode round in rounds)
            {
                foreach (JSONNode challenge in round["challenges"].AsArray)
                {
                    challenges.Add(challenge);
                }
            }
            return challenges;
        }
        public void initUI(JSONArray challenges, JSONArray participants)
        {
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
        public void InitBracketsAsync(JSONNode match, int pos, JSONArray participants, int tourIndex)
        {
            if (match != null)
            {
                float? user_1_score = null;
                float? user_2_score = null;
                float? user_1_old_score = null;
                float? user_2_old_score = null;

                if (match["user_1_score"] != null)
                {
                    user_1_score = match["user_1_score"].AsFloat;
                }

                if (match["user_2_score"] != null)
                {
                    user_2_score = match["user_2_score"].AsFloat;
                }

                if (match["users_old_scores"].Count > 0)
                {
                    user_1_old_score = match["users_old_scores"][match["users_old_scores"].Count - 1]["user_1_score"].AsFloat;
                }

                if (match["users_old_scores"].Count > 0)
                {
                    user_2_old_score = match["users_old_scores"][match["users_old_scores"].Count - 1]["user_2_score"].AsFloat;
                }

                var mFirstMatchedUser = getUserFromParticipants(match["matched_user_1"]["_id"].Value, participants);
                var mSecondMatchedUser = getUserFromParticipants(match["matched_user_2"]["_id"].Value, participants);

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
                            if (match["user_1_score"] != null)
                            {
                                mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.Score.text = ((float)Math.Round(match["user_1_score"].AsFloat * 100f) / 100f).ToString();
                            }

                            if (match["user_2_score"] != null)
                            {
                                mToursController.Tours[tourIndex].ToursChallenges[pos].Player2.Score.text = ((float)Math.Round(match["user_2_score"].AsFloat * 100f) / 100f).ToString();
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
                            if (match["matched_user_1"]["_id"].Value == match["winner_user"].Value)
                            {
                                mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.Score.text = user_1_score.ToString();
                                JSONNode user = getUserFromParticipants(match["matched_user_1"]["_id"].Value, participants);
                                mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.InitAsync(user);

                                if (match["matched_user_1"]["_id"].Value == UserManager.Get.getCurrentUserId())
                                {
                                    PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_WIN, PopupsText.Get.Win());
                                }
                            }
                            else if (match["matched_user_2"]["_id"].Value == match["winner_user"].Value)
                            {
                                mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.Score.text = user_2_score.ToString();
                                JSONNode user = getUserFromParticipants(match["matched_user_2"]["_id"].Value, participants);
                                mToursController.Tours[tourIndex].ToursChallenges[pos].Player1.InitAsync(user);

                                if (match["matched_user_2"]["_id"].Value == UserManager.Get.getCurrentUserId())
                                {
                                    PopupManager.Get.PopupController.ShowPopup(PopupType.POPUP_WIN, PopupsText.Get.Win(), match["gain"].Value);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void initBrakcetPlayersInfo(JSONNode player1, JSONNode player2, int pos, int tourIndex)
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
                user_1_score = tournamentJson["current_challenge"]["user_1_score"].AsFloat;
            }
            catch (NullReferenceException e) { Debug.LogWarning(e.Message); };
            float? user_2_score = null;
            try
            {
                user_2_score = tournamentJson["current_challenge"]["user_2_score"].AsFloat;
            }
            catch (NullReferenceException e) { Debug.LogWarning(e.Message); };

            string matched_user_1 = null;
            try
            {
                matched_user_1 = tournamentJson["current_challenge"]["matched_user_1"]["_id"].Value;
            }
            catch (NullReferenceException e) { Debug.LogWarning(e.Message); };

            string matched_user_2 = null;
            try
            {
                matched_user_2 = tournamentJson["current_challenge"]["matched_user_2"]["_id"].Value;
            }
            catch (NullReferenceException e) { Debug.LogWarning(e.Message); };

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
            foreach (JSONNode loser in tournamentJson["tournament"]["losers"].AsArray)
            {
                if (loser.Value == userId)
                {
                    lose = true;
                    break;
                }
            }

            if (lose == true)
            {
                return false;
            }
            else if (lose == false && tournamentJson["tournament"]["status"].Value == "finished")
            {
                return false;
            }
            return true;
        }
        private void setTournamentData()
        {
            rounds = tournamentJson["tournament"]["rounds"].AsArray;
            challenges = getChallenges(rounds);
            participants = tournamentJson["tournament"]["participants"].AsArray;
            TournamentController.CURRENT_TOURNAMENT_NB_PLAYER = tournamentJson["tournament"]["nb_players"].AsInt;
            TournamentController.CURRENT_TOURNAMENT_GAIN = tournamentJson["tournament"]["gain"].AsFloat;
            TournamentController.CURRENT_TOURNAMENT_GAIN_TYPE = tournamentJson["tournament"]["gain_type"].Value;
        }
        private JSONNode getUserFromParticipants(string id, JSONArray participants)
        {
            foreach (JSONNode player in participants)
            {
                if (player["_id"].Value == id)
                {
                    return player;
                }
            }
            return null;
        }

        #endregion
    }
}
