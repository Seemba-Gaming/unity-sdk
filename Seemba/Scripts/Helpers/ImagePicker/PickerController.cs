using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Kakera
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

        private void Awake()
        {
            try
            {
                imagePicker.Completed += (string path) =>
                {
                    LoadImage(path, imageRenderer);
                };
            }
            catch (NullReferenceException)
            {
            }
        }
        public void OnPressShowPicker()
        {
            try
            {
                imagePicker.Show("Select Image", "unimgpicker", 1024);
            }
            catch (NullReferenceException)
            {
            }
        }
        public void OnPressShowPicker(string name)
        {
            try
            {
                imagePicker.Show("Select Image", "unimgpicker", 1024);
                imageToUpload = name;
                //Debug.Log ("imageToUpload : "+imageToUpload);
            }
            catch (NullReferenceException)
            {
            }
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
            if (texture != null)
            {
                //if (imageToUpload == "avatar")
                //{
                //    byte[] bytes;
                //    bytes = texture.EncodeToPNG();
                //    var avatarUrl = await ImagesManager.FixImage(bytes);
                //    if (!string.IsNullOrEmpty(avatarUrl) && !avatarUrl.Equals("error"))
                //    {

                //        Texture2D RoundTxt = ImagesManager.RoundCrop(texture);
                //        Sprite newSprite = Sprite.Create(RoundTxt, new Rect(0, 0, RoundTxt.width, RoundTxt.height), new Vector2(0, 0));
                //        //Create Sprite and change ProfilePresenter Avatar
                //        try
                //        {
                //            GameObject.Find("_Avatar").GetComponent<Image>().sprite = newSprite;
                //        }
                //        catch (NullReferenceException ex)
                //        {
                //        }
                //        GameObject.Find("Avatar").GetComponent<Image>().sprite = newSprite;
                //        //Update Current user Avatar in Views
                //        UserManager.Get.CurrentAvatarBytesString = newSprite;
                //    }
                //}
                //else if (imageToUpload == "IDFront")
                //{
                //    Texture2D ScaledTxt = ImagesManager.ScaleTexture(texture, texture.width, texture.height);
                //    byte[] bytes = ScaledTxt.EncodeToPNG();
                //    System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "ImageIDFront.png", bytes);
                //    pathID1 = Application.persistentDataPath + '/' + "ImageIDFront.png";
                //    EventsController.Get.uploadDoc(pathID1, "IDFront");
                //}
                //else if (imageToUpload == "IDBack")
                //{
                //    Texture2D ScaledTxt = ImagesManager.ScaleTexture(texture, texture.width, texture.height);
                //    byte[] bytes = ScaledTxt.EncodeToPNG();
                //    System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "ImageIDBack.png", bytes);
                //    pathID1 = Application.persistentDataPath + '/' + "ImageIDBack.png";
                //    EventsController.Get.uploadDoc(pathID1, "IDBack");
                //}
                //else if (imageToUpload == "passport")
                //{
                //    Texture2D ScaledTxt = ImagesManager.ScaleTexture(texture, texture.width, texture.height);
                //    byte[] bytes = ScaledTxt.EncodeToPNG();
                //    System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "ImagePassport.png", bytes);
                //    pathPassport = Application.persistentDataPath + '/' + "ImagePassport.png";
                //    EventsController.Get.uploadDoc(pathPassport, "Passport");
                //}
                //else if (imageToUpload == "address")
                //{
                //    Texture2D ScaledTxt = ImagesManager.ScaleTexture(texture, texture.width, texture.height);
                //    byte[] bytes = ScaledTxt.EncodeToPNG();
                //    System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "ImageAddress.png", bytes);
                //    pathAddress = Application.persistentDataPath + '/' + "ImageAddress.png";
                //    EventsController.Get.uploadDoc(pathAddress, "Address");
                //}
                //else if (imageToUpload == "bank")
                //{
                //    Texture2D ScaledTxt = ImagesManager.ScaleTexture(texture, texture.width, texture.height);
                //    byte[] bytes = ScaledTxt.EncodeToPNG();
                //    System.IO.File.WriteAllBytes(Application.persistentDataPath + '/' + "ImageBank.png", bytes);
                //    pathBank = Application.persistentDataPath + '/' + "ImageBank.png";
                //    EventsController.Get.uploadDoc(pathBank, "Iban");
                //}
            }
        }
    }
}