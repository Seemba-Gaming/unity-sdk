using UnityEngine;
using UnityEngine.UI;
public class ResultWaitingTranslationController : MonoBehaviour
{
    [SerializeField]
    private Text waiting_for,
      player_2,
      score,
      continue_now,
      and_get_results_later,
      date,
      ID,
      _continue;
    // Start is called before the first frame update
    void Start()
    {
        TranslationManager.scene = "ResultWaiting";
        waiting_for.text = TranslationManager.Get("waiting_for") != string.Empty ? TranslationManager.Get("waiting_for") : waiting_for.text;
        player_2.text = TranslationManager.Get("player_2") != string.Empty ? TranslationManager.Get("player_2") : player_2.text;
        score.text = TranslationManager.Get("score") != string.Empty ? TranslationManager.Get("score") : score.text;
        continue_now.text = TranslationManager.Get("continue_now") != string.Empty ? TranslationManager.Get("continue_now") : continue_now.text;
        and_get_results_later.text = TranslationManager.Get("and_get_results_later") != string.Empty ? TranslationManager.Get("and_get_results_later") : and_get_results_later.text;
        date.text = TranslationManager.Get("date") != string.Empty ? TranslationManager.Get("date") : date.text;
        ID.text = TranslationManager.Get("ID") != string.Empty ? TranslationManager.Get("ID") : ID.text;
        _continue.text = TranslationManager.Get("continue") != string.Empty ? TranslationManager.Get("continue") : _continue.text;
    }
}
