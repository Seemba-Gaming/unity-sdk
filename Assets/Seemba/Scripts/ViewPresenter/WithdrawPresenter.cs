using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using System.Net;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine.SceneManagement;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
public class WithdrawPresenter : MonoBehaviour {
	public static float WithdrawMoney;
	public Text balance, TextEuro;
	public float bonus;
	UserManager um = new UserManager ();
    WithdrawManager withdrawManager = new WithdrawManager();
    public Button WithdrawButton;
	public InputField EnterWithdrawal;
	// Update is called once per frame
	void Update () {
        balance.text = float.Parse(UserManager.CurrentMoney).ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
	}
	// Use this for initialization
	void Start ()
	{

        balance = GameObject.Find ("balance").GetComponent<Text> ();
		//balance.text = UserManager.CurrentMoney+CurrencyManager.CURRENT_CURRENCY;
		UnityThreading.ActionThread thread;
		string userId = um.getCurrentUserId ();
		string token = um.getCurrentSessionToken();
		User user=null;
        string accountStatus=null;
        UnityThreadHelper.CreateThread (() => {
			//bonus = float.Parse (N ["argentBonus"].Value);
			 user = um.getUser (userId, token);
             accountStatus = withdrawManager.accountVerificationStatus(token);
            UnityThreadHelper.Dispatcher.Dispatch (() => {
				balance.text = user.money_credit.ToString("N2") + CurrencyManager.CURRENT_CURRENCY;
			});
		});
		WithdrawButton.onClick.AddListener (() => {
            Debug.Log("WithdrawButton.onClick");
			if(CountryController.checkCountry(UserManager.CurrentCountryCode)==true) {
						WithdrawMoney = float.Parse (EnterWithdrawal.text, CultureInfo.InvariantCulture);
                //if (/*!InformationAlreadyExist(user) || user.iban_uploaded == false ||*/ (user.id_proof_1_uploaded == false && user.passport_uploaded == false)) {
                if (accountStatus!= WithdrawManager.ACCOUNT_VERIFICATION_STATUS_VERIFIED)
                {
                    EventsController nbs=new EventsController();
						nbs.ShowPopupError("popupMissingInfo");
				} else {
                    try {
                        GameObject.Find("WithdrawalInfo").GetComponent<GameObject>();
                    } catch (NullReferenceException ex) {
                        Debug.Log("OpenUP");
                        SceneManager.LoadScene("WithdrawalInfo", LoadSceneMode.Additive);
                    }
				}
			}else{
				EventsController nbs=new EventsController();
				nbs.ShowPopupError("popupProhibitedLocationWithdraw");
			}
				});
		EnterWithdrawal.onValueChanged.AddListener (delegate {
			if (string.IsNullOrEmpty (EnterWithdrawal.text)) {
				WithdrawButton.interactable = false;
				TextEuro.text = "";
			} else {
				TextEuro.text = CurrencyManager.CURRENT_CURRENCY;
				if ((float.Parse (EnterWithdrawal.text, CultureInfo.InvariantCulture) > 0) && (float.Parse (EnterWithdrawal.text, CultureInfo.InvariantCulture) <= (float.Parse (UserManager.CurrentMoney)))) {
					WithdrawButton.interactable = true;
				} else {
					WithdrawButton.interactable = false;
				}
			}
		});
	}
	public bool InformationAlreadyExist(User user){
		return !string.IsNullOrEmpty (user.firstname) && !string.IsNullOrEmpty (user.lastname) && !string.IsNullOrEmpty (user.birthday) && !string.IsNullOrEmpty (user.adress) && !string.IsNullOrEmpty (user.city) && !string.IsNullOrEmpty (user.zipcode) && !string.IsNullOrEmpty (user.country);
	}
//
//
}
