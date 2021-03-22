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
            TranslationManager.scene = "Home";
            Challenge.text = TranslationManager.Get("challenge") != string.Empty ? TranslationManager.Get("challenge") : Challenge.text;
            Selection.text = TranslationManager.Get("selection") != string.Empty ? TranslationManager.Get("selection") : Selection.text;
            WinAndGet.text = TranslationManager.Get("win_and_get") != string.Empty ? TranslationManager.Get("win_and_get") : WinAndGet.text;
            EntryFee.text = TranslationManager.Get("entry_fee") != string.Empty ? TranslationManager.Get("entry_fee") : EntryFee.text;
            Bubbles.text = TranslationManager.Get("bubbles") != string.Empty ? TranslationManager.Get("bubbles") : Bubbles.text;
            DuelConfirmButtonText.text = TranslationManager.Get("go") != string.Empty ? TranslationManager.Get("go") : DuelConfirmButtonText.text;

            Select.text = TranslationManager.Get("select") != string.Empty ? TranslationManager.Get("select") : Select.text;
            PaymentMethod.text = TranslationManager.Get("payment_method") != string.Empty ? TranslationManager.Get("payment_method") : PaymentMethod.text;
            PaymentConfirmButtonText.text = TranslationManager.Get("confirm") != string.Empty ? TranslationManager.Get("confirm") : PaymentConfirmButtonText.text;
        }

    }
}
