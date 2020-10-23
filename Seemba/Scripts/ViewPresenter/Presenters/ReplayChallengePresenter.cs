using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ReplayChallengePresenter : MonoBehaviour
{
    public static Challenge ChallengeToReplay;
    public Text GameId, Date, Prize, sorryText;
    public Button Replay;

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
        EventsController.ChallengeType = ChallengeManager.CHALLENGE_TYPE_1V1;
        LoaderManager.Get.LoaderController.ShowLoader();
        BackgroundController.CurrentBackground = null;

        isReplay = true;
        old_game_level = GamesManager.GAME_LEVEL;
        GamesManager.GAME_LEVEL = ChallengeToReplay.game_level;

        SceneManager.LoadSceneAsync(GamesManager.GAME_SCENE_NAME, LoadSceneMode.Additive);
        LoaderManager.Get.LoaderController.HideLoader();
    }

    private void Init()
    {
        GameId.text = ChallengeToReplay._id;
        Date.text = DateTime.Parse(ChallengeToReplay.CreatedAt).ToString("MM/dd/yyyy hh:mm:ss"); ;
        if (ChallengeToReplay.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
        {
            Prize.text = float.Parse(ChallengeToReplay.gain).ToString("N2") + " " + CurrencyManager.CURRENT_CURRENCY;
        }
        else if (ChallengeToReplay.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES)
        {
            Prize.text = ChallengeToReplay.gain + " Bubbles";
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
