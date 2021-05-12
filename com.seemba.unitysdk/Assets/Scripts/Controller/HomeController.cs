using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SeembaSDK
{
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
                    break;
                }
                timer += Time.deltaTime;
                yield return null;
            }
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
                    ReconnectMessage.SetActive(false);
                    StartCoroutine(UnloadLoaderCoroutine());
                }
            }
        }
        public IEnumerator UnloadLoaderCoroutine()
        {
            while(Ongoing.transform.childCount != 0 || LastResult.transform.childCount != 0 || (NoOngoing == true && NoLastResult == true))
            {
                yield return new WaitForSeconds(0.5f);
                LoadingHomeBloc.SetActive(false);
            }
        }
    }
}
