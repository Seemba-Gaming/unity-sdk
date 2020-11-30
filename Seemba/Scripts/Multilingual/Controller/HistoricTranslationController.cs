using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
#pragma warning disable 0649
    public class HistoricTranslationController : MonoBehaviour
    {
        [SerializeField]
        private Text games_won,
                     in_a_row, BackButton;
        // Start is called before the first frame update
        void Start()
        {
            TranslationManager.scene = "Home";
            BackButton.text = TranslationManager.Get("back_button");
            TranslationManager.scene = "History";
            games_won.text = TranslationManager.Get("games_won") != string.Empty ? TranslationManager.Get("games_won") : games_won.text;
            in_a_row.text = TranslationManager.Get("in_a_row") != string.Empty ? TranslationManager.Get("in_a_row") : in_a_row.text;
        }
    }
}
