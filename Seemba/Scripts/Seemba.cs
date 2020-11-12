using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
public class Seemba : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
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
        UnityWebRequest www = new UnityWebRequest("https://www.google.fr");
        yield return www.SendWebRequest();
        if (www.error == null)
        {
            action(true);
        }
        else
        {
            action(false);
        }
    }
    private async Task<bool> LoadSeembaConfig()
    {
        try
        {
            Game Game = JsonUtility.FromJson<Game>(Resources.Load<TextAsset>("seemba-services").ToString());
            GamesManager.GAME_ID = Game._id;
            GamesManager.GAME_NAME = Game.name;
            GamesManager.GAME_SCENE_NAME = Game.game_scene_name;
        }
        catch (Exception)
        {
            Debug.LogError("Please complete the game integration before starting");
            Debug.Break();
        }
        var res = await DownloadAssets();
        if(res)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private async Task<bool> DownloadAssets()
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
            return true;
        }
        else
        {
            Debug.LogError("Please verify your game ID");
            Debug.Break();
            LoaderManager.Get.LoaderController.HideLoader();
            return false;
        }
    }
    private void InstantiateSentry()
    {
        SentryController.Instance.instantiate();
    }
    private void OpenSeemba()
    {
        ConnectivityController.CURRENT_ACTION = ConnectivityController.ENTER_ESPORT_TOURNAMENT_ACTION;
        LoaderManager.Get.LoaderController.ShowLoader(LoaderManager.CHECKING_CONNECTION);
        StartCoroutine(checkInternetConnection(async (isConnected) =>
        {
            if (isConnected == true)
            {
                LoaderManager.Get.LoaderController.ShowLoader(null);
                await LoadSeembaConfig();

                if (string.IsNullOrEmpty(GamesManager.GAME_ID))
                {
                    Debug.LogError("Please Insert game id in GamesManager Class(gameId)");
                }
                else
                {
                    IsSeemba = true;
                    InstantiateSentry();
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
