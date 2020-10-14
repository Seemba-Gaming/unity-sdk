using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultLoseTranslationController : MonoBehaviour
{
    [SerializeField]
    private Text you_lose,
      defeat,
      score1, score2,
      your_opponent_won,
      dont_give_up,
      date,
      ID,
      play_again,
      _continue;
    // Start is called before the first frame update
    void Start()
    {
        TranslationManager.scene = "ResultLose";
        you_lose.text = TranslationManager.Get("you_lose") != string.Empty ? TranslationManager.Get("you_lose") : you_lose.text;
        defeat.text = TranslationManager.Get("defeat") != string.Empty ? TranslationManager.Get("defeat") : defeat.text;
        score1.text = TranslationManager.Get("score") != string.Empty ? TranslationManager.Get("score") : score1.text;
        score2.text = TranslationManager.Get("score") != string.Empty ? TranslationManager.Get("score") : score2.text;
        your_opponent_won.text = TranslationManager.Get("your_opponent_won") != string.Empty ? TranslationManager.Get("your_opponent_won") : your_opponent_won.text;
        dont_give_up.text = TranslationManager.Get("dont_give_up") != string.Empty ? TranslationManager.Get("dont_give_up") : dont_give_up.text;
        date.text = TranslationManager.Get("date") != string.Empty ? TranslationManager.Get("date") : date.text;
        ID.text = TranslationManager.Get("ID") != string.Empty ? TranslationManager.Get("ID") : ID.text;
        play_again.text = TranslationManager.Get("play_again") != string.Empty ? TranslationManager.Get("play_again") : play_again.text;
        _continue.text = TranslationManager.Get("continue") != string.Empty ? TranslationManager.Get("continue") : _continue.text;
    }
}
