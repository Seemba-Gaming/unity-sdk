using System;
using UnityEngine;
using UnityEngine.UI;

[CLSCompliant(false)]
public class NotificationBadgeViewPresenter : MonoBehaviour
{
    // Start is called before the first frame update
    public Image[] dots;
    private static NotificationBadgeViewPresenter _Instance;

    private void Start()
    {
        _Instance = this;
    }
    public static NotificationBadgeViewPresenter getInstance()
    {
        return _Instance;
    }

    // Update is called once per frame
    private void Update()
    {
    }
    public void showNotificationBadge()
    {
        foreach (Image dot in dots)
        {
            dot.gameObject.SetActive(false); //turn into true if u want Notification badge works for docs verification..
        }
    }
}
