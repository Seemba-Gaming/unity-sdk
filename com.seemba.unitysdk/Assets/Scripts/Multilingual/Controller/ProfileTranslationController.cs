using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
#pragma warning disable 0649
    public class ProfileTranslationController : MonoBehaviour
    {
        [SerializeField]
        private Text favourite,
                     verified,
                     pending,
                     unverified,
                     history,
                     games_won,
                     in_a_row;
        // Start is called before the first frame update
        void Start()
        {
            TranslationManager._instance.scene = "Profile";
            games_won.text = TranslationManager._instance.Get("games_won") != string.Empty ? TranslationManager._instance.Get("games_won") : games_won.text;
            in_a_row.text = TranslationManager._instance.Get("in_a_row") != string.Empty ? TranslationManager._instance.Get("in_a_row") : in_a_row.text;
            favourite.text = TranslationManager._instance.Get("favourite") != string.Empty ? TranslationManager._instance.Get("favourite") : favourite.text;
            verified.text = TranslationManager._instance.Get("verified") != string.Empty ? TranslationManager._instance.Get("verified") : verified.text;
            pending.text = TranslationManager._instance.Get("pending") != string.Empty ? TranslationManager._instance.Get("pending") : pending.text;
            unverified.text = TranslationManager._instance.Get("unverified") != string.Empty ? TranslationManager._instance.Get("unverified") : unverified.text;
            history.text = TranslationManager._instance.Get("history") != string.Empty ? TranslationManager._instance.Get("history") : history.text;
        }
    }
}
