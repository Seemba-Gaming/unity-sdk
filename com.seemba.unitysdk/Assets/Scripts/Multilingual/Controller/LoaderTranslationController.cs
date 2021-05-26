using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class LoaderTranslationController : MonoBehaviour
    {
        [Header("Loader")]
        [SerializeField]
        private Text downloading;
        [SerializeField]
        private Text reconnect, check_connection, setting_language;
        void Start()
        {
            TranslationManager._instance.scene = "Loader";
        }
    }
}
