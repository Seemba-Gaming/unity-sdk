using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
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
            TranslationManager._instance.scene = "Bracket";
            Tour1.text = TranslationManager._instance.Get("tour1");
            SemiFinal.text = TranslationManager._instance.Get("semi_final");
            Final.text = TranslationManager._instance.Get("final");
            Champion.text = TranslationManager._instance.Get("champion");
            Play.text = TranslationManager._instance.Get("play");
            TranslationManager._instance.scene = "Home";
            Back.text = TranslationManager._instance.Get("back_button");
        }
    }
}
