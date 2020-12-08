using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class PopupManager : MonoBehaviour
    {
        #region Static
        public static PopupManager Get { get { return sInstance; } }
        private static PopupManager sInstance;
        #endregion

        #region Script Parameters
        public GameObject PopupPrefab;
        public PopupsController PopupController;
        public PopupsViewPresenter PopupViewPresenter;
        public PopupsTranslationController PopupsTranslationController;
        #endregion

        #region Fields
        private GameObject mPopupPrefab;
        #endregion
        #region Unity Methods
        private void Awake()
        {
            sInstance = this;
        }
        void Start()
        {
            InitPopup();
        }
        #endregion

        #region Methods
        public void InitPopup()
        {
            if(mPopupPrefab != null)
            {
                Destroy(mPopupPrefab);
            }
            mPopupPrefab = Instantiate(PopupPrefab);
            PopupController = mPopupPrefab.GetComponent<PopupsController>();
            PopupViewPresenter = mPopupPrefab.GetComponent<PopupsViewPresenter>();
            PopupsTranslationController = mPopupPrefab.GetComponent<PopupsTranslationController>();
        }
        #endregion

        #region Implementation
        #endregion
    }
}
