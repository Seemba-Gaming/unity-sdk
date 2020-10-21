using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
public class ViewsEvents : MonoBehaviour {
	EncartPlayerPresenter startUp;
	public static ViewsEvents Instance;
    //public GoogleAnalyticsV4 googleAnalytics;
	// Use this for initialization
	void Start () {
        
		//DontDestroyOnLoad (GameObject.Find ("Canvas").gameObject);
	}
	// Update is called once per frame
	void Update () {
	}
	
	public void WalletClick(string last_view)
    {
        EventsController.last_view = last_view;
        ShowScene("Wallet");
		
	}
    public void WinMoneyClick(){
        BottomMenuController._currentPage = 0;
        ShowScene ("WinMoney");
	}
    public void HaveFunClick(){
        ShowScene ("HaveFun");
	}
	public void HomeClick(){
		ShowScene ("Home");
	}
	public void LiveClick(){
		ShowScene ("Live");
	}
	public void SettingsClick(){
		ProfilLastResultListController.profileSceneOpened=false;
        BottomMenuController.Show();
		ShowScene ("Settings");
	}
	public void AchievementClick(){
		ShowScene ("Achievement");
	}
	public void HistoryClick(){
		ShowScene ("History");
	}
	public void SecurityClick(){
        ShowScene ("Security");
	}
	public void ProfileClick(){
        ProfilLastResultListController.profileSceneOpened=false;
		ShowScene ("Profilee");
	}
	public void LegalClick(){
        BottomMenuController.Hide();
        ShowScene("Legal");
	}
	public void ContactClick(){
        BottomMenuController.Hide();
        ShowScene("Contact");
	}
	public void ShowScene(string sceneName){
        
        
        ScrollSnapRect.currentView = sceneName;
        GameObject Canvas = GameObject.Find ("Canvas");
		foreach (Transform child in Canvas.transform) {
			if (child.name != sceneName && child.name != "Toolbar") {
				child.gameObject.SetActive (false);
			}
			else child.gameObject.SetActive (true);
		}
		startUp = new EncartPlayerPresenter ();
		startUp.OnEnable();
	}
}
