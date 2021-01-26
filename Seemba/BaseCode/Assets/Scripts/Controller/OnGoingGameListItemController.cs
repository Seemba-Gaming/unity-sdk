using UnityEngine;
using UnityEngine.UI;
using System;

namespace SeembaSDK
{
	[CLSCompliant(false)]
	public class OnGoingGameListItemController : MonoBehaviour
	{
		public Text status, titre, challengeResultId, challengeId, GainInBracket, DetailsInBracket, pending_text;
		public Image pro2, pro5, pro10, bubble2, bubble6, bubble10, bubble25, bubble50, bubble100, bracket7, bracket15, bracket30;
		public Button SeeResult, Result, GetMoney, GoToBracket;
	}
}