using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[CLSCompliant(false)]
public class ConnectionManager : MonoBehaviour
{
    #region Script Parameters
    public GameObject ConnectionPanel;
    public static bool ConnectionPanelActive = false;
    public static bool Launched;
    #endregion

    #region Unity Methods
    #endregion

    private IEnumerator Internet()
    {
        Timer aTimer = new Timer();
        aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        aTimer.Interval = 2000;
        aTimer.Enabled = true;
        aTimer.AutoReset = true;
        aTimer.Start();
        while (aTimer.Enabled == true)
        {
            yield return new WaitForSeconds(1);
        }
    }
    private void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        StartCoroutine(checkInternetConnection((isConnected) =>
        {
            // handle connection status here
            if (isConnected == false)
            {
                showConnectionPanel();
            }
            else if (isConnected == true)
            {
                hideConnectionPanel();
            }
        }));
    }
    public void showConnectionPanel()
    {
        if (ConnectionPanelActive == false)
        {
            ConnectionPanel.gameObject.SetActive(true);
            ConnectionPanelActive = true;
        }
    }
    public void hideConnectionPanel()
    {
        if (ConnectionPanelActive == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            ConnectionPanel.gameObject.SetActive(false);
            ConnectionPanelActive = false;
        }
    }
    private IEnumerator checkInternetConnection(Action<bool> action)
    {
        UnityWebRequest www = new UnityWebRequest("http://google.com");
        yield return www.SendWebRequest();
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }
}
