using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
using System;
public class IDProofPresenter : MonoBehaviour {
	UserManager um=new UserManager();
    WithdrawManager wm =new WithdrawManager();
    string userId,userToken;
	public Button ContinuePopup;
	//Button Confirm;
	// Use this for initialization
	void Start () {
		ContinuePopup.onClick.AddListener (() => {
			missingInfoContinue();
		});
		userId = um.getCurrentUserId ();
		userToken = um.getCurrentSessionToken ();
		SceneManager.LoadScene ("Loader", LoadSceneMode.Additive);
		UnityThreadHelper.CreateThread (() => {
			User user = um.getUser (userId, userToken);
            var account=wm.accountVerificationJSON(userToken);
            UnityThreadHelper.Dispatcher.Dispatch (() => {
                SceneManager.UnloadSceneAsync ("Loader");
                if (string.IsNullOrEmpty(user.birthday) || string.IsNullOrEmpty(user.adress) || string.IsNullOrEmpty(user.firstname) || string.IsNullOrEmpty(user.lastname) || string.IsNullOrEmpty(user.zipcode) || string.IsNullOrEmpty(user.city) || string.IsNullOrEmpty(user.phone))
                {
                    showMissingInfoPopup();
                }
                else
                {
                    string documentFrontID = null; 
                    string documentBackID = null;
                    string documentAddress = null;
                    var accountStatus = account["account"]["individual"]["verification"]["status"].Value;
                    Debug.Log(accountStatus);
                    try { 
                     documentFrontID = account["account"]["individual"]["verification"]["document"]["front"].Value;
                    }
                    catch (NullReferenceException ex) {}
                    try
                    {
                    documentBackID = account["account"]["individual"]["verification"]["document"]["back"].Value;
                     }
                    catch (NullReferenceException ex) { }
                    try
                    {
                        documentAddress = account["account"]["individual"]["verification"]["additional_document"]["front"].Value;
                    }
                    catch (NullReferenceException ex) { }
                    if (accountStatus.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_VERIFIED))
                    {
                        show("IDAddressSuccess");
                        if (!string.IsNullOrEmpty(documentBackID))
                        {
                            show("IDFrontSuccess");
                            show("IDBackSuccess");
                            closeInteractable("IDPassport");
                            disable("IDPassport");
                        }
                        else
                        {
                            show("IDPassportSuccess");
                            closeInteractable("IDFront");
                            closeInteractable("IDBack");
                            disable("IDFront");
                            disable("IDBack");
                        }
                    }
                    if (accountStatus.Equals(WithdrawManager.ACCOUNT_VERIFICATION_STATUS_PENDING))
                    {
                        show("IDAddressSuccess");
                        if (!string.IsNullOrEmpty(documentBackID))
                        {
                            show("IDFrontWaiting");
                            show("IDBackWaiting");
                            closeInteractable("IDPassport");
                            disable("IDPassport");
                        }
                        else { 
                            show("IDPassportWaiting");
                            closeInteractable("IDFront");
                            closeInteractable("IDBack");
                            disable("IDFront");
                            disable("IDBack");
                        }
                    }
                    if (!string.IsNullOrEmpty(documentBackID)&&user.id_proof_2_uploaded)
                    {
                        show("IDBackWaiting");
                        closeInteractable("IDPassport");
                        disable("IDPassport");
                    }
                    if (!string.IsNullOrEmpty(documentFrontID)&& user.id_proof_1_uploaded)
                    {
                        show("IDFrontWaiting");
                        closeInteractable("IDPassport");
                        disable("IDPassport");
                    }
                    if (!string.IsNullOrEmpty(documentBackID) && user.passport_uploaded)
                    {
                        show("IDPassportWaiting");
                        closeInteractable("IDFront");
                        closeInteractable("IDBack");
                        disable("IDFront");
                        disable("IDBack");
                    }
                }
			});
		});
    }
    void closeInteractable(string button)
    {
        GameObject.Find(button).GetComponent<Button>().interactable = false;
    }
    void show(string _object)
    {
        GameObject.Find(_object).GetComponent<Image>().transform.localScale = Vector3.one;
    }
    void disable(string doc){
		Image DocDisableimg=GameObject.Find (doc+"/disable").GetComponent<Image> ();
		var tempColor = DocDisableimg.color;
          tempColor.a = 0.50f;
          DocDisableimg.color = tempColor;
	}
	void showMissingInfoPopup(){
		GameObject.Find ("CalqueIdProof").transform.localScale = Vector3.one;
		var animator = GameObject.Find("popupMissing").GetComponent<Animator>();
		animator.SetBool("Show Error",true);
	}
	void missingInfoContinue ()
	{	
		var animator = GameObject.Find("popupMissing").GetComponent<Animator>();
		GameObject.Find ("CalqueIdProof").transform.localScale = Vector3.zero;
		animator.SetBool("Show Error",false);
		UnityThreading.ActionThread thread;
		thread = UnityThreadHelper.CreateThread (() => {
			Thread.Sleep(300);
			UnityThreadHelper.Dispatcher.Dispatch (() => {
				SceneManager.UnloadScene ("IDProof");
				SceneManager.LoadScene ("PersonalInfo", LoadSceneMode.Additive);
			});
		});
	}
	// Update is called once per frame
	void Update () {
	}
}
