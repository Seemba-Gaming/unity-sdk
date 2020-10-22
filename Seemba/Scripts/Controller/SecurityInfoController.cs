using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Threading;
public class SecurityInfoController : MonoBehaviour {
	public Text email;
	public InputField CurrentPassword, NewPassword, ConfirmPassword;
	public Button ChangePassword,UpdatePassword, nextCurrentPassword, greatPasswordChanged;
	UserManager um=new UserManager();
	EventsController eventsController;
	User usr;
	// Use this for initialization
	void Start () {
		string userId = um.getCurrentUserId ();
		string token = um.getCurrentSessionToken ();
		eventsController = new EventsController ();
		UnityThreadHelper.CreateThread(() => {
			//usr=um.getUser(userId,token);
			usr = UserManager.CurrentUser;
			UnityThreadHelper.Dispatcher.Dispatch(() => {
				CurrentPassword.onValueChanged.AddListener(delegate {
				GameObject.Find("LoadingInCurrentPassword").transform.localScale = Vector3.one;
				nextCurrentPassword.interactable = false;
				UnityThreadHelper.CreateThread(() => {
						string res =um.logingIn(usr.email, CurrentPassword.text);
					if (res=="success") {
						UnityThreadHelper.Dispatcher.Dispatch(() => {
							nextCurrentPassword.interactable = true;
						});
					} else {
						UnityThreadHelper.Dispatcher.Dispatch(() => {
							nextCurrentPassword.interactable = false;
						});
					}
					UnityThreadHelper.Dispatcher.Dispatch(() => {
						GameObject.Find("LoadingInCurrentPassword").transform.localScale = Vector3.zero;
					});
				});
			});
			NewPassword.onValueChanged.AddListener(delegate {
				if (NewPassword.text == ConfirmPassword.text) {
					UpdatePassword.interactable = true;
				} else {
					UpdatePassword.interactable = false;
				}
			});
			ConfirmPassword.onValueChanged.AddListener(delegate {
					if (NewPassword.text == ConfirmPassword.text) {
					UpdatePassword.interactable = true;
				} else {
					UpdatePassword.interactable = false;
				}
			});
				try {
					//Debug.Log("usr.email "+usr.email.Substring(0, 4));
					email.text = usr.email.Substring(0, 4);
					for (int i = 0; i < usr.email.Length; i++) {
						email.text = email.text + "*";
					}
					ChangePassword.onClick.AddListener(() => {
						eventsController.ShowPopup("popupCurrentPassword");
					});
					nextCurrentPassword.onClick.AddListener(() => {
						eventsController.HidePopup("popupCurrentPassword", false);
						UnityThreadHelper.CreateThread (() => {
							Thread.Sleep(650);
							UnityThreadHelper.Dispatcher.Dispatch (() => {
								eventsController.ShowPopup("popupUpdatePassword");
								CurrentPassword.text = "";
								nextCurrentPassword.interactable = false;
							});
						});
					});
				} catch (NullReferenceException ex) {
					//Debug.Log (ex);
					try {
						SceneManager.UnloadScene("Loader");
					} catch (ArgumentException e) {}
					//GameObject.Find ("Loading").transform.localScale = new Vector3 (0, 0, 0);
				}
			});
		});
	}
	// Update is called once per frame
	void Update () {
	}
}
