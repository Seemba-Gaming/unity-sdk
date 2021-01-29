using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
#pragma warning disable 0649
    public class WithdrawalInfoTranslationController : MonoBehaviour

    {
        public Text BackButton;
        public Text ContinuerButton;

        void Start()
        {
            TranslationManager.scene = "Home";
            BackButton.text = TranslationManager.Get("back_button");
            ContinuerButton.text = TranslationManager.Get("continue");
        }
    }
}
