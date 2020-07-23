using System;
using UnityEngine;
using UnityEngine.UI;
public class DatePickerAgeVerification : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public Button prev, next;
    Text Day, Month;
    private string _selectedDateString1 = "1995-09-15";
    private Text _textDatePredefined2;
    private String SelectedDateString2
    {
        get
        {
            return _selectedDateString1;
        }
        set
        {
            _selectedDateString1 = value;
            //UpdateLabels();
            try
            {
                //Debug.Log("NBS:69");
                _textDatePredefined2.text = SelectedDateString2;
            }
            catch (NullReferenceException ex)
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
        UserManager um = new UserManager();
        string UserId = um.getCurrentUserId();
        string UserToken = um.getCurrentSessionToken();
        EventsController nbs = new EventsController();
        try
        {
            _textDatePredefined2 = GameObject.Find("textDatePredefined2").GetComponent<Text>();
        }
        catch (NullReferenceException ex) { }
        NativePicker.Instance.ShowDatePicker(GetScreenRect(button as GameObject), NativePicker.DateTimeForDate(2012, 12, 23), (long val) =>
        {
            SelectedDateString2 = NativePicker.ConvertToDateTime(val).ToString("yyyy-MM-dd");
            ////Debug.Log("SelectedDateString2: "+SelectedDateString2);
        }, () =>
        {
            //SelectedDateString2 = "canceled";
            SelectedDateString2 = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                //_textDatePredefined.text=SelectedDateString2;
            }
            catch (NullReferenceException ex) { }
            try
            {
                GameObject.Find("WithdrawalInfo").GetComponent<GameObject>();
            }
            catch (NullReferenceException ex)
            {
                nbs.UpdateAge();
            }
        });
    }
}
