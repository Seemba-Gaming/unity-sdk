using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    #pragma warning disable 649
    public class HistoricTranslationController : MonoBehaviour
    {
        [SerializeField]
        private Text games_won,
                     in_a_row, BackButton;
        // Start is called before the first frame update
        void Start()
        {
            TranslationManager._instance.scene = "Home";
            BackButton.text = TranslationManager._instance.Get("back_button");
            TranslationManager._instance.scene = "History";
            games_won.text = TranslationManager._instance.Get("games_won") != string.Empty ? TranslationManager._instance.Get("games_won") : games_won.text;
            in_a_row.text = TranslationManager._instance.Get("in_a_row") != string.Empty ? TranslationManager._instance.Get("in_a_row") : in_a_row.text;
        }
    }
}
