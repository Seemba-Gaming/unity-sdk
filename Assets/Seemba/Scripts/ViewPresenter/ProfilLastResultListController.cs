using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class ProfilLastResultListController : MonoBehaviour {
	public GameObject ContentPanel;
	public GameObject ItemPrefab;
	ArrayList Items,lastResultItem;
	ChallengeManager challengeManager=new ChallengeManager();
	UserManager userManager=new UserManager();
	string UserId,token;
	public static bool profileSceneOpened;
	// Use this for initialization
	void OnDisable(){
		foreach (Transform child in ContentPanel.transform){
			Destroy (child.gameObject);
		}
	}
	void OnEnable () {
		try{
			token=userManager.getCurrentSessionToken();
			Items = new ArrayList ();
			//Debug.Log("profileSceneOpened " + profileSceneOpened);
			if(profileSceneOpened==true){
				profileSceneOpened=false;
				UserId=Profile.PlayerId;
			}else {
				UserId=	userManager.getCurrentUserId();
			}
			UnityThreading.ActionThread thread;
			//SceneManager.LoadScene("Loader",LoadSceneMode.Additive);
			thread = UnityThreadHelper.CreateThread (() => {
				lastResultItem = challengeManager.listChallenges (token);
				foreach (Challenge item in lastResultItem) {
					if (item.status == ChallengeManager.CHALLENGE_STATUS_FINISHED || (item.status == ChallengeManager.CHALLENGE_STATUS_SEE_RESULT_FOR_USER1 && item.matched_user_2._id==UserId) || (item.status == ChallengeManager.CHALLENGE_STATUS_SEE_RESULT_FOR_USER2 && item.matched_user_1._id==UserId)) {
						//Debug.Log("Item status "+item.status);
						Items.Add(item);
					}
				} 
				//Debug.Log("Items Count "+Items.Count);
				UnityThreadHelper.Dispatcher.Dispatch(()=>{
					if (Items!= null) {
						//GameObject.Find ("PanelEmpty").transform.localScale = new Vector3 (0, 0, 0);
						Items.Reverse();
						foreach(Challenge item in Items){
							if(item.status!= ChallengeManager.CHALLENGE_STATUS_RESULT_PENDING){
							GameObject newItem=null;
							try{
								newItem = Instantiate(ItemPrefab) as GameObject;
									if(item.user_1_score == item.user_2_score){
										newItem.transform.GetChild (4).gameObject.GetComponent<Image>().transform.localScale=Vector3.one;
									}
									if (UserId == item.matched_user_1._id) {
									string	UserToken=userManager.getCurrentSessionToken();
									 if (item.user_1_score > item.user_2_score) {
										//Win
										newItem.transform.GetChild (2).gameObject.GetComponent<Image>().transform.localScale=Vector3.one;
									}
									else if(item.user_1_score < item.user_2_score)
									{   
										//Lose
										newItem.transform.GetChild (3).gameObject.GetComponent<Image>().transform.localScale=Vector3.one;
									}  
								} 
								else {
									string UserToken=userManager.getCurrentSessionToken();
									 if (item.user_1_score > item.user_2_score) {
										//LOSE
										newItem.transform.GetChild (3).gameObject.GetComponent<Image>().transform.localScale=Vector3.one;
									}
									else if(item.user_1_score <  item.user_2_score)
									{   
										//Win
										newItem.transform.GetChild (2).gameObject.GetComponent<Image>().transform.localScale=Vector3.one;
                                    }
                                    }
                                newItem.transform.parent = ContentPanel.transform;
								RectTransform myLayoutElement = newItem.GetComponent<RectTransform> ();
								myLayoutElement.transform.localScale =Vector3.one;
							}catch(FormatException e){
								Destroy(newItem);
							}
						}
						}
					}
					});
				});
			}catch(NullReferenceException ex){
				//Debug.Log ("there is no challenge Result for this User");
			}
	}
	// Update is called once per frame
	void Update () {
	}
}
