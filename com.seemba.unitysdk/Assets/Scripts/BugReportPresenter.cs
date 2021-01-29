using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class BugReportPresenter : MonoBehaviour
    {
        public Button ReportBug;
        public InputField BugDescription;
        public Image ScreenShot;
        public Animator Animator;

        public void Init(Sprite screenShot)
        {
            ScreenShot.sprite = screenShot;
        }

        public void SendScreenShot()
        {
            Debug.LogWarning("Send Bug");
        }
    }
}
