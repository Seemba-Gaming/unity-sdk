using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SeembaSDK
{
    public class LoaderManager : MonoBehaviour
    {
        #region Static
        public static string SETTING_LANGUAGE;
        public static string CHECKING_CONNECTION;
        public static string RECONNECTING;
        public static string DONWLOADING;
        public static string ACCOUNT_CREATING;
        public static string SAVING;
        public static string FIRST_CHALLENGE;
        public static string LOADING;
        public static LoaderManager Get { get { return sInstance; } }
        private static LoaderManager sInstance;
        #endregion

        #region Script Parameters
        public GameObject LoaderPrefab;
        public LoaderController LoaderController;
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
            SetLoadingTexts();
            LoaderController = (Instantiate(LoaderPrefab) as GameObject).GetComponent<LoaderController>();
        }


        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Methods
        void SetLoadingTexts()
        {
            TranslationManager._instance.scene = "Loader";
            SETTING_LANGUAGE = TranslationManager._instance.Get("setting_language");
            CHECKING_CONNECTION = TranslationManager._instance.Get("checking_connection");
            RECONNECTING = TranslationManager._instance.Get("reconnecting");
            DONWLOADING = TranslationManager._instance.Get("downloading");
            LOADING = TranslationManager._instance.Get("loading");
            FIRST_CHALLENGE = TranslationManager._instance.Get("first_challenge_load");
            SAVING = TranslationManager._instance.Get("saving");
            ACCOUNT_CREATING = TranslationManager._instance.Get("account_creating");
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
}
