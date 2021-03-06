﻿using System;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
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
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("desactivate"), TranslationManager.Get("your_vpn"), TranslationManager.Get("you_cant_play_vpn"), TranslationManager.Get("got_it") };
        }
        public object[] dev_mode()
        {
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("desactivate"), TranslationManager.Get("The_development_settings"), TranslationManager.Get("you_cant_play_dev_mode"), TranslationManager.Get("got_it") };
        }
        public object[] prohibited_location()
        {
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("prohibited"), TranslationManager.Get("location"), TranslationManager.Get("real_money_tournaments"), TranslationManager.Get("got_it") };
        }
        public object[] download_from_store()
        {
            return new object[] { "DOWNLOAD", "FROM STORE", "To get all our features you need to download full version from our store", "DOWNLOAD" };
        }
        public object[] insufficient_balance()
        {
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("insufficient"), TranslationManager.Get("balance"), TranslationManager.Get("credit_your_account") + "?", TranslationManager.Get("credit") };
        }
        public object[] insufficient_bubbles()
        {
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("insufficient"), TranslationManager.Get("bubbles"), TranslationManager.Get("get_free_bubbles"), TranslationManager.Get("got_it") };
        }
        public object[] AgeVerification()
        {
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("age"), TranslationManager.Get("verification"), TranslationManager.Get("by_clicking"), TranslationManager.Get("confirm"), TranslationManager.Get("enter_age") };
        }
        public object[] PopupPayment()
        {
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("select"), TranslationManager.Get("payment_method"), TranslationManager.Get("select"), TranslationManager.Get("selected"), TranslationManager.Get("confirmed") };
        }
        public object[] UpdatePassword()
        {
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("change"), TranslationManager.Get("password"), TranslationManager.Get("new_password"), TranslationManager.Get("confirm_password"), TranslationManager.Get("confirm") };
        }
        public object[] CurrentPassword()
        {
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("change"), TranslationManager.Get("password"), TranslationManager.Get("current_password"), TranslationManager.Get("next") };
        }
        public object[] ForgetPassword()
        {
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("forgotten"), TranslationManager.Get("password"), TranslationManager.Get("email"), TranslationManager.Get("enter_your_email_below_to_reset_password"), TranslationManager.Get("submit") };
        }
        public object[] CongratsWithdraw()
        {
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("withdrawal"), TranslationManager.Get("confirmed"), TranslationManager.Get("you_will_receive"), TranslationManager.Get("working_days"), TranslationManager.Get("great") };
        }
        public object[] Congrats()
        {
            return new object[] { "DOWNLOAD", "FROM STORE", "To get all our features you need to download full version from our store", "DOWNLOAD" };
        }
        public object[] Win()
        {
            return new object[] { TranslationManager.Get("bubbles"), TranslationManager.Get("great") };
        }

        public object[] SecureConfirmation()
        {
            return new object[] { "3D SECURE", "CONFIRMATION", "3D Secure authentication must be completed for the payment to be successful", "GO AHEAD" };
        }
        public object[] TooYoung()
        {
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("sorry"), TranslationManager.Get("you_are_too_young"), TranslationManager.Get("we_apologize"), TranslationManager.Get("got_it") };
        }
        public object[] ProhibitedLocationWallet()
        {
            TranslationManager.scene = "Home";
            return new object[] { TranslationManager.Get("prohibited"), TranslationManager.Get("location"), TranslationManager.Get("credit_isnot_authorized"), TranslationManager.Get("got_it") };
        }
        public object[] ProhibitedLocationWithdraw()
        {
            return new object[] { TranslationManager.Get("prohibited"), TranslationManager.Get("location"), TranslationManager.Get("withdrawal_isnot_authorized"), TranslationManager.Get("got_it") };
        }
        public object[] PasswordUpdated()
        {
            return new object[] { TranslationManager.Get("password"), TranslationManager.Get("changed"), TranslationManager.Get("your_password"), TranslationManager.Get("great") + " !" };
        }
        public object[] MissingInfo()
        {
            return new object[] { TranslationManager.Get("unverified"), TranslationManager.Get("account"), TranslationManager.Get("please_provide"), TranslationManager.Get("got_it") };
        }
        public object[] Missing()
        {
            return new object[] { "missing", "information", "Complete your information to withdraw your funds", "Got it !" };
        }
        public object[] ConnectionFailed()
        {
            TranslationManager.scene = "Popups";
            return new object[] { TranslationManager.Get("weak"), TranslationManager.Get("network_reception"), TranslationManager.Get("weak_internet_desc"), TranslationManager.Get("i_understand") };
        }
        public object[] Unauthorized()
        {
            TranslationManager.scene = "Popups";
            return new object[] { TranslationManager.Get("sorry"), TranslationManager.Get("not_authorized"), TranslationManager.Get("not_authorized_desc"), TranslationManager.Get("i_understand") };
        }
        public object[] ServerError()
        {
            TranslationManager.scene = "Popups";
            return new object[] { TranslationManager.Get("error"), TranslationManager.Get("server_error"), TranslationManager.Get("server_error_desc"), TranslationManager.Get("i_understand") };
        }
        //stopped here work on missing translations
        public object[] EmailNotFound()
        {
            TranslationManager.scene = "Popups";
            return new object[] { TranslationManager.Get("email"), TranslationManager.Get("error"), TranslationManager.Get("email_not_found_desc"), TranslationManager.Get("i_understand") };
        }
        public object[] EmailSent()
        {
            return new object[] { TranslationManager.Get("email"), TranslationManager.Get("sent"), TranslationManager.Get("email_instructions"), TranslationManager.Get("ok"), "BUTTON_SEND_AGAIN" };
        }
        public object[] StripeIncorrectNumber()
        {
            TranslationManager.scene = "Popups";
            return new object[] { TranslationManager.Get("payment"), TranslationManager.Get("failed"), TranslationManager.Get("stripe_incorrect_number"), TranslationManager.Get("i_understand") };
        }
        public object[] StripeCardDeclined()
        {
            TranslationManager.scene = "Popups";
            return new object[] { TranslationManager.Get("payment"), TranslationManager.Get("failed"), TranslationManager.Get("stripe_card_declined"), TranslationManager.Get("i_understand") };
        }
        public object[] StripeIncorrectCVC()
        {
            TranslationManager.scene = "Popups";
            return new object[] { TranslationManager.Get("payment"), TranslationManager.Get("failed"), TranslationManager.Get("stripe_incorrect_cvc"), TranslationManager.Get("i_understand") };
        }
        public object[] StripeBalanceInsufficient()
        {
            TranslationManager.scene = "Popups";
            return new object[] { TranslationManager.Get("payment"), TranslationManager.Get("failed"), TranslationManager.Get("stripe_balance_insufficient"), TranslationManager.Get("i_understand") };
        }
        public object[] StripeTryAgainLater()
        {
            TranslationManager.scene = "Popups";
            return new object[] { TranslationManager.Get("payment"), TranslationManager.Get("failed"), TranslationManager.Get("stripe_try_again_later"), TranslationManager.Get("i_understand") };
        }
        public object[] GiftCardSuccess()
        {
            TranslationManager.scene = "Popups";
            return new object[] { TranslationManager.Get("congratulations"), TranslationManager.Get("payment_succeeded"), TranslationManager.Get("payment_succeeded_desc"), TranslationManager.Get("i_understand") };
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
