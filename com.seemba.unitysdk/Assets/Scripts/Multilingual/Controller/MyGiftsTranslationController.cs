using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class MyGiftsTranslationController : MonoBehaviour
    {
        public Text MyGifts;


        public void Init()
        {
            TranslationManager._instance.scene = "Home";
            MyGifts.text = TranslationManager._instance.Get("my_gifts");
        }
    }
}
