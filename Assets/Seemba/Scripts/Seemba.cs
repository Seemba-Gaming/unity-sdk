using UnityEngine;
using System.Collections;
using System;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System.IO;
using static PopupsViewPresenter;

public class Seemba : MonoBehaviour
{
    UserManager userManager = new UserManager();

    public static bool isFromSeembaStore = false;
   
    private void Awake()
    {
        //Set The FPS target in the Awake function to avoid all changes from outsides.
        Application.targetFrameRate = 60;
    }
    public void Enter()
    {
        //PlayerPrefs.DeleteAll();
#if (UNITY_ANDROID)
        if (!isFromSeembaStore) OpenSeemba();
        else
        {
            PopupsController.getInstance().ShowPopup(PopupsController.PopupType.DOWNLOAD_FROM_STORE, PopupsText.getInstance().download_from_store());
        }
#else
        OpenSeemba();   
#endif

    }

    #region OPEN_SEEMBA
    private void OpenSeemba()
    {
        CheckConnection();
    }
    private void CheckConnection()
    {
        EventsController nbs = new EventsController();
        ConnectivityController.CURRENT_ACTION = ConnectivityController.ENTER_ESPORT_TOURNAMENT_ACTION;

        StartCoroutine(nbs.checkInternetConnection((isConnected) =>
        {
            // handle connection status here
            if (isConnected == true)
            {

                SceneManager.UnloadSceneAsync("Loader");
                SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
                LoadSeembaConfig();
            }
            else
            {
                try
                {
                    SceneManager.UnloadSceneAsync("ConnectionFailed");
                }
                catch (ArgumentException ex) { }
                SceneManager.LoadScene("ConnectionFailed", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("Loader");
            }
        }));
    }
    private void LoadSeembaConfig()
    {
        try
        {
            var jsonTextFile = Resources.Load<TextAsset>("seemba-services");
            Game Game = JsonUtility.FromJson<Game>(jsonTextFile.ToString());
            GamesManager.GAME_ID = Game._id;
            GamesManager.GAME_NAME = Game.name;
            GamesManager.GAME_SCENE_NAME = Game.game_scene_name;
            DownloadAssets();
        }
        catch (Exception ex)
        {
            Debug.LogError("Please complete the game integration before starting");
            Debug.Break();
        }
    }
    private void DownloadAssets()
    {
        GamesManager gamesManager = new GamesManager();

        UnityThreadHelper.CreateThread(() =>
        {
            string res = gamesManager.getGamebyId(GamesManager.GAME_ID);
            if (!string.IsNullOrEmpty(res))
            {
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    StartCoroutine(BackgroundController.SaveBackgroundImage(GamesManager.BACKGROUND_IMAGE_URL));
                    StartCoroutine(GamesManager.SaveIcon(GamesManager.ICON_URL));
                    StartCoroutine(TranslationManager.SavePreferedLaguage());
                });
                while (GamesManager.backgroundSaved == null || TranslationManager.isDownloaded == null)
                {
                }
                if (GamesManager.backgroundSaved == true && TranslationManager.isDownloaded == true)
                {
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        //instantiate sentry for catching crashes...
                        InstantiateSentry();
                        if (userManager.getCurrentUserId() != null)
                        {
                            SceneManager.LoadSceneAsync("Home");
                        }
                        else
                        {
                            SceneManager.LoadSceneAsync("Intro");
                        }
                    });
                }
                else
                {
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        SceneManager.UnloadSceneAsync("Loader");
                        SceneManager.LoadScene("ConnectionFailed", LoadSceneMode.Additive);
                    });
                }
            }
            else
            {
                Debug.LogError("Please verify your game ID");
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        Debug.Break();
                        SceneManager.UnloadSceneAsync("Loader");
                    });
            }

        });
    }
    private void InstantiateSentry()
    {
        SentryController.Instance.instantiate();
    }
    #endregion
    public void Quit()
    {
        BackgroundController.CurrentBackground = null;
        EventsController.ChallengeType = null;
        SceneManager.LoadScene(0);
    }
    public void SetResult(int score)
    {
        //1vs1
        if (EventsController.ChallengeType == ChallengeManager.CHALLENGE_TYPE_1V1)
        {
            ChallengeManager challengeManager = new ChallengeManager();
            try
            {
                UserManager userManager = new UserManager();
                string userId = userManager.getCurrentUserId();
                string token = userManager.getCurrentSessionToken();
                SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
                UnityThreadHelper.CreateThread(() =>
                {
                    challengeManager.addScore(userId, token, ChallengeManager.CurrentChallengeId, float.Parse(score.ToString()));
                    UnityThreadHelper.Dispatcher.Dispatch(() =>
                    {
                        SceneManager.UnloadScene("Loader");
                        challengeManager.ShowResults();
                    });
                });
            }
            catch (FormatException ex)
            {
                //Catch
            }
        }
        else if (EventsController.ChallengeType == ChallengeManager.CHALLENGE_TYPE_BRACKET)
        {
            string userId = userManager.getCurrentUserId();
            string token = userManager.getCurrentSessionToken();
            TournamentManager tournamentManager = new TournamentManager();
            SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
            UnityThreadHelper.CreateThread(() =>
            {
                string TournamentId = tournamentManager.addScoreInTournament(TournamentController.getCurrentTournamentID(), score, userId, token);
                UnityThreadHelper.Dispatcher.Dispatch(() =>
                {
                    SceneManager.UnloadScene("Loader");
                    SceneManager.LoadScene("Bracket");
                });
            });
        }

    }
    
}
