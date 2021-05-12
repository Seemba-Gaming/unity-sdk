using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    #pragma warning disable 649
    public class LoginTranslationController : MonoBehaviour
    {
        [Header("--------------Signin------------")]
        [SerializeField]
        private Text
                welcome;
        [SerializeField]
        private Text
                back,
                incorrect,
                email_or_username,
                password,
                forgot_your_password,
                magic_code,
                new_player,
                create_account,
                _continue;
        [Header("--------------Email------------")]
        [SerializeField]
        private Text
                reset_password;
        [SerializeField]
        private Text
                enter_your_email_below_to_reset_password,
                email, _continue_email,
                back_email;
        [Header("--------------Code------------")]
        [SerializeField]
        private Text
                reset_password_code;
        [SerializeField]
        private Text
                Enter_the_code,
                code_expired,
                resend,
                submit;
        [Header("--------------Change password------------")]
        [SerializeField]
        private Text
                reset_password_change;
        [SerializeField]
        private Text
                Enter_the_new_password,
                new_password,
                confirm_password,
                code_expired_change,
                resend_change,
                done;
        void Start()
        {
            TranslationManager._instance.scene = "Login";
            //--------------Signin------------
            if(welcome)
                welcome.text = TranslationManager._instance.Get("welcome") != string.Empty ? TranslationManager._instance.Get("welcome") : welcome.text;
            if(back)
                back.text = TranslationManager._instance.Get("back") != string.Empty ? TranslationManager._instance.Get("back") : back.text;
            if (incorrect)
                incorrect.text = TranslationManager._instance.Get("incorrect") != string.Empty ? TranslationManager._instance.Get("incorrect") : incorrect.text;
            if (email_or_username)
                email_or_username.text = TranslationManager._instance.Get("email_or_username") != string.Empty ? TranslationManager._instance.Get("email_or_username") : email_or_username.text;
            if (password)
                password.text = TranslationManager._instance.Get("password") != string.Empty ? TranslationManager._instance.Get("password") : password.text;
            if (forgot_your_password)
                forgot_your_password.text = TranslationManager._instance.Get("forgot_your_password") != string.Empty ? TranslationManager._instance.Get("forgot_your_password") : forgot_your_password.text;
            if (magic_code)
                magic_code.text = TranslationManager._instance.Get("magic_code") != string.Empty ? TranslationManager._instance.Get("magic_code") : magic_code.text;
            if (new_player)
                new_player.text = TranslationManager._instance.Get("new_player") != string.Empty ? TranslationManager._instance.Get("new_player") : new_player.text;
            if (create_account)
                create_account.text = TranslationManager._instance.Get("create_account") != string.Empty ? TranslationManager._instance.Get("create_account") : create_account.text;
            if (_continue)
                _continue.text = TranslationManager._instance.Get("continue") != string.Empty ? TranslationManager._instance.Get("continue") : _continue.text;
            //--------------Email------------
            reset_password.text = TranslationManager._instance.Get("reset_password") != string.Empty ? TranslationManager._instance.Get("reset_password") : reset_password.text;
            enter_your_email_below_to_reset_password.text = TranslationManager._instance.Get("enter_your_email_below_to_reset_password") != string.Empty ? TranslationManager._instance.Get("enter_your_email_below_to_reset_password") : enter_your_email_below_to_reset_password.text;
            email.text = TranslationManager._instance.Get("email") != string.Empty ? TranslationManager._instance.Get("email") : email.text;
            //--------------Code------------
            reset_password_code.text = TranslationManager._instance.Get("reset_password") != string.Empty ? TranslationManager._instance.Get("reset_password") : reset_password_code.text;
            Enter_the_code.text = TranslationManager._instance.Get("Enter_the_code") != string.Empty ? TranslationManager._instance.Get("Enter_the_code") : Enter_the_code.text;
            code_expired.text = TranslationManager._instance.Get("code_expired") != string.Empty ? TranslationManager._instance.Get("code_expired") : code_expired.text;
            resend.text = TranslationManager._instance.Get("resend") != string.Empty ? TranslationManager._instance.Get("resend") : resend.text;
            submit.text = TranslationManager._instance.Get("submit") != string.Empty ? TranslationManager._instance.Get("submit") : submit.text;
            //--------------Change Password------------
            reset_password_change.text = TranslationManager._instance.Get("reset_password") != string.Empty ? TranslationManager._instance.Get("reset_password") : reset_password_change.text;
            Enter_the_new_password.text = TranslationManager._instance.Get("Enter_the_new_password") != string.Empty ? TranslationManager._instance.Get("Enter_the_new_password") : Enter_the_new_password.text;
            new_password.text = TranslationManager._instance.Get("new_password") != string.Empty ? TranslationManager._instance.Get("new_password") : new_password.text;
            confirm_password.text = TranslationManager._instance.Get("confirm_password") != string.Empty ? TranslationManager._instance.Get("confirm_password") : confirm_password.text;
            code_expired_change.text = TranslationManager._instance.Get("code_expired") != string.Empty ? TranslationManager._instance.Get("code_expired") : code_expired_change.text;
            resend_change.text = TranslationManager._instance.Get("resend") != string.Empty ? TranslationManager._instance.Get("resend") : resend_change.text;
            done.text = TranslationManager._instance.Get("done") != string.Empty ? TranslationManager._instance.Get("done") : done.text;
            TranslationManager._instance.scene = "Home";
            back_email.text = TranslationManager._instance.Get("back_button");
        }
    }
}