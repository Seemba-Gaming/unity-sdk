using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : MonoBehaviour
{
    #region Static
    public const string SETTING_LANGUAGE = "Setting up your language...";
    public const string CHECKING_CONNECTION = "Checking your internet connection...";
    public const string RECONNECTING = "Trying to reconnect...";
    public const string DONWLOADING = "Downloading assets...";
    public static LoaderManager Get { get { return sInstance; } }
    private static LoaderManager sInstance;
    #endregion

    #region Script Parameters
    public GameObject                           LoaderPrefab;
    public LoaderController                     LoaderController;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        sInstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoaderController = (Instantiate(LoaderPrefab) as GameObject).GetComponent<LoaderController>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Implementation
    private void OnSceneLoaded(Scene sceneName, LoadSceneMode mode)
    {
        if (sceneName.name.Equals("SeembaEsports"))
        {
            LoaderController.HideLoader();
        }
        SceneManager.SetActiveScene(sceneName);
    }
    #endregion
}
