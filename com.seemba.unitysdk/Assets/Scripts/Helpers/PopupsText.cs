using System;
using UnityEngine;

namespace SeembaSDK
{
    public class PopupsText : MonoBehaviour
    {
        public static PopupsText Get { get { return sInstance; } }
        private static PopupsText sInstance;

        private void Awake()
        {
            sInstance = this;

        }
        public object[] vpn()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("desactivate"), TranslationManager._instance.Get("your_vpn"), TranslationManager._instance.Get("you_cant_play_vpn"), TranslationManager._instance.Get("got_it") };
        }
        public object[] dev_mode()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("desactivate"), TranslationManager._instance.Get("The_development_settings"), TranslationManager._instance.Get("you_cant_play_dev_mode"), TranslationManager._instance.Get("got_it") };
        }
        public object[] prohibited_location()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("prohibited"), TranslationManager._instance.Get("location"), TranslationManager._instance.Get("real_money_tournaments"), TranslationManager._instance.Get("got_it") };
        }
        public object[] download_from_store()
        {
            return new object[] { "DOWNLOAD", "FROM STORE", "To get all our features you need to download full version from our store", "DOWNLOAD" };
        }
        public object[] insufficient_balance()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("insufficient"), TranslationManager._instance.Get("balance"), TranslationManager._instance.Get("credit_your_account") + "?", TranslationManager._instance.Get("credit") };
        }
        public object[] insufficient_bubbles()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("insufficient"), TranslationManager._instance.Get("bubbles"), TranslationManager._instance.Get("get_free_bubbles"), TranslationManager._instance.Get("got_it") };
        }
        public object[] AgeVerification()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("age"), TranslationManager._instance.Get("verification"), TranslationManager._instance.Get("by_clicking"), TranslationManager._instance.Get("confirm"), TranslationManager._instance.Get("enter_age") };
        }
        public object[] PopupPayment()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("select"), TranslationManager._instance.Get("payment_method"), TranslationManager._instance.Get("select"), TranslationManager._instance.Get("selected"), TranslationManager._instance.Get("confirmed") };
        }
        public object[] UpdatePassword()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("change"), TranslationManager._instance.Get("password"), TranslationManager._instance.Get("new_password"), TranslationManager._instance.Get("confirm_password"), TranslationManager._instance.Get("confirm") };
        }
        public object[] CurrentPassword()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("change"), TranslationManager._instance.Get("password"), TranslationManager._instance.Get("current_password"), TranslationManager._instance.Get("next") };
        }
        public object[] ForgetPassword()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("forgotten"), TranslationManager._instance.Get("password"), TranslationManager._instance.Get("email"), TranslationManager._instance.Get("enter_your_email_below_to_reset_password"), TranslationManager._instance.Get("submit") };
        }
        public object[] CongratsWithdraw()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("withdrawal"), TranslationManager._instance.Get("confirmed"), TranslationManager._instance.Get("you_will_receive"), TranslationManager._instance.Get("working_days"), TranslationManager._instance.Get("great") };
        }
        public object[] Congrats()
        {
            return new object[] { "DOWNLOAD", "FROM STORE", "To get all our features you need to download full version from our store", "DOWNLOAD" };
        }
        public object[] Win(int gain, string gainType)
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("bubbles"), TranslationManager._instance.Get("great"), gain, gainType };
        }

        public object[] SecureConfirmation()
        {
            return new object[] { "3D SECURE", "CONFIRMATION", "3D Secure authentication must be completed for the payment to be successful", "GO AHEAD" };
        }
        public object[] TooYoung()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("sorry"), TranslationManager._instance.Get("you_are_too_young"), TranslationManager._instance.Get("we_apologize"), TranslationManager._instance.Get("got_it") };
        }
        public object[] ProhibitedLocationWallet()
        {
            TranslationManager._instance.scene = "Home";
            return new object[] { TranslationManager._instance.Get("prohibited"), TranslationManager._instance.Get("location"), TranslationManager._instance.Get("credit_isnot_authorized"), TranslationManager._instance.Get("got_it") };
        }
        public object[] ProhibitedLocationWithdraw()
        {
            return new object[] { TranslationManager._instance.Get("prohibited"), TranslationManager._instance.Get("location"), TranslationManager._instance.Get("withdrawal_isnot_authorized"), TranslationManager._instance.Get("got_it") };
        }
        public object[] PasswordUpdated()
        {
            return new object[] { TranslationManager._instance.Get("password"), TranslationManager._instance.Get("changed"), TranslationManager._instance.Get("your_password"), TranslationManager._instance.Get("great") + " !" };
        }
        public object[] MissingInfo()
        {
            return new object[] { TranslationManager._instance.Get("unverified"), TranslationManager._instance.Get("account"), TranslationManager._instance.Get("please_provide"), TranslationManager._instance.Get("got_it") };
        }
        public object[] Missing()
        {
            return new object[] { "missing", "information", "Complete your information to withdraw your funds", "Got it !" };
        }
        public object[] ConnectionFailed()
        {
            TranslationManager._instance.scene = "Popups";
            return new object[] { TranslationManager._instance.Get("weak"), TranslationManager._instance.Get("network_reception"), TranslationManager._instance.Get("weak_internet_desc"), TranslationManager._instance.Get("i_understand") };
        }
        public object[] Unauthorized()
        {
            TranslationManager._instance.scene = "Popups";
            return new object[] { TranslationManager._instance.Get("sorry"), TranslationManager._instance.Get("not_authorized"), TranslationManager._instance.Get("not_authorized_desc"), TranslationManager._instance.Get("i_understand") };
        }
        public object[] ServerError()
        {
            TranslationManager._instance.scene = "Popups";
            return new object[] { TranslationManager._instance.Get("error"), TranslationManager._instance.Get("server_error"), TranslationManager._instance.Get("server_error_desc"), TranslationManager._instance.Get("i_understand") };
        }
        //stopped here work on missing translations
        public object[] EmailNotFound()
        {
            TranslationManager._instance.scene = "Popups";
            return new object[] { TranslationManager._instance.Get("email"), TranslationManager._instance.Get("error"), TranslationManager._instance.Get("email_not_found_desc"), TranslationManager._instance.Get("i_understand") };
        }
        public object[] EmailSent()
        {
            return new object[] { TranslationManager._instance.Get("email"), TranslationManager._instance.Get("sent"), TranslationManager._instance.Get("email_instructions"), TranslationManager._instance.Get("ok"), "BUTTON_SEND_AGAIN" };
        }
        public object[] StripeIncorrectNumber()
        {
            TranslationManager._instance.scene = "Popups";
            return new object[] { TranslationManager._instance.Get("payment"), TranslationManager._instance.Get("failed"), TranslationManager._instance.Get("stripe_incorrect_number"), TranslationManager._instance.Get("i_understand") };
        }
        public object[] StripeCardDeclined()
        {
            TranslationManager._instance.scene = "Popups";
            return new object[] { TranslationManager._instance.Get("payment"), TranslationManager._instance.Get("failed"), TranslationManager._instance.Get("stripe_card_declined"), TranslationManager._instance.Get("i_understand") };
        }
        public object[] StripeIncorrectCVC()
        {
            TranslationManager._instance.scene = "Popups";
            return new object[] { TranslationManager._instance.Get("payment"), TranslationManager._instance.Get("failed"), TranslationManager._instance.Get("stripe_incorrect_cvc"), TranslationManager._instance.Get("i_understand") };
        }
        public object[] StripeBalanceInsufficient()
        {
            TranslationManager._instance.scene = "Popups";
            return new object[] { TranslationManager._instance.Get("payment"), TranslationManager._instance.Get("failed"), TranslationManager._instance.Get("stripe_balance_insufficient"), TranslationManager._instance.Get("i_understand") };
        }
        public object[] StripeTryAgainLater()
        {
            TranslationManager._instance.scene = "Popups";
            return new object[] { TranslationManager._instance.Get("payment"), TranslationManager._instance.Get("failed"), TranslationManager._instance.Get("stripe_try_again_later"), TranslationManager._instance.Get("i_understand") };
        }
        public object[] GiftCardSuccess()
        {
            TranslationManager._instance.scene = "Popups";
            return new object[] { TranslationManager._instance.Get("congratulations"), TranslationManager._instance.Get("payment_succeeded"), TranslationManager._instance.Get("payment_succeeded_desc"), TranslationManager._instance.Get("i_understand") };
        }
        public object[] GiftCardIf()
        {
            TranslationManager._instance.scene = "Popups";
            return new object[] { TranslationManager._instance.Get("payment"), TranslationManager._instance.Get("failed"), TranslationManager._instance.Get("not_enough_crowns"), TranslationManager._instance.Get("i_understand") };
        }
        public object[] PaymentNotConfirmed()
        {
            return new object[] { "DOWNLOAD", "FROM STORE", "To get all our features you need to download full version from our store", "DOWNLOAD" };
        }
        public object[] EqualityRefund()
        {
            return new object[] { "Equality", "refund", "You have been credited back your entry fee", "GREAT!", "TOGGLE_DO_NOT_SHOW_AGAIN" };
        }
        public object[] NotAvailable()
        {
            return new object[] { "DOWNLOAD", "FROM STORE", "To get all our features you need to download full version from our store", "DOWNLOAD" };
        }
        public object[] SessionExpired()
        {
            return new object[] { "Unknown", "Device", "Sorry , ads are not yet supported on this device", "GOT IT!" };
        }
        public object[] PaimentFailed()
        {
            return new object[] { "Payment Failed", "oops !", "please make sur that your information are correct", "continue" };
        }
    }
}
