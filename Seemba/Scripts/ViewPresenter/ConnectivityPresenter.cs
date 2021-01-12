using System;
using UnityEngine;

namespace SeembaSDK
{
    [CLSCompliant(false)]
    public class ConnectivityPresenter : MonoBehaviour
    {
        public static bool beginPing;

        public ConnectivityController connectivity;
        public GameObject Home;

        private void OnEnable()
        {
            if (beginPing == true)
            {
                check_connection();
            }
            else
            {
                InvokeRepeating("checkStartPinging", 0f, 1f);
            }
        }

        private void checkStartPinging()
        {
            if (beginPing == true)
            {
                CancelInvoke();
                check_connection();
            }
        }
        public void check_connection()
        {
            LoaderManager.Get.LoaderController.ShowLoader(null);
            InvokeRepeating("ping", 0f, 1f);
        }
    }
}
