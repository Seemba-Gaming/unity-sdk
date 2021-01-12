using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class HomeController : MonoBehaviour
    {
        public static bool NoLastResult, NoOngoing;
        public GameObject Ongoing;
        public GameObject LastResult;
        public GameObject Home;
        public GameObject LoadingHomeBloc;
        public GameObject ReconnectMessage;

        private bool notConnected;

        private void OnEnable()
        {
            StartCoroutine(SelectHome());

            if (UserManager.Get.CurrentUser == null)
            {
                StartCoroutine(CheckHeader());
                StartCoroutine(CheckOngoingAndLastResult());
            }
        }
        private IEnumerator CheckHeader()
        {
            LoaderManager.Get.LoaderController.ShowLoader(null);
            float timer = 0;
            bool failed = false;

            while (UserManager.Get.CurrentUser != null)
            {
                if (timer > 12) { failed = true; break; }
                timer += Time.deltaTime;
                yield return null;
            }

            if (failed)
            {
                ConnectivityController.CURRENT_ACTION = ConnectivityController.HOME_ACTION;
                PopupManager.Get.PopupController.ShowPopup(PopupType.INFO_POPUP_CONNECTION_FAILED, PopupsText.Get.ConnectionFailed());
            }
            LoaderManager.Get.LoaderController.HideLoader();
        }
        private IEnumerator SelectHome()
        {
            notConnected = false;

            while (BottomMenuController.Get == null)
            {
                yield return new WaitForSeconds(0.1f);
            }
            if(ViewsEvents.Get != null)
            {
                ViewsEvents.Get.HomeClick();
            }
        }
        private IEnumerator CheckOngoingAndLastResult()
        {
            notConnected = false;
            float timer = 0;
            while (Ongoing.transform.childCount == 0 && LastResult.transform.childCount == 0)
            {
                if (timer > 2)
                {
                    if (!LoadingHomeBloc.activeSelf)
                    {
                        LoadingHomeBloc.SetActive(true);
                        ReconnectMessage.SetActive(true);
                    }
                    InvokeRepeating("tryingtoreconnect", 0f, 1f); ;
                    break;
                }
                timer += Time.deltaTime;
                yield return null;
            }
        }
        private void tryingtoreconnect()
        {
            StartCoroutine(checkInternetConnection());
        }
        public IEnumerator checkInternetConnection()
        {
            UnityWebRequest www = new UnityWebRequest("https://www.google.fr");
            yield return www.SendWebRequest();

            if (notConnected)
            {
                www.Dispose();
            }
            else
            {
                if (www.error == null)
                {
                    CancelInvoke();
                    ReconnectMessage.SetActive(false);
                    InvokeRepeating("unloadLoader", 0f, 0.5f);
                }
            }
        }
        public void unloadLoader()
        {
            if (Ongoing.transform.childCount != 0 || LastResult.transform.childCount != 0 || (NoOngoing == true && NoLastResult == true))
            {
                LoadingHomeBloc.SetActive(false);
                CancelInvoke();
            }
        }
    }
}
