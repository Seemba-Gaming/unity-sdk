using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SeembaSDK.Kakera
{
    #pragma warning disable 0649
    [CLSCompliant(false)]
    public class PickerController : MonoBehaviour
    {
        [SerializeField]
        private Unimgpicker imagePicker;
        [SerializeField]
        private Image imageRenderer;
        public static string pathID1, pathAddress, pathBank, pathPassport, imageToUpload;

        private void Start()
        {
            imagePicker.Completed += (string path) =>
            {
                Debug.LogWarning("call back added");
                LoadImage(path, imageRenderer);
            };
        }
        public void OnPressShowPicker()
        {
            imagePicker.Show("Select Image", "unimgpicker", 1024);
        }
        public void OnPressShowPicker(string name)
        {
            imagePicker.Show("Select Image", "unimgpicker", 1024);
            imageToUpload = name;
        }

        private string getDataPath()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            string path = "";
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
        private void setAvatar(Texture2D texture)
        {
            Texture2D RoundTxt = ImagesManager.RoundCrop(texture);
            Sprite newSprite = Sprite.Create(RoundTxt, new Rect(0, 0, RoundTxt.width, RoundTxt.height), new Vector2(0, 0));
            //Create Sprite and change ProfilePresenter Avatar
            try
            {
                imageRenderer.sprite = newSprite;
            }
            catch (NullReferenceException)
            {
            }
            imageRenderer.sprite = newSprite;
        }
        private async void LoadImage(string path, Image output)
        {
            var url = "file://" + path;
            var www = UnityWebRequestTexture.GetTexture(url);
            await www.SendWebRequest();
            var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Debug.LogWarning("Load Image at : " + url);
            Debug.LogWarning(imageToUpload);
            if (texture != null)
            {
                if (imageToUpload == "avatar")
                {
                    byte[] bytes;
                    bytes = texture.EncodeToPNG();
                    var avatarUrl = await ImagesManager.FixImage(bytes);
                    if (!string.IsNullOrEmpty(avatarUrl) && !avatarUrl.Equals("error"))
                    {
                        Texture2D RoundTxt = ImagesManager.RoundCrop(texture);
                        Sprite newSprite = Sprite.Create(RoundTxt, new Rect(0, 0, RoundTxt.width, RoundTxt.height), new Vector2(0, 0));
                        imageRenderer.sprite = newSprite;
                        UserManager.Get.CurrentAvatarBytesString = newSprite;
                    }
                }
            }
        }
    }
}