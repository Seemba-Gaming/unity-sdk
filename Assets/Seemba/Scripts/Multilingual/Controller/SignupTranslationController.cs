﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SignupTranslationController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text create_your, account, choose_a_username, enter_your_email,enter_your_password, confirm_your_password, have_you_got_an_account, signin, create, choose_your_character;
    void Start()
    {
        TranslationManager.scene = "Signup";
        create_your.text = TranslationManager.Get("create_your") !=string.Empty ? TranslationManager.Get("create_your") : create_your.text;
        account.text = TranslationManager.Get("account") !=string.Empty ? TranslationManager.Get("account") : account.text;
        choose_a_username.text = TranslationManager.Get("choose_a_username") !=string.Empty ? TranslationManager.Get("choose_a_username") : choose_a_username.text;
        enter_your_email.text = TranslationManager.Get("enter_your_email") !=string.Empty ? TranslationManager.Get("enter_your_email") : enter_your_email.text;
        enter_your_password.text = TranslationManager.Get("enter_your_password") !=string.Empty ? TranslationManager.Get("enter_your_password") : enter_your_password.text;
        confirm_your_password.text = TranslationManager.Get("confirm_your_password") !=string.Empty ? TranslationManager.Get("confirm_your_password") : confirm_your_password.text;
        have_you_got_an_account.text = TranslationManager.Get("have_you_got_an_account") !=string.Empty ? TranslationManager.Get("have_you_got_an_account") : have_you_got_an_account.text;
        signin.text = TranslationManager.Get("signin") !=string.Empty ? TranslationManager.Get("signin") : signin.text;
        create.text = TranslationManager.Get("create") !=string.Empty ? TranslationManager.Get("create") : create.text;
        choose_your_character.text = TranslationManager.Get("choose_your_character") !=string.Empty ? TranslationManager.Get("choose_your_character") : choose_your_character.text;
    }
}
