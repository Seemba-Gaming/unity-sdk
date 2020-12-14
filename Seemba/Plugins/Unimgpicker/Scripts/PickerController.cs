using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SeembaSDK.Kakera
{
    [CLSCompliant(false)]
    public class PickerController : MonoBehaviour
    {
        #region Static
        public static PickerController Get { get { return sInstance; } }
        private static PickerController sInstance;
        #endregion

        [SerializeField]
        private Unimgpicker imagePicker;

        [SerializeField]
        private Image imageRenderer;

        private int[] sizes = {1024, 256, 16};

        void Awake()
        {
            sInstance = this;
            imagePicker.Completed += (string path) =>
            {
                StartCoroutine(LoadImage(path, imageRenderer));
            };
        }

        public void OnPressShowPicker()
        {
            imagePicker.Show("Select Image", "unimgpicker");
        }

        private IEnumerator LoadImage(string path, Image output)
        {
            var url = "file://" + path;
            var unityWebRequestTexture = UnityWebRequestTexture.GetTexture(url);
            yield return unityWebRequestTexture.SendWebRequest();
            LoaderManager.Get.LoaderController.ShowLoader(LoaderManager.LOADING);
            var texture = ((DownloadHandlerTexture)unityWebRequestTexture.downloadHandler).texture;
            if (texture == null)
            {
                Debug.LogError("Failed to load texture url:" + url);
            }
            else
            {
                Texture2D RoundTxt = ImagesManager.RoundCrop(texture);
                Sprite newSprite = Sprite.Create(RoundTxt, new Rect(0, 0, RoundTxt.width, RoundTxt.height), new Vector2(0, 0));
                output.sprite = newSprite;
                UserManager.Get.CurrentAvatarBytesString = newSprite;
            }
            PopupManager.Get.PopupViewPresenter.HidePopupContent(PopupManager.Get.PopupController.PopupChooseCharacter);
            LoaderManager.Get.LoaderController.HideLoader();
        }
    }
}