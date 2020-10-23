using System;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Seemba : MonoBehaviour
{

    #region Static
    public static Seemba Get => sInstance;
    private static Seemba sInstance;
    public static bool gameOver = false;
    public static bool isFromSeembaStore = false;
    #endregion

    #region Script Parameters
    public bool IsSeemba = false;
    #endregion

    #region Fields
    private GameObject mSeembaManagers;
    #endregion
    private void Awake()
    {
        Application.targetFrameRate = 60;
        sInstance = this;
    }
    private void Start()
    {
        if (UserManager.Get == null)
        {
            mSeembaManagers = Instantiate(Resources.Load("SeembaManagers") as GameObject);
            DontDestroyOnLoad(mSeembaManagers);
        }

        if (EventsController.Get != null)
        {
            IsSeemba = true;
        }
    }
    public void Enter()
    {
#if (UNITY_ANDROID)
        if (!isFromSeembaStore)
        {
            OpenSeemba();
        }
        else
        {
            PopupManager.Get.PopupController.ShowPopup(PopupType.DOWNLOAD_FROM_STORE, PopupsText.Get.download_from_store());
        }
#else
   
        OpenSeemba();
        
#endif

    }
    public IEnumerator checkInternetConnection(Action<bool?> action)
    {

        WWW www = new WWW("https://www.google.fr");
        float timer = 0;
        bool failed = false;
        while (!www.isDone)
        {
            if (timer > 5) { failed = true; break; }
            timer += Time.deltaTime;
            yield return null;
        }


        if (failed)
        {
            www.Dispose();
            action(false);
        }
        else
        {
            if (www.error == null)
            {
                action(true);
            }
            else
            {
                action(false);
            }
        }
    }
    private void LoadSeembaConfig()
    {
        try
        {
            Game Game = JsonUtility.FromJson<Game>(Resources.Load<TextAsset>("seemba-services").ToString());
            GamesManager.GAME_ID = Game._id;
            GamesManager.GAME_NAME = Game.name;
            GamesManager.GAME_SCENE_NAME = Game.game_scene_name;
            DownloadAssets();
        }
        catch (Exception)
        {
            Debug.LogError("Please complete the game integration before starting");
            Debug.Break();
        }
    }
    private async void DownloadAssets()
    {
        var res = await GamesManager.Get.getGamebyId(GamesManager.GAME_ID);
        if (res != null)
        {
            LoaderManager.Get.LoaderController.ShowLoader(LoaderManager.DONWLOADING);
            await BackgroundController.SaveBackgroundImage(GamesManager.BACKGROUND_IMAGE_URL);
            await GamesManager.SaveIcon(GamesManager.ICON_URL);
            LoaderManager.Get.LoaderController.ShowLoader(LoaderManager.SETTING_LANGUAGE);
            await TranslationManager.SavePreferedLanguage();
            LoaderManager.Get.LoaderController.HideLoader();
        }
        else
        {
            Debug.LogError("Please verify your game ID");
            Debug.Break();
            LoaderManager.Get.LoaderController.HideLoader();
        }
    }
    private void InstantiateSentry()
    {
        SentryController.Instance.instantiate();
    }
    private async void OpenSeemba()
    {
        ConnectivityController.CURRENT_ACTION = ConnectivityController.ENTER_ESPORT_TOURNAMENT_ACTION;
        LoaderManager.Get.LoaderController.ShowLoader(LoaderManager.CHECKING_CONNECTION);
        StartCoroutine(checkInternetConnection(async (isConnected) =>
        {
            if (isConnected == true)
            {
                LoaderManager.Get.LoaderController.ShowLoader(null);
                LoadSeembaConfig();

                if (string.IsNullOrEmpty(GamesManager.GAME_ID))
                {
                    Debug.LogError("Please Insert game id in GamesManager Class(gameId)");
                }
                else
                {
                    IsSeemba = true;
                   // InstantiateSentry();
                    DontDestroyOnLoad(PopupManager.Get.PopupController.gameObject);
                    DontDestroyOnLoad(LoaderManager.Get.LoaderController.gameObject);
                    SceneManager.LoadSceneAsync("SeembaEsports");
                }
            }
            else
            {
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());
                LoaderManager.Get.LoaderController.HideLoader();
            }
        }));
    }
    public void Quit()
    {

        //IsSeemba = false;
        //BackgroundController.CurrentBackground = null;
        //EventsController.ChallengeType = null;
        //Destroy(gameObject);
        //SceneManager.LoadScene(0);
    }
    public async void setResult(float score)
    {
        if (EventsController.ChallengeType == ChallengeManager.CHALLENGE_TYPE_1V1)
        {
            LoaderManager.Get.LoaderController.ShowLoader(null);
            var resAddScore = await ChallengeManager.Get.addScore(ChallengeManager.CurrentChallengeId, score);
            ChallengeManager.Get.ShowResult();
            LoaderManager.Get.LoaderController.HideLoader();
        }
        else if (EventsController.ChallengeType == ChallengeManager.CHALLENGE_TYPE_BRACKET)
        {
            LoaderManager.Get.LoaderController.ShowLoader(null);
            await TournamentManager.Get.addScore(TournamentController.getCurrentTournamentID(), score);
            //StartCoroutine(TournamentManager.Get.AddScoreIEnum(TournamentController.getCurrentTournamentID(), score));
            ViewsEvents.Get.GoToMenu(ViewsEvents.Get.Brackets.gameObject);
            ViewsEvents.Get.Brackets.OnEnable();
        }
        SceneManager.UnloadSceneAsync(GamesManager.GAME_SCENE_NAME);
        LoaderManager.Get.LoaderController.HideLoader();
    }
    public void setGameOver(bool GameOver)
    {
        gameOver = GameOver;
    }
    public bool isGameOver()
    {
        return gameOver;
    }
    public void On_ApplicationQuit()
    {
        if (!isGameOver())
        {
        }
    }
}
