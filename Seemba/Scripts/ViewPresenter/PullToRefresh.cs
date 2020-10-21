using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using UnityEngine.SceneManagement;
public class PullToRefresh : MonoBehaviour {
	Vector2 firstPressPos;
	Vector2 secondPressPos;
	Vector2 currentSwipe;
	public float minSwipeDistY;
	public float minSwipeDistX;
	private Vector2 startPos;
	public ScrollRect scrollRect;
	public GameObject Content;
	public GameObject HomeController;
	public float ContentYini,ContentYCurrent,imgY;
	public Image image,anim;
	UnityEngine.Color __alpha;
	public GameObject ContentHome,PanelLastResults,PanelOngoing,WalletBloc;
	public static bool pullActivated;
	public static bool lastResultfinished, ongoingfinished, walletFinished;
	Vector3 pos;
	// Use this for initialization
	public void Start () {
		try{
		ContentYini = Content.transform.position.y;
		ContentYCurrent = ContentYini;
		 __alpha = image.color;
		}catch(NullReferenceException ex){
		}
		pos = Content.transform.position;
		imgY = pos.y;
	}
	// Update is called once per frame
	public void Anim() {
		if (!anim.IsActive()) {
			Animator a = anim.GetComponent<Animator> ();
			anim.gameObject.SetActive (true);
			a.gameObject.SetActive (true);
			UnityThreading.ActionThread thread;
					UserManager.CurrentUsername=null;
					refresh();
		    }
	}
	void refresh(){
		pullActivated = true;
		lastResultfinished = false;
		ongoingfinished = false;
		walletFinished = false;
		foreach (Transform child in PanelLastResults.transform) {
			DestroyImmediate (child.gameObject,true);
		}
		foreach (Transform child in PanelOngoing.transform) {
			DestroyImmediate (child.gameObject,true);
		}
		ContentHome.SetActive(false);
		ContentHome.SetActive(true);
		updateHeader ();
	}
	void updateHeader(){
		UserManager um = new UserManager ();
		string userid = um.getCurrentUserId ();
		string token = um.getCurrentSessionToken ();
		
        UnityThreadHelper.CreateThread (() => {
			User usr=um.getUser(userid,token);
			if(usr!=null) {
			Byte[] lnByte=um.getAvatar(usr.avatar);
			UnityThreadHelper.Dispatcher.Dispatch (() => {
				UserManager.CurrentUser.money_credit = usr.money_credit;
				UserManager.CurrentUser.bubble_credit = usr.bubble_credit;
				GameObject.Find("virtual_money").GetComponent<Text>().text= UserManager.CurrentUser.bubble_credit.ToString();
				GameObject.Find("solde_euro").GetComponent<Text>().text= UserManager.CurrentUser.money_credit.ToString("N2") + " "+CurrencyManager.CURRENT_CURRENCY;
                if (usr.money_credit>0) {
					GameObject.Find("Pro").transform.localScale=Vector3.one;
				}else{
					GameObject.Find("Pro").transform.localScale=Vector3.zero;
				}
                if(UserManager.CurrentAvatarURL!=usr.avatar) {
					Sprite avatar=ImagesManager.getSpriteFromBytes (lnByte);
					UserManager.CurrentAvatarBytesString=avatar;
					Image PlayerAvatar = GameObject.Find ("Avatar").GetComponent<Image> ();
					PlayerAvatar.sprite = avatar;
				}
                
                UserManager.CurrentUsername=usr.username;
				Text username = GameObject.Find ("Text_name").GetComponent<Text> ();
				username.text=usr.username;
				PullToRefresh.walletFinished = true;
                InvokeRepeating("hideAnimation",0f,0.5f);
			});
            } else{
                UnityThreadHelper.Dispatcher.Dispatch (() => {
				lastResultfinished= true;
				ongoingfinished = true;
				walletFinished = true;
                    InvokeRepeating("hideAnimation",0f,2f);
					HomeController.GetComponent<HomeController>().enabled=false;
					HomeController.GetComponent<HomeController>().enabled=true;
				});
			}
		});
	}
	void hideAnimation(){
        UnityThreadHelper.CreateThread (() => {
            Debug.Log("lastResultfinished:" + lastResultfinished);
            Debug.Log("ongoingfinished:" + ongoingfinished);
            Debug.Log("walletFinished:"+walletFinished);
			if (lastResultfinished != false || ongoingfinished != false || walletFinished != false) {
			UnityThreadHelper.Dispatcher.Dispatch (() => {
					CancelInvoke();
			Animator a = anim.GetComponent<Animator> ();
			a.gameObject.SetActive (false);
			anim.gameObject.SetActive (false);
			__alpha.a = 0f;
			image.color = __alpha;
			});
			}
		});
	}
	void Update () {
        if (__alpha.a >= 1f) {
			Anim ();
		} else {
			if (__alpha.a < 1f) {
				__alpha.a = (ContentYini - Content.transform.position.y);
				image.color = __alpha;
			}
			////Debug.Log("verticalNormalizedPosition: ");
			if (Input.GetMouseButtonDown (0)) {
				//save began touch 2d point
				firstPressPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			}
			if (Input.GetMouseButtonUp (0)) {
				//save ended touch 2d point
				secondPressPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				//create vector from the two points
				currentSwipe = new Vector2 (secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
				//normalize the 2d vector
				currentSwipe.Normalize ();
				//swipe upwards
				if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
				}
				//swipe down
				if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
				}
				//swipe left
				if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
				}
				//swipe right
				if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
				}
			}
			//#if UNITY_ANDROID
			if (Input.touchCount > 0) {
				Touch touch = Input.touches [0];
				switch (touch.phase) {
				case TouchPhase.Began:
					startPos = touch.position;
					break;
				case TouchPhase.Ended:
					float swipeDistVertical = (new Vector3 (0, touch.position.y, 0) - new Vector3 (0, startPos.y, 0)).magnitude;
					if (swipeDistVertical > minSwipeDistY) {
						float swipeValue = Mathf.Sign (touch.position.y - startPos.y);
						if (swipeValue > 0)//up swipe
						{}
						else if (swipeValue < 0)//down swipe
						{}
					}
					float swipeDistHorizontal = (new Vector3 (touch.position.x, 0, 0) - new Vector3 (startPos.x, 0, 0)).magnitude;
					if (swipeDistHorizontal > minSwipeDistX) {
						float swipeValue = Mathf.Sign (touch.position.x - startPos.x);
						if (swipeValue > 0)//right swipe
						//MoveRight ();
						{}
						else if (swipeValue < 0)//left swipe
						//MoveLeft ();
						{}
					}
					break;
				}
			}
		}
	}}
