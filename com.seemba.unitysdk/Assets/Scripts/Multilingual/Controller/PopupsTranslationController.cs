using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class PopupsTranslationController : MonoBehaviour
    {
        [Header("Popup duels")]
        public Text Challenge;
        public Text Selection;
        public Text WinAndGet;
        public Text EntryFee;
        public Text Bubbles;
        public Text DuelConfirmButtonText;

        [Header("Popup Payment")]
        public Text Select;
        public Text PaymentMethod;
        public Text PaymentConfirmButtonText;

        public void Init()
        {
            TranslationManager._instance.scene = "Home";
            Challenge.text = TranslationManager._instance.Get("challenge") != string.Empty ? TranslationManager._instance.Get("challenge") : Challenge.text;
            Selection.text = TranslationManager._instance.Get("selection") != string.Empty ? TranslationManager._instance.Get("selection") : Selection.text;
            WinAndGet.text = TranslationManager._instance.Get("win_and_get") != string.Empty ? TranslationManager._instance.Get("win_and_get") : WinAndGet.text;
            EntryFee.text = TranslationManager._instance.Get("entry_fee") != string.Empty ? TranslationManager._instance.Get("entry_fee") : EntryFee.text;
            Bubbles.text = TranslationManager._instance.Get("bubbles") != string.Empty ? TranslationManager._instance.Get("bubbles") : Bubbles.text;
            DuelConfirmButtonText.text = TranslationManager._instance.Get("go") != string.Empty ? TranslationManager._instance.Get("go") : DuelConfirmButtonText.text;

            Select.text = TranslationManager._instance.Get("select") != string.Empty ? TranslationManager._instance.Get("select") : Select.text;
            PaymentMethod.text = TranslationManager._instance.Get("payment_method") != string.Empty ? TranslationManager._instance.Get("payment_method") : PaymentMethod.text;
            PaymentConfirmButtonText.text = TranslationManager._instance.Get("confirm") != string.Empty ? TranslationManager._instance.Get("confirm") : PaymentConfirmButtonText.text;
        }

    }
}
