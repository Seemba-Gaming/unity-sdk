﻿using UnityEngine;
using System.Collections;
namespace Kakera
{
#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
    public class Unimgpicker : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
    {
        public delegate void ImageDelegate(string path);
        public delegate void ErrorDelegate(string message);
        public event ImageDelegate Completed;
        public event ErrorDelegate Failed;
        private IPicker picker = 
        #if UNITY_IOS && !UNITY_EDITOR
            new PickeriOS();
        #elif UNITY_ANDROID && !UNITY_EDITOR
            new PickerAndroid();
        #else
            new PickerUnsupported();
        #endif
        public void Show(string title, string outputFileName, int maxSize)
        {
			//Debug.Log ("L27 outputFileName : "+outputFileName);
            picker.Show(title, outputFileName, maxSize);
        }
        private void OnComplete(string path)
		{    //Debug.Log ("OnComplete : "+path);
            var handler = Completed;
            if (handler != null)
            {
                handler(path);
            }
        }
        private void OnFailure(string message)
        {
            var handler = Failed;
            if (handler != null)
            {
                handler(message);
            }
        }
    }
}