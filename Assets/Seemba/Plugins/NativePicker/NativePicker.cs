using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
public class NativePicker : MonoBehaviour {
	public enum PickerType {
		CustomPicker,
		DatePicker,
		TimePicker,
		DateTimePicker // iOS only
	}
#if UNITY_ANDROID
	AndroidJavaObject _pluginObject;
#elif UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern void showPicker(PickerType type, string[] items, int numItems, long selectedItem, int x, int y, int w, int h, string gameObject);
	[DllImport ("__Internal")]
	private static extern void hidePicker();
	[DllImport ("__Internal")]
	private static extern bool isIphone();
#endif
	static NativePicker _instance;
	private Action<long> _currentAction;
	private Action _cancelAction;
	public static NativePicker Instance {
		get {
			return _instance;
		}
	}
	bool isMobileRuntime {
		get {
			return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
		}
	}
	void Awake() {		
		_instance = this;
		if (isMobileRuntime == false) {
			//Debug.LogWarning("Due to platform specific NativePicker was designed to run only on iOS/Android device. Plugin function call has no effect on other platforms.");
			return;
		}
#if UNITY_ANDROID		
		_pluginObject = new AndroidJavaObject("ua.org.jeff.unity.nativepicker.AndroidPlugin");
#endif
	}
	void ItemPicked(String message) {
		//Debug.Log("Item picked " + message);
		long val = long.Parse(message);
		if (_currentAction != null) {
			_currentAction(val);
		}
	}
	void PickerCanceled(String message) {
		//Debug.Log("PickerCanceled");
		if (_cancelAction != null) {
			_cancelAction();
		}
	}
	void makeJNICall(PickerType type, string[] items, long selectedItem)
	{
		if (isMobileRuntime == false) {
			return;
		}
#if UNITY_ANDROID
        //Debug.Log(String.Format("Selected item -> {0}", selectedItem));
        _pluginObject.Call("showPicker", (int)type, items, selectedItem, "NativePicker");
#else
		return;
#endif
    }
	/**
	 * Check if application running on iPhone device
	 */
	public static bool iPhonePlatform {
		get{
			bool retval = false;
#if UNITY_ANDROID
#elif UNITY_IPHONE		
			retval = isIphone();
#endif
			return retval;
		}
	}
	/**
	 * Show date picker
	 * position - popover pointing rect (required by iPad platform)	 
	 * 
	 * long parameter of onValueSelectedAction will be set to selected date in unix timestamp format. 
	 * You can convert it to DateTime object using NativePicker.ConvertToDateTime function
	 * 
	 * onPickerCanceled is called on Cancel button click in Android, on tap outside the dialog on Android (dismiss) and tap outside the Popover on iOS (iPad only)
	 * can be null
	 */
	public void ShowDatePicker(Rect position, Action<long> onValueSelectedAction, Action onPickerCanceled) {
		ShowDatePicker(position, DateTime.Now, onValueSelectedAction, onPickerCanceled);
	}
	/**
	 * Show date picker
	 * position - popover pointing rect (required by iPad platform)
	 * date - date to select in the picker	 
	 * 
	 * long parameter of onValueSelectedAction will be set to selected date in unix timestamp format. 
	 * You can convert it to DateTime object using NativePicker.ConvertToDateTime function
	 * 
	 * onPickerCanceled is called on Cancel button click in Android, on tap outside the dialog on Android (dismiss) and tap outside the Popover on iOS (iPad only)
	 * can be null
	 * 
	 */
	public void ShowDatePicker(Rect position, DateTime date, Action<long> onValueSelectedAction, Action onPickerCanceled) {
		if (isMobileRuntime == false) {
			return;
		}
		_currentAction = onValueSelectedAction;
		_cancelAction = onPickerCanceled;
#if UNITY_ANDROID
		makeJNICall(PickerType.DatePicker, new string[] {}, ConvertToUnixTimestamp(date));
#elif UNITY_IPHONE
		//id = messageBox(caption, message, buttons, buttons.Length, "NativeDialogs");
		showPicker(PickerType.DatePicker, new string[] {}, 0, ConvertToUnixTimestamp(date), (int)position.x, (int)position.y, (int)position.width, (int)position.height, "NativePicker");
#endif
	}
	/**
	 * Show time picker
	 * position - popover pointing rect (required by iPad platform)	 
	 * 
	 * long parameter of onValueSelectedAction will be set to selected time in unix timestamp format. 
	 * You can convert it to DateTime object using NativePicker.ConvertToDateTime function
	 */
	public void ShowTimePicker(Rect position, Action<long> onValueSelectedAction, Action onPickerCanceled) {
		ShowTimePicker(position, DateTime.Now, onValueSelectedAction, onPickerCanceled);
	}
	/**
	 * Show time picker
	 * position - popover pointing rect (required by iPad platform)
	 * time - time to select in the picker	 
	 * 
	 * long parameter of onValueSelectedAction will be set to selected time in unix timestamp format. 
	 * You can convert it to DateTime object using NativePicker.ConvertToDateTime function
	 * 
	 * onPickerCanceled is called on Cancel button click in Android, on tap outside the dialog on Android (dismiss) and tap outside the Popover on iOS (iPad only)
	 * can be null
	 */
	public void ShowTimePicker(Rect position, DateTime time, Action<long> onValueSelectedAction, Action onPickerCanceled) {
		if (isMobileRuntime == false) {
			return;
		}
		_currentAction = onValueSelectedAction;
		_cancelAction = onPickerCanceled;
#if UNITY_ANDROID
		makeJNICall(PickerType.TimePicker, new string[] {}, ConvertToUnixTimestamp(time));
#elif UNITY_IPHONE		
		showPicker(PickerType.TimePicker, new string[] {}, 0, ConvertToUnixTimestamp(time), (int)position.x, (int)position.y, (int)position.width, (int)position.height, "NativePicker");
#endif
	}
	/**
	 * Show picker with custom item list
	 * position - popover pointing rect (required by iPad platform)	 
	 * items - custom item list
	 * 
	 * long parameter of onValueSelectedAction will be set to selected item index. 	 
	 * 
	 * onPickerCanceled is called on Cancel button click in Android, on tap outside the dialog on Android (dismiss) and tap outside the Popover on iOS (iPad only)
	 * can be null
	 */
	public void ShowCustomPicker(Rect position, string[] items, Action<long> onValueSelectedAction, Action onPickerCanceled) {
		ShowCustomPicker(position, items, -1, onValueSelectedAction, onPickerCanceled);
	}
	/**
	 * Show picker with custom item list
	 * position - popover pointing rect (required by iPad platform)	 
	 * items - custom item list
	 * selectedItem - item to select
	 * 
	 * long parameter of onValueSelectedAction will be set to selected item index. 	 
	 * 
	 * onPickerCanceled is called on Cancel button click in Android, on tap outside the dialog on Android (dismiss) and tap outside the Popover on iOS (iPad only)
	 * can be null
	 */
	public void ShowCustomPicker(Rect position, string[] items, int selectedItem, Action<long> onValueSelectedAction, Action onPickerCanceled) {
		if (isMobileRuntime == false) {
			return;
		}
		_currentAction = onValueSelectedAction;
		_cancelAction = onPickerCanceled;
#if UNITY_ANDROID
		makeJNICall(PickerType.CustomPicker, items, selectedItem);
#elif UNITY_IPHONE		
		showPicker(PickerType.CustomPicker, items, items.Length, selectedItem, (int)position.x, (int)position.y, (int)position.width, (int)position.height, "NativePicker");
#endif
	}
	/**
	 * Show date and time picker
	 * position - popover pointing rect (required by iPad platform)	 
	 * 
	 * long parameter of onValueSelectedAction will be set to selected date and time in unix timestamp format. 
	 * You can convert it to DateTime object using NativePicker.ConvertToDateTime function
	 */
	public void ShowDateTimePicker(Rect position, Action<long> onValueSelectedAction, Action onPickerCanceled) {
		ShowDateTimePicker(position, DateTime.Now, onValueSelectedAction, onPickerCanceled);
	}
	/**
	 * Show date and time picker
	 * position - popover pointing rect (required by iPad platform)
	 * time - time to select in the picker	 
	 * 
	 * long parameter of onValueSelectedAction will be set to selected date and time in unix timestamp format. 
	 * You can convert it to DateTime object using NativePicker.ConvertToDateTime function
	 * 
	 * onPickerCanceled is called on Cancel button click in Android, on tap outside the dialog on Android (dismiss) and tap outside the Popover on iOS (iPad only)
	 * can be null
	 */
	public void ShowDateTimePicker(Rect position, DateTime time, Action<long> onValueSelectedAction, Action onPickerCanceled) {
		if (isMobileRuntime == false) {
			return;
		}
		_currentAction = onValueSelectedAction;
		_cancelAction = onPickerCanceled;
		#if UNITY_ANDROID
		//Debug.LogWarning("Date and time picker is not support on Android platform.");
		#elif UNITY_IPHONE		
		showPicker(PickerType.DateTimePicker, new string[] {}, 0, ConvertToUnixTimestamp(time), (int)position.x, (int)position.y, (int)position.width, (int)position.height, "NativePicker");
		#endif
	}
	/**
	 * Convert unix timestamp to DateTime object
	 * val - unix timestamp value	 
	 */
	public static DateTime ConvertToDateTime(long val) {
		System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0);
		dtDateTime = dtDateTime.AddSeconds(val);
		return dtDateTime;
	}
	/**
	 * Convert DateTime object to unix timestamp
	 * dateTime - DateTime object to convert
	 */
	public static long ConvertToUnixTimestamp(DateTime dateTime)
	{
    	return (long)((dateTime - new DateTime(1970, 1, 1)).TotalSeconds);
	}
	/**
	 * Create DateTime object from provided date	 
	 */
	public static DateTime DateTimeForDate(int year, int month, int day) {
		return new DateTime(year, month, day);
	}
	public static DateTime DateTimeForYearsMonths(int year, int month) {
		return new DateTime(Convert.ToInt32(year),
			Convert.ToInt32(month),
			1);
	}
	/**
	 * Create DateTime object from provided time
	 */
	public static DateTime DateTimeForTime(int hour, int minute, int second) {
		return new DateTime(1970, 1, 1, hour, minute, second);
	}
	/**
	 * Create DateTime object from provided date and time
	 */
	public static DateTime DateTimeForDateTime(int year, int month, int day, int hour, int minute, int second) {
		return new DateTime(year, month, day, hour, minute, second);
	}
	/**
	 * Hide picker, used for iPhone platform
	 */
	public void HidePicker() {
		if (isMobileRuntime == false) {
			return;
		}
#if UNITY_ANDROID
#elif UNITY_IPHONE
		hidePicker();
#endif
	}
}
