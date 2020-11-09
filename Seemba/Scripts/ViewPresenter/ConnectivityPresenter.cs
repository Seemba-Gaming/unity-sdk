using System;
using System.Net;
using UnityEngine;
#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
public class ConnectivityPresenter : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
{
    public static bool beginPing;

    public ConnectivityController connectivity;
    public GameObject Home;

    private bool isLoader = false;

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
        isLoader = true;
        InvokeRepeating("ping", 0f, 1f);
    }
}
