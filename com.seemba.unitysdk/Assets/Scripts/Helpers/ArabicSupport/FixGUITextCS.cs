using UnityEngine;
using System.Collections;
using SeembaSDK.ArabicSupport;
using TMPro;
public class FixGUITextCS : MonoBehaviour {
	
	public string text;
	public bool tashkeel = true;
	public bool hinduNumbers = true;
	
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<TextMeshProUGUI>().text = ArabicFixer.Fix(text, tashkeel, hinduNumbers);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
