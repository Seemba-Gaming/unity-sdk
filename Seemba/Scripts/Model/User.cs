using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class User  {
	public string _id;
	public string username;
	public string avatar;
	public bool username_changed;
	public string lastname;
	public string firstname;
	public float money_credit;
	public float bubble_credit;
	public string email;
	public string password;
	public string personal_id_number;
    public string phone;
	public int amateur_bubble;
	public int novice_bubble;
	public int legend_bubble;
	public int confident_bubble;
	public int confirmed_bubble;
	public int champion_bubble;
	public int amateur_money;
	public int novice_money;
	public int legend_money;
	public int confident_money;
	public int confirmed_money;
	public int champion_money;
	public int losses_streak;
	public int current_victories_count;
	public string long_lat;
	public string last_bubble_click;
	public bool email_verified;
	public bool iban_uploaded;
	public int level;
	public string connect_account_id;
	public bool id_proof_1_uploaded;
	public bool id_proof_2_uploaded;
	public string city;
	public string country_code;
	public string state;
	public float max_withdraw;
	public string zipcode;
	public bool passport_uploaded;
	public string last_result;
	public string birthdate;
	public string address;
	public string country;
	public bool residency_proof_uploaded;
	public int victories_count;
	public string token;
	public string flag;
	public string payment_account_id;
	public bool proLabel;
	public User (string _id,string username, string avatar, bool username_changed,string personal_id_number, string lastname, string firstname, float money_credit, float bubble_credit,
		string email,  string password, int amateur_bubble, int novice_bubble, int legend_bubble, 
		int confident_bubble, int confirmed_bubble, int champion_bubble, int amateur_money, int novice_money,
	    int legend_money, int confident_money, int confirmed_money, int champion_money, int losses_streak,
	    int victories_streak, string long_lat, string last_bubble_click, bool email_verified, bool iban_uploaded,
		int level, string payment_account_id, bool id_proof_1_uploaded, bool id_proof_2_uploaded, string city,
		string country_code, string state, float max_withdraw, string zipcode, bool passport_uploaded,
		string last_result, string birthday, string adress, string country, bool residency_proof_uploaded,
		int victories_count,string phone)
    {
		this._id = _id;
		this.username = username;
		this.avatar = avatar;
		this.username_changed = username_changed;
		this.personal_id_number = personal_id_number;
		this.lastname = lastname;
		this.firstname = firstname;
		this.money_credit = money_credit;
		this.bubble_credit = bubble_credit;
		this.email = email;
		this.password = password;
        this.phone = phone;
		this.amateur_bubble = amateur_bubble;
		this.novice_bubble = novice_bubble;
		this.legend_bubble = legend_bubble;
		this.confident_bubble = confident_bubble;
		this.confirmed_bubble = confirmed_bubble;
		this.champion_bubble = champion_bubble;
		this.amateur_money = amateur_money;
		this.novice_money = novice_money;
		this.legend_money = legend_money;
		this.confident_money = confident_money;
		this.confirmed_money = confirmed_money;
		this.champion_money = champion_money;
		this.losses_streak = losses_streak;
		this.current_victories_count = victories_streak;
		this.long_lat = long_lat;
		this.last_bubble_click = last_bubble_click;
		this.email_verified = email_verified;
		this.iban_uploaded = iban_uploaded;
		this.level = level;
		this.connect_account_id = payment_account_id;
		this.id_proof_1_uploaded = id_proof_1_uploaded;
		this.id_proof_2_uploaded = id_proof_2_uploaded;
		this.passport_uploaded = passport_uploaded;
		this.city = city;
		this.country_code = country_code;
		this.state = state;
		this.max_withdraw = max_withdraw;
		this.zipcode = zipcode;
		this.last_result = last_result;
		this.birthdate = birthday;
		this.address = adress;
		this.country = country;
		this.residency_proof_uploaded = residency_proof_uploaded;
		this.victories_count = victories_count;
    }
	public User(){}
}
