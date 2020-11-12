using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CLSCompliant(false)]
#pragma warning disable 0649
public class BottomBarTranslationController : MonoBehaviour
{
    [SerializeField]
    private Text home, have_fun, win_money,setting;
    // Start is called before the first frame update
    void Start()
    {
        TranslationManager.scene = "BottomBar";
        home.text = TranslationManager.Get("home") != string.Empty ? TranslationManager.Get("home") : home.text;
        have_fun.text = TranslationManager.Get("have_fun") != string.Empty ? TranslationManager.Get("have_fun") : have_fun.text;
        win_money.text = TranslationManager.Get("win_money") != string.Empty ? TranslationManager.Get("win_money") : win_money.text;
        setting.text = TranslationManager.Get("settings") != string.Empty ? TranslationManager.Get("settings") : setting.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
