using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SeembaSDK
{
    #pragma warning disable 649
    public class SignupTranslationController : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private Text create_your, account, choose_a_username, enter_your_email, enter_your_password, confirm_your_password,
            have_you_got_an_account, signin, create;
        public TextMeshProUGUI TermsText;
        void Start()
        {
            TranslationManager._instance.scene = "Signup";
            create_your.text = TranslationManager._instance.Get("create_your") != string.Empty ? TranslationManager._instance.Get("create_your") : create_your.text;
            account.text = TranslationManager._instance.Get("account") != string.Empty ? TranslationManager._instance.Get("account") : account.text;
            choose_a_username.text = TranslationManager._instance.Get("choose_a_username") != string.Empty ? TranslationManager._instance.Get("choose_a_username") : choose_a_username.text;
            enter_your_email.text = TranslationManager._instance.Get("enter_your_email") != string.Empty ? TranslationManager._instance.Get("enter_your_email") : enter_your_email.text;
            enter_your_password.text = TranslationManager._instance.Get("enter_your_password") != string.Empty ? TranslationManager._instance.Get("enter_your_password") : enter_your_password.text;
            confirm_your_password.text = TranslationManager._instance.Get("confirm_your_password") != string.Empty ? TranslationManager._instance.Get("confirm_your_password") : confirm_your_password.text;
            if(have_you_got_an_account)
                have_you_got_an_account.text = TranslationManager._instance.Get("have_you_got_an_account") != string.Empty ? TranslationManager._instance.Get("have_you_got_an_account") : have_you_got_an_account.text;
            signin.text = TranslationManager._instance.Get("signin") != string.Empty ? TranslationManager._instance.Get("signin") : signin.text;
            create.text = TranslationManager._instance.Get("create") != string.Empty ? TranslationManager._instance.Get("create") : create.text;
            TermsText.text = TranslationManager._instance.Get("by_creating_this_account") + " <color=#FAB986>" + TranslationManager._instance.Get("terms_conditions") + "</color> "
                 + TranslationManager._instance.Get("and") + " <color=#FAB986>" + TranslationManager._instance.Get("privacy_policy") + "</color>";
        }
    }
}