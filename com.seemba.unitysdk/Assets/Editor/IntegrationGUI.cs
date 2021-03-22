#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Diagnostics;

namespace SeembaSDK
{
    [CLSCompliant(false)]
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
            catch (Exception) { }
        }
        #endregion
        #region METHOD

        [MenuItem("Seemba/Integration Parameters")]
        public static void ShowWindow()
        {
            GetWindow<IntegrationGUI>("Integration Parameters");
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
            catch (Exception) { }

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
        static void GetScenes(string path)
        {
            string absolute = Path.GetFullPath(path);
            string[] filePaths = Directory.GetFiles(@absolute, "*.unity").Select(Path.GetFileName)
                                .ToArray();

            foreach (var file in filePaths)
            {

                AddSceneToBuildSettings(path + file);
            }
        }
        static void AddSceneToBuildSettings(string pathOfSceneToAdd)
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

        [MenuItem("Seemba/Install Package")]
        public static void InstallPackge()
        {
            Game Game = JsonUtility.FromJson<Game>(Resources.Load<TextAsset>("seemba-services").ToString());
            GamesManager.GAME_ID = Game._id;
            GamesManager.GAME_NAME = Game.name;
            GamesManager.GAME_SCENE_NAME = Game.game_scene_name;
            if(string.IsNullOrEmpty(Game._id))
            {
                UnityEngine.Debug.LogError("Please Fill your game id in Seemba --> Integration Parameters");
            }
            string url = Endpoint.classesURL +"/games/" + GamesManager.GAME_ID;
            Request(url);
            EditorApplication.update += EditorUpdate;
        }
        #endregion
        #region Editor Web Request
        static UnityWebRequest www;

        static void Request(string url)
        {
            www = UnityWebRequest.Get(url);
            www.SendWebRequest();
        }

        static void EditorUpdate()
        {
            if (!www.isDone)
                return;

            if (www.isNetworkError)
            {
                UnityEngine.Debug.Log(www.error);
            }
 
            else
            {
                UnityEngine.Debug.Log(www.downloadHandler.text);
                SeembaResponse<GameInfo> response = JsonConvert.DeserializeObject<SeembaResponse<GameInfo>>(www.downloadHandler.text);
                GamesManager.GAME_ORIENTATION = response.data.game.orientation;
                UnityEngine.Debug.LogWarning(GamesManager.GAME_ORIENTATION);
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                if (GamesManager.GAME_ORIENTATION == "portrait")
                {
                    ExecuteProcessTerminal("openupm add com.seemba.unitysdk-vertical");
                }
                else
                {
                    ExecuteProcessTerminal("openupm add com.seemba.unitysdk-horizental");
                }
#else
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";
                if (GamesManager.GAME_ORIENTATION == "portrait")
                {
                    startInfo.Arguments = "/C openupm add com.seemba.unitysdk-vertical";
                    GetScenes("Packages/com.seemba.unitysdk-vertical/Seemba/Scenes");
                }
                else
                {
                    startInfo.Arguments = "/C openupm add com.seemba.unitysdk-horizental";
                    GetScenes("Packages/com.seemba.unitysdk-horizental/Seemba/Scenes");
                }
                process.StartInfo = startInfo;
                process.Start();
#endif

            }

            EditorApplication.update -= EditorUpdate;
        }

        private static void ExecuteProcessTerminal(string argument)
        {
                UnityEngine.Debug.Log("============== Start Executing [" + argument + "] ===============");
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = " -c \"" + argument + " \""
                };
                Process myProcess = new Process
                {
                    StartInfo = startInfo
                };
                myProcess.Start();
                string output = myProcess.StandardOutput.ReadToEnd();
                myProcess.WaitForExit();
                UnityEngine.Debug.Log("============== End ===============");
        }
        #endregion
    }
}
#endif