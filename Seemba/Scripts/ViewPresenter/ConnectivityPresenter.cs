using System;
using System.Net;
using UnityEngine;
public class ConnectivityPresenter : MonoBehaviour
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
    public void ping()
    {
        string url = "https://www.google.fr";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Timeout = 3000;
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    try
                    {
                        LoaderManager.Get.LoaderController.HideLoader();
                        isLoader = false;
                        beginPing = false;
                        CancelInvoke();
                        Home.SetActive(false);
                        Home.SetActive(true);
                    }
                    catch (NullReferenceException)
                    {
                    }
                }
            }
        }
        catch (WebException)
        {
            if (!isLoader)
            {
                LoaderManager.Get.LoaderController.ShowLoader(null);
                isLoader = true;
            }
        }
    }
    public static bool isConnected()
    {
        string url = "http://clients3.google.com/generate_204";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Timeout = 200;
        try
        {
            HttpWebResponse response;
            using (response = (HttpWebResponse)request.GetResponse())
            {
                System.IO.Stream s = response.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    return true;
                }
            }
        }
        catch (WebException)
        {
            return false;
        }
    }
}
