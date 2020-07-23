using UnityEngine;
using UnityEngine.UI;
public class ResultEqualityTranslationController : MonoBehaviour
{
    [SerializeField]
    private Text equality,
      try_again,
      score1, score2,
      you_will_win,
      never_give_up,
      date,
      ID,
      play_again,
      _continue;
    // Start is called before the first frame update
    void Start()
    {
        TranslationManager.scene = "ResultEquality";
        equality.text = TranslationManager.Get("equality") != string.Empty ? TranslationManager.Get("equality") : equality.text;
        try_again.text = TranslationManager.Get("try_again") != string.Empty ? TranslationManager.Get("try_again") : try_again.text;
        score1.text = TranslationManager.Get("score") != string.Empty ? TranslationManager.Get("score") : score1.text;
        score2.text = TranslationManager.Get("score") != string.Empty ? TranslationManager.Get("score") : score2.text;
        you_will_win.text = TranslationManager.Get("you_will_win") != string.Empty ? TranslationManager.Get("you_will_win") : you_will_win.text;
        never_give_up.text = TranslationManager.Get("never_give_up") != string.Empty ? TranslationManager.Get("never_give_up") : never_give_up.text;
        date.text = TranslationManager.Get("date") != string.Empty ? TranslationManager.Get("date") : date.text;
        ID.text = TranslationManager.Get("ID") != string.Empty ? TranslationManager.Get("ID") : ID.text;
        play_again.text = TranslationManager.Get("play_again") != string.Empty ? TranslationManager.Get("play_again") : play_again.text;
        _continue.text = TranslationManager.Get("continue") != string.Empty ? TranslationManager.Get("continue") : _continue.text;
    }
}
