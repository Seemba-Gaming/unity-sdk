using UnityEngine;
using System.Net;
using UnityEngine.SceneManagement;
using System;
public class ConnectivityPresenter : MonoBehaviour {
    ConnectivityController connectivity;
    public static bool beginPing;
	private bool isLoader=false;
	public GameObject Home;
	void OnEnable() {
		connectivity=new ConnectivityController();
		if (beginPing == true) {
			check_connection ();
		} else {
			InvokeRepeating ("checkStartPinging",0f, 1f);
		}
	}
	void checkStartPinging(){
		if (beginPing == true) {
			CancelInvoke ();
			check_connection ();
		}
	}
	public void check_connection(){
		Debug.Log("check");
		SceneManager.LoadSceneAsync ("Loader", LoadSceneMode.Additive);
		isLoader = true;
		InvokeRepeating ("ping",0f, 1f);
	}
	public void ping() {
		UserManager um = new UserManager();
		string url = "https://www.google.fr";
		HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
		request.Method = "GET";
		request.Timeout = 3000;
		try {
			HttpWebResponse response;
			using(response = (HttpWebResponse) request.GetResponse()) {
				System.IO.Stream s = response.GetResponseStream();
				using(System.IO.StreamReader sr = new System.IO.StreamReader(s)) {
					try{
						GameObject.Find("reconnect").transform.localScale=Vector3.zero;
					SceneManager.UnloadSceneAsync("Loader");
						isLoader=false;
						ConnectivityPresenter.beginPing=false;
						CancelInvoke();
						Home.SetActive(false);
						Home.SetActive(true);
					}catch(NullReferenceException ex){
					}
				} 
			}
		} catch (WebException ex) {
			if (!isLoader) {
				SceneManager.LoadSceneAsync ("Loader", LoadSceneMode.Additive);
				GameObject.Find ("reconnect").transform.localScale = Vector3.one;
				isLoader = true;
			}
		}
	}
	public static bool isConnected() {
		UserManager um = new UserManager();
		string url = "http://clients3.google.com/generate_204";
		HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
		request.Method = "GET";
		request.Timeout = 200;
		try {
			HttpWebResponse response;
			using(response = (HttpWebResponse) request.GetResponse()) {
				System.IO.Stream s = response.GetResponseStream();
				using(System.IO.StreamReader sr = new System.IO.StreamReader(s)) {
					return true;
				} 
			}
		} catch (WebException ex) {
			return false;
		}
	}
}
