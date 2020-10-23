using System;
using UnityEngine;
using UnityEngine.UI;

public class DatePickerAgeVerification : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public Button prev, next;
    private Text Day, Month;
    private string _selectedDateString1 = "1995-09-15";
    private Text _textDatePredefined2;
    private String SelectedDateString2
    {
        get => _selectedDateString1;
        set
        {
            _selectedDateString1 = value;
            try
            {
                _textDatePredefined2.text = SelectedDateString2;
            }
            catch (NullReferenceException)
            {//Debug.Log("ex: "+ex);}
            }
        }
    }
    private Rect GetScreenRect(GameObject gameObject)
    {
        RectTransform transform = gameObject.GetComponent<RectTransform>();
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
        rect.x -= (transform.pivot.x * size.x);
        rect.y -= ((1.0f - transform.pivot.y) * size.y);
        return rect;
    }
    public void showDatePickerPersoInfo(UnityEngine.Object button)
    {
        string UserId = UserManager.Get.getCurrentUserId();
        string UserToken = UserManager.Get.getCurrentSessionToken();
        NativePicker.Instance.ShowDatePicker(GetScreenRect(button as GameObject), NativePicker.DateTimeForDate(2012, 12, 23), (long val) =>
        {
            SelectedDateString2 = NativePicker.ConvertToDateTime(val).ToString("yyyy-MM-dd");
        }, () =>
        {
            SelectedDateString2 = DateTime.Now.ToString("yyyy-MM-dd");
        });
    }
}
