using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;
namespace Kakera
{
    public class PickerController : MonoBehaviour
    {
        [SerializeField]
        private Unimgpicker imagePicker;
        [SerializeField]
		private Image imageRenderer;
		public static string pathID1, pathAddress, pathBank,pathPassport,imageToUpload;
		Sprite s;
		private UserManager um=new UserManager();
        void Awake()
        {
			try{
			imagePicker.Completed += (string path) =>
            {
                StartCoroutine(LoadImage(path));
            };
			}catch(NullReferenceException ex){
			}
        }
		public void OnPressShowPicker()
		{try{
				imagePicker.Show("Select Image", "unimgpicker", 1024);
			}catch(NullReferenceException ex){
			}
		}
		public void OnPressShowPicker(string name)
		{
			try{
            imagePicker.Show("Select Image", "unimgpicker", 1024);
				imageToUpload=name;
				//Debug.Log ("imageToUpload : "+imageToUpload);
			}catch(NullReferenceException ex){
			}
        }
		string getDataPath() {
			string path = "";
			#if UNITY_ANDROID && !UNITY_EDITOR
			try {
			IntPtr obj_context = AndroidJNI.FindClass("android/content/ContextWrapper");
			IntPtr method_getFilesDir = AndroidJNIHelper.GetMethodID(obj_context, "getFilesDir", "()Ljava/io/File;");
			using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
			IntPtr file = AndroidJNI.CallObjectMethod(obj_Activity.GetRawObject(), method_getFilesDir, new jvalue[0]);
			IntPtr obj_file = AndroidJNI.FindClass("java/io/File");
			IntPtr method_getAbsolutePath = AndroidJNIHelper.GetMethodID(obj_file, "getAbsolutePath", "()Ljava/lang/String;");
			path = AndroidJNI.CallStringMethod(file, method_getAbsolutePath, new jvalue[0]);
			if(path != null) {
			}
			else {
			path = "/data/data/com.DefaultCompany.sdkGame/files";
			}
			}
			}
			return path;
			}
			catch(Exception e) {
			return "";
			}
			#else
			return Application.persistentDataPath;
			#endif
		}
		
		private IEnumerator LoadImage(string path)
        {	
			EventsController nb = new EventsController ();
			var url = "file://" + path;	
			var www = new WWW(url);
            yield return www;
            var texture = www.texture;
			if (texture != null) {
				if (imageToUpload == "avatar")
				{
					ProfileViewPresenter.Instance.loadNewAvatar(texture);
				}
				else
				{
					Texture2D ScaledTxt = ImagesManager.ScaleTexture(texture, texture.width, texture.height);
					byte[] bytes = ScaledTxt.EncodeToPNG();

					if (imageToUpload == "IDFront")
					{
						System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "ImageIDFront.png", bytes);
						pathID1 = Application.persistentDataPath + '/' + "ImageIDFront.png";
						
					}
					else if (imageToUpload == "IDBack")
					{
						System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "ImageIDBack.png", bytes);
						pathID1 = Application.persistentDataPath + '/' + "ImageIDBack.png";
					}
					else if (imageToUpload == "passport")
					{
						System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "ImagePassport.png", bytes);
						pathPassport = Application.persistentDataPath + '/' + "ImagePassport.png";
					}
					else if (imageToUpload == "address")
					{
						System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "ImageAddress.png", bytes);
						pathAddress = Application.persistentDataPath + '/' + "ImageAddress.png";
					}
					nb.uploadDoc(pathID1, imageToUpload);
				}
			}		
        }		
    }
}