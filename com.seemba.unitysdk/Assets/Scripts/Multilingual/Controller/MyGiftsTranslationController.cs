using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class MyGiftsTranslationController : MonoBehaviour
    {
        public Text MyGifts;


        public void Init()
        {
            TranslationManager.scene = "Home";
            MyGifts.text = TranslationManager.Get("my_gifts");
        }
    }
}
