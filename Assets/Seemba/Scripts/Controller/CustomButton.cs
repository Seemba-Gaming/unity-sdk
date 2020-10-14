using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CustomButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler  {
	InputField password;
	// Use this for initialization
	void Update()
	{
		if (ispressed) {
			password.contentType = InputField.ContentType.Standard;
		} else {
			password.contentType = InputField.ContentType.Password;
		}
		password.ForceLabelUpdate ();
	} 
	bool ispressed = false;
	public void OnPointerDown(PointerEventData eventData)
	{
		ispressed = true;
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		ispressed = false;
	}
	void Start(){
		password = GameObject.Find ("password").GetComponent<InputField> ();
	}
}