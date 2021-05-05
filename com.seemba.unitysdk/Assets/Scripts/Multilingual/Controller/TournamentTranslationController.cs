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
        public Text OneHundredTwentyEight;
        public Text Sixtyfour;
        public Text ThirtyTwo;
        public Text Sixteenth;
        public Text QuarterFinal;
        public Text SemiFinal;
        public Text Final;
        public Text Champion;
        public Text Play;
        public Text Back;
        #endregion
        void Start()
        {
            TranslationManager.scene = "Bracket";
            if(Sixtyfour)
                Sixtyfour.text = TranslationManager.Get("64");
            if(OneHundredTwentyEight)
                OneHundredTwentyEight.text = TranslationManager.Get("128");

            if (ThirtyTwo)
                ThirtyTwo.text = TranslationManager.Get("32");

            if (Sixteenth)
                Sixteenth.text = TranslationManager.Get("16");

            if(QuarterFinal)
                QuarterFinal.text = TranslationManager.Get("8");
            if (SemiFinal)
                SemiFinal.text = TranslationManager.Get("4");

            if (Final)
                Final.text = TranslationManager.Get("2");

            if (Champion)
                Champion.text = TranslationManager.Get("champion");

            if (Play)
                Play.text = TranslationManager.Get("play");

                TranslationManager.scene = "Home";
            if (Back)
                Back.text = TranslationManager.Get("back_button");
        }
    }
}
