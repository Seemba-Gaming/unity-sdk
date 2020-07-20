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
                StartCoroutine(LoadImage(path, imageRenderer));
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
		private void setAvatar(Texture2D texture ){
			Texture2D RoundTxt = ImagesManager.RoundCrop (texture);
			Sprite newSprite = Sprite.Create (RoundTxt, new Rect (0, 0, RoundTxt.width, RoundTxt.height), new Vector2 (0, 0));
			//Create Sprite and change Profile Avatar
			try {
				GameObject.Find ("_Avatar").GetComponent<Image> ().sprite = newSprite;
			} catch (NullReferenceException ex) {
			}
			GameObject.Find ("Avatar").GetComponent<Image> ().sprite = newSprite;
		}
		private IEnumerator LoadImage(string path, Image output)
        {	
			EventsController nb = new EventsController ();
			var url = "file://" + path;	
			var www = new WWW(url);
            yield return www;
            var texture = www.texture;
			if (texture != null) {
				if(imageToUpload=="avatar") {
					byte[] bytes;
					bytes = texture.EncodeToPNG ();
					CoroutineWithData cd = new CoroutineWithData(this, ImagesManager.FixImage (bytes) );
					yield return cd.coroutine;
					if (!string.IsNullOrEmpty (cd.result.ToString ())&&!cd.result.ToString ().Equals("error")) {
						ImagesManager.AvatarURL = cd.result.ToString ();
						if (!string.IsNullOrEmpty (ImagesManager.AvatarURL)) {
							www = new WWW (ImagesManager.AvatarURL);
							yield return www;
							Texture2D RoundTxt = ImagesManager.RoundCrop (www.texture);
							Sprite newSprite = Sprite.Create (RoundTxt, new Rect (0, 0, RoundTxt.width, RoundTxt.height), new Vector2 (0, 0));
							//Create Sprite and change Profile Avatar
							try {
								GameObject.Find ("_Avatar").GetComponent<Image> ().sprite = newSprite;
							} catch (NullReferenceException ex) {
							}
							GameObject.Find ("Avatar").GetComponent<Image> ().sprite = newSprite;
							//Update Current user Avatar in Views
							UserManager.CurrentAvatarBytesString = newSprite;
							//Get Byte from Sprite
							if (SceneManager.GetActiveScene ().name != "Signup") {
								string userId = um.getCurrentUserId ();
								string token = um.getCurrentSessionToken ();
								UnityThreadHelper.CreateThread (() => {
									//Update Avatar in DATABASE
									string[] attrib = { "avatar" };
									string[] value = { ImagesManager.AvatarURL };
									um.UpdateUserByField (userId, token, attrib, value);
								});
							}	
						}
					}
				}
				else if (imageToUpload=="IDFront") {
					Texture2D ScaledTxt = ImagesManager.ScaleTexture (texture, texture.width, texture.height);
					byte[] bytes = ScaledTxt.EncodeToPNG();
					System.IO.File.WriteAllBytes (Application.persistentDataPath + '/' + "ImageIDFront.png", bytes);
					pathID1 = Application.persistentDataPath + '/' + "ImageIDFront.png";
					nb.uploadDoc(pathID1,"IDFront");
				}
                else if (imageToUpload == "IDBack")
                {
                    Texture2D ScaledTxt = ImagesManager.ScaleTexture(texture, texture.width, texture.height);
                    byte[] bytes = ScaledTxt.EncodeToPNG();
                    System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "ImageIDBack.png", bytes);
                    pathID1 = Application.persistentDataPath + '/' + "ImageIDBack.png";
                    nb.uploadDoc(pathID1, "IDBack");
                }
                else if (imageToUpload=="passport") {
					Texture2D ScaledTxt = ImagesManager.ScaleTexture (texture, texture.width, texture.height);
					byte[] bytes = ScaledTxt.EncodeToPNG();
					System.IO.File.WriteAllBytes (Application.persistentDataPath+'/'+"ImagePassport.png", bytes);
					pathPassport =  Application.persistentDataPath +'/'+"ImagePassport.png";
					nb.uploadDoc(pathPassport,"Passport");
				}  
				else if (imageToUpload=="address") {
					Texture2D ScaledTxt = ImagesManager.ScaleTexture (texture, texture.width, texture.height);
					byte[] bytes = ScaledTxt.EncodeToPNG();
					System.IO.File.WriteAllBytes (Application.persistentDataPath+'/'+"ImageAddress.png", bytes);
					pathAddress =  Application.persistentDataPath +'/'+"ImageAddress.png";
					nb.uploadDoc(pathAddress,"Address");
				} else if (imageToUpload=="bank") {
					Texture2D ScaledTxt = ImagesManager.ScaleTexture (texture, texture.width, texture.height);
					byte[] bytes = ScaledTxt.EncodeToPNG();
					System.IO.File.WriteAllBytes (Application.persistentDataPath+'/'+"ImageBank.png", bytes);
					pathBank = Application.persistentDataPath +'/'+"ImageBank.png";
					nb.uploadDoc(pathBank,"Iban");
				} 
			}		
        }		
    }
}