#if UNITY_ANDROID
using UnityEngine;
namespace SeembaSDK.Kakera
{
    internal class PickerAndroid : IPicker
    {
        private static readonly string PickerClass = "com.kakeragames.unimgpicker.Picker";
        public void Show(string title, string outputFileName, int maxSize)
        {
            using (var picker = new AndroidJavaClass(PickerClass))
			{	//Debug.Log ("outputFileName : "+outputFileName);
                picker.CallStatic("show", title, outputFileName, maxSize);
            }
        }
    }
}
#endif