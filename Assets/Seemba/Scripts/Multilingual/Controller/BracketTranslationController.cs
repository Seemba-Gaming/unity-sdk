using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BracketTranslationController : MonoBehaviour
{
    [SerializeField]
    private Text tour1,
                  semi_final;
    [SerializeField]
    private Text
                  final,
                  champion,play;
    [SerializeField]
    private Text[] to_be_determined;
    // Start is called before the first frame update
    void Start()
    {
        TranslationManager.scene = "Bracket";
        tour1.text = TranslationManager.Get("tour1") != string.Empty ? TranslationManager.Get("tour1") : tour1.text;
        semi_final.text = TranslationManager.Get("semi_final") != string.Empty ? TranslationManager.Get("semi_final") : semi_final.text;
        final.text = TranslationManager.Get("final") != string.Empty ? TranslationManager.Get("final") : final.text;
        champion.text = TranslationManager.Get("champion") != string.Empty ? TranslationManager.Get("champion") : champion.text;
        play.text = TranslationManager.Get("play") != string.Empty ? TranslationManager.Get("play") : play.text;
        /*foreach (Text _text in to_be_determined) {
            _text.text = TranslationManager.Get("to_be_determined") != string.Empty ? TranslationManager.Get("to_be_determined") : _text.text;
        }*/
        for (int pos = 1; pos <= 8; pos++)
        {
            for (int player = 1; player <= 2; player++)
            {
                try { 
                Debug.Log("Challenge (" + (pos) + ")/Player" + player + "/Username");
                    Text to_be_determined = GameObject.Find("Challenge (" + (pos) + ")/Player"+ player + "/Username").GetComponent<Text>();
                to_be_determined.text = TranslationManager.Get("to_be_determined") != string.Empty ? TranslationManager.Get("to_be_determined") : to_be_determined.text;
                }
                catch (System.NullReferenceException ex) { }
            }
        }
    }
}
