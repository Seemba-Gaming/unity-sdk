using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class ReplayChallenge : MonoBehaviour {
	public static Challenge ChallengeToReplay;
	public Text GameId,Date,Prize,sorryText;
	public Button Replay;
	public GameObject Loader;
	// Use this for initialization
	void Start () {
		////Debug.Log (ChallengeToReplay.ChallengeId);
		GameId.text = ChallengeToReplay._id;
		Date.text = DateTime.Parse (ChallengeToReplay.CreatedAt).ToString ("MM/dd/yyyy hh:mm:ss");;
		if (ChallengeToReplay.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_CASH)
			Prize.text = float.Parse (ChallengeToReplay.gain).ToString ("N2") + " "+ CurrencyManager.CURRENT_CURRENCY;
		else if (ChallengeToReplay.gain_type == ChallengeManager.CHALLENGE_WIN_TYPE_BUBBLES) {
			Prize.text = ChallengeToReplay.gain + " Bubbles";
		}
		//add sorry text 
		Replay.onClick.AddListener (() => {
			Loader.SetActive(true);
			ChallengeManager.CurrentChallengeId=ChallengeToReplay._id;
			EventsController.ChallengeType=ChallengeManager.CHALLENGE_TYPE_1V1;
			SceneManager.LoadScene ("Loader",LoadSceneMode.Additive);
			BackgroundController.CurrentBackground=null;
			SceneManager.LoadSceneAsync (GamesManager.GAME_SCENE_NAME);
		});
	}
	// Update is called once per frame
	void Update () {
	}
}
