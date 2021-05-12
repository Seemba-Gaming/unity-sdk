using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class OnGoingTournamentListItemController : MonoBehaviour
    {
		public TextMeshProUGUI titre;
        public Text Date,tournamentId, status;
        public string gain, gainType, CreatedAt;
        public Button GoToBracket;
    }
}
