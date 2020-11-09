using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
public class ToursController : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
{
    public List<Tour> Tours = new List<Tour>();
}
