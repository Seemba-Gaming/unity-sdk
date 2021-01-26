using UnityEngine;
using UnityEngine.UI;
using System;
namespace SeembaSDK
{
	[CLSCompliant(false)]
	public class LastResultListController : MonoBehaviour
	{
		public Image FeeIcon, proWon, bubbleWon, lost, avatar;
		public Text GameDate, AdversaryName, Fee, gainPro, gainBubble, result, victory, defeat, equality, matchId, AdvId;
		public Image Drapeau;
		public Button showResult;
	}
}