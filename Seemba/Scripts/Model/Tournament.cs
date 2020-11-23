using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
[CLSCompliant(false)]
public class Tournament {
    int nb_player;
    float gain;
    string gain_type;
    Game game;
    ArrayList users;
	string tournamentId;
	public Tournament(string tournamentId,float gain,string gain_type,int nb_player,Game game,ArrayList users){
		this.nb_player=nb_player;
		this.tournamentId=tournamentId;
		this.gain=gain;
		this.gain_type=gain_type;
		this.game=game;
		this.users=users; 
	}	
}

[CLSCompliant(false)]
public class TournamentData
{
	public bool success;
	public string message;
	public Tournament data;
}
