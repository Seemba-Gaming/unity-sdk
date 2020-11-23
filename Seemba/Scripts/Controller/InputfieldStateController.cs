using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CLSCompliant(false)]
public class InputfieldStateController : MonoBehaviour
{

    public GameObject Accepted;
    public GameObject Declined;
    public GameObject Loading;
    public GameObject Editable;
    
    public void ShowAccepted()
    {
        Accepted.SetActive(true);
        Declined.SetActive(false);
        Loading.SetActive(false);
        if (Editable != null)
        {
            Editable.SetActive(false);
        }
    }

    public void ShowDeclined()
    {
        Accepted.SetActive(false);
        Declined.SetActive(true);
        Loading.SetActive(false);
        if (Editable != null)
        {
            Editable.SetActive(false);
        }
    }

    public void ShowLoading()
    {
        Accepted.SetActive(false);
        Declined.SetActive(false);
        Loading.SetActive(true);
        if (Editable != null)
        {
            Editable.SetActive(false);
        }
    }

    public void ShowEditable()
    {
        Accepted.SetActive(false);
        Declined.SetActive(false);
        Loading.SetActive(false);
        if (Editable != null)
        {
            Editable.SetActive(true);
        }
    }

    public void HideEditable()
    {
        if (Editable != null)
        {
            Editable.SetActive(false);
        }
    }

}
