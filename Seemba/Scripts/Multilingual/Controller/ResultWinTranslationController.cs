using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
public class ResultWinTranslationController : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
{
    [SerializeField]
    private Text you_win,
      victory,
      score1, score2,
      congratulation,
      date,
      ID,
      _continue;
    // Start is called before the first frame update
    void Start()
    {
        TranslationManager.scene = "ResultWin";
        you_win.text = TranslationManager.Get("you_win") != string.Empty ? TranslationManager.Get("you_win") : you_win.text;
        score1.text = TranslationManager.Get("score") != string.Empty ? TranslationManager.Get("score") : score1.text;
        score2.text = TranslationManager.Get("score") != string.Empty ? TranslationManager.Get("score") : score2.text;
        victory.text = TranslationManager.Get("victory") != string.Empty ? TranslationManager.Get("victory") : victory.text;
        congratulation.text = TranslationManager.Get("congratulation") != string.Empty ? TranslationManager.Get("congratulation") : congratulation.text;
        date.text = TranslationManager.Get("date") != string.Empty ? TranslationManager.Get("date") : date.text;
        ID.text = TranslationManager.Get("ID") != string.Empty ? TranslationManager.Get("ID") : ID.text;
        _continue.text = TranslationManager.Get("continue") != string.Empty ? TranslationManager.Get("continue") : _continue.text;
    }
}
