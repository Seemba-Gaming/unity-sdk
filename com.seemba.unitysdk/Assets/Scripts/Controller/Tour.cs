using System.Collections.Generic;
using UnityEngine;


namespace SeembaSDK
{
    [System.Serializable]
    public class Tour : MonoBehaviour
    {
        public List<TourChallengePresenter> ToursChallenges = new List<TourChallengePresenter>();
    }
}

