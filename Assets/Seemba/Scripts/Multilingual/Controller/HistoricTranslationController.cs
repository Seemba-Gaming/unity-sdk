using UnityEngine;
using UnityEngine.UI;
public class HistoricTranslationController : MonoBehaviour
{
    [SerializeField]
    private Text games_won,
                 in_a_row;
    // Start is called before the first frame update
    void Start()
    {
        TranslationManager.scene = "History";
        games_won.text = TranslationManager.Get("games_won") != string.Empty ? TranslationManager.Get("games_won") : games_won.text;
        in_a_row.text = TranslationManager.Get("in_a_row") != string.Empty ? TranslationManager.Get("in_a_row") : in_a_row.text;
    }
}
