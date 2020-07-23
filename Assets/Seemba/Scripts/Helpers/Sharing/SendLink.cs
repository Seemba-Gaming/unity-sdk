using UnityEngine;
using UnityEngine.UI;
public class SendLink : MonoBehaviour
{
    public Button sendLink;
    // Use this for initialization
    void Start()
    {
        string ExtraSubject = "";
        string ExtraTitle = "";
        string ExtraText = "Download Modern Snike and earn money *TestMessage* ";
        sendLink.onClick.AddListener(() =>
        {
#if UNITY_ANDROID
			AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
			intentObject.Call<AndroidJavaObject> ("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));
			intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), ExtraText);
			AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
			currentActivity.Call ("startActivity", intentObject);
#endif
#if UNITY_IOS
			GeneralSharingiOSBridge.ShareSimpleText (ExtraText);
#endif
        });
    }
    // Update is called once per frame
    void Update()
    {
    }
}
