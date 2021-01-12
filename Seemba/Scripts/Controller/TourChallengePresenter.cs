using System;
using UnityEngine;

namespace SeembaSDK
{
    [System.Serializable]
    [CLSCompliant(false)]
    public class TourChallengePresenter : MonoBehaviour
    {
        public TourPlayer Player1;
        public TourPlayer Player2;
        public int winner;
    }
}
