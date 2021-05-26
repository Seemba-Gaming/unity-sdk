using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SeembaSDK.ArabicSupport;
public class SetArabicTextExample : MonoBehaviour {
	
	public string text;
	
	// Use this for initialization
	void Start () {	
		gameObject.GetComponent<Text>().text = "This sentence (wrong display):\n" + text +
			"\n\nWill appear correctly as:\n" + ArabicFixer.Fix(text, false, false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
