using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.Net;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Timers;
public class TournamentPresenter : MonoBehaviour
{
    UserManager um;
    TournamentManager tm;
    public JSONNode tournamentJson;
    public Button Play, Great;
    public GameObject Bubbles, Others;
    private JSONArray rounds;
    private JSONArray challenges;
    private JSONArray participants;
    private string userId;
    private string token;
    void Start()
    {
        tm = new TournamentManager();
        UserManager um = new UserManager();
        userId = um.getCurrentUserId();
        token = um.getCurrentSessionToken();
        UnityThreadHelper.CreateThread(() =>
        {
            tournamentJson = tm.getTournament(TournamentController.getCurrentTournamentID(), token);
            UnityThreadHelper.Dispatcher.Dispatch(() =>
            {
                setTournamentData();
                initUI(challenges, participants);

                if (!isAvailable())
                {
                    hidePlayButton();
                }
                if (!isNextChallengeAvailable())
                {
                    hidePlayButton();
                }
            });
        });
        Play.onClick.AddListener(() =>
        {
            EventsController.ChallengeType = "Bracket";
            SceneManager.LoadScene(GamesManager.GAME_SCENE_NAME);
        });
        Great.onClick.AddListener(() =>
        {
            EventsController eventsController = new EventsController();
            eventsController.HidePopup("popupWIN", true);
        });
    }
    private bool isNextChallengeAvailable()
    {
        float? user_1_score = null;
        try
        {
            user_1_score = tournamentJson["current_challenge"]["user_1_score"].AsFloat;
        }
        catch (NullReferenceException ex) { }
        float? user_2_score = null;
        try
        {
            user_2_score = tournamentJson["current_challenge"]["user_2_score"].AsFloat;
        }
        catch (NullReferenceException ex) { }
        string matched_user_1 = null;
        try
        {
            matched_user_1 = tournamentJson["current_challenge"]["matched_user_1"]["_id"].Value;
        }
        catch (NullReferenceException ex) { }
        string matched_user_2 = null;
        try
        {
            matched_user_2 = tournamentJson["current_challenge"]["matched_user_2"]["_id"].Value;
        }
        catch (NullReferenceException ex) { }
        if (matched_user_1 == userId && user_1_score != null && user_2_score == null)
        {
            return false;
        }
        if (matched_user_2 == userId && user_2_score != null && user_1_score == null)
        {
            return false;
        }
        return true;
    }
    private bool isAvailable()
    {
        var lose = false;
        foreach (JSONNode loser in tournamentJson["tournament"]["losers"].AsArray)
        {

            if (loser.Value == userId)
            {
                lose = true;
                break;
            }
        }

        if (lose == true)
        {
            return false;
        }
        else if (lose == false && tournamentJson["tournament"]["status"].Value == "finished")
        {
            return false;
        }
        return true;
    }
    private void setTournamentData()
    {
        rounds = tournamentJson["tournament"]["rounds"].AsArray;
        challenges = getChallenges(rounds);
        participants = tournamentJson["tournament"]["participants"].AsArray;
        TournamentController.CURRENT_TOURNAMENT_NB_PLAYER = tournamentJson["tournament"]["nb_players"].AsInt;
        TournamentController.CURRENT_TOURNAMENT_GAIN = tournamentJson["tournament"]["gain"].AsFloat;
        TournamentController.CURRENT_TOURNAMENT_GAIN_TYPE = tournamentJson["tournament"]["gain_type"].Value;
    }
    public JSONArray getChallenges(JSONArray rounds)
    {
        JSONArray challenges = new JSONArray();
        foreach (JSONNode round in rounds)
        {
            foreach (JSONNode challenge in round["challenges"].AsArray)
            {
                challenges.Add(challenge);
            }
        }
        return challenges;
    }
    public void initUI(JSONArray challenges, JSONArray participants)
    {
        //Show Loaders
        GameObject.Find("Loaders").transform.localScale = Vector3.one;
        //Start init
        um = new UserManager();
        int pos = 1;
        foreach (JSONNode match in challenges)
        {
            initMatch(match, pos, participants);
            pos++;
        }
        //Hide Loaders
        GameObject.Find("Loaders").transform.localScale = Vector3.zero;
    }
    void initMatch(JSONNode match, int pos, JSONArray participants)
    {
        UserManager um = new UserManager();
        float? user_1_score = null, user_2_score = null;
        initPlayersUI(getUserFromParticipants(match["matched_user_1"]["_id"].Value, participants), getUserFromParticipants(match["matched_user_2"]["_id"].Value, participants), pos);
        try
        {
            user_1_score = match["user_1_score"].AsFloat;
            GameObject.Find("Challenge (" + pos + ")/Player1/Score").GetComponent<Text>().text = ((float)Math.Round(match["user_1_score"].AsFloat * 100f) / 100f).ToString();
        }
        catch (NullReferenceException ex) { }
        try
        {
             
            user_2_score = match["user_2_score"].AsFloat;
            //Set Score
            GameObject.Find("Challenge (" + pos + ")/Player2/Score").GetComponent<Text>().text = ((float)Math.Round(match["user_2_score"].AsFloat * 100f) / 100f).ToString();
        }
        catch (NullReferenceException ex) { }
        
        if (pos == TournamentController.CURRENT_TOURNAMENT_NB_PLAYER - 1)
        {
            try
            {
                if (user_1_score > user_2_score)
                {
                    GameObject.Find("Challenge (" + (pos + 1) + ")/Player1/Score").GetComponent<Text>().text = user_1_score.ToString();
                    JSONNode user = getUserFromParticipants(match["matched_user_1"]["_id"].Value, participants);
                    initChampionUI(user, pos + 1);
                    setPlayerFlag(GameObject.Find("Challenge (" + (pos + 1) + ")/Player1/Flag").GetComponent<Image>(), um.GetFlagByte(getUserFromParticipants(match["matched_user_1"]["_id"].Value, participants)["country_code"].Value));
                    if (user["money_credit"].AsFloat > 0f)
                    {
                        GameObject.Find("Challenge (" + (pos + 1) + ")/Player1/Pro").transform.localScale = Vector3.one;
                    }
                }
                else if (user_1_score < user_2_score)
                {
                    GameObject.Find("Challenge (" + (pos + 1) + ")/Player1/Score").GetComponent<Text>().text = user_2_score.ToString();
                    JSONNode user = getUserFromParticipants(match["matched_user_2"]["_id"].Value, participants);
                    initChampionUI(user, pos + 1);
                    setPlayerFlag(GameObject.Find("Challenge (" + (pos + 1) + ")/Player1/Flag").GetComponent<Image>(), um.GetFlagByte(getUserFromParticipants(match["matched_user_2"]["_id"].Value, participants)["country_code"].Value));
                    if (user["money_credit"].AsFloat > 0f)
                    {
                        GameObject.Find("Challenge (" + (pos + 1) + ")/Player1/Pro").transform.localScale = Vector3.one;
                    }
                }
            }
            catch (NullReferenceException ex) { }
        }
    }
    JSONNode getUserFromParticipants(string id, JSONArray participants)
    {
        foreach (JSONNode player in participants)
        {
            if (player["_id"].Value == id) return player;
        }
        return null;
    }
    void initPlayersUI(JSONNode player1, JSONNode player2, int pos)
    {
        try
        {
            //Set Username
            GameObject.Find("Challenge (" + (pos) + ")/Player1/Username").GetComponent<Text>().text = player1["username"].Value;
            //Set Avatar
            StartCoroutine(initPlayerAvatar(player1["avatar"].Value, GameObject.Find("Challenge (" + (pos) + ")/Player1/Avatar").GetComponent<Image>()));
            //Set Flag
            setPlayerFlag(GameObject.Find("Challenge (" + (pos) + ")/Player1/Flag").GetComponent<Image>(), um.GetFlagByte(player1["country_code"].Value));
            //Set PRO Flag
            if (player1["money_credit"].AsFloat > 0f)
            {
                GameObject.Find("Challenge (" + (pos) + ")/Player1/Pro").transform.localScale = Vector3.one;
            }
        }
        catch (NullReferenceException ex) { }
        try
        {
            //Set Username
            GameObject.Find("Challenge (" + (pos) + ")/Player2/Username").GetComponent<Text>().text = player2["username"].Value;
            //Set Avatar
            StartCoroutine(initPlayerAvatar(player2["avatar"].Value, GameObject.Find("Challenge (" + (pos) + ")/Player2/Avatar").GetComponent<Image>()));
            //Set Flag
            setPlayerFlag(GameObject.Find("Challenge (" + (pos) + ")/Player2/Flag").GetComponent<Image>(), um.GetFlagByte(player2["country_code"].Value));
            //Set PRO Flag
            if (player2["money_credit"].AsFloat > 0f)
            {
                GameObject.Find("Challenge (" + (pos) + ")/Player2/Pro").transform.localScale = Vector3.one;
            }
        }
        catch (NullReferenceException ex) { }
    }
    void initChampionUI(JSONNode player, int pos)
    {
        GameObject.Find("Challenge (" + (pos) + ")/Player1/Username").GetComponent<Text>().text = player["username"].Value;
        StartCoroutine(initPlayerAvatar(player["avatar"].Value, GameObject.Find("Challenge (" + (pos) + ")/Player1/Avatar").GetComponent<Image>()));
    }
    public IEnumerator initPlayerAvatar(string url, Image avatar)
    {
        var www = new WWW(url);
        yield return www;
        var texture = www.texture;
        Texture2D RoundTxt = ImagesManager.RoundCrop(texture);
        avatar.sprite = Sprite.Create(RoundTxt, new Rect(0, 0, RoundTxt.width, RoundTxt.height), new Vector2(0, 0));
    }
    void setPlayerFlag(Image flag, string flagByte)
    {
        Byte[] img1 = Convert.FromBase64String(flagByte);
        Texture2D txt1 = new Texture2D(1, 1);
        txt1.LoadImage(img1);
        flag.sprite = Sprite.Create(txt1 as Texture2D, new Rect(0f, 0f, txt1.width, txt1.height), Vector2.zero);
    }
    public void hidePlayButton()
    {
        GameObject.Find("PLAY").GetComponent<Button>().interactable = false;
    }
}
