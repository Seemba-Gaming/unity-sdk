using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace SeembaSDK
{
	[CLSCompliant(false)]
	public class LastResultTournamentListController : MonoBehaviour
	{
		public TextMeshProUGUI title;
		public Text victory, defeat, tournamentID, date;
		public Button showResult;
	}
}
