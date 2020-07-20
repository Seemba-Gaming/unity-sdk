using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NotificationBadgeViewPresenter : MonoBehaviour
{
    // Start is called before the first frame update
    public Image[] dots;
    static NotificationBadgeViewPresenter _Instance;
    void Start()
    {
        _Instance = this;
    }
    public static NotificationBadgeViewPresenter getInstance() {
        return _Instance;
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void showNotificationBadge() {
        foreach (Image dot in dots)
            dot.gameObject.SetActive(false); //turn into true if u want Notification badge works for docs verification..
    }
}
