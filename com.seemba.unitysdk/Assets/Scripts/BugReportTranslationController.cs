using System;
using UnityEngine;
using UnityEngine.UI;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class BugReportTranslationController : MonoBehaviour
    {
        public Text Title;
        public Text GameId;
        public Text GameName;
        public Text Describe;
        public Text ReportABug;

        private void Start()
        {
            TranslationManager._instance.scene = "BugReport";
        }
    }
}
