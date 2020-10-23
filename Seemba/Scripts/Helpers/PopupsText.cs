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
        return new object[] { "DESACTIVATE", "YOUR VPN", "You can't play real money tournament with a vpn.", "GOT IT!" };
    }
    public object[] dev_mode()
    {
        return new object[] { "DESACTIVATE", "The development settings", "You can't play real money tournament when your development settings are enable", "GOT IT!" };
    }
    public object[] prohibited_location()
    {
        return new object[] { "prohibited", "Location", "real money tournaments are not authorized in your territory.", "GOT IT!" };

    }
    public object[] download_from_store()
    {
        return new object[] { "DOWNLOAD", "FROM STORE", "To get all our features you need to download full version from our store", "DOWNLOAD" };
    }
    public object[] insufficient_balance()
    {
        return new object[] { "insufficient", "balance", "credit your account ?", "Credit" };
    }
    public object[] insufficient_bubbles()
    {
        return new object[] { "insufficient", "bubbles", "get free bubbles or watch a commercial to win more bubbles", "GOT IT!" };

    }
    public object[] AgeVerification()
    {
        return new object[] { "Age", "verification", "By clicking the continue button you acknowledge to have the legal age to play with real money in your country.", "confirm", "ENTER DATE" };
    }
    public object[] PopupPayment()
    {
        return new object[] { "SELECT", "PAYMENT METHOD", "SELECT", "DESELECT", "CONFIRM" };
    }
    public object[] Logout()
    {
        return new object[] { "DOWNLOAD", "FROM STORE", "To get all our features you need to download full version from our store", "DOWNLOAD" };
    }
    public object[] UpdatePassword()
    {
        return new object[] { "CHANGE", "PASSWORD", "NEW PASSWORD", "CONFIRM PASSWORD", "CONFIRM" };
    }
    public object[] Popup()
    {
        return new object[] { "DOWNLOAD", "FROM STORE", "To get all our features you need to download full version from our store", "DOWNLOAD" };
    }
    public object[] CurrentPassword()
    {
        return new object[] { "CHANGE", "PASSWORD", "CURRENT PASSWORD", "NEXT" };
    }
    public object[] ForgetPassword()
    {
        return new object[] { "FORGOTTEN", "PASSWORD", "ENTER EMAIL", "you will receive an email to define a new password", "SEND LINK" };
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
        return new object[] { "BUBBLES", "GREAT"};
    }
    public object[] Error()
    {
        return new object[] { "DOWNLOAD", "FROM STORE", "To get all our features you need to download full version from our store", "DOWNLOAD" };
    }
    public object[] SecureConfirmation()
    {
        return new object[] { "3D SECURE", "CONFIRMATION", "3D Secure authentication must be completed for the payment to be successful", "GO AHEAD" };
    }
    public object[] TooYoung()
    {
        return new object[] { "SORRY", "You are too young", "we apologize , real money tournaments are strictly regulated. keep having fun with bubbles", "Got it !" };
    }
    public object[] ProhibitedLocationWallet()
    {
        return new object[] { "prohibited", "Location", "Credit is not authorized in your territory.", "Got it !" };
    }
    public object[] ProhibitedLocationWithdraw()
    {
        return new object[] { "prohibited", "Location", "Withdrawal is not authorized in your territory.", "Got it !" };
    }
    public object[] PasswordUpdated()
    {
        return new object[] { "password", "changed", "Your password has been successfully updated", "Great !" };
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
