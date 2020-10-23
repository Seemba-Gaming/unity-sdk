using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        TranslationManager.scene = "ProfilePresenter";
        games_won.text = TranslationManager.Get("games_won") != string.Empty ? TranslationManager.Get("games_won") : games_won.text;
        in_a_row.text = TranslationManager.Get("in_a_row") != string.Empty ? TranslationManager.Get("in_a_row") : in_a_row.text;
        favourite.text = TranslationManager.Get("favourite") != string.Empty ? TranslationManager.Get("favourite") : favourite.text;
        verified.text = TranslationManager.Get("verified") != string.Empty ? TranslationManager.Get("verified") : verified.text;
        pending.text = TranslationManager.Get("pending") != string.Empty ? TranslationManager.Get("pending") : pending.text;
        unverified.text = TranslationManager.Get("unverified") != string.Empty ? TranslationManager.Get("unverified") : unverified.text;
        history.text = TranslationManager.Get("history") != string.Empty ? TranslationManager.Get("history") : history.text;
    }
}
