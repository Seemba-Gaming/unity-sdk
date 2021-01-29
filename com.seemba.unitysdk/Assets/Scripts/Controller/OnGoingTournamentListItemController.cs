using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class OnGoingTournamentListItemController : MonoBehaviour
    {
        public Text Date, titre, tournamentId, status;
        public string gain, gainType, CreatedAt;
        public Button GoToBracket;
    }
}
