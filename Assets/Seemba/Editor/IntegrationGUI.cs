﻿using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class IntegrationGUI : EditorWindow
{
    private const string CONFIG_FILE_NAME = "seemba-services";
    private const string FIILE_PATH = "Assets/Seemba/Resources/"+ CONFIG_FILE_NAME +".json";
     
    private Texture2D m_Logo = null;
    
    string GAME_ID = "";
    string GAME_NAME = "";
    string GAME_SCENE_NAME = "";
    string GAME_LEVEL = "";


    #region UNITY_METHOD
    private void OnGUI()
    {
        Debug.Log(Application.streamingAssetsPath);
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
            if (!string.IsNullOrEmpty(SavedGame.game_level))
            {
                GAME_LEVEL = SavedGame.game_level;
            }
        }
        catch (Exception ex) { }
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
        GUILayout.Label("");
        GUILayout.Label("Please fill in all information below and click on apply : ", EditorStyles.boldLabel);
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
        if (!string.IsNullOrEmpty(game._id) || !string.IsNullOrEmpty(game.name) || !string.IsNullOrEmpty(game.game_scene_name))
        {
            if (!string.IsNullOrEmpty(game.game_level) && int.TryParse(game.game_level,out int level))
            {
                string str = JsonUtility.ToJson(game);
                using (FileStream fs = new FileStream(FIILE_PATH, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(str);
                    }
                }
                UnityEditor.AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("GAME_LEVEL should be a decimal number");
            }
        }
        else
        {
            Debug.LogError("Cannot save empty content");
        }
    }
    Game GetSavedGame()
    {
        var jsonTextFile = Resources.Load<TextAsset>(CONFIG_FILE_NAME);
        Game SavedGame = JsonUtility.FromJson<Game>(jsonTextFile.ToString());
        return SavedGame;
    }
    #endregion
}

