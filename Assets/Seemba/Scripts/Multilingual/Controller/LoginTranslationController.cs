using UnityEngine;
using UnityEngine.UI;
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
            email, _continue_email;
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
        TranslationManager.scene = "Login";
        //--------------Signin------------
        welcome.text = TranslationManager.Get("welcome") != string.Empty ? TranslationManager.Get("welcome") : welcome.text;
        back.text = TranslationManager.Get("back") != string.Empty ? TranslationManager.Get("back") : back.text;
        incorrect.text = TranslationManager.Get("incorrect") != string.Empty ? TranslationManager.Get("incorrect") : incorrect.text;
        email_or_username.text = TranslationManager.Get("email_or_username") != string.Empty ? TranslationManager.Get("email_or_username") : email_or_username.text;
        password.text = TranslationManager.Get("password") != string.Empty ? TranslationManager.Get("password") : password.text;
        forgot_your_password.text = TranslationManager.Get("forgot_your_password") != string.Empty ? TranslationManager.Get("forgot_your_password") : forgot_your_password.text;
        magic_code.text = TranslationManager.Get("magic_code") != string.Empty ? TranslationManager.Get("magic_code") : magic_code.text;
        new_player.text = TranslationManager.Get("new_player") != string.Empty ? TranslationManager.Get("new_player") : new_player.text;
        create_account.text = TranslationManager.Get("create_account") != string.Empty ? TranslationManager.Get("create_account") : create_account.text;
        _continue.text = TranslationManager.Get("_continue") != string.Empty ? TranslationManager.Get("_continue") : _continue.text;
        //--------------Email------------
        reset_password.text = TranslationManager.Get("reset_password") != string.Empty ? TranslationManager.Get("reset_password") : reset_password.text;
        enter_your_email_below_to_reset_password.text = TranslationManager.Get("enter_your_email_below_to_reset_password") != string.Empty ? TranslationManager.Get("enter_your_email_below_to_reset_password") : enter_your_email_below_to_reset_password.text;
        email.text = TranslationManager.Get("email") != string.Empty ? TranslationManager.Get("email") : email.text;
        //--------------Code------------
        reset_password_code.text = TranslationManager.Get("reset_password") != string.Empty ? TranslationManager.Get("reset_password_code") : reset_password_code.text;
        Enter_the_code.text = TranslationManager.Get("Enter_the_code") != string.Empty ? TranslationManager.Get("Enter_the_code") : Enter_the_code.text;
        code_expired.text = TranslationManager.Get("code_expired") != string.Empty ? TranslationManager.Get("code_expired") : code_expired.text;
        resend.text = TranslationManager.Get("resend") != string.Empty ? TranslationManager.Get("resend") : resend.text;
        submit.text = TranslationManager.Get("submit") != string.Empty ? TranslationManager.Get("submit") : submit.text;
        //--------------Change Password------------
        reset_password_change.text = TranslationManager.Get("reset_password") != string.Empty ? TranslationManager.Get("reset_password_code") : reset_password_change.text;
        Enter_the_new_password.text = TranslationManager.Get("Enter_the_new_password") != string.Empty ? TranslationManager.Get("Enter_the_new_password") : Enter_the_new_password.text;
        new_password.text = TranslationManager.Get("new_password") != string.Empty ? TranslationManager.Get("new_password") : new_password.text;
        confirm_password.text = TranslationManager.Get("confirm_password") != string.Empty ? TranslationManager.Get("confirm_password") : confirm_password.text;
        code_expired_change.text = TranslationManager.Get("code_expired") != string.Empty ? TranslationManager.Get("code_expired") : code_expired_change.text;
        resend_change.text = TranslationManager.Get("resend") != string.Empty ? TranslationManager.Get("resend") : resend_change.text;
        done.text = TranslationManager.Get("done") != string.Empty ? TranslationManager.Get("done") : done.text;
    }
}
