using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Timers;
using UnityEngine.SceneManagement;
public class ConnectionManager : MonoBehaviour {
	public  GameObject ConnectionPanel ;
	public static bool ConnectionPanelActive=false;
	public static bool Launched;
	// Use this for initialization
	void Start () {
//		if (Launched == false) {
//			SceneManager.LoadSceneAsync ("ConnectionFailed", LoadSceneMode.Additive);
//			Launched = true;
//		}
		try{
		GameObject.Find ("popupConnectionFailed").GetComponent<Animator> ().SetBool ("Show Error", true);
		}catch(NullReferenceException ex){
		}
	} 
	IEnumerator internet(){
		System.Timers.Timer aTimer = new System.Timers.Timer();
		aTimer.Elapsed+=new ElapsedEventHandler(OnTimedEvent);
		aTimer.Interval=2000;
		aTimer.Enabled=true;
		aTimer.AutoReset = true;
		aTimer.Start ();
		while (aTimer.Enabled == true) {
			yield return new WaitForSeconds (1);
		}
	}
	private  void OnTimedEvent(object source, ElapsedEventArgs e)
	{  
		UnityThreadHelper.Dispatcher.Dispatch (() => {
			try{
				StartCoroutine( checkInternetConnection((isConnected)=>{
					// handle connection status here
					if(isConnected==false){
						//SceneManager.LoadScene ("ConnectionFailed", LoadSceneMode.Additive);
						showConnectionPanel();
						//ConnectionFailedActive=true;
					}else if(isConnected==true ){
						//SceneManager.UnloadScene ("ConnectionFailed");
						hideConnectionPanel();
					}
					////Debug.Log("Connected ? :"+isConnected);
				})
				);
			}catch(MissingReferenceException ex){
				////Debug.Log(ex);
			}
		});
	}
	// Update is called once per frame
	void Update () {
	}
	public  void showConnectionPanel(){
		if (ConnectionPanelActive == false) {
			ConnectionPanel.gameObject.SetActive (true);
			ConnectionPanelActive = true;
		}
	}
	public  void hideConnectionPanel(){
		if (ConnectionPanelActive == true) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			ConnectionPanel.gameObject.SetActive (false);
			ConnectionPanelActive = false;
		}
	}
	IEnumerator checkInternetConnection(Action<bool> action){
		WWW www = new WWW("http://google.com");
		yield return www;
		if (www.error != null) {
			action (false);
		} else {
			action (true);
		}
	} 
}
