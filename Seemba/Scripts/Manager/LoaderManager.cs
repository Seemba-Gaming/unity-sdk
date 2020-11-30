using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SeembaSDK
{
    [CLSCompliant(false)]
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
            TranslationManager.scene = "Loader";
            SETTING_LANGUAGE = TranslationManager.Get("setting_language");
            CHECKING_CONNECTION = TranslationManager.Get("checking_connection");
            RECONNECTING = TranslationManager.Get("reconnecting");
            DONWLOADING = TranslationManager.Get("downloading");
            LOADING = TranslationManager.Get("loading");
            FIRST_CHALLENGE = TranslationManager.Get("first_challenge_load");
            SAVING = TranslationManager.Get("saving");
            ACCOUNT_CREATING = TranslationManager.Get("account_creating");
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
