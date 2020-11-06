using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
public class NotificationBadgeViewPresenter : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
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
