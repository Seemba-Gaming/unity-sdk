using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class Seemba : MonoBehaviour
{
    UserManager userManager = new UserManager();
    public static bool gameOver = false;
    //public GoogleAnalyticsV4 googleAnalytics;	
    public void setGameOver(bool GameOver)
    {
        gameOver = GameOver;
    }
    public bool isGameOver()
    {
        return gameOver;
    }
    private void Awake()
    {
        //Set The FPS target in the Awake function to avoid all changes from outsides.
        Application.targetFrameRate = 60;
    }
    public void Enter()
    {
        //PlayerPrefs.DeleteAll();

        ConnectivityController.CURRENT_ACTION = ConnectivityController.ENTER_ESPORT_TOURNAMENT_ACTION;
        GamesManager gamesManager = new GamesManager();
        EventsController nbs = new EventsController();
        UserManager um = new UserManager();
        StartCoroutine(nbs.checkInternetConnection((isConnected) =>
        {
                // handle connection status here
                if (isConnected == true)
            {
                SceneManager.UnloadSceneAsync("Loader");
                SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
                UnityThreadHelper.CreateThread(() =>
                {
                    if (string.IsNullOrEmpty(GamesManager.GAME_ID))
                    {
                        Debug.LogError("Please Insert game id in GamesManager Class(gameId)");
                    }
                    else
                    {
                        string res = gamesManager.getGamebyId(GamesManager.GAME_ID);
                        if (!string.IsNullOrEmpty(res))
                        {
                            UnityThreadHelper.Dispatcher.Dispatch(() =>
                            {
                                StartCoroutine(BackgroundController.SaveBackgroundImage(BackgroundController.backgroundURL));
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
                                    if (string.IsNullOrEmpty(GamesManager.GAME_ID))
                                    {
                                        Debug.LogError("Please Insert the correct game id in GamesManager Class(gameId)");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (um.getCurrentUserId() != null)
                                            {

                                                SceneManager.LoadSceneAsync("Home");
                                            }
                                            else
                                            {
                                                SceneManager.LoadSceneAsync("Intro");
                                            }
                                        }
                                        catch (NullReferenceException ex)
                                        {
                                                //Catch
                                            }
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
                            Debug.LogError("Please Insert game id in GamesManager Class(gameId)");
                            UnityThreadHelper.Dispatcher.Dispatch(() =>
                            {
                                SceneManager.UnloadSceneAsync("Loader");
                            });
                        }
                    }
                });
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
    public void Quit()
    {
        BackgroundController.CurrentBackground = null;
        EventsController.ChallengeType = null;
        SceneManager.LoadScene(0);
    }
    public void setResult(int score)
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
                        challengeManager.waitAdversaryFinishGame1();
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
    public void On_ApplicationQuit()
    {
        if (!isGameOver())
        {
        }
    }
}