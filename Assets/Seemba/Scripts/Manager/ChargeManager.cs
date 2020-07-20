using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using SimpleJSON;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System;
using UnityEngine.UI;
public class ChargeManager : MonoBehaviour {
    public const string PAYMENT_TYPE = "card";
    public const string PAYMENT_STATUS_REQUIRES_SOURCE_ACTION = "requires_source_action";
    public const string PAYMENT_STATUS_REQUIRES_PAYMENT_METHOD = "requires_payment_method";
    public const string PAYMENT_STATUS_REQUIRES_ACTION = "requires_action";
    public const string PAYMENT_STATUS_SUCCEEDED = "succeeded";
    public const string PAYMENT_STATUS_CANCELED = "canceled";
    public const string PAYMENT_STATUS_PROCESSING = "processing";
    public const string PAYMENT_NEXT_ACTION_TYPE_USE_STRIPE_SDK = "use_stripe_sdk";
    public const string PAYMENT_NEXT_ACTION_TYPE_REDIRECT_TO_URL = "redirect_to_url"; 
    public JSONNode Charge(string card_number, string cvc, string card_expiry_month, string card_expiry_year, string card_holder, float amount, string token, string userId){
		UserManager um = new UserManager ();
		string url = Endpoint.classesURL + "/payments/charge/"+userId;
		ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
		JSONNode res;
		request.Method = "POST";
		request.Headers["x-access-token"]=token;
		request.ContentType = "application/x-www-form-urlencoded";
		string json;
		using (var stream = request.GetRequestStream ()) {   
			//Amount en Cent
			json = "card_number=" + card_number + "&card_expiry_month=" + card_expiry_month +"&card_expiry_year="+card_expiry_year+"&cvc="+cvc+ "&amount=" + amount*100;
			byte[] jsonAsBytes = Encoding.UTF8.GetBytes (json);
			stream.Write (jsonAsBytes, 0, jsonAsBytes.Length);
		}
		try {
			HttpWebResponse response;
			using (response = (HttpWebResponse)request.GetResponse ()) {
				System.IO.Stream s = response.GetResponseStream ();
				using (System.IO.StreamReader sr = new System.IO.StreamReader (s)) {
					var jsonResponse = sr.ReadToEnd ();
					//Debug.Log (jsonResponse);
					res = JSON.Parse (jsonResponse);
					if(res["success"].AsBool==false){
						return null;
					}else{
						return res;
					}
				}
			}
		} catch (WebException ex) {
			if (ex.Response != null) {
				using (var errorResponse = (HttpWebResponse)ex.Response) {
					using (var reader = new StreamReader (errorResponse.GetResponseStream ())) {
						string error = reader.ReadToEnd ();
						//Debug.Log (error);
					}
				}
			}
			return null;
		}
	}
    public string createPaymentMethod(string card_number, string cvc, int card_expiry_month, int card_expiry_year)
    {
        UserManager um = new UserManager();
        string url = Endpoint.stripeURL + "/payment_methods";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        JSONNode res;
        request.Method = "POST";
        request.Headers["Authorization"] = "Bearer " + Endpoint.TokenizationAccount;
        request.ContentType = "application/x-www-form-urlencoded";
        string json;
        using (var stream = request.GetRequestStream())
        {
            Debug.Log("card[exp_month]:" + card_expiry_month + " card[exp_year]:" + card_expiry_year);
            //Amount en Cent
            json = "card[number]=" + card_number + "&card[exp_month]=" + card_expiry_month + "&card[exp_year]=" + card_expiry_year + "&card[cvc]=" + cvc + "&type=" + PAYMENT_TYPE;
            byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);
            stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
        }
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log(jsonResponse);
                    res = JSON.Parse(jsonResponse);
                    Debug.Log("id:"+res["id"].Value);
                    return res["id"].Value;
                    /*if (res["success"].AsBool == false)
                    {
                        return null;
                    }
                    else
                    {
                        return res;
                    }*/
                }
            }
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        Debug.Log (error);
                    }
                }
            }
            return null;
        }
    }
    public JSONNode createPaymentIntent(string _paymentMethod,float amount,string token)
    {
        Debug.Log("createPaymentIntent");
        UserManager um = new UserManager();
        string url = Endpoint.classesURL + "/payments/charge/";
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        JSONNode res;
        request.Method = "POST";
        request.Headers["Authorization"] = "Bearer " + Endpoint.TokenizationAccount;
        request.Headers["x-access-token"] = token;
        request.ContentType = "application/x-www-form-urlencoded";
        string json;
        using (var stream = request.GetRequestStream())
        {
            //Amount en Cent
            json = "payment_method="+_paymentMethod+"&amount=" + amount*100;
            byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);
            stream.Write(jsonAsBytes, 0, jsonAsBytes.Length);
        }
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Debug.Log(jsonResponse);
                    res = JSON.Parse(jsonResponse);
                    return res;
                }
            }
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        Debug.Log (error);
                    }
                }
            }
            return null;
        }
    }
    public string isChargeConfirmed(string _paymentIntent,string token) {
		UserManager um = new UserManager();
		string url = Endpoint.classesURL +"/payments/charge/" + _paymentIntent;
		HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
		request.Method = "GET";
        request.Headers["x-access-token"] = token;
        try {
			HttpWebResponse response;
			using(response = (HttpWebResponse) request.GetResponse()) {
				System.IO.Stream s = response.GetResponseStream();
				using(System.IO.StreamReader sr = new System.IO.StreamReader(s)) {
					var jsonResponse = sr.ReadToEnd();
					Debug.Log(jsonResponse);
					var N = JSON.Parse(jsonResponse);
				    Debug.Log("status: " + N["data"]["status"].Value);
					return N["data"]["status"].Value;
				}
			}
		} catch (WebException ex) {
            if (ex.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)ex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        Debug.Log(error);
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
}
