using UnityEngine;
using System.Collections;
using System;
using System.Text;
using SimpleJSON;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class GenericTournament
    {
        public string                               _id;
        public int                                  nb_current_players;
        public bool                                 is_full;
        public Round[]                              rounds;
        public string[]                             losers;
        public User[]                               participants;
        public string                               game_level;
        public int                                  nb_players;
        public string                               gain;
        public string                               gain_type;
        public string                               game;
        public string                               status;
        public string                               createdAt;
        public string                               updatedAt;
        public string                               __v;
        public string                               winner_user;
    }

    [CLSCompliant(false)]
    public class OldScores
    {
        public int                                  user_1_score;
        public int                                  user_2_score;
    }
        [CLSCompliant(false)]
    public class GenericChallenge
    {
        public string                               _id;
        public float?                               user_1_score;
        public float?                               user_2_score;
        public OldScores[]                          users_old_scores;
        public string                               tournament_id;
        public GenericGameInfo                      game;
        public int?                                 game_level;
        public string                               status;
        public string                               challenge_type;
        public string                               gain;
        public string                               gain_type;
        public string                               next_challenge_id;
        public string                               createdAt;
        public string                               updatedAt;
        public User                                 matched_user_1;
        public string                               matched_user_1_contry;
        public string                               user_1_joined_at;
        public User                                 matched_user_2;
        public string                               matched_user_2_contry;
        public string                               user_2_joined_at;
        public string                               winner_user;
        public string                               __v;
        public GenericTournament                    tournament;
    }
    [CLSCompliant(false)]
    public class GenericUser
    {
        public string                               _id;
        public bool                                 username_changed;
        public bool                                 email_verified;
        public string                               address;
        public string                               country;
        public string                               firstname;
        public string                               lastname;
        public int                                  highest_victories_streak;
        public int                                  current_victories_count;
        public string                               last_bubble_click;
        public string                               level;
        public string                               payment_account_id;
        public string                               city;
        public string                               state;
        public string                               max_withdraw;
        public bool                                 is_bot;
        public string[]                             games;
        public string                               username;
        public string                               email;
        public string                               avatar;
        public string                               country_code;
        public string                               long_lat;
        public string                               paymentGateway;
        public string                               createdAt;
        public string                               updatedAt;
        public string                               bubbles;
        public string                               coins;
        public string                               customer_id;
        public string                               last_connection;
        public string                               zipcode;
    }
    [CLSCompliant(false)]
    public class GenericGameInfo
    {
        public string                               _id;
        public string[]                             platforms;
        public string[]                             brackets;
        public string[]                             tournaments;
        public string                               name;
        public string                               editor;
        public string                               team;
        public string                               appstore_id;
        public string                               bundle_id;
        public string                               createdAt;
        public string                               description;
        public string                               store_link; //android
        public string                               icon;
        public string                               background_image;
        public string                               gcm_api_key;
        public string                               status;
        public string                               orientation;
        public string                               engine;
        public string                               secret;
        public string                               android_name;
        public string                               android_version;
        public string                               ios_name;
        public string                               ios_version;
        public string                               __v;
        public bool                                 completed;
        public bool                                 deleted;
        public string                               app_store_link;
        public string                               score_mode;
        public string                               updatedAt;
    }
    [CLSCompliant(false)]
    public class ChallengeManager : MonoBehaviour
    {
        #region Static
        public static ChallengeManager Get { get { return sInstance; } }

        private static ChallengeManager sInstance;
        #endregion

        public string                               AdversaryId;
        public static string                        CurrentChallengeGain, CurrentChallengeGainType;
        public static string                        CurrentChallengeId;
        public static Challenge                     CurrentChallenge;
        public static string                        CurrentChallengeRequestId;
        public static string                        CurrentChallengeStatus;
        public static System.Timers.Timer           aTimer = new System.Timers.Timer();
        public static System.Timers.Timer           t;
        public static string date;
        //Set All Challenges Details
        public static float                         FEE_1V1_CASH_CONFIDENT;// = 1.20f;
        public static float                         FEE_1V1_CASH_CHAMPION;// = 3.00f;
        public static float                         FEE_1V1_CASH_LEGEND;// = 6.00f;
        public static float                         WIN_1V1_CASH_CONFIDENT;// = 2.00f;
        public static float                         WIN_1V1_CASH_CHAMPION;// = 5.00f;
        public static float                         WIN_1V1_CASH_LEGEND;// = 10.00f;
        public static int                           FEE_1V1_BUBBLES_CONFIDENT;// = 1.00f;
        public static int                           FEE_1V1_BUBBLES_CHAMPION;// = 3.00f;
        public static int                           FEE_1V1_BUBBLES_LEGEND;// = 5.00f;
        public static int                           WIN_1V1_BUBBLES_CONFIDENT;// = 2f;
        public static int                           WIN_1V1_BUBBLES_CHAMPION;// = 6f;
        public static int                           WIN_1V1_BUBBLES_LEGEND;// = 10f;
        public const string                         CHALLENGE_TYPE_BRACKET = "Bracket";
        public const string                         CHALLENGE_TYPE_1V1 = "1vs1";
        public const string                         CHALLENGE_WIN_TYPE_BUBBLES = "bubble";
        public const string                         CHALLENGE_WIN_TYPE_CASH = "cash";
        public const string                         CHALLENGE_STATUS_RESULT_PENDING = "results pending";
        public const string                         CHALLENGE_STATUS_SEE_RESULT_FOR_USER1 = "see results for user 1";
        public const string                         CHALLENGE_STATUS_SEE_RESULT_FOR_USER2 = "see results for user 2";
        public const string                         CHALLENGE_STATUS_FINISHED = "finished";

        public const string                         CHALLENGE_TYPE_AMATEUR = "Amateur";
        public const string                         CHALLENGE_TYPE_NOVICE = "Novice";
        public const string                         CHALLENGE_TYPE_CONFIRMED = "Confirmed";

        public static List<string>                  AVALAIBLE_CHALLENGE = new List<string>();

        private void Awake()
        {
            sInstance = this;
        }
        // Use this for initialization
        void Start()
        {
            //UserManager.Get = new UserManager ();
        }

        public void InitFees(GameChallengesInfo fees)
        {
            FEE_1V1_CASH_CONFIDENT = fees.duels.Confident.cash;
            FEE_1V1_CASH_CHAMPION = fees.duels.Champion.cash;
            FEE_1V1_CASH_LEGEND = fees.duels.Legend.cash;
            FEE_1V1_BUBBLES_CONFIDENT = (int)fees.duels.Confident.bubbles;
            FEE_1V1_BUBBLES_CHAMPION = (int)fees.duels.Champion.bubbles;
            FEE_1V1_BUBBLES_LEGEND = (int)fees.duels.Legend.bubbles;
        }

        public void InitGains(GameChallengesInfo gain)
        {

            WIN_1V1_CASH_CONFIDENT = gain.duels.Confident.cash;
            WIN_1V1_CASH_CHAMPION = gain.duels.Champion.cash;
            WIN_1V1_CASH_LEGEND = gain.duels.Legend.cash;
            WIN_1V1_BUBBLES_CONFIDENT = (int)gain.duels.Confident.bubbles;
            WIN_1V1_BUBBLES_CHAMPION = (int)gain.duels.Champion.bubbles;
            WIN_1V1_BUBBLES_LEGEND = (int)gain.duels.Legend.bubbles;
        }

        public float GetChallengeFee(float gain, string gainType)
        {
            if (gainType.Equals(CHALLENGE_WIN_TYPE_CASH))
            {
                if (gain.Equals(WIN_1V1_CASH_CONFIDENT))
                {
                    return FEE_1V1_CASH_CONFIDENT;
                }
                else if (gain.Equals(WIN_1V1_CASH_CHAMPION))
                {
                    return FEE_1V1_CASH_CHAMPION;
                }
                else
                {
                    return FEE_1V1_CASH_LEGEND;
                }
            }
            else
            {
                if (gain.Equals(WIN_1V1_BUBBLES_CONFIDENT))
                {
                    return FEE_1V1_BUBBLES_CONFIDENT;
                }
                else if (gain.Equals(WIN_1V1_BUBBLES_CHAMPION))
                {
                    return FEE_1V1_BUBBLES_CHAMPION;
                }
                else
                {
                    return FEE_1V1_BUBBLES_LEGEND;
                }
            }
        }

        private void Win()
        {
            string[] attrib = { "last_result" };
            string[] values = { "WIN" };
            UserManager.Get.UpdateUserByField(attrib, values);
        }
        private void Loss()
        {
            string[] attrib = { "last_result" };
            string[] values = { "loss" };
            UserManager.Get.UpdateUserByField(attrib, values);
        }
        public void ShowResult()
        {
            EventsController.Get.AudioListener.enabled = true;
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.ResultPresenter.gameObject);
        }
        public async Task<GenericChallenge> getChallengebyIdAsync(string challengeId)
        {
            string url = Endpoint.classesURL + "/challenges/" + challengeId;
            var response = await SeembaWebRequest.Get.HttpsGetJSON<GenericChallenge>(url);
            if (response != null)
            {
                return response;
            }
            else
            {
                return null;
            }
        }
        public async Task<ArrayList> getPendingChallenges(string token)
        {
            string url = Endpoint.classesURL + "/challenges/pending?game_id=" + GamesManager.GAME_ID;
            Challenge[] challengesList = await SeembaWebRequest.Get.HttpsGetJSON<Challenge[]>(url);
            ArrayList pendingChallenges = new ArrayList();
            foreach (Challenge challenge in challengesList)
            {
                pendingChallenges.Add(challenge);
            }
            return pendingChallenges;
        }
        public async Task<ArrayList> getSeeResultsChallenges(string token)
        {
            string url = Endpoint.classesURL + "/challenges/see_results?game_id=" + GamesManager.GAME_ID;
            Challenge[] challengesList = await SeembaWebRequest.Get.HttpsGetJSON<Challenge[]>(url);
            ArrayList seeResultChallenges = new ArrayList();
            foreach (Challenge challenge in challengesList)
            {
                seeResultChallenges.Add(challenge);
            }
            return seeResultChallenges;
        }
        public async Task<GenericChallenge[]> GetOnGoingChallenges(int page, int pageSize)
        {
            string url = Endpoint.classesURL + "/challenges/ongoing?game_id=" + GamesManager.GAME_ID + "&page=" + page + "&pagesize=" + pageSize;
            Debug.LogWarning(url);
            var response = await SeembaWebRequest.Get.HttpsGetJSON<GenericChallenge[]>(url);
            return response;
        }
        public async Task<GenericChallenge[]> GetLastResulatChallenges(int page, int pageSize)
        {
            string url = Endpoint.classesURL + "/challenges/last_results?game_id=" + GamesManager.GAME_ID + "&page=" + page + "&pagesize=" + pageSize;
            var response = await SeembaWebRequest.Get.HttpsGetJSON<GenericChallenge[]>(url);
            //Debug.LogWarning(url);
            return response;
        }
        public async Task<ArrayList> getFinishedChallenges(string token)
        {
            string url = Endpoint.classesURL + "/challenges/finished?game_id=" + GamesManager.GAME_ID;
            var challengeListData = await SeembaWebRequest.Get.HttpsGetJSON<Challenge[]>(url);
            ArrayList finishedChallenges = new ArrayList();
            foreach (Challenge challenge in challengeListData)
            {
                finishedChallenges.Add(challenge);
            }
            return finishedChallenges;
        }
        public async Task<ArrayList> listChallenges()
        {
            string url = Endpoint.classesURL + "/challenges?game_id=" + GamesManager.GAME_ID;
            Challenge[] Challenges = await SeembaWebRequest.Get.HttpsGetJSON<Challenge[]>(url);
            ArrayList challenges = new ArrayList();
            foreach (Challenge challenge in Challenges)
            {
                challenges.Add(challenge);
            }
            return challenges;
        }
        public async Task<Challenge> getChallenge(string challenge_id)
        {
            string url = Endpoint.classesURL + "/challenges/" + challenge_id;
            return await SeembaWebRequest.Get.HttpsGetJSON<Challenge>(url);
        }

        public async Task<bool> addScore(string challengeId, float score)
        {
            string url = Endpoint.classesURL + "/challenges";

            string json = "challenge_id=" + challengeId + "&score=" + score;
            byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);
            var response = await SeembaWebRequest.Get.HttpsPut(url, jsonAsBytes);
            var challengeData = JsonConvert.DeserializeObject<ChallengeData>(response);

            var challenge = challengeData.data;
            CurrentChallenge = challengeData.data;

            if (challenge != null)
            {
                if (challenge.matched_user_1._id == UserManager.Get.getCurrentUserId())
                {
                    UserManager.Get.UpdateUserMoneyCredit(challenge.matched_user_1.money_credit.ToString());
                    UserManager.Get.UpdateUserBubblesCredit(challenge.matched_user_1.bubble_credit.ToString());
                }
                else if (!string.IsNullOrEmpty(challenge.matched_user_2._id))
                {
                    if (challenge.matched_user_2._id == UserManager.Get.getCurrentUserId())
                    {
                        UserManager.Get.UpdateUserMoneyCredit(challenge.matched_user_2.money_credit.ToString());
                        UserManager.Get.UpdateUserBubblesCredit(challenge.matched_user_2.bubble_credit.ToString());
                    }
                }

                if (ReplayChallengePresenter.IsReplayChallenge())
                {
                    ReplayChallengePresenter.ReplayCompleted();
                    GamesManager.GAME_LEVEL = ReplayChallengePresenter.GetOldGameLevel();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<Challenge> AddChallenge(string challenge_type, string gain, string gain_type, int level, string token = null)
        {
            string url = Endpoint.classesURL + "/challenges";
            WWWForm form = new WWWForm();
            form.AddField("game", GamesManager.GAME_ID);
            form.AddField("gain", gain);
            form.AddField("gain_type", gain_type);
            form.AddField("challenge_type", challenge_type);
            form.AddField("level", level);
            form.AddField("game_level", GamesManager.GAME_LEVEL.ToString());
            var response = await SeembaWebRequest.Get.HttpsPost(url, form);
            ChallengeData challengeData = JsonConvert.DeserializeObject<ChallengeData>(response);
            if (challengeData.success)
            {
                CurrentChallengeId = challengeData.data._id;
                CurrentChallengeStatus = challengeData.data.status;
                return challengeData.data;
            }
            else
            {
                return null;
            }
        }
        public async Task<ArrayList> getUserOngoingChallenges(string token)
        {
            ArrayList challenges = new ArrayList();
            challenges = await getSeeResultsChallenges(token);
            challenges.AddRange(await getPendingChallenges(token));
            return challenges;
        }
        public async Task<GenericChallenge> UpdateChallengeStatusToFinishedAsync(string token, string challengeId)
        {
            string url = Endpoint.classesURL + "/challenges";
            string json = "challenge_id=" + challengeId + "&is_finished=" + true;
            byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);
            var responseText = await SeembaWebRequest.Get.HttpsPut(url, jsonAsBytes);

            SeembaResponse<GenericChallenge> response = JsonConvert.DeserializeObject<SeembaResponse<GenericChallenge>>(responseText);

            float? user_1_score, user_2_score;
            int? game_level;

            try
            {
                user_2_score = response.data.user_2_score;
            }
            catch (NullReferenceException)
            {
                user_2_score = null;
            }

            try
            {
                user_1_score = response.data.user_1_score;
            }
            catch (NullReferenceException)
            {
                user_1_score = null;
            }

            try
            {
                game_level = response.data.game_level;
            }
            catch (NullReferenceException)
            {
                game_level = null;
            }
            Game game =  new Game(response.data.game._id, response.data.game.name); 
            var created_at = string.IsNullOrEmpty(response.data.createdAt) ? response.data.createdAt : response.data.createdAt;
            return response.data;
        }
        public async Task<ArrayList> getChallengesUserResults(string token)
        {
            ArrayList challenges = new ArrayList();
            challenges = await getSeeResultsChallenges(token);
            challenges.AddRange(await getPendingChallenges(token));
            challenges.AddRange(await getFinishedChallenges(token));
            return challenges;
        }
    }
}
