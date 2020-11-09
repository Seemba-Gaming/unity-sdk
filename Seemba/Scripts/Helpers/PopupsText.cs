using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        return new object[] { "DOWNLOAD", "FROM STORE", "To get all our features you need to download full version from our store", "DOWNLOAD" };
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
        return new object[] { TranslationManager.Get("password"), TranslationManager.Get("changed"), TranslationManager.Get("your_password"), TranslationManager.Get("great") +  " !" };
    }
    public object[] MissingInfo()
    {
        return new object[] { "unverified", "Account", "Please provide the following information so we can verify your account", "Got it !" };
    }
    public object[] Missing()
    {
        return new object[] { "missing", "information", "Complete your information to withdraw your funds", "Got it !" };
    }
    public object[] ConnectionFailed()
    {
        return new object[] { "Weak", "Network reception", "We are trying to access internet but your connection is too weak...", "Got it !" };
    }
    public object[] Unauthorized()
    {
        return new object[] { "Weak", "Network reception", "We are trying to access internet but your connection is too weak...", "Got it !" };
    }
    public object[] ServerError()
    {
        return new object[] { "Weak", "Network reception", "We are trying to access internet but your connection is too weak...", "Got it !" };
    }
    //stopped here work on missing translations
    public object[] EmailNotFound()
    {
        return new object[] { "Email", "error", "We’re sorry but no account has been created on Seemba with this email. Please try another one.", "GOT IT!" };
    }
    public object[] EmailSent()
    {
        return new object[] { "Email", "sent", "follow the instructions received on your email account", "GOT IT!", "BUTTON_SEND_AGAIN" };
    }
    public object[] PaymentNotConfirmed()
    {
        return new object[] { "DOWNLOAD", "FROM STORE", "To get all our features you need to download full version from our store", "DOWNLOAD" };
    }
    public object[] EqualityRefund()
    {
        return new object[] { "Equality", "refund", "You have been credited back your entry fee", "GREAT!","TOGGLE_DO_NOT_SHOW_AGAIN" };
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
