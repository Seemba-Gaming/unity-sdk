﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [System.Serializable]
    [CLSCompliant(false)]
    public class ToursController : MonoBehaviour
    {
        public List<Tour> Tours = new List<Tour>();
    }
}