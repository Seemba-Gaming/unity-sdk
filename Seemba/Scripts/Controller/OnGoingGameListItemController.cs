using UnityEngine;
using UnityEngine.UI;
using System;

namespace SeembaSDK
{
	[CLSCompliant(false)]
	public class OnGoingGameListItemController : MonoBehaviour
	{
		public Text status, titre, challengeResultId, Gain, challengeId, GainInBracket, DetailsInBracket, CreatedAt, pending_text;
		public Button SeeResult, Result, GetMoney;
	}
}