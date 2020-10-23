using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressedListenerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public InputField password;
    bool ispressed = false;
    void Update()
    {
        //if (ispressed)
        //{
        //    password.contentType = InputField.ContentType.Standard;
        //}
        //else
        //{
        //    password.contentType = InputField.ContentType.Password;
        //}
        //password.ForceLabelUpdate();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        ispressed = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        ispressed = false;
    }
}

