using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
public class LoaderTranslationController : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
{
    [Header("Loader")]
    [SerializeField]
    private Text downloading;
    [SerializeField]
    private Text reconnect,check_connection,setting_language;
    void Start()
    {
        TranslationManager.scene = "Loader";
        /*downloading.text = TranslationManager.Get("downloading") != string.Empty ? TranslationManager.Get("downloading") : downloading.text;
        reconnect.text = TranslationManager.Get("reconnect") != string.Empty ? TranslationManager.Get("reconnect") : reconnect.text;
        check_connection.text = TranslationManager.Get("check_connection") != string.Empty ? TranslationManager.Get("check_connection") : check_connection.text;
        setting_language.text = TranslationManager.Get("setting_language") != string.Empty ? TranslationManager.Get("setting_language") : setting_language.text;*/
    }
}
