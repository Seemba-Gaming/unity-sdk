using UnityEngine;
using UnityEngine.UI;
using System;
namespace SeembaSDK
{
	[CLSCompliant(false)]
	public class LastResultListController : MonoBehaviour
	{
		public Image avatar;
		public Text GameDate, AdversaryName, result, victory, defeat, equality, matchId, AdvId;
		public Image Drapeau;
		public Button showResult;
	}
}