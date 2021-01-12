using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class TournamentTranslationController : MonoBehaviour
    {
        #region Script Parameters
        public Text Tour1;
        public Text SemiFinal;
        public Text Final;
        public Text Champion;
        public Text Play;
        public Text Back;
        #endregion
        void Start()
        {
            TranslationManager.scene = "Bracket";
            Tour1.text = TranslationManager.Get("tour1");
            SemiFinal.text = TranslationManager.Get("semi_final");
            Final.text = TranslationManager.Get("final");
            Champion.text = TranslationManager.Get("champion");
            Play.text = TranslationManager.Get("play");
            TranslationManager.scene = "Home";
            Back.text = TranslationManager.Get("back_button");
        }
    }
}
