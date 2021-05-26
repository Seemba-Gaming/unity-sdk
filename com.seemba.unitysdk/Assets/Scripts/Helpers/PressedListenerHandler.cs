using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SeembaSDK
{
    public class PressedListenerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public InputField password;
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
        }
        public void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}

