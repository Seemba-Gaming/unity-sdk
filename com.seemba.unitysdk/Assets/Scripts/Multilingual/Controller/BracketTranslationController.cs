using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class BracketTranslationController : MonoBehaviour
    {
        [SerializeField]
        private Text tour1,
                      semi_final;
        [SerializeField]
        private Text
                      final,
                      champion, play;
        [SerializeField]
        private Text[] to_be_determined;
        // Start is called before the first frame update
        void Start()
        {
            TranslationManager._instance.scene = "Bracket";
            tour1.text = TranslationManager._instance.Get("tour1") != string.Empty ? TranslationManager._instance.Get("tour1") : tour1.text;
            semi_final.text = TranslationManager._instance.Get("semi_final") != string.Empty ? TranslationManager._instance.Get("semi_final") : semi_final.text;
            final.text = TranslationManager._instance.Get("final") != string.Empty ? TranslationManager._instance.Get("final") : final.text;
            champion.text = TranslationManager._instance.Get("champion") != string.Empty ? TranslationManager._instance.Get("champion") : champion.text;
            play.text = TranslationManager._instance.Get("play") != string.Empty ? TranslationManager._instance.Get("play") : play.text;
        }
    }
}
