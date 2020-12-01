using System;
using System.Collections.Generic;
using UnityEngine;


namespace SeembaSDK
{
    [System.Serializable]
    [CLSCompliant(false)]
    public class Tour : MonoBehaviour
    {
        public List<TourChallengePresenter> ToursChallenges = new List<TourChallengePresenter>();
    }
}

