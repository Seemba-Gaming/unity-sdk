using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Net;
using System.Linq;
public class FavouritTournament : MonoBehaviour
{
	UserManager um = new UserManager ();
	public static bool FavInPopUpProfile;
	// Use this for initialization
	void Start ()
	{
		string userId;
		string token = um.getCurrentSessionToken ();
		if (FavInPopUpProfile) {
			FavInPopUpProfile = false;
			userId = ProfileViewPresenter.PlayerId;
		} else {
			userId = um.getCurrentUserId ();
		}
          UnityThreadHelper.CreateThread (() => {
			string tournament_name = GetFavoriteTournament (userId, token);
			UnityThreadHelper.Dispatcher.Dispatch (() => { 
				switch (tournament_name) {
				case "novice_bubble":
					GameObject.Find ("noviceBubble").transform.localScale = new Vector3 (1, 1, 1);
					break;
				case "amateur_money":
					GameObject.Find ("amateurCash").transform.localScale = new Vector3 (1, 1, 1);
					break;
				case "novice_money":
					GameObject.Find ("noviceCash").transform.localScale = new Vector3 (1, 1, 1);
					break;
				case "confirmed_money":
					GameObject.Find ("confirmedCash").transform.localScale = new Vector3 (1, 1, 1);
					break;
				case "amateur_bubble":
					GameObject.Find ("amateurBubble").transform.localScale = new Vector3 (1, 1, 1);
					break;
				case "confirmed_bubble":
					GameObject.Find ("confirmedBubble").transform.localScale = new Vector3 (1, 1, 1);
					break;
				}
			});
		});
	}
	public string GetFavoriteTournament (string userId, string token)
	{
		string url = Endpoint.classesURL + "/users/" + userId + "/favorites";
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
		request.Method = "GET";
		request.Headers ["x-access-token"] = token;
		try {
			using (System.IO.Stream s = request.GetResponse ().GetResponseStream ()) {
				using (System.IO.StreamReader sr = new System.IO.StreamReader (s)) {
					var jsonResponse = sr.ReadToEnd ();

					var N = JSON.Parse (jsonResponse);
					JSONArray M = N ["data"].AsArray;
					var P = M.Children.First ();
					return (P ["game_type"]);
				}
			}
		} catch (WebException ex) {
			return "novice_bubble";
		}
	}
	// Update is called once per frame
	void Update ()
	{
	}
}
