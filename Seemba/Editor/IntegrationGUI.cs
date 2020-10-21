//#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Diagnostics;
using System.Linq;

public class IntegrationGUI : EditorWindow
{
    private Texture2D m_Logo = null;

    string GAME_ID = "";
    string GAME_SCENE_NAME = "";
    string GAME_NAME = "";
    string GAME_LEVEL = "";

    #region UNITY_METHOD
    private void OnGUI()
    {
        SetLogo();
        SetFields();
        SetValidateButton();
    }
    void OnEnable()
    {
        m_Logo = (Texture2D)Resources.Load("Logo/seemba_logo", typeof(Texture2D));
    }
    public void Awake()
    {
        try
        {
            var SavedGame = GetSavedGame();
            if (!string.IsNullOrEmpty(SavedGame._id))
            {
                GAME_ID = SavedGame._id;
            }
            if (!string.IsNullOrEmpty(SavedGame.name))
            {
                GAME_NAME = SavedGame.name;
            }
            if (!string.IsNullOrEmpty(SavedGame.game_scene_name))
            {
                GAME_SCENE_NAME = SavedGame.game_scene_name;
            }
        }
        catch (Exception ex) { }
        GetScenes();
    }
    #endregion
    #region METHOD
    [MenuItem("Seemba/Integration Parameters")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<IntegrationGUI>("Integration Parameters");
    }
    void SetLogo()
    {
        /****************** SET SEEMBA LOGO *********************/
        GUILayout.BeginHorizontal("label");
        GUILayout.FlexibleSpace();
        GUILayout.Label(m_Logo);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        /********************************************************/
    }
    void SetFields()
    {
        /****************** SET SEEMBA FIELDS *********************/
        GUILayout.Label("Please Fill the Infos bellow to start using Seemba: ", EditorStyles.boldLabel);
        GUILayout.Label("");
        GAME_ID = EditorGUILayout.TextField("GAME_ID: ", GAME_ID, GUILayout.Width(800), GUILayout.Height(20));
        GAME_NAME = EditorGUILayout.TextField("GAME_NAME: ", GAME_NAME, GUILayout.Width(800), GUILayout.Height(20));
        GAME_SCENE_NAME = EditorGUILayout.TextField("GAME_SCENE_NAME: ", GAME_SCENE_NAME, GUILayout.Width(800), GUILayout.Height(20));
        GAME_LEVEL = EditorGUILayout.TextField("GAME_LEVEL: ", GAME_LEVEL, GUILayout.Width(800), GUILayout.Height(20));
        GUILayout.Label("");
        /**********************************************************/
    }
    void SetValidateButton()
    {
        /****************** APPLY BUTTON *********************/

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.white;

        if (GUILayout.Button("Apply", GUILayout.Width(400), GUILayout.Height(50)))
        {
            SaveConfig(new Game(GAME_ID, GAME_NAME, GAME_SCENE_NAME, GAME_LEVEL));
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
    public void SaveConfig(Game game)
    {
        try
        {
            var SavedGame = GetSavedGame();
            if (string.IsNullOrEmpty(game._id))
            {
                game._id = SavedGame._id;
            }
            if (string.IsNullOrEmpty(game.name))
            {
                game.name = SavedGame.name;
            }
            if (string.IsNullOrEmpty(game.game_scene_name))
            {
                game.game_scene_name = SavedGame.game_scene_name;
            }
        }
        catch (Exception ex) { }

        if (string.IsNullOrEmpty(game._id) && string.IsNullOrEmpty(game.name) && string.IsNullOrEmpty(game.game_scene_name))
        {
            UnityEngine.Debug.LogError("Cannot save empty content");
            return;

        }
        if (!string.IsNullOrEmpty(game.game_level) && !int.TryParse(game.game_level, out int val))
        {
            UnityEngine.Debug.LogError("GAME_LEVEL should be a decimal number");
            return;
        }
        if (!Directory.Exists(ConfigFileHelper.PATH))
        {
            Directory.CreateDirectory(ConfigFileHelper.PATH);
        }
        string str = JsonUtility.ToJson(game);
        using (FileStream fs = new FileStream(ConfigFileHelper.FILE_PATH, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(str);
            }
        }
        UnityEditor.AssetDatabase.Refresh();
    }
    Game GetSavedGame()
    {
        if (File.Exists(ConfigFileHelper.FILE_PATH)) { UnityEngine.Debug.Log("Exist"); }
        TextAsset mConfigFile = (TextAsset)AssetDatabase.LoadAssetAtPath(ConfigFileHelper.RELATIVE_FILE_PATH, typeof(TextAsset));
        Game SavedGame = JsonUtility.FromJson<Game>(mConfigFile.ToString());
        return SavedGame;
    }
    void GetScenes()
    {
        string absolute = Path.GetFullPath("Packages/com.seemba.unitysdk/Seemba/Scenes");
        string[] filePaths = Directory.GetFiles(@absolute, "*.unity").Select(Path.GetFileName)
                            .ToArray();

        foreach (var file in filePaths)
        {

            AddSceneToBuildSettings("Packages/com.seemba.unitysdk/Seemba/Scenes/" + file);
        }
    }
    void AddSceneToBuildSettings(string pathOfSceneToAdd)
    {
        //Loop through and see if the scene already exist in the build settings
        int indexOfSceneIfExist = -1;

        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            if (EditorBuildSettings.scenes[i].path == pathOfSceneToAdd)
            {
                indexOfSceneIfExist = i;
                break;
            }
        }

        EditorBuildSettingsScene[] newScenes;

        if (indexOfSceneIfExist == -1)
        {
            newScenes = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length + 1];

            //Seems inefficent to add scene to build settings after creating each scene (rather than doing it all at once
            //after they are all created, however, it's necessary to avoid memory issues.
            int i = 0;
            for (; i < EditorBuildSettings.scenes.Length; i++)
                newScenes[i] = EditorBuildSettings.scenes[i];

            newScenes[i] = new EditorBuildSettingsScene(pathOfSceneToAdd, true);
        }
        else
        {
            newScenes = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length];

            int i = 0, j = 0;
            for (; i < EditorBuildSettings.scenes.Length; i++)
            {
                //skip over the scene that is a duplicate
                //this will effectively delete it from the build settings
                if (i != indexOfSceneIfExist)
                    newScenes[j++] = EditorBuildSettings.scenes[i];
            }
            newScenes[j] = new EditorBuildSettingsScene(pathOfSceneToAdd, true);
        }
        EditorBuildSettings.scenes = newScenes;
    }
    #endregion
}
//#endif
