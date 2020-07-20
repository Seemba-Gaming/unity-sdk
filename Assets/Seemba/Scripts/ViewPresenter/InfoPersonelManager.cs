using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using SimpleJSON;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using System.Net.Security;
using System.Threading;
using UnityEngine.Networking;
using System.Threading.Tasks;
public class InfoPersonelManager: MonoBehaviour {
	JSONNode N;
	public Button tryAgainConfirmingPayement, cancelConfirmPayement;
	public InputField username, LastName, FirstName, Adress, city, zip, state, country,personal_id_number, email, deposit, MaxWithdraw, AutoWithdrawal, OKDatePicker;
	public Button UpdatePassword, CreatePassword, password, DateOfBirth, DOF, ContinueBanking, ShowExpDatePicker;
	public InputField cardHolder, cardNumber, CVV, Zip;
	public Toggle TermsToggel;
	public static string SourceView;
	public static string HeaderText, FieldName;
	public static bool makeWithdraw = false;
	System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
	public UserManager usermanager;
	public EventsController eventsController;
	public Text Montant, TextExpDate, birthday, placeholderAge;
	public Dropdown Years, Month;
	private User usr;
	private Text _textDatePredefined; 
	public Text TextDatePredefined;
	private string _selectedDateString2 = "1995-09-15";
	private static string orderId;
	private static string forwardUrl;
	private static bool isBackAfterPayment;
	UserManager um = new UserManager();
	private string trasactionState;
	private String SelectedDateString2 {
		get {
			return _selectedDateString2;
		}
		set {
			_selectedDateString2 = value;
			//UpdateLabels();
			_textDatePredefined.text = SelectedDateString2;
			GameObject.Find("TextAge").transform.localScale=Vector3.zero;
			//birthday.text = _textDatePredefined.text;
            Debug.Log("birthdate :" + _textDatePredefined.text);
		}
	}
	// Use this for initialization
	void Start() {
		try {
			GameObject.Find("editable lastname").GetComponent < Image > ().color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
			GameObject.Find("editable firstname").GetComponent < Image > ().color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
			GameObject.Find("editable adress").GetComponent < Image > ().color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
			GameObject.Find("editable city").GetComponent < Image > ().color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
			GameObject.Find("editable zipCode").GetComponent < Image > ().color = new Color(246 / 255f, 123 / 255f, 50 / 255f); 	
			GameObject.Find("editable country").GetComponent < Image > ().color = new Color(246 / 255f, 123 / 255f, 50 / 255f);
		} catch (Exception e) {}
		try {
			tryAgainConfirmingPayement.onClick.AddListener(() => {
				isBackAfterPayment = true;
				SceneManager.LoadScene("Loader", LoadSceneMode.Additive);
				//Debug.Log("load Loader");
				//Debug.Log("confirm payement again");
				UnityThreadHelper.Dispatcher.Dispatch(() => {
					Application.OpenURL(forwardUrl);
				});
				GameObject.Find("popupPaymentNotConfirmed").transform.localScale = Vector3.zero;
			});
		} catch (NullReferenceException) {
		}
		try {
			cancelConfirmPayement.onClick.AddListener(() => {
				GameObject.Find("popupPaymentNotConfirmed").transform.localScale = Vector3.zero;
			});
		} catch (NullReferenceException) {
		}
		isBackAfterPayment = false;
		usermanager = new UserManager();
		string userId = usermanager.getCurrentUserId();
		string Token = usermanager.getCurrentSessionToken();
		eventsController = new EventsController();
		UnityThreading.ActionThread thread;
		try {
			Montant.text = WalletScript.LastCredit.ToString("N2").Replace(",", ".") + CurrencyManager.CURRENT_CURRENCY;
			TermsToggel.onValueChanged.AddListener(delegate {
				if (TermsToggel.isOn == true) {
					ContinueBanking.interactable = true;
				} else
					ContinueBanking.interactable = false;
			});
		} catch (NullReferenceException) {
		}
	}
	public  void showExpPicker(UnityEngine.Object button) {
		_textDatePredefined = GameObject.Find("AgeInfoPers").GetComponent < Text > ();
        if (PersonelInfoController.getInstance().Age_Containter.GetComponent<Animator>().GetBool("young") == true)
        {
            PersonelInfoController.getInstance().Age_Containter.GetComponent<Animator>().SetBool("young", false);
        }
        NativePicker.Instance.ShowDatePicker(GetScreenRect(button as GameObject), NativePicker.DateTimeForDate(2012, 12, 23), async (long val) => {
			SelectedDateString2 = NativePicker.ConvertToDateTime(val).ToString("yyyy-MM-dd");
			_textDatePredefined.text = SelectedDateString2;
            Debug.Log("showExpPicker: birthdate: " + _textDatePredefined.text);
            string[] attrib = { "birthdate" };
            string[] values = { _textDatePredefined.text };
            string userId = um.getCurrentUserId();
            string token = um.getCurrentSessionToken();
            string[] date = _textDatePredefined.text.Split(new char[] { '-' }, 3);
            string Years = date[0];
            string Days = date[2];
            string Months = date[1];
            Debug.Log("Years: " + Years);
            if (DateTime.UtcNow.Year - int.Parse(Years) >= 18)
            {
                    if (await PersonelInfoController.getInstance().updateStripeAccount("birthdate", _textDatePredefined.text))
                        PersonelInfoController.getInstance().updateUser(attrib, values);
            }
            else
            {
                // Age <18 : Show Error Text
                PersonelInfoController.getInstance().Age_Containter.GetComponent<Animator>().SetBool("young", true);
            }
        }, () => {
			//SelectedDateString2 = "canceled";
		});
	}
	private String SelectedDateString {
		get {
			return _selectedDateString2;
		}
		set {
			_selectedDateString2 = value;
			//UpdateLabels();
			TextExpDate.text = SelectedDateString;
		}
	}
	private Rect GetScreenRect(GameObject gameObject) {
		RectTransform transform = gameObject.GetComponent < RectTransform > ();
		Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
		Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
		rect.x -= (transform.pivot.x * size.x);
		rect.y -= ((1.0f - transform.pivot.y) * size.y);
		return rect;
	}
	// Update is called once per frame
	void Update() {
		try {
			if (!string.IsNullOrEmpty(UserManager.currentBirthdate))
			{	
				placeholderAge.transform.localScale=Vector3.zero;
				TextDatePredefined.text=UserManager.currentBirthdate;
				string userId = usermanager.getCurrentUserId();
				string token = usermanager.getCurrentSessionToken();
				string[] attrib = { "birthdate" };
				string[] value5 = { TextDatePredefined.text};
				um.UpdateUserByField (userId, token, attrib, value5);
				UserManager.currentBirthdate=null;
			}
		} catch (NullReferenceException ex) {
		}
		try {
			if (LastName.isFocused == true) {
				LastName.textComponent.color = new Color(1, 1, 1);
				LastName.placeholder.color = new Color(1, 1, 1);
				GameObject.Find("editable lastname").GetComponent < Image > ().color = new Color(1, 1, 1);
			}
			if (FirstName.isFocused == true) {
				FirstName.textComponent.color = new Color(1, 1, 1);
				FirstName.placeholder.color = new Color(1, 1, 1);
				GameObject.Find("editable firstname").GetComponent < Image > ().color = new Color(1, 1, 1);
			}
			if (Adress.isFocused == true) {
				Adress.textComponent.color = new Color(1, 1, 1);
				Adress.placeholder.color = new Color(1, 1, 1);
				GameObject.Find("editable adress").GetComponent < Image > ().color = new Color(1, 1, 1);
			}
			if (city.isFocused == true) {
				city.textComponent.color = new Color(1, 1, 1);
				city.placeholder.color = new Color(1, 1, 1);
				GameObject.Find("editable city").GetComponent < Image > ().color = new Color(1, 1, 1);
			}
			if (zip.isFocused == true) {
				zip.textComponent.color = new Color(1, 1, 1);
				zip.placeholder.color = new Color(1, 1, 1);
				GameObject.Find("editable zipCode").GetComponent < Image > ().color = new Color(1, 1, 1);
			}
			/*if(state.isFocused ==true){
			state.textComponent.color = new Color (1,1,1);
			state.placeholder.color = new Color (1,1,1);
			GameObject.Find ("editable state").GetComponent<Image>().color=new Color (1,1,1);
		}*/
			if (country.isFocused == true) {
				country.textComponent.color = new Color(1, 1, 1);
				country.placeholder.color = new Color(1, 1, 1);
				GameObject.Find("editable country").GetComponent < Image > ().color = new Color(1, 1, 1);
			}
		} catch (NullReferenceException) {
		}
//		try{
//			if(!string.IsNullOrEmpty(cardHolder.text)&&!string.IsNullOrEmpty(cardNumber.text)&&!string.IsNullOrEmpty(CVV.text)&&valueYears
//		}catch(NullReferenceException ex){
//		}
	}
	void HipayConfig(string card_number, string cvc, string card_expiry_month, string card_expiry_year, string card_holder, string amount, string token, string userId) {
		UserManager um = new UserManager();
		string url = Endpoint.classesURL + "/payments/charge/"+userId;
		ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
		HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
		request.Method = "POST";
		request.Headers["x-access-token"] = token;
		request.Headers["Access-Control-Request-Headers"] = "Content-Type";
		request.Headers["Access-Control-Request-Headers"] = "Authorization";
		request.ContentType = @"application/json";
		using(var stream = request.GetRequestStream()) {
			byte[] jsonAsBytes = Encoding.UTF8.GetBytes("{\"card_number\":\"" + card_number + "\",\"cvc\":\"" + cvc + "\",\"card_expiry_month\":\"" + card_expiry_month + "\",\"card_expiry_year\":\"" + card_expiry_year + "\",\"card_holder\":\"" + card_holder.ToUpper() + "\",\"amount\":\"" + amount + "\",\"user_id\":\"" + userId + "\"}");
			stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
		}
		try {
			using(HttpWebResponse response = (HttpWebResponse) request.GetResponse()) {
				////Debug.Log ("Line: 822");
				System.IO.Stream s = response.GetResponseStream();
				using(System.IO.StreamReader sr = new System.IO.StreamReader(s)) {
					//	//Debug.Log ("Line: 824");
					var jsonResponse = sr.ReadToEnd();
					//Debug.Log(jsonResponse);
					N = JSON.Parse(jsonResponse);
					try {
						//Get the order id to check payment confirmation
						orderId = N["details"]["order"]["id"].Value;
						//TODO TO Verify
						if (UserManager.CurrentHipayOrderId != null && UserManager.CurrentHipayOrderId != orderId) {
							UserManager.CurrentHipayOrderId = orderId;
						}
						//Debug.Log("forwardUrl: " + N["forwardUrl"].Value);
						forwardUrl = N["details"]["forwardUrl"].Value;
						if (!string.IsNullOrEmpty(N["details"]["forwardUrl"].Value)) {
							//Open Browser to verify URL
							UnityThreadHelper.Dispatcher.Dispatch(() => {
								isBackAfterPayment = true;
								Application.OpenURL(N["details"]["forwardUrl"].Value);
							});
							/*-----------------------------------------------------------------*/
							//checkStatus(orderId);
							/*-----------------------------------------------------------------*/
						} else {
							//Debug.Log("state: " + N["details"]["state"].Value);
							if (N["details"]["state"].Value != "declined") {
								//Debug.Log("before notification ......");
								/*-----------------------------------------------------------------*/
								//Get Server-to-server Notification from hipay to confirm transaction
								/*-----------------------------------------------------------------*/
								Thread.Sleep(2000);
								//Debug.Log("after thread sleeping ......");
								var notification = getNotificationfromServer(N["details"]["order"]["id"].Value);
								//Debug.Log("notification state: " + notification["data"]["state"].Value);
								if (notification["data"]["state"].Value == "completed") {
									UnityThreadHelper.Dispatcher.Dispatch(() => {
										//Debug.Log("user argent after float parse " + usr.money_credit);
										//Debug.Log("WalletScript.LastCredit " + WalletScript.LastCredit);
										//Debug.Log("CurrentMoney " + (usr.money_credit + WalletScript.LastCredit).ToString("N2").Replace(",", "."));
										// Payment Completed                   
										orderId = null;
										isBackAfterPayment = false;
										UserManager.CurrentMoney = (usr.money_credit + WalletScript.LastCredit).ToString("N2").Replace(",", ".");
										//TODO
										ViewsEvents viewEvents = new ViewsEvents();
                                        viewEvents.WinMoneyClick();
										eventsController.ShowPopup("popupCongrat");
										Text TextMain = GameObject.Find("TextMain").GetComponent < Text > ();
										TextMain.text = WalletScript.LastCredit.ToString("N2").Replace(",", ".") + CurrencyManager.CURRENT_CURRENCY;
										SceneManager.UnloadScene("BankingInformation");
									});
								} else if (notification["data"]["state"].Value == "declined") {
									UnityThreadHelper.Dispatcher.Dispatch(() => {
										//turn back to trophy with Ouups popup
										TurnBackToTrophy();
									});
								} else {
									if (notification["data"]["state"].Value == "pending") {
										if (notification["data"]["message"].Value == "Risk Challenged") {
											UnityThreadHelper.Dispatcher.Dispatch(() => {
												GameObject.Find("popupPaymentWaitingForApproval").transform.localScale = Vector3.zero;
												//turn back to trophy with Ouups popup
												TurnBackToTrophy();
											});
										}
									}
								}
							} else if (N["details"]["state"].Value == "declined") {
								UnityThreadHelper.Dispatcher.Dispatch(() => {
									//turn back to trophy with Ouups popup
									TurnBackToTrophy();
								});
							}
						}
					} catch (Exception e) {
						UnityThreadHelper.Dispatcher.Dispatch(() => {
							//turn back to trophy with Ouups popup
							//Debug.Log(e);
							TurnBackToTrophy();
						});
					}
				}
			}
		} catch (WebException ex) {
			//Debug.Log(ex);
			UnityThreadHelper.Dispatcher.Dispatch(() => {
				//turn back to trophy with Ouups popup
				TurnBackToTrophy();
			});
		}
	}
	private void TurnBackToTrophy() {
		ViewsEvents viewEvents = new ViewsEvents();
        viewEvents.WinMoneyClick();
		SceneManager.UnloadScene("BankingInformation");
		eventsController.ShowPopupError("popupOups");
		//Debug.Log("turn back to trophy with Ouups popup");
	}
	public JSONNode getNotificationfromServer(string orderId) {
		UserManager um = new UserManager();
		string url = Endpoint.classesURL + "/payments/" + orderId;
		HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
		request.Method = "GET";
		try {
			//Debug.Log("Line:1013 NOTIFICATION ");
			HttpWebResponse response;
			using(response = (HttpWebResponse) request.GetResponse()) {
				System.IO.Stream s = response.GetResponseStream();
				using(System.IO.StreamReader sr = new System.IO.StreamReader(s)) {
					var jsonResponse = sr.ReadToEnd();
					//Debug.Log(jsonResponse);
					var N = JSON.Parse(jsonResponse);
					return N;
				}
			}
		} catch (WebException ex) {
			//Debug.Log("Line:1005 catch NOTIFICATION ");
			return null;
		}
	}
	public string checkStatus(string orderId,string token) {
		UserManager um = new UserManager();
		string url = Endpoint.classesURL + "/payments/checkstatus/" + orderId;
		HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
		request.Method = "GET";
		request.Headers["x-access-token"] = token;
		try {
			//Debug.Log("Line 1074");
			HttpWebResponse response;
			using(response = (HttpWebResponse) request.GetResponse()) {
				System.IO.Stream s = response.GetResponseStream();
				using(System.IO.StreamReader sr = new System.IO.StreamReader(s)) {
					var jsonResponse = sr.ReadToEnd();
					//Debug.Log("check status json: ");
					//Debug.Log(jsonResponse);
					var N = JSON.Parse(jsonResponse);
					return N["data"]["transaction"]["state"].Value;
				}
			}
		} catch (WebException ex) {
			//Debug.Log("Line 1088: "+ ex);
			return null;
		}
	}
    public string checkStatusss(string orderId) {
		//string url = "https://stage-secure-gateway.hipay-tpp.com/rest/v1/transaction?orderid="+orderId;
		string url = "http://seembaapi.herokuapp.com/api/functions/checkStatus";
		//Debug.Log("url: " + url);
		HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
		request.Method = "POST";
		request.Headers["X-Parse-Application-Id"] = "seembaapi";
		request.Headers["Access-Control-Request-Headers"] = "Content-Type";
		request.Headers["Access-Control-Request-Headers"] = "Authorization";
		request.ContentType = @"application/json";
		using(var stream = request.GetRequestStream()) {
			byte[] jsonAsBytes = encoding.GetBytes("{\"orderId\":\"" + orderId + "\"}");
			stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
		}
		try {
			HttpWebResponse response;
			using(response = (HttpWebResponse) request.GetResponse()) {
				System.IO.Stream s = response.GetResponseStream();
				using(System.IO.StreamReader sr = new System.IO.StreamReader(s)) {
					var jsonResponse = sr.ReadToEnd();
					//Debug.Log(jsonResponse);
					try {
						string json = jsonResponse.Substring(15);
						json = json.Remove(json.Length - 1);
						var N = JSON.Parse(json);
						//Debug.Log("state: " + N["state"].Value);
						return N["state"].Value;
					} catch (Exception e) {
						return null;
					}
				}
			}
		} catch (WebException ex) {
			if (ex.Response != null) {
				using(var errorResponse = (HttpWebResponse) ex.Response) {
					using(var reader = new StreamReader(errorResponse.GetResponseStream())) {
						string error = reader.ReadToEnd();
						//Debug.Log(error);
						return null;
					}
				}
			}
			return null;
		}
	}
	public bool MyRemoteCertificateValidationCallback(System.Object sender,
		X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
		bool isOk = true;
		// If there are errors in the certificate chain,
		// look at each error to determine the cause.
		if (sslPolicyErrors != SslPolicyErrors.None) {
			for (int i = 0; i < chain.ChainStatus.Length; i++) {
				if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown) {
					continue;
				}
				chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
				chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
				chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
				chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
				bool chainIsValid = chain.Build((X509Certificate2) certificate);
				if (!chainIsValid) {
					isOk = false;
					break;
				}
			}
		}
		return isOk;
	}
	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus) {
			//android onPause()
		} else {
			//android onResume()
			//Debug.Log("OnResume");
			// is Back after payement ? 
			UnityThreading.ActionThread myThread;
			string token=um.getCurrentSessionToken();
			myThread = UnityThreadHelper.CreateThread(() => {
				//Debug.Log("check status");
				string trasactionState = checkStatus(orderId,token);
				//
				//				UnityThreadHelper.Dispatcher.Dispatch (() => {     
				//					try {
				//						SceneManager.UnloadScene ("Loader");
				//					} catch (Exception ex) {
				//					}
				//				});
				if (isBackAfterPayment == true) {
					//Debug.Log("orderId " + orderId);
					//Debug.Log("trasactionState " + trasactionState);
					if (!string.IsNullOrEmpty(orderId)) {
						//Debug.Log("Line:1184 ");
						if (!string.IsNullOrEmpty(trasactionState)) {
							//Debug.Log("Line:1189 ");
							if (trasactionState == "completed") {
								//Debug.Log("Line:1196");
								//Debug.Log("Line:1197");
								Thread.Sleep(6000);
								var notification = getNotificationfromServer(N["details"]["order"]["id"]);
								//Debug.Log("Line:1202: " + notification);
								if (notification["data"]["state"].Value == "completed") {
									//Debug.Log("Line:1205:");
									UnityThreadHelper.Dispatcher.Dispatch(() => {
                                        // Payment Completed 
										orderId = null;
										isBackAfterPayment = false;
										//	SceneManager.UnloadScene ("Loader");
										UserManager.CurrentMoney = (usr.money_credit + WalletScript.LastCredit).ToString("N2").Replace(",", ".");
										ViewsEvents viewEvents = new ViewsEvents();
                                        viewEvents.WinMoneyClick();
										EventsController nbs = new EventsController();
										nbs.ShowPopup("popupCongrat");
										//GameObject.Find ("popupCongrat").transform.localScale = Vector3.one;
										Text TextMain = GameObject.Find("TextMain").GetComponent < Text > ();
										TextMain.text = WalletScript.LastCredit.ToString("N2").Replace(",", ".") + CurrencyManager.CURRENT_CURRENCY;
										try {
											SceneManager.UnloadScene("Loader");
										} catch (Exception ex) {}
										SceneManager.UnloadScene("BankingInformation");
									});
								}
							} else if (trasactionState == "forwarding") {
								//Debug.Log("Line:1252:");
								UnityThreadHelper.Dispatcher.Dispatch(() => {
									try {
										SceneManager.UnloadScene("Loader");
									} catch (Exception ex) {}
									//Debug.Log("please confirm your payment");
									GameObject.Find("popupPaymentWaitingForApproval").transform.localScale = Vector3.zero;
									GameObject.Find("popupPaymentNotConfirmed").transform.localScale = Vector3.one;
								});
							} else if (trasactionState == "declined") {
								//Debug.Log("Line:1265:");
								UnityThreadHelper.Dispatcher.Dispatch(() => {
									//SceneManager.UnloadScene ("Loader");
									try {
										SceneManager.UnloadScene("Loader");
									} catch (Exception ex) {}
									GameObject.Find("popup_payment").transform.localScale = Vector3.zero;
									ViewsEvents viewEvents = new ViewsEvents();
                                    viewEvents.WinMoneyClick();
									SceneManager.UnloadScene("BankingInformation");
									eventsController.ShowPopupError("popupOups");
									//Debug.Log("turn back to trophy with Ouups popup");
								});
							} else {
							}
						} else {
							//Debug.Log("Line:1286:");
							UnityThreadHelper.Dispatcher.Dispatch(() => {
								ViewsEvents viewEvents = new ViewsEvents();
                                viewEvents.WinMoneyClick();
								try {
									SceneManager.UnloadScene("Loader");
								} catch (Exception ex) {}
								SceneManager.UnloadScene("BankingInformation");
								eventsController.ShowPopupError("popupOups");
							});
						}
					}
				}
			});
		}
	}
	public void showDatePicker (UnityEngine.Object button)
	{
		string UserId = um.getCurrentUserId ();
		string UserToken = um.getCurrentSessionToken ();
		try{	
			_textDatePredefined = GameObject.Find ("textDatePredefined").GetComponent<Text> ();
		}catch(NullReferenceException ex){}
		NativePicker.Instance.ShowDatePicker (GetScreenRect (button as GameObject), NativePicker.DateTimeForDate (2012, 12, 23), (long val) => {
			SelectedDateString2 = NativePicker.ConvertToDateTime (val).ToString ("yyyy-MM-dd");
			////Debug.Log("SelectedDateString2: "+SelectedDateString2);
		}, () => {
			//SelectedDateString2 = "canceled";
			SelectedDateString2 = DateTime.Now.ToString ("yyyy-MM-dd");
			try{
				//_textDatePredefined.text=SelectedDateString2;
			}catch(NullReferenceException ex){}
		});
	}
}