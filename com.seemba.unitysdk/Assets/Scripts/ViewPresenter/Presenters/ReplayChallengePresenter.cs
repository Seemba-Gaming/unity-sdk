using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace SeembaSDK
{
    public class ReplayChallengePresenter : MonoBehaviour
    {
        public static GenericChallenge ChallengeToReplay;
        public Text GameId, Date, sorryText;
        public Button Replay;
        public TextMeshProUGUI Prize;

        private static bool isReplay;
        private static int? old_game_level;

        // Use this for initialization
        private void Start()
        {
            Init();
            Replay.onClick.AddListener(() =>
            {
                ReplayMissedChallenge();
            });
        }

        private void ReplayMissedChallenge()
        {
            ChallengeManager.CurrentChallengeId = ChallengeToReplay._id;
            if(string.IsNullOrEmpty(ChallengeToReplay.tournament_id))
            {
                EventsController.ChallengeType = ChallengeManager.CHALLENGE_TYPE_1V1;
            }
            else
            {
                EventsController.ChallengeType = ChallengeManager.CHALLENGE_TYPE_BRACKET;
                TournamentController.setCurrentTournamentID(ChallengeToReplay.tournament_id);
            }
            LoaderManager.Get.LoaderController.ShowLoader();
            BackgroundController.CurrentBackground = null;

            isReplay = true;
            old_game_level = GamesManager.GAME_LEVEL;
            GamesManager.GAME_LEVEL = ChallengeToReplay.game_level;
            ViewsEvents.Get.GetCurrentMenu().SetActive(false);
            SceneManager.LoadSceneAsync(GamesManager.GAME_SCENE_NAME, LoadSceneMode.Additive);
            LoaderManager.Get.LoaderController.HideLoader();
        }

        private void Init()
        {
            GameId.text = ChallengeToReplay._id;
            Date.text = DateTime.Parse(ChallengeToReplay.createdAt).ToString("MM/dd/yyyy hh:mm:ss");
            if (ChallengeToReplay.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
            {
                Prize.text = (float.Parse(ChallengeToReplay.gain) * 100) + " <sprite=1>";
            }
            else if (ChallengeToReplay.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES)
            {
                Prize.text = ChallengeToReplay.gain + " <sprite=0>";
            }
        }

        public static bool IsReplayChallenge()
        {
            return isReplay;
        }
        public static void ReplayCompleted()
        {
            isReplay = false;
        }
        public static int? GetOldGameLevel()
        {
            return old_game_level;
        }
    }
}