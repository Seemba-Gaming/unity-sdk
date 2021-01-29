using System;
using UnityEngine;

namespace SeembaSDK
{
	[CLSCompliant(false)]
	public class ConnectivityController : MonoBehaviour
	{
		public static string CURRENT_ACTION;
		public const string CHALLENGE_ACTION = "challenge";
		public const string HOME_ACTION = "home";
		public const string ENTER_ESPORT_TOURNAMENT_ACTION = "enterEsportTournament";
		public const string CREDIT_ACTION = "credit";
		public const string LOGIN_ACTION = "Login";
		public const string PERSONNEL_INFO_ACTION = "personelInfo";
		public const string PERSONNEL_INFO_WITHDRAW_ACTION = "personelInfoWithdraw";
		public const string HISTORY_ACTION = "history";
	}
}
